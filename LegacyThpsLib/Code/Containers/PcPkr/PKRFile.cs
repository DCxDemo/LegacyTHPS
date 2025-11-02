using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LegacyThps;
using LegacyThps.Shared;

namespace LegacyThps.Containers
{
    public class PKRFile
    {
        public string name = "empty_file";
        public uint checksum = 0;
        public int offset = 0;
        public int size = 0;
        public int fullsize = 0;
        public byte[] data;
        public bool isCompressed = false;

        public PKRFile()
        {
        }

        public PKRFile(string filename)
        {
            name = Path.GetFileName(filename);
            data = File.ReadAllBytes(filename);
            size = data.Length;
            fullsize = size;
        }

        public PKRFile(BinaryReader br, PkrVersion pkr)
        {
            name = Encoding.ASCII.GetString(br.ReadBytes(32)).Split('\0')[0];

            if (pkr == PkrVersion.PKR3)
                checksum = br.ReadUInt32(); //checksum???

            int compression = br.ReadInt32(); //compression type, 0xFEFFFFFF for raw data
            offset = br.ReadInt32();
            size = br.ReadInt32();      // actual size
            fullsize = br.ReadInt32(); // decompressed size

            if (size != fullsize) isCompressed = true;
        }

        public void GetData(BinaryReader br)
        {
           // long x = br.BaseStream.Position;

            br.BaseStream.Position = offset;
            data = br.ReadBytes(size);

          //  br.BaseStream.Position = x;

            if (isCompressed)
            {
                data = Decompress();
                size = data.Length;
                fullsize = size;
            }
        }


        public static float ZipThreshold = 70f;

        string[] exclude = new string[] { ".bik", ".wav", ".bmp", ".fnt" };

        public void TryCompress()
        {
            if (isCompressed) return;
            // we dont want to compress small files
            if (data.Length < 16 * 1024) return;

            if (exclude.Contains(Path.GetExtension(name).ToLower())) return;

            var comp = new MemoryStream();
            var zip = new GZipStream(comp, CompressionMode.Compress, true);

            var final = new MemoryStream();

            zip.Write(data, 0, data.Length);

            if ((float)comp.Length / data.Length * 100f <= ZipThreshold)
            {
                //MessageBox.Show($"compressed {name} because {comp.Length} < {data.Length}, {(float)comp.Length / data.Length * 100f}%");

                // remember sizes
                size = (int)comp.Length;
                fullsize = data.Length;
                data = comp.ToArray();
                isCompressed = true;
            }
        }

        public byte[] Decompress()
        {
            if (isCompressed)
            {
                var archive = new GZipStream(new MemoryStream(data), CompressionMode.Decompress, true);

                var decomp = new MemoryStream();
                archive.CopyTo(decomp);

                return decomp.ToArray();
            }
            else
            {
                return data;
            }
        }

        public void Save(string filename)
        {
            File.WriteAllBytes(filename, Decompress());
        }

        public void Write(BinaryWriter bw, PkrVersion pkr)
        {
            char[] chars = new char[32];

            if (name.Length > 32)
                MessageBox.Show("Name too long: " + name);

            Array.Copy(name.ToCharArray(), chars, name.Length);

            bw.Write(chars);
           
            if (pkr == PkrVersion.PKR3)
                bw.Write(Checksum.crc32b(name)); // checksum? does it matter?

            if (isCompressed)
                bw.Write((int)2);
            else
                bw.Write(0xFFFFFFFE);

            bw.Write(offset);
            bw.Write(size);      // actual size
            bw.Write(fullsize); // decompressed size
        }

        public override string ToString()
        {
            return $"{name} ({size}) {(isCompressed ? "[ZIP]" : "")}";
        }
    }
}