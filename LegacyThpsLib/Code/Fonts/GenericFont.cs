using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace LegacyThps.LegacyThps.Fonts
{
    public class GenericGlyph
    {
        public char CharCode = '\0';
        public int VShift = 0;
        public Bitmap Texture;

        public Point Location = new Point(0, 0);
        public Size Size = new Size(0, 0);

        public Rectangle Region => new Rectangle(Location, Size);
    }

    public class GenericFont
    {
        public int Height = 0;
        public int VShift = 0;

        public List<GenericGlyph> Glyphs = new List<GenericGlyph>();

        public Size Size = new Size(0, 0);

        //generates final atlas, assuming glyphs are sorted correctly already
        public Bitmap Generate()
        {
            var region = new Rectangle(0, 0, Size.Width, Size.Height);
            var result = new Bitmap(Size.Width, Size.Height);
            var graphics = Graphics.FromImage(result);

            foreach (var glyph in Glyphs)
            {   
                //validate range
                if (!region.Contains(glyph.Region))
                    throw new ArgumentOutOfRangeException($"Glyph outside the atlas!!!\r\n{glyph}");

                //draw glyph to atlas
                graphics.DrawImage(glyph.Texture, glyph.Location);
            }

            return result.Clone(region, PixelFormat.Format8bppIndexed);
        }
    }
}