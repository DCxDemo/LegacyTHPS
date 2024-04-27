using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ColourQuantization;

namespace LegacyThps.Fonts
{
    public enum EVersion
    {
        FNT0 = 0, //THPS1,2
        FNT1 = 1, //THPS3,4,UG
        FNT2 = 2, //THUG2
        FNT3 = 3  //THAW
    }

    public class FNT
    {
        public static EVersion version = EVersion.FNT1;

        public List<Glyph> Glyphs = new List<Glyph>();

        public int LetterCount => Glyphs != null ? Glyphs.Count : 0;

        //this is probably called baseline/ascent/descent

        private int fontHeight;
        [DisplayName("1. Height"), Category("Font params")]
        public int FontHeight
        {
            get => fontHeight;
            set => fontHeight = value;
        }

        private int fontVShift;
        [DisplayName("2. Vertical shift"), Category("Font params")]
        public int FontVShift
        {
            get => fontVShift;
            set => fontVShift = value;
        }


        public byte[] palette;
        public byte[] bmpraw;

        public Bitmap Atlas;


        //private short width;
        [DisplayName("1. Width"), Category("Bitmap")]
        public short Width
        {
            get => (Atlas != null ? (short)Atlas.Width : (short)0);
            set
            {
                Atlas = new Bitmap(value, Atlas.Height);
                RearrangeGlyphs(Atlas.Width);
                GenerateBitmap(false);
            }
        }

        //private short height;
        [DisplayName("2. Height"), Category("Bitmap")]
        public short Height
        {
            get => (Atlas != null ? (short)Atlas.Height : (short)0);
            set
            {
                Atlas = new Bitmap(Atlas.Width, value);
                RearrangeGlyphs(Atlas.Width);
                GenerateBitmap(false);
            }
        }

        private short bpp;
        [DisplayName("3. Bit depth"), Category("Bitmap")]
        public short Bpp
        {
            get => bpp;
            set => bpp = value;
        }
        //public int Size { 
        //    get => size; 
        //    set => size = value; 
        //}


        //methods

        private bool isFNT3(BinaryReader s)
        {
            s.BaseStream.Position = 8;
            int offset1 = s.ReadInt32();
            s.BaseStream.Position = offset1 - 4;
            int offset2 = s.ReadInt32();
            s.BaseStream.Position = offset2 + 0xC;

            int w = s.ReadInt16();
            int h = s.ReadInt16();

            s.BaseStream.Position = offset2 + 0x10;

            if (s.BaseStream.Length != offset2 + 0x20 + 1024 + w * h) return false;

            Console.WriteLine((offset2 + 0x20 + 1024 + w * h).ToString() + " " + s.BaseStream.Length, ToString());

            s.BaseStream.Position = 0;
            return true;
        }

        //constructors
        public FNT()
        {
        }

        public FNT(BinaryReader br) => Read(br);

        public static FNT FromReader(BinaryReader br) => new FNT(br);

        public static FNT FromFile(string filename)
        {
            using (var br = new BinaryReader(File.OpenRead(filename)))
            {
                return FromReader(br);
            }
        }

        private void Read(BinaryReader br)
        {
            //try
            {
                ReadFNT1or2(br);
            }
         //   catch
            {
           //     ReadFNT0(br, false);
            }

            /*
            //check whether we've got an actual font.
            if (isFNT1(br)) { format = 1; }          //YAY!
            else                                    //dont'panic yet, could be THUG2 font
            {
                if (isFNT2(br)) { format = 2; }      //YAY!!
                else                                //keep calm, this could be THAW font
                {
                    if (isFNT3(br)) { format = 3; }  //YAY!!!
                    else                            //Okay, now you're officially allowed to run in circles screaming
                    {
                        MessageBox.Show(Errormsg.not_fnt_file);
                        return false;
                    }
                }

            }

            br.BaseStream.Position = 0;

            //read font depending on format
            //we don't need default behaviour cause of return up there

            switch (format)
            {
                case 1:
                case 2: ReadFNT1or2(br); break;
                case 3: ReadFNT3(br); break;
            }
            */
        }


