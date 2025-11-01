using System;
using System.IO;
using System.Numerics;

namespace LegacyThps.Shared
{
    public static class BinaryReaderExtensions
    {
        public static string ReadTrgString(this BinaryReader br)
        {
            string result = "";

            byte buf = br.ReadByte();
            while (buf != '\0')
            {
                result += Convert.ToChar(buf);
                buf = br.ReadByte();
            }

            br.Pad(2);  // original code loads shorts

            return result;
        }

        public static void WriteTrgString(this BinaryWriter bw, string text)
        {
            bw.Write(text.ToCharArray());
            bw.Write((byte)0);
            bw.Pad(2);
        }

        public static bool CanRead(this BinaryReader br)
        {
            return br.BaseStream.Position < br.BaseStream.Length;
        }

        public static void Pad(this BinaryReader br, uint pad = 4)
        {
            var mod = br.BaseStream.Position % pad;

            if (mod != 0)
                br.BaseStream.Position += pad - mod;
        }

        public static void Pad(this BinaryWriter bw, uint pad = 4)
        {
            var mod = bw.BaseStream.Position % pad;

            if (mod != 0)
                bw.BaseStream.Position += pad - mod;
        }

        public static Vector3 ReadInt32Vector3(this BinaryReader br)
        {
            var result = new Vector3(
                br.ReadInt32(),
                br.ReadInt32(),
                br.ReadInt32()
            );

            #if DEBUG
                Console.WriteLine( result );
            #endif

            return result;
        }

        public static Vector3 ReadInt16Vector3(this BinaryReader br)
        {
            var result = new Vector3(
                br.ReadInt16(),
                br.ReadInt16(),
                br.ReadInt16()
            );

            #if DEBUG
                Console.WriteLine(result);
            #endif

            return result;
        }
    }
}