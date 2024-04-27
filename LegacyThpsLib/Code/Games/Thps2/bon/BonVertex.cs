using System;
using System.Numerics;
using System.Drawing;
using System.IO;
using System.Text;
using LegacyThps.Helpers;

namespace LegacyThps.Thps2
{
    public class BonVertex
    {
        public Vector3 Position { get; set; } = new Vector3();
        public float unk1 { get; set; } = 1.0f;
        public Vector3 Normal { get; set; } = new Vector3();
        // this defines how wobbly the vertex is in the wind
        // particularly used for shirts and hair.
        public Color WobbleWeight { get; set; } = Color.Black;
        public Vector2 TexCoords { get; set; } = new Vector2();

        public BonVertex(BinaryReader br) => Read(br);

        public void Read(BinaryReader br)
        {
            Position = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            unk1 = br.ReadSingle();
            Normal = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            WobbleWeight = Color.FromArgb(br.ReadInt32());
            TexCoords = new Vector2(br.ReadSingle(), br.ReadSingle());
        }

        public static BonVertex FromReader(BinaryReader br) => new BonVertex(br);

        public void Write(BinaryWriter bw)
        {
            bw.Write(Position.X);
            bw.Write(Position.Y);
            bw.Write(Position.Z);
            bw.Write(unk1);
            bw.Write(Normal.X);
            bw.Write(Normal.Y);
            bw.Write(Normal.Z);
            bw.Write(WobbleWeight.ToArgb());
            bw.Write(TexCoords.X);
            bw.Write(TexCoords.Y);
        }

        private Vector2 negateUV = new Vector2(1, -1);

        public void ToObj(ObjWriter obj)
        {
            obj.WriteColoredVertex(Position, WobbleWeight, 0.01f);
            obj.WriteNormal(Normal);
            obj.WriteUV(TexCoords * negateUV);
        }

        public string ToObj(float scale = 1.0f)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"v {Position.X * scale} {Position.Y * scale} {Position.Z * scale} {WobbleWeight.R / 255.0} {WobbleWeight.G / 255.0} {WobbleWeight.B / 255.0}");
            sb.AppendLine($"vn {Normal.X} {Normal.Y} {Normal.Z}");
            sb.AppendLine($"vt {TexCoords.X} {-TexCoords.Y}");

            return sb.ToString();
        }
    }
}