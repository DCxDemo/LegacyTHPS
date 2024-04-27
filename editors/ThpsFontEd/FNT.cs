

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;

namespace FNTEdit.Removed
{
    class FNT
    {
        public int format = 0;

        public int size;
        public int letter_count;
        public int fheight;
        public int fvshift;

        public List<Letter> letters = new List<Letter>();

        public int size2;
        public int w;
        public int h;
        public int bpp;

        private MyStream palette;
        private MyStream bmpraw;

        public Bitmap bmp;

        //methods
    
        //test the stream against font formats, return true if matches
        private bool isFNT1(MyStream s)
        {
            s.p = 0;
            int size = s.GetInt();

            if (size != s.GetSize()) return false;

            int let_count = s.GetInt();


            if ((4*4 + let_count * 4 + 4 + 4) > s.GetSize()) return false;

            s.Skip(4+4+let_count*4+4);

            int w = s.GetInt16();
            int h = s.GetInt16();

            int prop_size = 4 * 4 + 4 * let_count + 4 * 4 + 256 * 4 + w * h + 4 + let_count * 8;

            if (prop_size != size) return false;

            s.p = 0;

            return true;
         }
        private bool isFNT2(MyStream s)
        {
            s.p = 0;
            s.Skip(8);
            int let_count = s.GetInt();

            s.Skip(4 + 4 + let_count * 6 + 4);

            int w = s.GetInt16();
            int h = s.GetInt16();

            int prop_size = 5 * 4 + 6 * let_count + 4 * 4 + 256 * 4 + w * h + 4 + let_count * 8;

            if (prop_size != s.GetSize()) return false;

            s.p = 0;

            return true;
        }
        private bool isFNT3(MyStream s)
        {
            s.p = 0;
            s.Skip(8);
            int offset1 = s.GetInt();
            s.Jump(offset1 - 4);
            int offset2 = s.GetInt();
            s.Jump(offset2);
            s.Skip(28);
            s.Skip(1024);
            int w = s.GetInt16();
            int h = s.GetInt16();

            if (s.GetSize() != offset2+16+12+1024+4+w*h) return false;

            s.p = 0;
            return true;
        }

        //constructors
        public FNT()
        {
        }

        public FNT(string s)
        {
            //read all bytes
            MyStream ms = new MyStream(File.ReadAllBytes(s));

            //check whether we've got an actual font.
            if (isFNT1(ms)) { format = 1; }          //YAY!
            else                                    //dont'panic yet, could be THUG2 font
            {
                if (isFNT2(ms)) { format = 2; }      //YAY!!
                else                                //keep calm, this could be THAW font
                {
                    if (isFNT3(ms)) { format = 3; }  //YAY!!!
                    else                            //Okay, now you're officially allowed to run in circles screaming
                    {
                        MessageBox.Show(Errormsg.not_fnt_file);
                        return;
                    }
                }
            }

            using (BinaryReader br = new BinaryReader(File.OpenRead(s)))
            {
                //read font depending on format
                //we don't need default behaviour cause of return up there
                switch (format)
                {
                    case 1:
                    case 2: ReadFNT1or2(br); break;
                    case 3: ReadFNT3(ms); break;
                }
            }
        }

        /*
        private void ReadFNT1or2(MyStream s)
        {
            //read header
            size = s.GetInt();
            if (format == 2) s.Skip(4);
            letter_count = s.GetInt();
            fheight = s.GetInt();
            fvshift = s.GetInt();

            //read letters char and height
            for (int i = 0; i < letter_count; i++) letters.Add(new Letter(s, format));


            //read font params
            size2 = s.GetInt();
            w = s.GetInt16();
            h = s.GetInt16();
            bpp = s.GetInt16();
            s.Skip(6); //0xCCCCCC

            //read font bitmap
            bmpraw = s.CopyArray(w * h);
            palette = s.CopyArray(1024);

            int num2 = s.GetInt();

            //fill the rest of letter data
            foreach (Letter l in letters) { l.FillLayout(s); }

            bmp = getBMP();

            foreach (Letter l in letters) { l.CutoutLetter(bmp); }
        }

        */


        private void ReadFNT3(MyStream s)
        {
            s.Skip(8);
            int offset1 = s.GetInt();
            s.Jump(offset1 - 4);
            int offset2 = s.GetInt();
            s.Jump(offset2);
            s.Skip(16);
            s.Skip(12);

            palette = s.CopyArray(1024);
            w = s.GetInt16();
            h = s.GetInt16();

            bmpraw = s.CopyArray(w * h);

            bmp = getBMP();
        }

        public void ImportTHP8(string str)
        {
            MyStream s = new MyStream(File.ReadAllBytes(str));
            s.Skip(8);
            int offset1 = s.GetInt();
            MessageBox.Show("" + offset1.ToString("X"));
            s.Jump(offset1 - 4);
            int offset2 = s.GetInt();
            s.Jump(offset2);
            MessageBox.Show("" + offset2.ToString("X"));
            s.Skip(8);
            w = s.GetInt16();
            h = s.GetInt16();

            s.Skip(128 - 12);

            MessageBox.Show(s.p.ToString("X8"));

            bmpraw = s.CopyArray(w * h);
            MessageBox.Show(s.p.ToString("X8"));
            palette = s.CopyArray(1024);

            bmp = getBMP();

        }

        public void Save(string s)
        {
            getBMP().Save(s);
        }

        public Bitmap getBMP()
        {
            BMP32Builder bbq = new BMP32Builder(w, h);

            foreach (byte k in bmpraw.stream)
            {
                palette.Jump(k*4);
                bbq.PlaceColor(palette.CopyArray(4).stream);
            }

            FastBMP p = new FastBMP(bbq.data, w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            return p.GetBMP();
        }

        public string Info()
        {
            string s = size2.ToString() + " " +w.ToString() + " " + h.ToString();
            return s;
        }

        public void FillListbox(ListBox x)
        {
            x.Items.Clear();
            foreach (Letter l in letters) x.Items.Add(l.Print());
        }

        public Bitmap Draw()
        {
            Bitmap x = new Bitmap(800, 100);
            Graphics xx = Graphics.FromImage(x);

            int offset = 0;
            foreach (Letter l in letters)
            {
                xx.DrawImage(l.bmp, offset, 0);
                offset += l.w;
            }
            return x;
        }
    }
}
