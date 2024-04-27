using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using LegacyThps.Fonts;

namespace ThpsFontEd
{
    public static class GraphicsHelper
    {
        public static SizeF MeasureString(string s, Font font)
        {
            SizeF result;
            using (var image = new Bitmap(1, 1))
            {
                using (var g = Graphics.FromImage(image))
                {
                    result = g.MeasureString(s, font);
                }
            }

            return result;
        }
    }

    public class BitmapTrueType
    {
        public static FNT Create(string charset, Font font)
        {
            var result = new FNT();

            result.Atlas = new Bitmap(512, 512);

            var graph = Graphics.FromImage(result.Atlas);

            graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            int border = 2;
            var cursor = new Point(border, border);
            int maxHeightThisLine = 0;

            foreach (char c in charset)
            {
                var size = GraphicsHelper.MeasureString(c.ToString(), font);

                if (cursor.X + size.Width + border >= result.Atlas.Width)
                {
                    cursor.Y += maxHeightThisLine + border;
                    cursor.X = border;

                    maxHeightThisLine = 0;
                }

                graph.DrawString(c.ToString(), font, Brushes.Black, cursor);

                var glyph = new Glyph()
                {
                    Value = c,
                    Region = new Rectangle(cursor.X, cursor.Y, (int)size.Width, (int)size.Height),
                    VShift = 0
                };

                result.Glyphs.Add(glyph);

                cursor.X += (int)(size.Width + border);

                if (size.Height > maxHeightThisLine) maxHeightThisLine = (int)size.Height;
            }

            result.FontHeight = result.Glyphs[0].Region.Height;
            result.FontVShift = 0;

            result.Recut();

            return result;
        }
    }
}