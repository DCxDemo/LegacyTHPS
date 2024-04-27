// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using System.Collections.Generic;

namespace Kaitai
{

    /// <summary>
    /// Describes skater model in BON format found in THPS2 ports by Treyarch:
    ///   * THPS2 (Dreamcast)
    ///   * THPS2x (Original Xbox)
    /// 
    /// Version 1 found on Dreamcast, uses PVR textures, stores separate buffers per mesh
    /// Versions 3 and 4 found on Xbox, uses DDS textures, stores data in global buffers
    /// 
    /// File mapped by DCxDemo*.
    /// </summary>
    /// <remarks>
    /// Reference: <a href="https://github.com/DCxDemo/LegacyThps/blob/master/formats/thps2_bon.ksy">Source</a>
    /// </remarks>
    public partial class Thps2Bon : KaitaiStruct
    {
        public static Thps2Bon FromFile(string fileName)
        {
            return new Thps2Bon(new KaitaiStream(fileName));
        }

        public Thps2Bon(KaitaiStream p__io, KaitaiStruct p__parent = null, Thps2Bon p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _magic = m_io.ReadBytes(4);
            if (!((KaitaiStream.ByteArrayCompare(Magic, new byte[] { 66, 111, 110, 0 }) == 0)))
            {
                throw new ValidationNotEqualError(new byte[] { 66, 111, 110, 0 }, Magic, M_Io, "/seq/0");
            }
            _version = m_io.ReadU4le();
            switch (Version) {
            case 1: {
                _data = new BonDc(m_io, this, m_root);
                break;
            }
            case 3: {
                _data = new BonXbox(m_io, this, m_root);
                break;
            }
            case 4: {
                _data = new BonXbox(m_io, this, m_root);
                break;
            }
            }
        }
        public partial class MatSplitDc : KaitaiStruct
        {
            public static MatSplitDc FromFile(string fileName)
            {
                return new MatSplitDc(new KaitaiStream(fileName));
            }

