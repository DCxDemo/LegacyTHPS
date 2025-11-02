using System.IO;

namespace LegacyThps.Helpers
{

    public class MtlWriter : StreamWriter
    {
        public MtlWriter(string filename) : base(filename)
        {
        }

        public void WriteMaterial(string name)
        {
            WriteLine($"newmtl {name}");
        }

        public void WriteKd(string path)
        {
            WriteLine($"map_Kd {path}");
        }
    }
}