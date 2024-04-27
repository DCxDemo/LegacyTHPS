using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace LegacyThps.Thps2
{
    public struct BonMatSplit
    {
        public short MaterialIndex;
        public short Offset;
        public short Size;
    }

    public class BonMesh
    {
        public string Name { get; set; }
        public float[] Matrix1 { get; set; } = new float[9];
        public Vector3 Position { get; set; } = new Vector3();
        public List<BonMesh> Children { get; set; } = new List<BonMesh>();
        public float[] Matrix2 { get; set; } = new float[9];

        public List<BonMatSplit> BaseSplits { get; set; } = new List<BonMatSplit>();
        public List<BonMatSplit> JointSplits { get; set; } = new List<BonMatSplit>();

        public BonMesh(BinaryReader br) => Read(br);

        public static BonMesh FromReader(BinaryReader br) => new BonMesh(br);

        public void Read(BinaryReader br)
        {
            byte magic = br.ReadByte();

            if (magic != 0xB1)
                throw new Exception("not a mesh!");

            int size = br.ReadInt16();
            Name = new string(br.ReadChars(size));

            Console.WriteLine(Name + " " + GuessBone().ToString());

            for (int i = 0; i < 9; i++)
                Matrix1[i] = br.ReadSingle();

            Position = new Vector3(
                br.ReadSingle(),
                br.ReadSingle(),
                br.ReadSingle()
                );

            int numChildren = br.ReadInt16();

            for (int i = 0; i < numChildren; i++)
                Children.Add(BonMesh.FromReader(br));

            for (int i = 0; i < 9; i++)
                Matrix2[i] = br.ReadSingle();

            int numBaseSplits = br.ReadInt16();

            for (int i = 0; i < numBaseSplits; i++)
                BaseSplits.Add(new BonMatSplit { MaterialIndex = br.ReadInt16(), Offset = br.ReadInt16(), Size = br.ReadInt16() });

            int numJointSplits = br.ReadInt16();

            for (int i = 0; i < numJointSplits; i++)
                JointSplits.Add(new BonMatSplit { MaterialIndex = br.ReadInt16(), Offset = br.ReadInt16(), Size = br.ReadInt16() });
        }


        public void Write(BinaryWriter bw)
        {
            //write stuff
            bw.Write((byte)0xB1);
            bw.Write((short)Name.Length);
            bw.Write(Name.ToCharArray());

            foreach (var f in Matrix1)
                bw.Write(f);

            bw.Write(Position.X);
            bw.Write(Position.Y);
            bw.Write(Position.Z);

            bw.Write((short)Children.Count);

            foreach (var child in Children)
                child.Write(bw);

            foreach (var f in Matrix2)
                bw.Write(f);

            bw.Write((short)BaseSplits.Count);
            foreach (var split in BaseSplits)
            {
                bw.Write(split.MaterialIndex);
                bw.Write(split.Offset);
                bw.Write(split.Size);
            }

            bw.Write((short)JointSplits.Count);
            foreach (var split in JointSplits)
            {
                bw.Write(split.MaterialIndex);
                bw.Write(split.Offset);
                bw.Write(split.Size);
            }
        }

        public Thps2Bone GuessBone()
        {
            for (var i = Thps2Bone.Pelvis; i < Thps2Bone.TotalBones; i++)
            {
                if (Name.Replace("_", "").ToLower().Contains(i.ToString().ToLower()))
                    return i;
            }

            return Thps2Bone.None;
        }
    }
}