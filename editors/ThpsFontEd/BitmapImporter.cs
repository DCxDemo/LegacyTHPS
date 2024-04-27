using LegacyThps.Fonts;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace ThpsFontEd
{
    class BitmapImporter
    {
        string lets = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ'!:,<=-%.+?_/>";
        //string lets = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.,-!?:'+/^®()*@`¡¢£¤¥¦§¨©ª«¬{_#$%&\\=<>ßÄÜÖàâÄêèéëìîïôòÖùûÜçœüäö¼½¾¿ºáóúíñŒäüÚÁÉÓÚÑÌÍÇÀ™￿￾�￼￠";
        //string lets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789?!:.,-%+'#/$abcdefghijklmnopqrstuvwxyz";
        //"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890-=!@#$%^&*()_+{}[]:;\"\'|\\<>,./?"; //@"ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        Bitmap bmp;
        //public FNT fnt = new FNT();
        public FNT fnt = new FNT();

        public BitmapImporter(string fn)
        {
            bmp = new Bitmap(fn);
        }

        public void SetCharset(string s)
        {
            lets = s;
        }

        List<int> statechanges = new List<int>();

        bool oldstate = false;


        public string Info()
        {
            fnt.Bpp = 8;
            fnt.Atlas = new Bitmap(256, 256);

            FastBMP bb = new FastBMP(bmp);
            int curline = 0;

            if (!bb.isLineTransparent(0)) statechanges.Add(0);
            oldstate = bb.isLineTransparent(0);


            LoaderForm lf = new LoaderForm();
            lf.Show();
            lf.ResetBar();


            while (curline < bmp.Width)
            {
                if (bb.isLineTransparent(curline) != oldstate) statechanges.Add(curline);
                oldstate = bb.isLineTransparent(curline);
                curline++;

                lf.progressBar1.Value = (int)(curline / (float)bmp.Width * 100.0f);
                if (lf.progressBar1.Value % 5 == 0) lf.label1.Text = "Scanning line " + curline + " out of " + bmp.Width;
                System.Windows.Forms.Application.DoEvents();
            }

            lf.Close();
            lf = null;


            if (!bb.isLineTransparent(bmp.Width - 1)) statechanges.Add(bmp.Width);

            int baseline = 0;

            for (int i = 0; i < statechanges.Count; i += 2)
            {
                Glyph g = new Glyph(0, 'A', new Rectangle(0, 0, statechanges[i + 1] - statechanges[i], bmp.Height));
                g.bmp = bmp.Clone(new Rectangle(statechanges[i], 0, g.Region.Width, g.Region.Height), PixelFormat.Format32bppArgb);


                FastBMP fb = new FastBMP(g.bmp);
                //System.Windows.Forms.MessageBox.Show(""+fb.GetTopSpaceWidth());

                int top = fb.GetTopSpaceWidth();

                Rectangle r = g.Region;
                r.Height -= (fb.GetTopSpaceWidth() + fb.GetBottomSpaceWidth() - 1);
                g.Region = r;

                // g.VShift = (ushort)fb.GetTopSpaceWidth();
                g.bmp = fb.GetBMP().Clone(new Rectangle(0, fb.GetTopSpaceWidth(), g.Region.Width, g.Region.Height), PixelFormat.Format32bppArgb);

                //using first symbol for the baseline
                if (baseline == 0)
                    baseline = top + g.Region.Height;

                g.VShift = (ushort)(baseline - top);

                fnt.Glyphs.Add(g);
                //fnt2.letters.Add(l);

            }


            for (int i = 0; i < fnt.Glyphs.Count; i++)
            {
                try
                {
                    fnt.Glyphs[i].Value = (char)lets[i];
                }
                catch
                {
                    fnt.Glyphs[i].Value = 'X';
                }
            }

            fnt.FontVShift = (short)baseline - 5;
            fnt.Height = (short)baseline;

            FNT.version = EVersion.FNT1;

            //fnt2.LetterCount = fnt2.letters.Count;
            //fnt2.format = 1;

            fnt.RearrangeGlyphs(256);

            //System.Windows.Forms.MessageBox.Show("" + bb.GetNumCols());

            string res = "";
            foreach (int i in statechanges)
            {
                res += i + " ";
            }

            return res;
        }

    }
}
