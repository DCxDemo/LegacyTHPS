using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FNTEdit
{
    class FNTNG
    {
        byte[] fontdata;
        MemoryStream ms;
        BinaryReader br;

        public int headerSize;
        public int letterSize;
        public int paramzSize;
        public int bitmapSize;
        public int paletteSize;
        public int paramzOffset;
        public int bitmapOffset;
        public int paletteOffset;
        public int infoOffset;

        public FNTNG(string s)
        {
            fontdata = File.ReadAllBytes(s);
            ms = new MemoryStream(fontdata);
            br = new BinaryReader(ms);

            headerSize = 16;
            letterSize = GetLetterCount() * 4;
            paramzSize = 16;
            bitmapSize = GetWidth() * GetHeight();
            paletteSize = 4 * 256;

            paramzOffset = headerSize + letterSize;
            bitmapOffset = paramzOffset + paramzSize;
            paletteOffset = bitmapOffset + bitmapSize;
            infoOffset = paletteOffset + paletteSize;
        }

        public void Jump(int x) { br.BaseStream.Position = x; } 

        public int GetSize() { Jump(0*4); return br.ReadInt32(); }
        public int GetLetterCount() { Jump(1*4); return br.ReadInt32(); }
        public int GetFontHeight() { Jump(2 * 4); return br.ReadInt32(); }
        public int GetVShift() { Jump(3 * 4); return br.ReadInt32(); }

        public int GetLetterShift(int x) { Jump(headerSize + x * 4); return br.ReadInt16(); }
        public int GetLetterCode(int x) { Jump(headerSize + x * 4 + 2); return br.ReadInt16(); }

        public int GetSize2() { Jump(paramzOffset); return br.ReadInt32(); }
        public int GetWidth() { Jump(paramzOffset + 4); return br.ReadInt16(); }
        public int GetHeight() { Jump(paramzOffset + 6); return br.ReadInt16(); }
        public int GetBPP() { Jump(paramzOffset + 8); return br.ReadInt16(); }

        public byte[] GetBitmap() { Jump(bitmapOffset); return br.ReadBytes(bitmapSize); }
        public byte[] GetPalette() { Jump(paletteOffset); return br.ReadBytes(paletteSize); }

        public int GetX(int x) { Jump(infoOffset + 8 * x); return br.ReadInt16(); }
        public int GetY(int x) { Jump(infoOffset + 8 * x + 2); return br.ReadInt16(); }
        public int GetW(int x) { Jump(infoOffset + 8 * x + 4); return br.ReadInt16(); }
        public int GetH(int x) { Jump(infoOffset + 8 * x + 6); return br.ReadInt16(); }

        public int GetCharIndex(char c)
        {
            for (int i = 0; i < GetLetterCount(); i++)
                if (c == GetLetterCode(i)) return i;

            return -1;
        }
    }
}
