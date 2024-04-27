using System;

namespace LegacyThps.Fonts
{
    public class BMP32Builder
    {
        public int width;
        public int height;
        public byte[] data;

        public int ind = 0;

        public BMP32Builder(int w, int h)
        {
            width = w;
            height = h;
            data = new byte[w * h * 4];
        }

        public void PlaceColor(byte[] col)
        {
            Array.Copy(col, 0, data, ind * 4, 4);
            ind++;
        }
    }
}