            public MatSplitDc(KaitaiStream p__io, Thps2Bon.MeshDc p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _materialIndex = m_io.ReadU2le();
                _numIndices = m_io.ReadU4le();
                _indices = new List<ushort>();
                for (var i = 0; i < NumIndices; i++)
                {
                    _indices.Add(m_io.ReadU2le());
                }
            }
            private ushort _materialIndex;
            private uint _numIndices;
            private List<ushort> _indices;
            private Thps2Bon m_root;
            private Thps2Bon.MeshDc m_parent;
            public ushort MaterialIndex { get { return _materialIndex; } }
            public uint NumIndices { get { return _numIndices; } }
            public List<ushort> Indices { get { return _indices; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.MeshDc M_Parent { get { return m_parent; } }
        }
        public partial class Vector4f : KaitaiStruct
        {
            public static Vector4f FromFile(string fileName)
            {
                return new Vector4f(new KaitaiStream(fileName));
            }

            public Vector4f(KaitaiStream p__io, KaitaiStruct p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _x = m_io.ReadF4le();
                _y = m_io.ReadF4le();
                _z = m_io.ReadF4le();
                _w = m_io.ReadF4le();
            }
            private float _x;
            private float _y;
            private float _z;
            private float _w;
            private Thps2Bon m_root;
            private KaitaiStruct m_parent;
            public float X { get { return _x; } }
            public float Y { get { return _y; } }
            public float Z { get { return _z; } }
            public float W { get { return _w; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Vector3f : KaitaiStruct
        {
            public static Vector3f FromFile(string fileName)
            {
                return new Vector3f(new KaitaiStream(fileName));
            }

            public Vector3f(KaitaiStream p__io, KaitaiStruct p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _x = m_io.ReadF4le();
                _y = m_io.ReadF4le();
                _z = m_io.ReadF4le();
            }
            private float _x;
            private float _y;
            private float _z;
            private Thps2Bon m_root;
            private KaitaiStruct m_parent;
            public float X { get { return _x; } }
            public float Y { get { return _y; } }
            public float Z { get { return _z; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class BonXbox : KaitaiStruct
        {
            public static BonXbox FromFile(string fileName)
            {
                return new BonXbox(new KaitaiStream(fileName));
            }

            public BonXbox(KaitaiStream p__io, Thps2Bon p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                switch (M_Root.Version) {
                case 3: {
                    _numMats = m_io.ReadU2le();
                    break;
                }
                case 4: {
                    _numMats = m_io.ReadU4le();
                    break;
                }
                }
                _materials = new List<MaterialXbox>();
                for (var i = 0; i < NumMats; i++)
                {
                    _materials.Add(new MaterialXbox(m_io, this, m_root));
                }
                switch (M_Root.Version) {
                case 3: {
                    _numVertices = m_io.ReadU2le();
                    break;
                }
                case 4: {
                    _numVertices = m_io.ReadU4le();
                    break;
                }
                }
                switch (M_Root.Version) {
                case 3: {
                    _numUnk2 = m_io.ReadU2le();
                    break;
                }
                case 4: {
                    _numUnk2 = m_io.ReadU4le();
                    break;
                }
                }
                _vertices = new List<VertexXbox>();
                for (var i = 0; i < NumVertices; i++)
                {
                    _vertices.Add(new VertexXbox(m_io, this, m_root));
                }
                switch (M_Root.Version) {
                case 3: {
                    _numIndices = m_io.ReadU2le();
                    break;
                }
                case 4: {
                    _numIndices = m_io.ReadU4le();
                    break;
                }
                }
                _indices = new List<ushort>();
                for (var i = 0; i < NumIndices; i++)
                {
                    _indices.Add(m_io.ReadU2le());
                }
                switch (M_Root.Version) {
                case 3: {
                    _numHier = m_io.ReadU2le();
                    break;
                }
                case 4: {
                    _numHier = m_io.ReadU4le();
                    break;
                }
                }
                _hier = new List<MeshXbox>();
                for (var i = 0; i < NumHier; i++)
                {
                    _hier.Add(new MeshXbox(m_io, this, m_root));
                }
            }
            private uint _numMats;
            private List<MaterialXbox> _materials;
            private uint _numVertices;
            private uint _numUnk2;
            private List<VertexXbox> _vertices;
            private uint _numIndices;
            private List<ushort> _indices;
            private uint _numHier;
            private List<MeshXbox> _hier;
            private Thps2Bon m_root;
            private Thps2Bon m_parent;
            public uint NumMats { get { return _numMats; } }
            public List<MaterialXbox> Materials { get { return _materials; } }
            public uint NumVertices { get { return _numVertices; } }
            public uint NumUnk2 { get { return _numUnk2; } }
            public List<VertexXbox> Vertices { get { return _vertices; } }
            public uint NumIndices { get { return _numIndices; } }
            public List<ushort> Indices { get { return _indices; } }
            public uint NumHier { get { return _numHier; } }
            public List<MeshXbox> Hier { get { return _hier; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon M_Parent { get { return m_parent; } }
        }
        public partial class MeshDc : KaitaiStruct
        {
            public static MeshDc FromFile(string fileName)
            {
                return new MeshDc(new KaitaiStream(fileName));
            }

            public MeshDc(KaitaiStream p__io, KaitaiStruct p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _entryType = m_io.ReadU1();
                _name = new Bonstring(m_io, this, m_root);
                _someFlags = m_io.ReadU1();
                _numMatrices = m_io.ReadU4le();
                _matrix = new List<MatrixDc>();
                for (var i = 0; i < NumMatrices; i++)
                {
                    _matrix.Add(new MatrixDc(m_io, this, m_root));
                }
                _numChildren = m_io.ReadU4le();
                _children = new List<MeshDc>();
                for (var i = 0; i < NumChildren; i++)
                {
                    _children.Add(new MeshDc(m_io, this, m_root));
                }
                _unk = new List<float>();
                for (var i = 0; i < 6; i++)
                {
                    _unk.Add(m_io.ReadF4le());
                }
                _numVertices = m_io.ReadU4le();
                _vertices = new List<VertexDc>();
                for (var i = 0; i < NumVertices; i++)
                {
                    _vertices.Add(new VertexDc(m_io, this, m_root));
                }
                _numSplits = m_io.ReadU4le();
                _splits = new List<MatSplitDc>();
                for (var i = 0; i < NumSplits; i++)
                {
                    _splits.Add(new MatSplitDc(m_io, this, m_root));
                }
            }
            private byte _entryType;
            private Bonstring _name;
            private byte _someFlags;
            private uint _numMatrices;
            private List<MatrixDc> _matrix;
            private uint _numChildren;
            private List<MeshDc> _children;
            private List<float> _unk;
            private uint _numVertices;
            private List<VertexDc> _vertices;
            private uint _numSplits;
            private List<MatSplitDc> _splits;
            private Thps2Bon m_root;
            private KaitaiStruct m_parent;
            public byte EntryType { get { return _entryType; } }
            public Bonstring Name { get { return _name; } }
            public byte SomeFlags { get { return _someFlags; } }
            public uint NumMatrices { get { return _numMatrices; } }
            public List<MatrixDc> Matrix { get { return _matrix; } }
            public uint NumChildren { get { return _numChildren; } }
            public List<MeshDc> Children { get { return _children; } }
            public List<float> Unk { get { return _unk; } }
            public uint NumVertices { get { return _numVertices; } }
            public List<VertexDc> Vertices { get { return _vertices; } }
            public uint NumSplits { get { return _numSplits; } }
            public List<MatSplitDc> Splits { get { return _splits; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Matrix : KaitaiStruct
        {
            public static Matrix FromFile(string fileName)
            {
                return new Matrix(new KaitaiStream(fileName));
            }

            public Matrix(KaitaiStream p__io, Thps2Bon.MeshXbox p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _entries = new List<float>();
                for (var i = 0; i < 9; i++)
                {
                    _entries.Add(m_io.ReadF4le());
                }
            }
            private List<float> _entries;
            private Thps2Bon m_root;
            private Thps2Bon.MeshXbox m_parent;
            public List<float> Entries { get { return _entries; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.MeshXbox M_Parent { get { return m_parent; } }
        }
        public partial class Color : KaitaiStruct
        {
            public static Color FromFile(string fileName)
            {
                return new Color(new KaitaiStream(fileName));
            }

            public Color(KaitaiStream p__io, KaitaiStruct p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _r = m_io.ReadU1();
                _g = m_io.ReadU1();
                _b = m_io.ReadU1();
                _a = m_io.ReadU1();
            }
            private byte _r;
            private byte _g;
            private byte _b;
            private byte _a;
            private Thps2Bon m_root;
            private KaitaiStruct m_parent;
            public byte R { get { return _r; } }
            public byte G { get { return _g; } }
            public byte B { get { return _b; } }
            public byte A { get { return _a; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class MatrixDc : KaitaiStruct
        {
            public static MatrixDc FromFile(string fileName)
            {
                return new MatrixDc(new KaitaiStream(fileName));
            }

            public MatrixDc(KaitaiStream p__io, Thps2Bon.MeshDc p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _entries = new List<float>();
                for (var i = 0; i < 11; i++)
                {
                    _entries.Add(m_io.ReadF4le());
                }
            }
            private List<float> _entries;
            private Thps2Bon m_root;
            private Thps2Bon.MeshDc m_parent;
            public List<float> Entries { get { return _entries; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.MeshDc M_Parent { get { return m_parent; } }
        }
        public partial class Colorf : KaitaiStruct
        {
            public static Colorf FromFile(string fileName)
            {
                return new Colorf(new KaitaiStream(fileName));
            }

            public Colorf(KaitaiStream p__io, Thps2Bon.MaterialDc p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _r = m_io.ReadF4le();
                _g = m_io.ReadF4le();
                _b = m_io.ReadF4le();
                _a = m_io.ReadF4le();
            }
            private float _r;
            private float _g;
            private float _b;
            private float _a;
            private Thps2Bon m_root;
            private Thps2Bon.MaterialDc m_parent;
            public float R { get { return _r; } }
            public float G { get { return _g; } }
            public float B { get { return _b; } }
            public float A { get { return _a; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.MaterialDc M_Parent { get { return m_parent; } }
        }
        public partial class TextureXbox : KaitaiStruct
        {
            public static TextureXbox FromFile(string fileName)
            {
                return new TextureXbox(new KaitaiStream(fileName));
            }

            public TextureXbox(KaitaiStream p__io, Thps2Bon.MaterialXbox p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _name = new Bonstring(m_io, this, m_root);
                _flag1 = m_io.ReadU1();
                _addressU = m_io.ReadU1();
                _addressV = m_io.ReadU1();
                _size = m_io.ReadU4le();
                _data = m_io.ReadBytes(Size);
            }
            private Bonstring _name;
            private byte _flag1;
            private byte _addressU;
            private byte _addressV;
            private uint _size;
            private byte[] _data;
            private Thps2Bon m_root;
            private Thps2Bon.MaterialXbox m_parent;
            public Bonstring Name { get { return _name; } }
            public byte Flag1 { get { return _flag1; } }
            public byte AddressU { get { return _addressU; } }
            public byte AddressV { get { return _addressV; } }
            public uint Size { get { return _size; } }
            public byte[] Data { get { return _data; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.MaterialXbox M_Parent { get { return m_parent; } }
        }
        public partial class MatSplit : KaitaiStruct
        {
            public static MatSplit FromFile(string fileName)
            {
                return new MatSplit(new KaitaiStream(fileName));
            }

            public MatSplit(KaitaiStream p__io, Thps2Bon.MeshXbox p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _materialIndex = m_io.ReadU2le();
                _offset = m_io.ReadU2le();
                _size = m_io.ReadU2le();
            }
            private ushort _materialIndex;
            private ushort _offset;
            private ushort _size;
            private Thps2Bon m_root;
            private Thps2Bon.MeshXbox m_parent;
            public ushort MaterialIndex { get { return _materialIndex; } }
            public ushort Offset { get { return _offset; } }
            public ushort Size { get { return _size; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.MeshXbox M_Parent { get { return m_parent; } }
        }
        public partial class VertexXbox : KaitaiStruct
        {
            public static VertexXbox FromFile(string fileName)
            {
                return new VertexXbox(new KaitaiStream(fileName));
            }

            public VertexXbox(KaitaiStream p__io, Thps2Bon.BonXbox p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _position = new Vector3f(m_io, this, m_root);
                _unk1 = m_io.ReadF4le();
                _normal = new Vector3f(m_io, this, m_root);
                _wobbliness = new Color(m_io, this, m_root);
                _uv = new Vector2f(m_io, this, m_root);
            }
            private Vector3f _position;
            private float _unk1;
            private Vector3f _normal;
            private Color _wobbliness;
            private Vector2f _uv;
            private Thps2Bon m_root;
            private Thps2Bon.BonXbox m_parent;
            public Vector3f Position { get { return _position; } }
            public float Unk1 { get { return _unk1; } }
            public Vector3f Normal { get { return _normal; } }
            public Color Wobbliness { get { return _wobbliness; } }
            public Vector2f Uv { get { return _uv; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.BonXbox M_Parent { get { return m_parent; } }
        }
        public partial class TextureDc : KaitaiStruct
        {
            public static TextureDc FromFile(string fileName)
            {
                return new TextureDc(new KaitaiStream(fileName));
            }

            public TextureDc(KaitaiStream p__io, Thps2Bon.MaterialDc p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _name = new Bonstring(m_io, this, m_root);
                _flag1 = m_io.ReadU1();
                _addressU = m_io.ReadU1();
                _addressV = m_io.ReadU1();
                _size = m_io.ReadU4le();
                _data = m_io.ReadBytes(Size);
            }
            private Bonstring _name;
            private byte _flag1;
            private byte _addressU;
            private byte _addressV;
            private uint _size;
            private byte[] _data;
            private Thps2Bon m_root;
            private Thps2Bon.MaterialDc m_parent;
            public Bonstring Name { get { return _name; } }
            public byte Flag1 { get { return _flag1; } }
            public byte AddressU { get { return _addressU; } }
            public byte AddressV { get { return _addressV; } }
            public uint Size { get { return _size; } }
            public byte[] Data { get { return _data; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.MaterialDc M_Parent { get { return m_parent; } }
        }
        public partial class MeshXbox : KaitaiStruct
        {
            public static MeshXbox FromFile(string fileName)
            {
                return new MeshXbox(new KaitaiStream(fileName));
            }

            public MeshXbox(KaitaiStream p__io, KaitaiStruct p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _entryType = m_io.ReadU1();
                _name = new Bonstring(m_io, this, m_root);
                _matrix = new Matrix(m_io, this, m_root);
                _position = new Vector3f(m_io, this, m_root);
                _numChildren = m_io.ReadU2le();
                _children = new List<MeshXbox>();
                for (var i = 0; i < NumChildren; i++)
                {
                    _children.Add(new MeshXbox(m_io, this, m_root));
                }
                _matrix2 = new Matrix(m_io, this, m_root);
                _numBaseSplits = m_io.ReadU2le();
                _baseSplits = new List<MatSplit>();
                for (var i = 0; i < NumBaseSplits; i++)
                {
                    _baseSplits.Add(new MatSplit(m_io, this, m_root));
                }
                _numJointSplits = m_io.ReadU2le();
                _jointSplits = new List<MatSplit>();
                for (var i = 0; i < NumJointSplits; i++)
                {
                    _jointSplits.Add(new MatSplit(m_io, this, m_root));
                }
            }
            private byte _entryType;
            private Bonstring _name;
            private Matrix _matrix;
            private Vector3f _position;
            private ushort _numChildren;
            private List<MeshXbox> _children;
            private Matrix _matrix2;
            private ushort _numBaseSplits;
            private List<MatSplit> _baseSplits;
            private ushort _numJointSplits;
            private List<MatSplit> _jointSplits;
            private Thps2Bon m_root;
            private KaitaiStruct m_parent;
            public byte EntryType { get { return _entryType; } }
            public Bonstring Name { get { return _name; } }
            public Matrix Matrix { get { return _matrix; } }
            public Vector3f Position { get { return _position; } }
            public ushort NumChildren { get { return _numChildren; } }
            public List<MeshXbox> Children { get { return _children; } }
            public Matrix Matrix2 { get { return _matrix2; } }
            public ushort NumBaseSplits { get { return _numBaseSplits; } }
            public List<MatSplit> BaseSplits { get { return _baseSplits; } }
            public ushort NumJointSplits { get { return _numJointSplits; } }
            public List<MatSplit> JointSplits { get { return _jointSplits; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class MaterialXbox : KaitaiStruct
        {
            public static MaterialXbox FromFile(string fileName)
            {
                return new MaterialXbox(new KaitaiStream(fileName));
            }

            public MaterialXbox(KaitaiStream p__io, Thps2Bon.BonXbox p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _name = new Bonstring(m_io, this, m_root);
                _color = new Color(m_io, this, m_root);
                _unkFloat1 = m_io.ReadF4le();
                _unkFloat2 = m_io.ReadF4le();
                _hasTexture = m_io.ReadU1();
                if (HasTexture == 1) {
                    _texture = new TextureXbox(m_io, this, m_root);
                }
            }
            private Bonstring _name;
            private Color _color;
            private float _unkFloat1;
            private float _unkFloat2;
            private byte _hasTexture;
            private TextureXbox _texture;
            private Thps2Bon m_root;
            private Thps2Bon.BonXbox m_parent;
            public Bonstring Name { get { return _name; } }
            public Color Color { get { return _color; } }
            public float UnkFloat1 { get { return _unkFloat1; } }
            public float UnkFloat2 { get { return _unkFloat2; } }
            public byte HasTexture { get { return _hasTexture; } }
            public TextureXbox Texture { get { return _texture; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.BonXbox M_Parent { get { return m_parent; } }
        }
        public partial class Bonstring : KaitaiStruct
        {
            public static Bonstring FromFile(string fileName)
            {
                return new Bonstring(new KaitaiStream(fileName));
            }

            public Bonstring(KaitaiStream p__io, KaitaiStruct p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                switch (M_Root.Version) {
                case 1: {
                    _length = m_io.ReadU1();
                    break;
                }
                default: {
                    _length = m_io.ReadU2le();
                    break;
                }
                }
                _content = System.Text.Encoding.GetEncoding("ascii").GetString(m_io.ReadBytes(Length));
            }
            private ushort _length;
            private string _content;
            private Thps2Bon m_root;
            private KaitaiStruct m_parent;
            public ushort Length { get { return _length; } }
            public string Content { get { return _content; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Vector2f : KaitaiStruct
        {
            public static Vector2f FromFile(string fileName)
            {
                return new Vector2f(new KaitaiStream(fileName));
            }

            public Vector2f(KaitaiStream p__io, KaitaiStruct p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _x = m_io.ReadF4le();
                _y = m_io.ReadF4le();
            }
            private float _x;
            private float _y;
            private Thps2Bon m_root;
            private KaitaiStruct m_parent;
            public float X { get { return _x; } }
            public float Y { get { return _y; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class VertexDc : KaitaiStruct
        {
            public static VertexDc FromFile(string fileName)
            {
                return new VertexDc(new KaitaiStream(fileName));
            }

            public VertexDc(KaitaiStream p__io, Thps2Bon.MeshDc p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _position = new Vector3f(m_io, this, m_root);
                _normal = new Vector3f(m_io, this, m_root);
                _empty = new Vector3f(m_io, this, m_root);
                _uv1 = new Vector2f(m_io, this, m_root);
                _uv2 = new Vector2f(m_io, this, m_root);
            }
            private Vector3f _position;
            private Vector3f _normal;
            private Vector3f _empty;
            private Vector2f _uv1;
            private Vector2f _uv2;
            private Thps2Bon m_root;
            private Thps2Bon.MeshDc m_parent;
            public Vector3f Position { get { return _position; } }
            public Vector3f Normal { get { return _normal; } }
            public Vector3f Empty { get { return _empty; } }
            public Vector2f Uv1 { get { return _uv1; } }
            public Vector2f Uv2 { get { return _uv2; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.MeshDc M_Parent { get { return m_parent; } }
        }
        public partial class MaterialDc : KaitaiStruct
        {
            public static MaterialDc FromFile(string fileName)
            {
                return new MaterialDc(new KaitaiStream(fileName));
            }

            public MaterialDc(KaitaiStream p__io, Thps2Bon.BonDc p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _name = new Bonstring(m_io, this, m_root);
                _color = new Colorf(m_io, this, m_root);
                _unkFloat1 = m_io.ReadF4le();
                _unkFloat2 = m_io.ReadF4le();
                _unkFloat3 = m_io.ReadF4le();
                _unkFlag = m_io.ReadU1();
                _hasTexture = m_io.ReadU1();
                if (HasTexture == 1) {
                    _texture = new TextureDc(m_io, this, m_root);
                }
            }
            private Bonstring _name;
            private Colorf _color;
            private float _unkFloat1;
            private float _unkFloat2;
            private float _unkFloat3;
            private byte _unkFlag;
            private byte _hasTexture;
            private TextureDc _texture;
            private Thps2Bon m_root;
            private Thps2Bon.BonDc m_parent;
            public Bonstring Name { get { return _name; } }
            public Colorf Color { get { return _color; } }
            public float UnkFloat1 { get { return _unkFloat1; } }
            public float UnkFloat2 { get { return _unkFloat2; } }
            public float UnkFloat3 { get { return _unkFloat3; } }
            public byte UnkFlag { get { return _unkFlag; } }
            public byte HasTexture { get { return _hasTexture; } }
            public TextureDc Texture { get { return _texture; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon.BonDc M_Parent { get { return m_parent; } }
        }
        public partial class BonDc : KaitaiStruct
        {
            public static BonDc FromFile(string fileName)
            {
                return new BonDc(new KaitaiStream(fileName));
            }

            public BonDc(KaitaiStream p__io, Thps2Bon p__parent = null, Thps2Bon p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _numMats = m_io.ReadU4le();
                _materials = new List<MaterialDc>();
                for (var i = 0; i < NumMats; i++)
                {
                    _materials.Add(new MaterialDc(m_io, this, m_root));
                }
                _numHier = m_io.ReadU4le();
                _hier = new List<MeshDc>();
                for (var i = 0; i < NumHier; i++)
                {
                    _hier.Add(new MeshDc(m_io, this, m_root));
                }
            }
            private uint _numMats;
            private List<MaterialDc> _materials;
            private uint _numHier;
            private List<MeshDc> _hier;
            private Thps2Bon m_root;
            private Thps2Bon m_parent;
            public uint NumMats { get { return _numMats; } }
            public List<MaterialDc> Materials { get { return _materials; } }
            public uint NumHier { get { return _numHier; } }
            public List<MeshDc> Hier { get { return _hier; } }
            public Thps2Bon M_Root { get { return m_root; } }
            public Thps2Bon M_Parent { get { return m_parent; } }
        }
        private byte[] _magic;
        private uint _version;
        private KaitaiStruct _data;
        private Thps2Bon m_root;
        private KaitaiStruct m_parent;
        public byte[] Magic { get { return _magic; } }
        public uint Version { get { return _version; } }
        public KaitaiStruct Data { get { return _data; } }
        public Thps2Bon M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