        private void ReadFNT0(BinaryReader br, bool altmode)
        {
            Atlas = new Bitmap(256, 128);

            br.BaseStream.Position = 0;
            List<Color> palette = new List<Color>();

            int numGlyphs = br.ReadInt32();

            string charset = DetectTh2Charset(numGlyphs);

            for (int i = 0; i < numGlyphs; i++)
            {

                int a1 = br.ReadInt32();
                int a2 = br.ReadInt32();
                int a3 = br.ReadInt32();

                if (a1 > 64 || a2 > 64 || a3 > 64)
                    throw new Exception("fnt0 glyph size sanity check failed.");

                if (!altmode)
                {
                    int a4 = br.ReadInt32();
                    Glyphs.Add(new Glyph((ushort)a3, (ushort)0, new Rectangle(0, 0, a4, a2)));
                }
                else
                {
                    Glyphs.Add(new Glyph((ushort)a3, (ushort)0, new Rectangle(0, 0, a3, a2)));
                }
            }

            if (!altmode)
            {
                for (int i = 0; i < 16; i++)
                    palette.Add(Int16ToColor(br.ReadUInt16()));
            }
            else
            {
                int pos = (int)br.BaseStream.Position;

                br.BaseStream.Position = br.BaseStream.Length - 2 * 16;

                for (int i = 0; i < 16; i++)
                    palette.Add(Int16ToColor(br.ReadUInt16()));

                br.BaseStream.Position = pos;
            }

            for (int i = 0; i < numGlyphs; i++)
            {
                if (Glyphs[i].Region.Height > FontHeight)
                    FontHeight = Glyphs[i].Region.Height;

                try
                {
                    ReadLetterBitmap(Glyphs[i], br, palette);
                }
                catch
                {
                    ReadFNT0(br, true);
                    Glyphs[i].bmp = new Bitmap(8, 8);
                }
            }

            FontVShift = FontHeight;


            if (charset != "")
            {
                for (int i = 0; i < numGlyphs; i++)
                    Glyphs[i].Value = (ushort)charset[i];
            }

            Bpp = 8;
            RearrangeGlyphs(256);

            //Glyphs = Glyphs.OrderBy(p => p.Character).ToList();

        }


        public void ReadLetterBitmap(Glyph g, BinaryReader br, List<Color> palette)
        {
            //if (s.p+1 % 2 == 1) s.Skip(2);
            //if (s.GetUshort() != 2) { s.Skip(-2); } else { MessageBox.Show("got 02!"); MessageBox.Show("" + bmpsize + " " + bmpsize % 4 + " " + bmpsize % 2); }

            int w = g.Region.Width;
            int h = g.Region.Height;

            int realw = w;
            int realh = h;

            //WRNING this is neede to align by 4 pixels (2 bytes)
            if (realw % 4 != 0) w = realw + (4 - realw % 4);
            //if (realh % 2 != 0) h = realh + 1;

            Bitmap let = new Bitmap(w, h);
            g.Region = new Rectangle(g.Region.X, g.Region.Y, w, h);

            int bmpsize = 0;

            for (int j = 0; j < h; j++)
                for (int i = 0; i < w / 2; i++)
                {
                    int index = br.ReadByte();

                    let.SetPixel(i * 2, j, palette[index & 0x0F]);
                    let.SetPixel(i * 2 + 1, j, palette[index >> 4]);

                    bmpsize += 1;
                }

            if (bmpsize % 4 != 0) br.ReadUInt16();

            g.bmp = let;//cropImage(let, new Rectangle(0,l.a1, realw, realh));
        }

        public Color Int16ToColor(ushort x)
        {
            Color lol = Color.FromArgb(
                255 * (((x & 0x8000) > 0) ? 1 : 0),
                (x & 0x1F) << 3,
                (x & 0x3E0) >> 2,
                (x & 0x7C00) >> 7
                );
            return lol;
        }

        public string latincaps = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public string latinlows = "abcdefghijklmnopqrstuvwxyz";
        public string numbers = "0123456789";
        public string specific = "ĀÇĒÊŪŒÄÖÜß";
        public string specificlow = "āçēêūœäöü";
        public string punctuation = "?!:.,-%+'#/$_";
        public string specific2 = "ĀÇĒÊŪŪŒÄÖÜß";

