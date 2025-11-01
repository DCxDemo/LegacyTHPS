using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyThps.Thps2.Triggers
{
    // defines basic node behaviour
    public interface INode
    {
        void Read(BinaryReader br);
        void Write(BinaryWriter bw);

        void Compile(string text);
        string Decompile();
    }
}
