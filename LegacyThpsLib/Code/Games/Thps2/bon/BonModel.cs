using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using LegacyThps.Helpers;

namespace LegacyThps.Thps2
{
    public class BonModel
    {
        // known versions are 1 (dreamcast), 3 and 4 (xbox)
        public int Version { get; set; }
        public List<BonMaterial> Materials { get; set; } = new List<BonMaterial>();
        public List<BonVertex> Vertices { get; set; } = new List<BonVertex>();
        public List<short> Indices { get; set; } = new List<short>();
        public List<BonMesh> RootMeshes { get; set; } = new List<BonMesh>();

        int numUnk { get; set; } = 0;

        public BonModel(BinaryReader br) => Read(br);

        public static BonModel FromFile(string filename)
        {
            using (var br = new BinaryReader(File.OpenRead(filename)))
            {
                return FromReader(br);
            }
        }

        public static BonModel FromReader(BinaryReader br) => new BonModel(br);

        public void Read(BinaryReader br)
        {
            var magic = new string(br.ReadChars(4));

            if (magic != "Bon\0")
                throw new Exception("not a BON file.");

            Version = br.ReadInt32();

            int numMats = 0;

            switch (Version)
            {
                case 3: numMats = br.ReadInt16(); break;
                case 4: numMats = br.ReadInt32(); break;
            }

            Materials.Clear();

            for (int i = 0; i < numMats; i++)
                Materials.Add(BonMaterial.FromReader(br));

            int numVertices = 0;

            switch (Version)
            {
                case 3: numVertices = br.ReadInt16(); break;
                case 4: numVertices = br.ReadInt32(); break;
            }

            int numUnk = 0;

            switch (Version)
            {
                case 3: numUnk = br.ReadInt16(); break;
                case 4: numUnk = br.ReadInt32(); break;
            }

            for (int i = 0; i < numVertices; i++)
                Vertices.Add(BonVertex.FromReader(br));

            int numIndices = 0;

            switch (Version)
            {
                case 3: numIndices = br.ReadInt16(); break;
                case 4: numIndices = br.ReadInt32(); break;
            }

            for (int i = 0; i < numIndices; i++)
                Indices.Add(br.ReadInt16());

            int numMeshes = 0;

            switch (Version)
            {
                case 3: numMeshes = br.ReadInt16(); break;
                case 4: numMeshes = br.ReadInt32(); break;
            }

            Console.WriteLine(numMeshes);

            for (int i = 0; i < numMeshes; i++)
                RootMeshes.Add(BonMesh.FromReader(br));
        }

        public void ExtractTextures(string path)
        { 
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var mat in Materials)
            {
                mat.SaveAsPng(path);
            }
        }

        public void ReplaceTextures(string path)
        {
            foreach (var mat in Materials)
            {
                Console.Write($"Material: {mat.Name}... ");

                if (mat.Texture != null)
                {
                    var file = Path.Combine(path, $"{mat.Texture.Name}.dds");

                    if (File.Exists(file))
                    {
                        mat.Texture.Data = File.ReadAllBytes(file);
                        Console.WriteLine("found and replaced!");
                    }
                }
            }
        }

        /// <summary>
        /// Saves BON model to disk.
        /// </summary>
        /// <param name="filename"></param>
        public void Save(string filename)
        {
            //first make a backup
            if (File.Exists(filename))
            {
                var backupPath = $"{filename}.bkp";

                if (!File.Exists(backupPath))
                    File.Copy(filename, backupPath);
            }

            using (var bw = new BinaryWriter(File.OpenWrite(filename)))
            {
                Write(bw);

                //truncate the rest of file in case we write to an existing one
                bw.BaseStream.SetLength(bw.BaseStream.Position);
            }
        }

        /// <summary>
        /// Writes BON model to a BinaryWriter.
        /// </summary>
        /// <param name="bw"></param>
        public void Write(BinaryWriter bw)
        {
            bw.Write("Bon\0".ToCharArray());
            bw.Write(Version);
            bw.Write(Materials.Count);

            foreach (var mat in Materials)
                mat.Write(bw);

            bw.Write(Vertices.Count);
            bw.Write(numUnk);

            foreach (var vert in Vertices)
                vert.Write(bw);

            bw.Write(Indices.Count);

            foreach (var i in Indices)
                bw.Write(i);

            bw.Write(RootMeshes.Count);

            foreach (var mesh in RootMeshes)
                mesh.Write(bw);
        }

        public void ToObj(string filename)
        {
            var obj = new ObjWriter(filename);

            //obj.WriteComment($"# {Name}");
            obj.WriteComment($"# im a thps2x BON model!");
            obj.WriteComment("# tool by dcxdemo\r\n");

            //obj.WriteComment($"mtllib {modelname}.mtl");
            obj.WriteComment("obj file");

            foreach (var vertex in Vertices)
                vertex.ToObj(obj);
        }
    }
}