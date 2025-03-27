using System.IO;
using System.Numerics;

namespace LegacyThps.QScript.Helpers
{
    public class BinaryReaderEx : BinaryReader
    {
        public BinaryReaderEx(MemoryStream stream) : base(stream)
        {
        }
        public BinaryReaderEx(FileStream stream) : base(stream)
        {
        }

        public void Skip(long x)
        {
            this.BaseStream.Position += x;
        }

        public void Jump(long x)
        {
            this.BaseStream.Position = x;
        }

        public Vector3 ReadVector3()
        {
            float X = ReadSingle();
            float Y = ReadSingle();
            float Z = ReadSingle();
            return new Vector3(X, Y, Z);
        }

        public Vector3 ReadVector2()
        {
            return new Vector3(ReadSingle(), ReadSingle(), 0);
        }

        public string ReadFixedString(int count)
        {
            string data_string = System.Text.Encoding.Default.GetString(ReadBytes(count));
            return data_string.Split('\0')[0]; //make sure only stuff before \0 is taken
        }

        public string ReadNTString()
        {
            long pos = BaseStream.Position;

            int len = 0;

            do len++;
            while (ReadByte() != 0);

            BaseStream.Position = pos;

            return ReadFixedString(len);
        }
    }
}
