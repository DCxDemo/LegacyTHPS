
namespace LegacyThps.QScript
{
    public enum QBFormat
    {
        //don't use newline to determine version as all games support both

        THPS3 = 3,  //default
        THPS4 = 4,  //if got comma or math?
        THUG1 = 5,  //if got thugrandom or switch? was switch in th4?
        THUG2 = 6   //if got if2
    }

    public enum DataGroup
    {
        Unknown,
        Empty,
        Short,
        Int,
        Uint,
        Float,
        Vector2,
        Vector3,
        SymbolDef,
        FixedString,
        Random
    }

    public enum OpLogic
    {
        Unknown,
        Halt,
        Linefeed,
        RegionBegin,
        RegionEnd,
        Math,
        Relation,
        Separator,
        Symbol,
        SymbolDef,
        Numeric,
        String,
        Keyword,
        Random,
        Vector,
        Global,
        Logic,
        Reserved
    }

    public enum NestCommand
    {
        None,
        Open,
        Close,
        Break
    }



    //outdate this
    //or maybe not yet

    public enum QBcode
    {
        none = 0xFF, // just used an empty instruction
        endfile = 0x00,
        newline1 = 0x01,
        newline2 = 0x02,
        structure = 0x03,
        endstructure = 0x04,
        array = 0x05,
        endarray = 0x06,
        math_eq = 0x07,
        property = 0x08,
        comma = 0x09,
        qbsub = 0x0A,
        qbadd = 0x0B,
        qbdiv = 0x0C,
        qbmul = 0x0D,
        roundopen = 0x0E,
        roundclose = 0x0F,
        //0x10 - debug info?
        //0x11 - same as?
        less = 0x12,
        lesseq = 0x13,
        greater = 0x14,
        greatereq = 0x15,
        symbol = 0x16,
        val_int = 0x17,
        //0x18 - hexint?
        //0x19 - enum?
        val_float = 0x1A,
        val_string = 0x1B,
        val_string_param = 0x1C,
        //0x1D - array?
        val_vector3 = 0x1E,
        val_vector2 = 0x1F,
        repeat = 0x20,
        repeatend = 0x21,
        repeatbreak = 0x22,
        script = 0x23,
        endscript = 0x24,
        qbif = 0x25,
        qbelse = 0x26,
        qbelseif = 0x27,
        qbendif = 0x28,
        qbreturn = 0x29,
        //0x2A - unused
        symboldef = 0x2B,
        globalall = 0x2C,
        global = 0x2D,
        randomjump = 0x2E,
        random = 0x2F,
        randomrange = 0x30,
        //0x31 - "at" @
        qbor = 0x32,
        qband = 0x33,
        //0x34 - xor
        //0x35 - shl
        //0x36 - shr
        random2 = 0x37,
        randomrange2 = 0x38,
        qbnot = 0x39,
        //0x3A
        //0x3B
        qbswitch = 0x3C,
        qbendswitch = 0x3D,
        qbcase = 0x3E,
        qbdefault = 0x3F,
        randomnorepeat = 0x40,
        randompermute = 0x41,
        member = 0x42,
        unk_43 = 0x43, // runtime only - cfunc
        unk_44 = 0x44, // runtime only - memberfunc
        //0x45
        unk_46 = 0x46,
        qbif2 = 0x47,
        qbelse2 = 0x48,
        qbendswitch2 = 0x49
    }





    //useless as of now

    public enum NodeClass
    {
        BouncyObject,
        Empty,
        EnvironmentObject,
        GameObject,
        GenericNode,
        Pedestrian,
        RailNode,
        Restart,
        Waypoint,
        Vehicle
    }

    public enum TerrainType
    {
        None,
        TERRAIN_METALSMOOTH,
        TERRAIN_MEATALSMOOTH, //awesome NS typo
        TERRAIN_CONCSMOOTH,
        TERRAIN_WOOD
    }

    public enum RailType
    {
        None,
        Crown,
        CTF_1,
        CTF_2,
        Zone,
        Zone_Key,
        MultiPlayer,
        Metal,
        Wood,
        Concrete,
        UserDefined
    }

    public enum RestartType
    {
        None,
        MultiPlayer,
        Horse,
        Player1,
        CTF_1,
        CTF_2
    }


    public enum GapFlag
    {
        Cancel_Ground,
        Cancel_Air,
        Cancel_Rail,
        Cancel_Wall,
        Cancel_Lip,
        Cancel_Manual,
        Require_Ground,
        Require_Air,
        Require_Rail,
        Require_Wall,
        Require_Lip,
        Require_Manual,
        Pure_Ground,
        Pure_Air,
        Pure_Rail,
        Pure_Wall,
        Pure_Lip,
        Pure_Manual
    }
}