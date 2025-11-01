using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegacyThps.Shared;

namespace LegacyThps.Thps2.Triggers
{

    public enum RailSound
    {
        Concrete = 0,
        Wood = 1,
        Metal = 2
    }

    public class RailDefNode : GenericNode3D
    {
        public ushort Flags;

        public RailSound RailSound => (RailSound)(Flags & 3);

        public RailDefNode(TriggerFile context) : base(context)
        {
            Context = context;
        }

        public override void Read(BinaryReader br)
        {
           // foreach (int i in ReadLinks(br))
           //     Links.Add(Context[i]);

            br.Pad();

            Position = br.ReadInt32Vector3();
            // Angles = br.ReadInt16Vector3();

            Flags = br.ReadUInt16();
        }

        public override string Decompile()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"@{Name}");

            if (Links.Count > 0)
            {
                sb.AppendLine($"\t-<Links>");

                foreach (var link in Links)
                    sb.AppendLine($"\t\t#{link.Name}");
            }

            sb.AppendLine($"\t-vec3: {Position.X} {Position.Y} {Position.Z}");
            // sb.AppendLine($"\t-vec3: {Angles.X} {Angles.Y} {Angles.Z}");
            sb.AppendLine($"\t-word: {Flags}");

            return sb.ToString();
        }
    }

    public class RailPointNode : RailDefNode
    {
        public RailPointNode(TriggerFile context) : base(context)
        {
            Context = context;
        }
    }
}
