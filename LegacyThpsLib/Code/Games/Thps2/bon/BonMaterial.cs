using System;
using System.IO;
using System.Drawing;
using System.ComponentModel;

namespace LegacyThps.Thps2
{
    public class BonMaterial
    {
        public string Name { get; set; }
        public Color DiffuseColor { get; set; } = Color.Gray;
        public float unk1 { get; set; } = 0.25f;
        public float unk2 { get; set; } = 0.05f;

        [Browsable(false)]
        public BonTexture Texture { get; set; }

        public BonMaterial(BinaryReader br) => Read(br);

        public static BonMaterial FromReader(BinaryReader br) => new BonMaterial(br);

        public void Read(BinaryReader br)
        {
            int size = br.ReadInt16();
            Name = new string(br.ReadChars(size));

            DiffuseColor = Color.FromArgb(br.ReadInt32());
            unk1 = br.ReadSingle();
            unk2 = br.ReadSingle();

            bool hasTexture = (br.ReadByte() == 1);

            if (hasTexture)
                Texture = BonTexture.FromReader(br);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((short)Name.Length);
            bw.Write(Name.ToCharArray());

            bw.Write(DiffuseColor.ToArgb());
            bw.Write(unk1);
            bw.Write(unk2);

            bw.Write((byte)(Texture != null ? 1 : 0));

            if (Texture != null)
                Texture.Write(bw);
        }

        public void SaveAsPng(string path)
        {
            if (Texture != null)
                Texture.SaveAsPng(path);
        }

        public void Replace(string path)
        {
            if (Texture == null)
                Texture = BonTexture.FromFile(path);
            else
                Texture.Replace(path);
        }
    }
}