        public string DetectTh2Charset(int x)
        {
            switch (x)
            {
                case 10: return numbers;
                case 10 + 1: return numbers + ":";
                case 13: return "△○×□←→↑↓↗↖↙↘+";
                case 26: return latincaps;
                case 26 + 2: return latincaps + "01";
                case 26 + 10: return latincaps + numbers;
                case 26 + 10 + 1: return latincaps + numbers + "/";
                case 26 + 10 + 4: return latincaps + numbers + "'!:.";
                case 26 + 10 + 9: return latincaps + numbers + "?!:.,-_%+";
                case 26 + 10 + 10: return latincaps + numbers + "?!:.,-%+'#";
                case 26 + 10 + 12: return latincaps + numbers + "?!:.,-%+'#";
                case 26 + 10 + 13: return latincaps + numbers + punctuation;
                case 26 + 26 + 10: return latincaps + numbers + "?!:.,-%+'#/$";
                case 26 + 10 + 23: return latincaps + numbers + punctuation + specific;
                case 26 + 10 + 24: return latincaps + numbers + punctuation + specific2;
                case 94: return latincaps + numbers + punctuation + latinlows + specific + specificlow;
                case 26 + 10 + 12 + 26: return latincaps + numbers + "?!:.,-%+'#/$" + latinlows;
                default: return "";
                    //26 + 10 + 13 + 26 + 10
            }
        }

        private void ReadFNT1or2(BinaryReader br)
        {
            //reset version
            FNT.version = EVersion.FNT1;

            //read header
            int size = br.ReadInt32();

            if (FNT.version == EVersion.FNT1 && size != br.BaseStream.Length)
            {
                //it's not a valid fnt1 file, let's try fnt2
                FNT.version = EVersion.FNT2;
            }

            //skip extra int
            if (FNT.version == EVersion.FNT2)
                if (br.ReadInt32() != 1)
                    throw new Exception("thug2 magic val is not 1, that's unusual.");

            int letCnt = br.ReadInt32();


            //if (FNT.version == EVersion.FNT2 && size != br.BaseStream.Length - (4 + 2 * letCnt))
            //   throw new Exception("Size mismatch - not a valid FNT2 file.");


            fontHeight = br.ReadInt32();
            fontVShift = br.ReadInt32();

            //read letters char and height
            for (int i = 0; i < letCnt; i++)
            {
                Glyph g = new Glyph();
                g.ReadHeader(br);
                Glyphs.Add(g);
            }

            //read font params
            if (size != br.ReadInt32())
                Console.Write("Warning, size check fail"); //wait what, they are not same, fix
                                                           //throw new Exception("Size mismatch - not a valid FNT1 file.");

            int w = br.ReadInt16();
            int h = br.ReadInt16();
            bpp = br.ReadInt16();
            br.BaseStream.Position += 6;

            if (bpp != 8)
                Console.WriteLine("bpp is not 8, that's unusual.");

            //read font bitmap
            bmpraw = br.ReadBytes(w * h);
            palette = br.ReadBytes(1024);

            int letCnt2 = br.ReadInt32();
            if (letCnt2 != letCnt)
                throw new Exception("Letters count mismatch - not a valid FNT file. " + w + " " + h + " " + br.BaseStream.Position.ToString("X8"));

            //fill the rest of letter data
            foreach (Glyph g in Glyphs)
                g.ReadRegion(br);

            Atlas = RawToBmp(w, h);

            foreach (Glyph g in Glyphs)
                g.CutoutLetter(Atlas);

            GenerateBitmap(false);
        }


