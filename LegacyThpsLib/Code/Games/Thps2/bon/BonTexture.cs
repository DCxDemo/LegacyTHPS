using System;
using System.IO;
using System.Drawing;
using System.Threading;
using DDS;
using System.ComponentModel;

namespace LegacyThps.Thps2
{
    public enum TextureAddressMode
    {
        Clamp = 0,
        Wrap = 1,
        Mirror = 2
    }

    public class BonTexture
    {
        public string Name { get; set; }

        // doesnt seem to do anything, always 1 in game files
        public byte unkFlag { get; set; } = 1;

        // this affects negative UV outside the texture space
        // used to avoid tshirt mirroring, default is wrap
        public TextureAddressMode AddressU { get; set; } = TextureAddressMode.Wrap;
        public TextureAddressMode AddressV { get; set; } = TextureAddressMode.Wrap;

        [Browsable(false)]
        public int DataSize => (Data != null ? Data.Length : 0);

        [Browsable(false)]
        public byte[] Data { get; set; }

        public Bitmap GetBitmap()
        {
            var dds = new DDSImage(Data);
            return dds.BitmapImage;
        }

        public BonTexture() { }

        public BonTexture(BinaryReader br) => Read(br);

        public static BonTexture FromReader(BinaryReader br) => new BonTexture(br);

        public static BonTexture FromFile(string filename) => new BonTexture() { 
            Name = Path.GetFileNameWithoutExtension(filename), 
            Data = File.ReadAllBytes(filename) 
        };

        public void Read(BinaryReader br)
        {
            int size = br.ReadInt16();
            Name = new string(br.ReadChars(size));
            unkFlag = br.ReadByte();
            AddressU = (TextureAddressMode)br.ReadByte();
            AddressV = (TextureAddressMode)br.ReadByte();
            size = br.ReadInt32();
            Data = br.ReadBytes(size);
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write((short)Name.Length);
            bw.Write(Name.ToCharArray());
            bw.Write(unkFlag);
            bw.Write((byte)AddressU);
            bw.Write((byte)AddressV);
            bw.Write(DataSize);
            if (Data.Length > 0)
                bw.Write(Data);
        }

        public void SaveAsPng(string filename)
        {
            var png = Path.Combine(filename, $"{Name}.png");
            GetBitmap().Save(png);

            var dds = Path.Combine(filename, $"{Name}.dds");
            File.WriteAllBytes(dds, Data);
        }

        public void Replace(string name)
        {
            if (File.Exists(name) && Path.GetExtension(name).ToLower() == ".dds")
                Data = File.ReadAllBytes(name);
        }
    }
}