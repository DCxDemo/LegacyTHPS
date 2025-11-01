namespace LegacyThps.Thps2.Triggers
{
    public enum ParsingMode
    {
        THPS1,
        THPS2,
        // MHPB // need a special case here? at least commands - gaps funcs are different
    }

    public enum NodeType
    { 
        // a scripted node, often a model. inherited from apocalypse hostile enemies
        // contains a "baddy script"
        Baddy = 1,

        // a model, able to shatter?
        Crate = 2,

        // a point in space, apparently used for teleports and kill planes
        Point = 3,

        // level entry point, loads required psx files and send intitialization pulses
        AutoExec = 4,

        // bonuses, cash, tapes, skate letters, level specific items, etc.
        PowerUp = 5,

        // a generic script node, often used to link nodes, 
        Command = 6,

        // a baddy that is... seedable?
        SeedableBaddy = 7,

        // a restart node - a point in space that spawns skater in and performs extra loading tasks
        Restart	= 8,

        // same as crate? ever used? not in thps i think
        Barrel	= 9,

        // defines a rail point in space. has links to other rail points, forming a line to grind.
        RailDef	= 10,

        // same as above. maybe originally intended to be a rail ending point without links. 
        RailPoint = 11,

        // a trick object. only contains checksum and serves as a connection to mesh object
        TrickOb	= 12,

        // camera point, used for replays
        CamPt = 13,

        // goal object, only used in thps1 for objects within the level (tables in school1). later games only spawn powerups.
        GoalOb = 14,

        // same as autoexec, but for 2p
        AutoExec2 = 15,

        // this node type is found in mhpb
        Mysterious16 = 16,

        // cluster of trick objects. same as trickob, but contains multiple checksums. only saw this in mhpb
        TrickObCluster = 17,

        // ending node. it's unclear why they need this, since the number of nodes is known beforehand
        Terminator = 255,

        // actually used in mhpb
        Light = 500,

        // unknown, found in mall, new york
        OffLight = 501,

        // contains baddy script without a baddy.
        // thps just uses baddy of type ScriptOnlyBaddy
        ScriptPoint	= 1000,

        // a camera path i suppose? maybe used in apocalypse/spiderman
        CameraPath = 1001
    }
}