        private void ReadFNT3(BinaryReader br)
        {
            fontVShift = br.ReadInt32();
            fontHeight = br.ReadInt32();

            int offset1 = br.ReadInt32();
            //MessageBox.Show("" + offset1.ToString("X"));
            br.BaseStream.Position = offset1 - 4;
            int offset2 = br.ReadInt32();
            br.BaseStream.Position = offset2 + 0xC;
            //MessageBox.Show("" + offset2.ToString("X"));
            int w = br.ReadInt16();
            int h = br.ReadInt16();

            br.BaseStream.Position += 0x0C;

            //MessageBox.Show(s.p.ToString("X8"));

            palette = br.ReadBytes(1024);

            br.ReadInt32();

            bmpraw = br.ReadBytes(Width * Height);
            //MessageBox.Show(s.p.ToString("X8"));


            Atlas = RawToBmp(w, h);



            br.BaseStream.Position = 0xC;

            byte[] ascii = br.ReadBytes(288);

            for (int i = 0; i < 288; i++)
            {
                byte index = ascii[i];

                // MessageBox.Show((char)i + " = " + index.ToString());

                if ((index != 0x9D && index != 0x90 && index != 0) || (char)i == 'A' || (char)i == '0')
                {
                    br.BaseStream.Position = 0x17C + 0x18 * index;

                    int x1 = (int)(Width * br.ReadSingle());
                    int y1 = (int)(Height * br.ReadSingle());
                    int x2 = (int)(Width * br.ReadSingle());
                    int y2 = (int)(Height * br.ReadSingle());
                    br.BaseStream.Position += 4;
                    ushort vs = br.ReadUInt16();
                    br.BaseStream.Position += 2;

                    Glyph l = new Glyph(vs, (ushort)i, new Rectangle(x1, y1, x2 - x1, y2 - y1));
                    Glyphs.Add(l);
                }
            }

            //LetterCount = letters.Count();
            bpp = 8;


            foreach (Glyph l in Glyphs) { l.CutoutLetter(Atlas); }

        }


        public Bitmap GetSourceCharset(int spaceBetween)
        {
            int total_width = spaceBetween;

            foreach (var glyph in Glyphs)
                total_width += glyph.Region.Width + spaceBetween;

            try
            {
                var bmp = new Bitmap(total_width, 100);//fheight * 2);
                var graphics = Graphics.FromImage(bmp);

                int currentx = spaceBetween;

                foreach (var glyph in Glyphs)
                {
                    graphics.DrawImage(glyph.bmp, new Point(currentx, fontVShift - glyph.VShift));
                    currentx += glyph.Region.Width + spaceBetween;
                }

                return bmp;
            }
            catch (Exception e)
            {
                Console.WriteLine(total_width + " " + fontHeight * 2 + "\r\n" + e.ToString());
                return null;
            }
        }

        public void Recut()
        {
            foreach (var glyph in Glyphs)
                glyph.CutoutLetter(Atlas);
        }

        public void ExportAtlas(string s)
        {
            Atlas.Save(s);
            //RawToBmp(Width, Height).Save(s);
        }

        public int MaxWidth
        {
            get
            {
                int value = 0;

                foreach (var glyph in Glyphs)
                {
                    if (glyph.Region.X + glyph.Region.Width > value)
                        value = glyph.Region.X + glyph.Region.Width + 1;
                }

                int result = 1;

                while (Math.Pow(2, result) < value)
                    result++;

                return (int)Math.Pow(2, result);
            }
        }

        public int MaxHeight
        {
            get
            {
                int value = 0;

                foreach (var glyph in Glyphs)
                {
                    if (glyph.Region.Y + glyph.Region.Height > value)
                        value = glyph.Region.Y + glyph.Region.Height + 1;
                }

                int result = 1;

                while (Math.Pow(2, result) < value)
                    result++;

                return (int)Math.Pow(2, result);
            }
        }

