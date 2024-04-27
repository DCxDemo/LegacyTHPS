using System.Collections.Generic;
using System.IO;

namespace LegacyThps.Containers
{
    public class Kat
    {
        public List<KatEntry> Entries = new List<KatEntry>();

        public Kat(BinaryReader br, bool xsb)
        {
            Read(br, xsb);
        }

        public void Read(BinaryReader br, bool xsb)
        {
            int numEntries = br.ReadInt32();

            for (int i = 0; i < numEntries; i++)
                Entries.Add(!xsb ? KatEntry.FromReader(br) : XsbEntry.FromReader(br) as KatEntry);

            for (int i = 0; i < numEntries; i++)
                Entries[i].GetSampleData(br);
        }

        public static Kat FromFile(string filename)
        {
            bool xsb = false;

            if (Path.GetExtension(filename).ToLower() == ".xsb")
                xsb = true;

            using (BinaryReader br = new BinaryReader(File.OpenRead(filename)))
            {
                return new Kat(br, xsb);
            }
        }

        public void Extract(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var entry in Entries)
            {
                entry.ToWav(path);
                //File.AppendAllText(Path.Combine(path, "report.log"), entry.ToString());
            }
        }
    }
}