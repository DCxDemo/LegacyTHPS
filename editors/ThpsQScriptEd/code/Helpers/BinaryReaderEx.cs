using System.IO;

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

        public Vector3f ReadVector3()
        {
            float X = ReadSingle();
            float Y = ReadSingle();
            float Z = ReadSingle();
            return new Vector3f(X, Y, Z);
        }

        public Vector3f ReadVector2()
        {
            return new Vector3f(ReadSingle(), ReadSingle());
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

            /* rewrote using readfixedstring
            List<byte> str = new List<byte>();

            byte x;

            do
            {
                x = ReadByte();
                if (x != 0) str.Add(x);
            }
            while (x != 0);

            return UTF8Encoding.UTF8.GetString(str.ToArray());
             * 
             * */
        }
    }
}
