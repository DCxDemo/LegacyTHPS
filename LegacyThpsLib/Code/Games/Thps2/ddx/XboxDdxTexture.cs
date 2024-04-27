using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DDS;
using LegacyThps.Helpers;
using System.Diagnostics;

namespace LegacyThps.Thps2
{
    public class XboxDdxTexture
    {
        public int Offset = 0; //a pointer relative to pTextures
        public int Size = 0; 
        public string Name; // 256 bytes
        public byte[] Data;
        public DDSImage Image;

        public XboxDdxTexture()
        {
        }

        public XboxDdxTexture(BinaryReader br) => Read(br);

        public static XboxDdxTexture FromReader(BinaryReader br) => new XboxDdxTexture(br);

        /// <summary>
        /// Factory method, reads DDS texture from disk and creates DDX texture entry.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static XboxDdxTexture FromFile(string filename)
        {
            var tex = new XboxDdxTexture();

            tex.Name = Path.GetFileName(filename);
            tex.Data = File.ReadAllBytes(filename);
            tex.Size = tex.Data.Length;
            tex.Image = new DDSImage(tex.Data);

            return tex;
        }

        /// <summary>
        /// Reads texture struct from reader.
        /// </summary>
        /// <param name="br"></param>
        public void Read(BinaryReader br)
        {
            Offset = br.ReadInt32();
            Size = br.ReadInt32();
            Name = Path.GetFileNameWithoutExtension(new string(br.ReadChars(256)).Split('\0')[0].Replace(" ", "_"));
        }

        /// <summary>
        /// Saves DDS and PNG files to disk.
        /// </summary>
        /// <param name="path"></param>
        public void Save(string path)
        {
            DebugLog.Write($"Writing {Name}... ");

            Image.BitmapImage.Save(Path.Combine(path, $"{Name}.png"));
            File.WriteAllBytes(Path.Combine(path, $"{Name}.dds"), Data);

            DebugLog.WriteLine("done.");
        }
    }
}