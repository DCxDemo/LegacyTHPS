using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace LegacyThps.Fonts
{
    public class FastBMP
    {
        Bitmap bmp;

        public FastBMP(Bitmap b)
        {
            bmp = b;
        }

        public Bitmap GetBMP()
        {
            return bmp;
        }

        public FastBMP(byte[] data, int width, int height, PixelFormat pf)
        {
            bmp = new Bitmap(width, height, pf);
            CopyMap(bmp, data);
        }

        private void CopyMap(Bitmap b, byte[] data)
        {
            Rectangle rect = new Rectangle(0, 0, b.Width, b.Height);
            BitmapData bmpData = b.LockBits(rect, ImageLockMode.ReadWrite, b.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * b.Height;
            byte[] rgbValues = new byte[bytes];
            rgbValues = data;
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            b.UnlockBits(bmpData);
        }

        public bool isLineTransparent(int n)
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            int[] rgbValues = new int[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes / 4);

            for (int i = 0; i < bmp.Height; i++)
            {
                Color g = Color.FromArgb(rgbValues[i * bmp.Width + n]);
                if (g.A != 0) { bmp.UnlockBits(bmpData); return false; }
            }

            bmp.UnlockBits(bmpData);
            return true;
        }


        public int GetTopSpaceWidth()
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            int[] rgbValues = new int[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes / 4);

            bool foundpixel = false;
            int topspace = -1;

            do
            {
                topspace++;
                for (int i = 0; i < bmp.Width; i++)
                {
                    Color g = Color.FromArgb(rgbValues[topspace * bmp.Width + i]);
                    if (g.A != 0) foundpixel = true;
                }

            }
            while (!foundpixel);

            bmp.UnlockBits(bmpData);
            return topspace;
        }



        public int GetBottomSpaceWidth()
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            int[] rgbValues = new int[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes / 4);

            bool foundpixel = false;
            int bottomspace = -1;

            do
            {
                bottomspace++;
                for (int i = bmp.Width; i > 0; i--)
                {
                    Color g = Color.FromArgb(rgbValues[(bmp.Height - bottomspace) * bmp.Width + i]);
                    if (g.A != 0) foundpixel = true;
                }

            }
            while (!foundpixel);

            bmp.UnlockBits(bmpData);
            return bottomspace;
        }


        public List<int> palette = new List<int>();

        public int GetNumCols()
        {
            palette.Clear();

            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            int[] rgbValues = new int[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes / 4);


            for (int i = 0; i < rgbValues.Length; i++)
            {
                if (!palette.Contains(rgbValues[i]))
                {
                    palette.Add(rgbValues[i]);
                }
            }

            bmp.UnlockBits(bmpData);

            return palette.Count;
        }

        public void THUGifyPalette()
        {
            for (int i = 0; i < palette.Count; i++)
            {
                byte[] pal = BitConverter.GetBytes(palette[i]);

                byte x = pal[0];

                pal[0] = pal[2];
                pal[2] = x;

                palette[i] = BitConverter.ToInt32(pal, 0);

            }
        }

        public byte[] GetRawPalette()
        {
            byte[] result = new byte[256 * 4];
            Buffer.BlockCopy(palette.ToArray(), 0, result, 0, palette.Count * 4);

            return result;
        }


        public byte[] GetRaw()
        {
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
            IntPtr ptr = bmpData.Scan0;
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            int[] rgbValues = new int[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes / 4);


            byte[] raw = new byte[bmp.Width * bmp.Height];

            for (int i = 0; i < raw.Length; i++)
            {
                if (palette.Contains(rgbValues[i]))
                {
                    raw[i] = (byte)palette.IndexOf(rgbValues[i]);
                }
                else
                {
                    Console.WriteLine("PANIC: no color in palette" + palette.Count);
                }
            }

            bmp.UnlockBits(bmpData);

            return raw;
        }

    }
}
