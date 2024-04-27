using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LegacyThps.Fonts
{
    public class Glyph
    {
        private ushort vShift;
        [DisplayName("1. vShift"), Category("Glyph params")]
        public ushort VShift { get => vShift; set => vShift = value; }


        private ushort character;
        [DisplayName("2. Character"), Category("Glyph params")]
        public ushort Value { get => character; set => character = value; }

        private Rectangle region;
        [DisplayName("3. Region"), Category("Glyph params")]
        public Rectangle Region { get => region; set => region = value; }

        public Glyph()
        {
        }

        public Bitmap bmp;

        public Glyph(ushort v, ushort c, Rectangle reg)
        {
            VShift = v;
            Value = c;
            Region = reg;
        }

        public void ReadHeader(BinaryReader br)
        {
            VShift = br.ReadUInt16();
            switch (FNT.version)
            {
                case EVersion.FNT1: Value = br.ReadUInt16(); break;
                case EVersion.FNT2:
                    {
                        Value = br.ReadUInt16();
                        if (br.ReadUInt16() != 0)
                            Console.WriteLine("ug2 extra char value is not null");
                        break;
                    }
                default: throw new Exception("Unsopported version");
            }
        }

        public void ReadRegion(BinaryReader br)
        {
            region = new Rectangle(
                br.ReadInt16(),
                br.ReadInt16(),
                br.ReadInt16(),
                br.ReadInt16()
            );
        }

        public string Print()
        {
            return (char)Value + " " + Value.ToString() + " [" + VShift + "] at (" + Region.X + ", " + Region.Y + ") " + Region.Width + "x" + Region.Height;
        }

        public void CutoutLetter(Bitmap atlas)
        {
            //some debug fonts are larger than the atlas. hence clone fails

            try
            {
                if (Region.Width + Region.X > atlas.Width)
                    region.Width = atlas.Width - region.X;

                if (Region.Height + Region.Height > atlas.Height)
                    region.Height = atlas.Height - region.Y;

                bmp = new Bitmap(Region.Width, Region.Height);
                bmp = atlas.Clone(region, atlas.PixelFormat);
            }
            catch (Exception ex) 
            {
            }
        }

        public void Scale(int scale)
        {
            var newbmp = new Bitmap(Region.Width * scale, Region.Height * scale);
            var graph = Graphics.FromImage(newbmp);

            graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            graph.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            graph.DrawImage(bmp,
                new Rectangle(0, 0, newbmp.Width, newbmp.Height),
                new Rectangle(0, 0, bmp.Width, bmp.Height),
                GraphicsUnit.Pixel
                );

            region.X *= 2;
            region.Y *= 2;
            region.Width *= 2;
            region.Height *= 2;

            bmp = newbmp;
        }

        public void WriteHeader(BinaryWriter bw, EVersion version)
        {
            bw.Write((short)VShift);
            switch (version)
            {
                case EVersion.FNT1:
                    bw.Write(Value);
                    break;

                case EVersion.FNT2:
                    bw.Write(Value);
                    bw.Write((ushort)0);
                    break;
            }
        }

        public void WriteRegion(BinaryWriter bw)
        {
            bw.Write((short)region.X);
            bw.Write((short)region.Y);
            bw.Write((short)region.Width);
            bw.Write((short)region.Height);
        }
    }
}
