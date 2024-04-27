using System;
using System.Collections.Generic;
using System.IO;
using DDS;
using LegacyThps.Helpers;

namespace LegacyThps.Thps2
{
    /// <summary>
    /// A collection of DDS textures found in THPS2x.
    /// </summary>
    public class XboxDdx : List<XboxDdxTexture>
    {
        public XboxDdx()
        {
        }

        public XboxDdx(BinaryReader br) => Read(br);

        /// <summary>
        /// Factory method, reads DDX from file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static XboxDdx FromFile(string filename)
        {
            using (var br = new BinaryReader(File.OpenRead(filename)))
            {
                return FromReader(br);
            }
        }

        /// <summary>
        /// Factory method, reads DDX from a reader.
        /// </summary>
        /// <param name="br"></param>
        /// <returns></returns>
        public static XboxDdx FromReader(BinaryReader br) => new XboxDdx(br);

        /// <summary>
        /// Reads DDX data from reader.
        /// </summary>
        /// <param name="br"></param>
        public void Read(BinaryReader br)
        {
            int version = br.ReadInt32();

            if (version != 0)
                Console.WriteLine($"Warning, version check failed! expected 0, got {version}");

            int size = br.ReadInt32();

            if (size != br.BaseStream.Length)
                Console.WriteLine($"Warning, file size check failed! expected {br.BaseStream.Length}, got {size}");

            int dataOffset = br.ReadInt32();
            int numTextures = br.ReadInt32();

            for (int i = 0; i < numTextures; i++)
                this.Add(new XboxDdxTexture(br));

            foreach (var texture in this)
            {
                br.BaseStream.Position = dataOffset + texture.Offset;
                texture.Data = br.ReadBytes(texture.Size);
                texture.Image = new DDSImage(texture.Data);
            }
        }

        /// <summary>
        /// Extracts all textures from DDX and creates a list.txt.
        /// </summary>
        /// <param name="path"></param>
        public void ExtractAll(string path)
        {
            DebugLog.WriteLine($"Extracting DDX to {path}");

            var lines = new string[this.Count];

            for (int i = 0; i < this.Count; i++)
            {
                this[i].Save(path);
                lines[i] = this[i].Name;
            }

            File.WriteAllLines(Path.Combine(path, "list.txt"), lines);
        }

        /// <summary>
        /// Creates DDX file and populates it with all DDS textures found in a folder. Uses list.txt if present. 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XboxDdx FromFolder(string path)
        {
            var ddx = new XboxDdx();

            var listpath = Path.Combine(path, "list.txt");

            if (File.Exists(listpath))
            {
                var lines = File.ReadAllLines(listpath);

                foreach (var line in lines)
                {
                    if (line == "") continue;

                    ddx.AddFile(Path.Combine(path, $"{line}.dds"));
                }
            }
            else
            {
                Console.Write("Warning, list.txt not found.");

                foreach (var file in Directory.GetFiles(path, "*.dds"))
                    ddx.AddFile(file);
            }

            return ddx;
        }

        /// <summary>
        /// Add DDS texture to the collection.
        /// </summary>
        /// <param name="path"></param>
        public void AddFile(string path)
        {
            if (File.Exists(path))
                this.Add(XboxDdxTexture.FromFile(path));
        }

        /// <summary>
        /// Writes DDX data to writer.
        /// </summary>
        /// <param name="bw"></param>
        public void Write(BinaryWriter bw)
        {
            int totalSize = PrepareData();

            int textures = Pad(16 + this.Count * (256 + 8), 0x1000);

            bw.Write((int)0); //always 0
            bw.Write((int)totalSize);
            bw.Write((int)textures);
            bw.Write((int)this.Count);

            foreach (var texture in this)
            {
                bw.Write((int)texture.Offset);
                bw.Write((int)texture.Size);

                byte[] test = new byte[256];

                Array.Copy(System.Text.Encoding.ASCII.GetBytes(texture.Name), 0, test, 0, System.Text.Encoding.ASCII.GetBytes(texture.Name).Length);

                bw.Write(test);
            }

            foreach (var texture in this)
            {
                bw.BaseStream.Position = texture.Offset + textures;
                bw.Write(texture.Data);
            }

            bw.BaseStream.SetLength(bw.BaseStream.Position);
        }

        /// <summary>
        /// Saves DDX to disk. Creates backup too.
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            var backup = filename + ".bak";

            if (File.Exists(filename) && !File.Exists(backup))
                File.Copy(filename, backup);

            using (var bw = new BinaryWriter(File.OpenWrite(filename)))
            {
                Write(bw);
            }
        }

        // move somewhere
        private int Pad(int value, int margin)
        {
            return (value + margin) & ~(margin-1);
        }

        /// <summary>
        /// Recalculates internal pointer fields, must be called before writing.
        /// </summary>
        /// <returns></returns>
        private int PrepareData()
        {
            //works fine without padding too, someone should test it on real hardware
            var pTextures = /*Pad(*/ 16 + this.Count * (256 + 8) /*, 0x1000)*/ ;

            var pCursor = pTextures;

            foreach (var texture in this)
            {
                texture.Size = texture.Data.Length;
                texture.Offset = pCursor;

                pCursor += texture.Size;
            }

            return pTextures + pCursor;
        }

        // =================================================================

        public static void Extract(string input, string output = null)
        {
            try
            {
                var ddxname = Path.GetFileNameWithoutExtension(input);

                if (output == null)
                    output = Path.Combine(Path.GetDirectoryName(input));

                output = Path.Combine(output, ddxname);

                if (!Directory.Exists(output))
                    Directory.CreateDirectory(output);

                DebugLog.WriteLine($"input: {input}");
                DebugLog.WriteLine($"output: {output}");

                var ddx = XboxDdx.FromFile(input);
                ddx.ExtractAll(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }

        public static void Build(string input, string output = null)
        {
            try
            {
                var ddxname = Path.GetDirectoryName(input);

                if (output == null)
                    output = Path.Combine(Directory.GetParent(input).FullName, $"{ddxname}.ddx");

                DebugLog.WriteLine($"input: {input}");
                DebugLog.WriteLine($"output: {output}");

                var ddx = XboxDdx.FromFolder(input);
                ddx.Save(output);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
            }
        }
    }
}