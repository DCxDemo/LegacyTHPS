using System;
using Kaitai;
using System.IO;
using LegacyThps.Helpers;
using DDS;
using System.Drawing;

namespace bon_extract
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("bon_extract - early version");
                Console.WriteLine("Give me THPS2X XBOX BON files.");
                Console.WriteLine("2024, dcxdemo.");
                return;
            }

            foreach (var arg in args)
            {

                var bon = Thps2Bon.FromFile(arg);

                var modelname = Path.GetFileNameWithoutExtension(arg);
                var objname = Path.ChangeExtension(arg, ".obj");
                var mtlname = Path.ChangeExtension(arg, ".mtl");


                switch (bon.Version)
                {
                    case 1:

                        var data = bon.Data as Thps2Bon.BonDc;

                        var mtllib = Path.GetFileNameWithoutExtension(objname) + ".mtl";

                        using (var obj = new ObjWriter(objname) { IndexMode = ObjFaceIndexMode.Full })
                        {
                            obj.WriteMaterialLibrary(mtllib);

                            foreach (var model in data.Hier)
                                DcMeshToObj(data, model, obj);
                        }



                        using (var mtl = new MtlWriter(mtlname))
                        {
                            foreach (var mat in data.Materials)
                            {
                                mtl.WriteMaterial(mat.Name.Content.Replace(" ", "_"));

                                if (mat.Texture != null)
                                    mtl.WriteKd($"{modelname}\\{Path.GetFileNameWithoutExtension(mat.Texture.Name.Content.Replace(" ", "_"))}.png");
                                else
                                    mtl.WriteKd($"{modelname}\\no_texture.png");

                                mtl.WriteLine();
                            }
                        }

                        //extract textures
                        Directory.CreateDirectory(modelname);

                        foreach (var mat in data.Materials)
                        {
                            if (mat.Texture != null)
                            {
                                var texturename = $"{Path.GetFileNameWithoutExtension(mat.Texture.Name.Content).Replace(" ", "_")}.pvr";
                                var path = $"{modelname}\\{texturename}";

                                File.WriteAllBytes(path, mat.Texture.Data);

                                //pvr sucks
                            }
                        }



                        break;

                    case 3:
                    case 4:

                        XboxBonToObj(bon, objname);

                        break;

                    default:
                        Console.WriteLine("unknown version");
                        break;
                }

                Console.WriteLine($"{modelname} done.");
            }
        }

        public static void DcMeshToObj(Thps2Bon.BonDc data, Thps2Bon.MeshDc mesh, ObjWriter obj)
        {
            obj.WriteObject(mesh.Name.Content);

            obj.Checkpoint();

            foreach (var v in mesh.Vertices)
            {
                obj.WriteVertex(new System.Numerics.Vector3(v.Position.X, v.Position.Y, v.Position.Z), 0.01f);
                obj.WriteNormal(new System.Numerics.Vector3(v.Normal.X, v.Normal.Y, v.Normal.Z));
                obj.WriteUV(new System.Numerics.Vector2(v.Uv1.Y, -v.Uv2.X));
            }

            foreach (var split in mesh.Splits)
                DcSplitToObj(data, split, obj);

            foreach (var c in mesh.Children)
                DcMeshToObj(data, c, obj);

        }


        public static void DcSplitToObj(Thps2Bon.BonDc data, Thps2Bon.MatSplitDc split, ObjWriter obj)
        {
            // so dreamcast just stores trilist

            obj.WriteUseMaterial(data.Materials[split.MaterialIndex].Name.Content.Replace(" ", "_"));

            ushort[] array = split.Indices.ToArray();

            for (int i = 0; i < array.Length / 3; i++)
                obj.WriteFace(array[i * 3], array[ i * 3 + 1], array[i * 3 + 2]);
        }



        public static void XboxBonToObj(Thps2Bon bon, string objname)
        {
            var data = bon.Data as Thps2Bon.BonXbox;

            var mtllib = Path.GetFileNameWithoutExtension(objname) + ".mtl";

            //write obj

            using (var obj = new ObjWriter(objname) { IndexMode = ObjFaceIndexMode.Full })
            {
                obj.WriteComment("kek");

                obj.WriteMaterialLibrary(mtllib);

                foreach (var vert in data.Vertices)
                {
                    obj.WriteVertex(new System.Numerics.Vector3(vert.Position.X, vert.Position.Y, vert.Position.Z), 0.01f);
                    obj.WriteNormal(new System.Numerics.Vector3(vert.Normal.X, vert.Normal.Y, vert.Normal.Z));
                    obj.WriteUV(new System.Numerics.Vector2(vert.Uv.X, -vert.Uv.Y));
                }

                foreach (var mesh in data.Hier)
                    MeshToObj(bon, mesh, obj);
            }

            var modelname = Path.GetFileNameWithoutExtension(objname);
            var mtlname = Path.ChangeExtension(objname, ".mtl");


            using (var mtl = new MtlWriter(mtlname))
            {
                foreach (var mat in data.Materials)
                {
                    mtl.WriteMaterial(mat.Name.Content.Replace(" ", "_"));

                    if (mat.Texture != null)
                        mtl.WriteKd($"{modelname}\\{mat.Texture.Name.Content.Replace(" ", "_")}.png");
                    else
                        mtl.WriteKd($"{modelname}\\no_texture.png");

                    mtl.WriteLine();
                }
            }

            //extract textures
            Directory.CreateDirectory(modelname);

            foreach (var mat in data.Materials)
            {
                if (mat.Texture != null)
                {

                    File.WriteAllBytes($"{modelname}\\{mat.Texture.Name.Content.Replace(" ", "_")}.dds", mat.Texture.Data);

                    DDSImage dds = new DDSImage(File.ReadAllBytes($"{modelname}\\{mat.Texture.Name.Content.Replace(" ", "_")}.dds"));
                    dds.BitmapImage.Save($"{modelname}\\{mat.Texture.Name.Content.Replace(" ", "_")}.png");
                }
            }


            /*

            sb.Clear();


            foreach (var mesh in bon.Hier)
            {
                sb.AppendLine(BoneToSkel(mesh));
            }

            File.WriteAllText($"skel.obj", sb.ToString());
            */
        }

        /*
        public static string BoneToSkel(Thps2xBon.Mesh mesh)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"# {mesh.Name.Content}");
            sb.AppendLine($"v {mesh.Position.X * Scale} {mesh.Position.Y * Scale} {mesh.Position.Z * Scale}");

            foreach (var child in mesh.Children)
                sb.AppendLine(BoneToSkel(child));

            return sb.ToString();
        }
        */

        public static void MeshToObj(Thps2Bon bon, Thps2Bon.MeshXbox mesh, ObjWriter obj)
        {
            obj.WriteObject($"{mesh.Name.Content}");

            //base meshes               
            if (mesh.NumBaseSplits > 0)
            {
                obj.WriteGroup($"{mesh.Name.Content}_base");

                foreach (var mat in mesh.BaseSplits)
                    MatsplitToMesh(bon, mat, obj);
            }

            //joint meshes
            if (mesh.NumJointSplits > 0)
            {
                obj.WriteGroup($"{mesh.Name.Content}_joint");

                foreach (var mat in mesh.JointSplits)
                    MatsplitToMesh(bon, mat, obj);
            }

            //process child meshes
            foreach (var cMesh in mesh.Children)
                MeshToObj(bon, cMesh, obj);
        }

        public static void MatsplitToMesh(Thps2Bon bon, Thps2Bon.MatSplit mat, ObjWriter obj)
        {
            var data = (Thps2Bon.BonXbox)bon.Data;

            // sb.Append($"usemtl {(bon.Data as Thps2Bon.BonXbox).Materials[mat.MaterialIndex].Name.Content.Replace(" ", "_")}\r\n");

            ushort[] array = new ushort[mat.Size];
            Array.Copy(data.Indices.ToArray(), mat.Offset, array, 0, mat.Size);


            obj.WriteUseMaterial($"{data.Materials[mat.MaterialIndex].Name.Content.Replace(" ", "_")}");


            int a = 0;
            int b = 0;
            int c = 0;
            int d = 0;

            for (int pos = 0; pos < array.Length - 2; pos++)
            {
                if (pos % 2 == 0)
                {
                    a = array[pos + 0];
                    b = array[pos + 1];
                    c = array[pos + 2];

                    if (a != b && b != c)
                        obj.WriteFace(a, c, b);
                       // sb.AppendLine($"f {a + 1}/{a + 1}/{a + 1} {c + 1}/{c + 1}/{c + 1} {b + 1}/{b + 1}/{b + 1}");
                }
                else
                {
                    d = array[pos + 2];

                    if (d != b && b != c)
                        obj.WriteFace(d, b, c);
                        // sb.AppendLine($"f {d + 1}/{d + 1}/{d + 1} {b + 1}/{b + 1}/{b + 1} {c + 1}/{c + 1}/{c + 1}");
                }
            }
        }
    }
}