        public void GenerateBitmap(bool bgr)
        {
            if (Atlas != null)
            {
                var newbmp = new Bitmap(MaxWidth, MaxHeight);
                var graphics = Graphics.FromImage(newbmp);

                foreach (var glyph in Glyphs)
                    graphics.DrawImage(glyph.bmp, glyph.Region);

                var bb = new FastBMP(newbmp);

                //newbmp.Save("test.png");

                if (bb.GetNumCols() > 256)
                {
                    MessageBox.Show("more than 256 colors, autoquantization is applied.\r\nTo avoid, make sure your color count does not exceed 256 colors.");

                    var quantized = Octree.Quantize(Atlas, 256);

                    MessageBox.Show(quantized.PixelFormat.ToString());

                    Atlas = new Bitmap(Atlas.Width, Atlas.Height, PixelFormat.Format32bppArgb);
                    var graph = Graphics.FromImage(Atlas);
                    graph.DrawImage(quantized, 0, 0);

                    //Atlas.Save("kek.png");
                    //Process.Start("kek.png");

                    bb = new FastBMP(Atlas);
                    
                    
                }

                bmpraw = bb.GetRaw();
                if (bgr) bb.THUGifyPalette();
                palette = bb.GetRawPalette();

                Atlas = RawToBmp(MaxWidth, MaxHeight);
            }
            else
            {
                throw new Exception("bitmap is null");
            }
        }

        public Bitmap RawToBmp(int w, int h)
        {
            var bbq = new BMP32Builder(w, h);

            foreach (byte k in bmpraw)
            {
                // RGB ORDERING, IMPLEMENT SOME OPTION

                byte[] x = new byte[4] { palette[k * 4], palette[k * 4 + 1], palette[k * 4 + 2], palette[k * 4 + 3] };
                bbq.PlaceColor(x);
            }

            FastBMP p = new FastBMP(bbq.data, w, h, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            return p.GetBMP();
        }

        public string GetCharset(bool spaces)
        {
            string result = "";

            foreach (Glyph l in Glyphs)
            {
                char c = 'x';

                switch (l.Value)
                {
                    case 0x0099: c = (char)0x2122; break;
                    case 0x008C: c = (char)0x0152; break;
                    case 0x009C: c = (char)0x0153; break;
                    default: c = (char)l.Value; break;
                }

                result += c + (spaces ? " " : "");
            }

            return result;
        }


        public void RearrangeGlyphs(int width_limit)
        {
            //Glyphs = Glyphs.OrderByDescending(p => p.Region.Width).ToList();

            int currentx = 1;
            int currenty = 1;

            int spacing = 2;

            int maxythisline = 0;

            foreach (var glyph in Glyphs)
            {
                var r = glyph.Region;

                r.X = currentx;
                r.Y = currenty;

                if (maxythisline < r.Height) maxythisline = r.Height;

                if ((currentx + r.Width + spacing) < width_limit)
                {
                    currentx += r.Width + spacing;
                }
                else
                {
                    currentx = 1;
                    currenty += maxythisline + spacing;

                    r.X = currentx;
                    r.Y = currenty;

                    maxythisline = r.Height;

                    currentx += r.Width + spacing;
                }

                glyph.Region = r;
            }

            GenerateBitmap(false);
        }

        public void Save(string filename)
        {
            using (var bw = new BinaryWriter(File.OpenWrite(filename)))
            {
                Write(bw);
            }
        }

        public void Write(BinaryWriter bw)
        {
            //header
            bw.Write(0);
            if (version == EVersion.FNT2)
                bw.Write(1);
            bw.Write(Glyphs.Count);
            bw.Write(fontHeight);
            bw.Write(fontVShift);

            foreach (Glyph g in Glyphs)
                g.WriteHeader(bw, version);

            //remember the position for later use
            int pos = (int)bw.BaseStream.Position;

            bw.Write(0);
            bw.Write(Width);
            bw.Write(Height);

            if (Bpp != 8)
            {
                Bpp = 8;
                Console.WriteLine("bpp is not 8");
            }
            bw.Write(Bpp);


            bw.Write((short)0);
            bw.Write((short)0);
            bw.Write((short)0);

            bw.Write(bmpraw);
            bw.Write(palette);

            bw.Write(Glyphs.Count);

            foreach (Glyph g in Glyphs)
                g.WriteRegion(bw);

            //taking care of file size fields
            int size = (int)bw.BaseStream.Length;
            if (FNT.version == EVersion.FNT2)
                size -= LetterCount * 2 + 4;

            bw.BaseStream.Position = 0;
            bw.Write(size);

            bw.BaseStream.Position = pos;
            bw.Write(size - pos + (LetterCount * 2 + 4));
        }
    }
}