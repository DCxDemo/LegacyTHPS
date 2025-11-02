using System.Drawing;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading;

namespace LegacyThps.Helpers
{
    public enum ObjFaceIndexMode
    {
        Basic,
        Textured,
        Normal,
        Full
    }

    public class ObjWriter : StreamWriter
    {
        public ObjWriter(string filename) : base(filename)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("us-US");
        }

        public void WriteVertex(Vector3 position, float scale = 1f)
        {
            var pos = position * scale;

            WriteLine($"v {pos.X.ToString("0.#####")} {pos.Y.ToString("0.#####")} {pos.Z.ToString("0.#####")}");

            TotalVertices++;
        }

        public void WriteNormal(Vector3 pos)
        {
            WriteLine($"vn {pos.X.ToString("0.#####")} {pos.Y.ToString("0.#####")} {pos.Z.ToString("0.#####")}");
        }

        public void WriteUV(Vector2 uv)
        {
            WriteLine($"vt {uv.X.ToString("0.#####")} {uv.Y.ToString("0.#####")}");
        }

        public void WriteColoredVertex(Vector3 position, Color color, float scale = 1f)
        {
            var pos = position * scale;

            WriteLine($"v {pos.X.ToString("0.####")} {pos.Y.ToString("0.####")} {pos.Z.ToString("0.####")} " +
                $"{(color.R / 255.0).ToString("0.###")} {(color.G / 255.0).ToString("0.###")} {(color.B / 255.0).ToString("0.###")}");

            TotalVertices++;
        }

        public int TotalVertices = 0;
        public int LastCheckpoint = 0;

        public void Checkpoint()
        {
            LastCheckpoint = TotalVertices;
        }

        public void WriteFace(int a, int b, int c)
        {
            Write("f ");
            WriteFaceIndex(LastCheckpoint + a);
            Write(" ");
            WriteFaceIndex(LastCheckpoint + b);
            Write(" ");
            WriteFaceIndex(LastCheckpoint + c);
            WriteLine();
        }


        public void WriteStrip(ushort[] indices)
        {
            Write($"f ");

            foreach (var s in indices)
            {
                Write(-(s + 1));
                Write(" ");
            }

            WriteLine();
        }

        public void WriteObject(string name)
        {
            WriteLine($"o {name}");
        }
        public void WriteGroup(string name)
        {
            WriteLine($"g {name}");
        }

        public void WriteMaterialLibrary(string file)
        {
            WriteLine($"mtllib {file}");
        }

        public void WriteUseMaterial(string name)
        {
            WriteLine($"usemtl {name}");
        }

        public void WriteQuad(int a, int b, int c, int d)
        {
            WriteLine($"f {a + 1} {b + 1} {c + 1} {d + 1}");
        }

        public ObjFaceIndexMode IndexMode = ObjFaceIndexMode.Basic;

        public void WriteFaceIndex(int index)
        {
            switch (IndexMode)
            {
                case ObjFaceIndexMode.Textured: Write($"{index + 1}"); break;
                case ObjFaceIndexMode.Normal: Write($"{index + 1}/{index + 1}"); break;
                case ObjFaceIndexMode.Full: Write($"{index + 1}/{index + 1}/{index + 1}"); break;
                case ObjFaceIndexMode.Basic:
                default:
                    Write($"{index + 1}"); break;
            }
        }

        public void WriteComment(string comment)
        {
            WriteLine($"# {comment}");
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}