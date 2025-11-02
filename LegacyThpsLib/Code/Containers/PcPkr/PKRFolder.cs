using System;
using System.Collections.Generic;
using System.IO;

namespace LegacyThps.Containers
{
    public class PKRFolder
    {
        public static int SizeOf = 40;

        public string name;
        public uint offset; // files to skip
        public uint count;

        public List<PKRFile> Files = new List<PKRFile>();

        public PKRFolder()
        {
        }

        public PKRFolder(BinaryReader br)
        {
            // folder name ends with a '/'
            name = new string(br.ReadChars(32)).Split('\0')[0];
            offset = br.ReadUInt32();
            count = br.ReadUInt32();
        }

        public void Write(BinaryWriter bw)
        {
            char[] chars = new char[32];
            Array.Copy(name.ToCharArray(), chars, name.Length);

            bw.Write(chars);
            bw.Write(offset);
            bw.Write(count);
        }

        public override string ToString() => name;
    }
}