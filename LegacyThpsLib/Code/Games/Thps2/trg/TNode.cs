using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Numerics;
using System.Text;
using LegacyThps.Shared;

namespace LegacyThps.Thps2.Triggers
{
    // huge node class with all the fields found across the nodes
    [Serializable]
    public class TNode
    {
        public int Number;                              // node number 
        public int Size;                                // node size
        public int Offset;                              // node offset

        public NodeType Type { get; set; }              // node type
        public string Name { get; set; }                // node name
        
        //the rest is non-obligatory
        public short LinkCount;                         //number of links
        public List<short> Links = new List<short>();   //list of links
        public List<TNode> LinkedNodes = new List<TNode>();     // array of actual linked nodes

        //wild zero helpers
        public int SizeDiff;                            // keeps the difference between assumed size and real size
        public bool Alert = false;                      // used to alert if there are probs with wild zero
           
        // rails
        public Vector3 Position { get; set; }           // position xyz, the sturct is above
        public short Flags { get; set; }                // used by rails, 0-3 - sound, 4-5 - number of players
        public bool InRailCluster = false;
        public short Terminator;                        // should be 0xFFFF or -1. in fact i guess this is part of command list, always empty though.

        public uint Checksum { get; set; }              // used in Trickob, crate, etc.

        // power ups
        public short PowerUpType;                       // letters, tapes, level specific
        public short CreationTime;                      // created/not created at start param
        public short DropTime;                          // whether bonus will drop or not, useless in th3.
        public short LifeTime;                          // i guess some stuff may expire? FFFF is infinite.

        public List<uint> TrickObCluster;

        public short CamPtRadius;                       //radius defined in campt
        public short CamPtType;                         //campt type

        public short BaddyType;
        public short Priority;
        public int Creation;                            //same as CreationTime in powerup, but contains 3 byte values + FF 
        public Vector3 Rotation;

        //now this is temporary
        public string BaddyScript;
        public string CommandList;

        public string RestartName { get; set; }

        //these vars are not original, deducted from baddy script.
        //in fact i failed.
        public short Bouncy = 0;
        public short Killer = 0;

        //=========================methods start here=================================


        //use these boolean methods to sort nodes out 
        [Browsable(false)] public bool IsRail => Type == NodeType.RailDef || Type == NodeType.RailPoint;
        [Browsable(false)] public bool IsTrickOb => Type == NodeType.TrickOb;
        [Browsable(false)] public bool IsPoint => Type == NodeType.Point;
        [Browsable(false)] public bool IsCrate => Type == NodeType.Crate;
        [Browsable(false)] public bool IsPowerUp => Type == NodeType.PowerUp;
        [Browsable(false)] public bool IsCamPt => Type == NodeType.CamPt;
        [Browsable(false)] public bool IsGoalOb => Type == NodeType.GoalOb;
        [Browsable(false)] public bool IsBaddy => Type == NodeType.Baddy;
        [Browsable(false)] public bool IsCommand => Type == NodeType.Command;
        [Browsable(false)] public bool IsAutoExec => Type == NodeType.AutoExec || Type == NodeType.AutoExec2;
        [Browsable(false)] public bool IsRestart => Type == NodeType.Restart;
        [Browsable(false)] public bool IsScriptPoint => Type == NodeType.ScriptPoint;
        [Browsable(false)] public bool IsTrickObCluster => Type == NodeType.TrickObCluster;


        //===============helper methods that wrap some blocks of node reading=================

        //reads links
        public void ReadLinks(BinaryReader br)
        {
            LinkCount = br.ReadInt16();

            for (int i = 0; i < LinkCount; i++) 
                Links.Add(br.ReadInt16());
        }

        //reads position
        public void ReadPosition(BinaryReader br)
        {
            br.Pad(4);

            Position = new Vector3(
                br.ReadInt32(),
                br.ReadInt32(),
                br.ReadInt32()
            );
        }

        //reads angles
        public void ReadAngle(BinaryReader br)
        {
            // padding check, but it's always after position anyways, hence aligned properly
            br.Pad(4);

            Rotation = new Vector3(
                br.ReadInt16(),
                br.ReadInt16(),
                br.ReadInt16()
            );
        }

        public void ReadChecksum(BinaryReader br)
        {
            br.Pad(4);

            Checksum = br.ReadUInt32();
        }

        //=====================node reading scripts start here==========================

        //reads the node from file
        public void ReadNode(BinaryReader b)
        {
            // seek offset
            b.BaseStream.Seek(Offset, SeekOrigin.Begin);

            // read node type
            Type = (NodeType)b.ReadUInt16();

            // generate automatic name
            Name = $"@{Number.ToString("0000")}_{Type}_auto";

            // if links = -1, assume no links section in node
            LinkCount = -1;

            // read node depending on type
            if (IsRail) ReadRail(b);
            else if (IsTrickOb || IsCrate || IsGoalOb) ReadTrickObCrate(b);
            else if (IsBaddy) ReadBaddy(b);
            else if (IsCommand) ReadCommand(b);
            else if (IsPowerUp) ReadPowerUp(b);
            else if (IsAutoExec) ReadAutoExec(b);
            else if (IsRestart) ReadRestart(b);
            else if (IsCamPt) ReadCamPt(b);
            else if (IsPoint) ReadPoint(b);
            else if (IsScriptPoint) ReadScriptPoint(b);
            else if (IsTrickObCluster) ReadTrickObCluster(b);
            else if (Type == NodeType.Terminator) { }
            //else throw new Exception($"not implemented node type {Type} yet");
        }

        public void ReadPoint(BinaryReader br)
        {
            ReadLinks(br);
            ReadPosition(br);
        }

        public void ReadTrickObCluster(BinaryReader br)
        {
            ReadLinks(br);

            br.Pad(4);

            TrickObCluster = new List<uint>();

            uint entry = 1;

            do
            {
                entry = br.ReadUInt32();

                if (entry != 0)
                    TrickObCluster.Add(entry);
            }
            while (entry != 0);
        }

        public void ReadRail(BinaryReader br)
        {
            ReadLinks(br);
            ReadPosition(br);

            //rail specific
            if (TriggerFile.ParsingMode == ParsingMode.THPS1)
                ReadAngle(br);

            Flags = br.ReadInt16();

            if (TriggerFile.ParsingMode == ParsingMode.THPS1)
                Terminator = br.ReadInt16();
        }

        public void ReadCamPt(BinaryReader b)
        {
            ReadLinks(b);
            ReadPosition(b);
            //campt specific
            CamPtRadius = b.ReadInt16();
            CamPtType = b.ReadInt16();
        }

        //crate and trickob share similiar format. just trickob additionally has terminator.
        public void ReadTrickObCrate(BinaryReader b)
        {
            ReadLinks(b);

            ReadChecksum(b);

            if (Checksum != 0)
                Name += $"_{Checksum.ToString("X8")}";

            if (!IsCrate)
                Terminator = b.ReadInt16();
        }

        public void ReadPowerUp(BinaryReader b)
        {
            PowerUpType = b.ReadInt16();

            ReadLinks(b);
            ReadPosition(b);

            CreationTime = b.ReadInt16();
            DropTime = b.ReadInt16();
            LifeTime = b.ReadInt16();

            Terminator = b.ReadInt16();

            Name += "_" + GetName.PowerUp(PowerUpType);
        }

        public void ReadBaddy(BinaryReader b)
        {
            BaddyType = b.ReadInt16();
            Priority = b.ReadInt16();

            ReadLinks(b);

            // TODO Check whether we need a pad check here

            Creation = b.ReadInt32();

            ReadPosition(b);
            ReadAngle(b);

            // read baddy script till the end of the node
            if (SizeDiff != Size) 
                BaddyScript = ReadBaddyScript(b);

            Name += $"_{GetName.BaddyType(BaddyType)}";
        }





        public void ReadCommand(BinaryReader b)
        {
            ReadLinks(b);

            ReadChecksum(b);

            if (Checksum != 0)
                Name += $"_{Checksum.ToString("X8")}";

            CommandList = ReadCommandList(b);

            if (CommandList.Contains("GapPolyHit"))
                Name += "_Gap";
        }

        public void ReadRestart(BinaryReader br)
        {
            ReadLinks(br);
            ReadPosition(br);
            ReadAngle(br);

            RestartName = br.ReadTrgString();

            Name += "_" + RestartName;

            CommandList = ReadCommandList(br);
        }

        public void ReadScriptPoint(BinaryReader b)
        {
            ReadLinks(b);
            ReadPosition(b);
            ReadAngle(b);

            BaddyScript = ReadBaddyScript(b);
        }


        public void ReadAutoExec(BinaryReader b)
        {
            CommandList = ReadCommandList(b);
        }



        // TODO refactor this to a separate class

        public string ReadCommandList(BinaryReader b)
        {
            var back = b.BaseStream.Position;
            string fail = "";

            try
            {
                var cmd = new CommandList();

                cmd.Read(b);

                fail = cmd.ToString() + "\r\n";

                return cmd.ToString();

            } catch (Exception ex)
            {
                fail += $"back:{back.ToString("X8")}\r\nnew code fail\r\n" + ex.Message + "\r\n" + ex.ToString();
                // go to old code

                b.BaseStream.Position = back;
            }

            short x = 0;

            string res = "";
            string r = "";

            bool initgap = true;

            while (x != -1)
            {
                x = b.ReadInt16();

                switch (x)
                {

                    //set cheat restarts is useless for th3, but we need to skip it correctly
                    //i finally found the restarts list ends with 00, which makes parsing much easier
                    //so basically we have to read strings until we meet empty one "".
                    case 2:
                        {
                            r = GetName.Command(x);

                            string k="";

                            do
                            {
                                k = b.ReadTrgString();
                                if (k!="") r += "\r\n" + "\t\t" + k ;
                            }
                            while (k != "");

                            break;
                        }

                    case 3: r = GetName.Command(x); break;
                    case 4: r = GetName.Command(x); break;
                    case 5: r = GetName.Command(x); break;
                    case 10: r = GetName.Command(x); break;
                    case 11: r = GetName.Command(x); break;
                    case 12: r = GetName.Command(x); break;
                    case 13: r = GetName.Command(x) + " " + b.ReadInt16().ToString(); ; break;

                    // ???
                    case 119:
                        r = GetName.Command(x) + " " + b.ReadTrgString(); break;

                    //Set fogging params: ZHither=10, DpqMin=5000, Range=1024
                    case 104: r = GetName.Command(x) + " zhither=" + b.ReadInt16().ToString() + " Dpqmin=" + b.ReadInt16().ToString() + " range=" + b.ReadInt16().ToString(); break;
                    case 126: r = GetName.Command(x) + " " + b.ReadTrgString(); break;
                    case 127: r = GetName.Command(x) + " " + b.ReadTrgString(); break;
                    case 128: r = GetName.Command(x) + " " + b.ReadTrgString(); break;
                    case 131: r = GetName.Command(x) + " " + b.ReadInt16().ToString(); break;
                    case 132: r = GetName.Command(x) + " " + b.ReadInt16().ToString(); break;
                    case 134: r = GetName.Command(x) + " " + b.ReadInt16().ToString(); break;

                    case 151: r = GetName.Command(x) + " " + b.ReadInt16().ToString(); break;

                    case 140: r = GetName.Command(x) + " " + b.ReadTrgString(); break;
                    case 142: r = GetName.Command(x) + " " + b.ReadTrgString(); break;
                    case 147: r = GetName.Command(x) + " " + b.ReadInt16().ToString();  break;
                    case 166: r = GetName.Command(x) + " " + b.ReadInt16().ToString(); break;
                    case 169: r = GetName.Command(x) + " " + b.ReadInt16().ToString(); break;

                    case 171:
                        {
                            r = GetName.Command(x);

                            //CommandWildZero(b);

                            b.Pad(4);

                            r +=  " Checksum=0x" + b.ReadInt32().ToString("X") + " angles=(" +
                             b.ReadInt16().ToString() + ", " + b.ReadInt16().ToString() + ", " + b.ReadInt16().ToString() + ")";
                            
                            break;

                        }

                    case 178: r = GetName.Command(x) + " " + b.ReadTrgString(); break;


                    //visibility in box was my headache before, by i think i figured it ouy
                    case 141:
                        {
                            r = GetName.Command(x) + " to "
                                + b.ReadInt16().ToString() + " "    //ON/OFF
                                + b.ReadInt16().ToString();         //INSIDE/OUTSIDE

 //=========================WARNING: wild zero
                            //have to implement another wild zero check 
                            short buf = b.ReadInt16();

                            if (buf != 0) b.BaseStream.Position -= 2;

                            //looping through boxes
                            buf = b.ReadInt16(); 

                            while (buf != 0xFF) //i have no idea what happens if there are no boxes at all.
                                                //in theory this should work if they always denote the end of box list as 0xFF.
                                                //not sure if it's even possible, but who knows.
                            {
                                b.BaseStream.Position-=2; //if value is not 0xFF, go back and read the box
                                r += "\r\n\t\t[(" +
                                     b.ReadInt32().ToString() + ", " +
                                      b.ReadInt32().ToString() + ", " +
                                      b.ReadInt32().ToString() + "), (" +
                                     b.ReadInt32().ToString() + ", " +
                                      b.ReadInt32().ToString() + ", " +
                                      b.ReadInt32().ToString() + ")]";

                                //break reading if we excess node size
                                //should not be a problem now
                                //uncomment if it cant open trg
                                if (b.BaseStream.Position > Offset + Size) break;

                                //read potential end of box list for the next iteration
                                buf = b.ReadInt16();
                            }
                            

                            break;
                        }

                    case 149: r = GetName.Command(x); break;
                    case 152: { r = GetName.Command(x); Killer = 1; Name += "_KillCommand";  break; }
                    case 157: r = GetName.Command(x) + " " + b.ReadInt16().ToString(); break;
                    case 158: r = GetName.Command(x); break;

                    case 200: 
                        
                        
                        
                        r = GetName.Command(x) + " color=0x" + b.ReadInt32().ToString("X"); break;

                        //alert, checksum may face wildzero
                        //rather brute zero check, assuming initial gap poly hit in the list has it and the rest dont.
                        //dont forget to tset it on multiple files. it seems to work fine on downtown.
                        //normal gappolyhit command should have something like
                        //type = 3000 (some adequate number, usually 4 digits) and no unknown commands
                    case 201:
                        {
                        if (initgap)
                        { 
                            b.ReadInt16(); 
                            initgap = false;
                        }

                        r = GetName.Command(x) + " destcrc = 0x" + b.ReadInt32().ToString("X") + " type = " + b.ReadInt16().ToString();
                        Name += "_GapCommand";
                        break;
                        }
                    case 202: r = GetName.Command(x) + " color=0x" + b.ReadInt32().ToString("X"); break;
                    case 203: r = GetName.Command(x) + " " + b.ReadInt16().ToString(); break;
                    case 204: r = $"{GetName.Command(x)} id = {b.ReadInt16()} score = {b.ReadInt16()} name = {b.ReadTrgString()}"; break;

                    default: { r = GetName.Command(x); break; }
                }


                if (r != "")
                {
                    if (res != "") res += "\r\n";
                    res += "\t" + r;
                }
            }

            return fail + res;
        }




        public string DoBaddyAction(BinaryReader b, short x)
        {
            string r = "";
            short b16 = 0;
            int b32 = 0;

            switch (x)
            {
                case 0x4100:
                case 0x4203:
                case 0x4204:
                case 0x4105:
                case 0x4107:
                case 0x4120:
                case 0x4205:
                    r = GetName.BSFunc(x);
                    break;

                //label (n) = no labels in th3? i guess used with IFs mostly, so can translate into IF
                case 0x4104:
                case 0x4102: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; }


                case 0x42B0: // C_TEXT_MESSAGE
                    r = $"{GetName.BSFunc(x)} \"{b.ReadTrgString()}\"";
                    break;

                // 2 short arguments
                case 0x4112:
                case 0x4113:
                case 0x4114:
                case 0x4306:
                case 0x4307:
                case 0x4308:
                    {
                        r = GetName.BSFunc(x);

                        // read arg 1
                        b16 = b.ReadInt16();

                        if (GetName.isBaddyVar(b16))
                        { r += GetName.Param(DoBaddyAction(b, b16)); }
                        else
                        { r += GetName.Param(b16); }

                        // read arg 2
                        b16 = b.ReadInt16();

                        if (GetName.isBaddyVar(b16))
                        { r += GetName.Param(DoBaddyAction(b, b16)); }
                        else
                        { r += GetName.Param(b16); }

                        break;
                    }

                // 1 short argument
                case 0x4304:
                case 0x4110:
                    {
                        r = GetName.BSFunc(x);

                        b16 = b.ReadInt16();
                        if (GetName.isBaddyVar(b16))
                        { r += GetName.Param(DoBaddyAction(b, b16)); }
                        else
                        { r += GetName.Param(b16); }

                        break;
                    }


                //wait (n) = wait n frames?
                case 0x4280:
                    {
                        b16 = b.ReadInt16();
                        if (GetName.isBaddyVar(b16))
                        {
                            r = GetName.BSFunc(x) + GetName.Param(DoBaddyAction(b, b16));
                        }
                        else
                        {
                            r = GetName.BSFunc(x) + GetName.Param(b16);
                        }
                        break;
                    }

                case 0x4281: { r = GetName.BSFunc(x); break; }

                //play (n) = PlaySound
                case 0x4290: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; }

                //play_positional_sfx (n) = PlaySound, positioning is defined at loadsound
                case 0x4291: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; }

                //spark (n) = creates spark at the node position. have no idea how we'll simulate this
                case 0x4292: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; }

                case 0x4298: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; }

                case 0x429E: { r = GetName.BSFunc(x); break; }

                case 0x4507: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; }
                case 0x4509: { r = GetName.BSFunc(x); break; }


                case 0x4294: { r = GetName.BSFunc(x); break; }
                case 0x4299: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; }
                case 0x4293: { b32 = b.ReadInt32(); r = GetName.BSFunc(x) + GetName.Param("0x" + b32.ToString("X")); break; }

                case 0x429C:
                    {
                        r = GetName.BSFunc(x) + GetName.Param(
                            b.ReadInt16().ToString()+", "+
                            b.ReadInt16().ToString()+", "+
                            b.ReadInt16().ToString()+", "+
                            b.ReadInt16().ToString()+", "+
                            b.ReadInt16().ToString());
                        break;
                    }

                case 0x42C0: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; }

                case 0x4115:
                case 0x4116:
                case 0x4221:
                case 0x42B1:
                case 0x42B2:
                case 0x42B4: { 

                    b16 = b.ReadInt16();
                    if (GetName.isBaddyVar(b16))
                    {
                        r = GetName.BSFunc(x) + GetName.Param(DoBaddyAction(b, b16));
                    }
                    else
                    {
                        r = GetName.BSFunc(x) + GetName.Param(b16);
                    }
                    break; 

                }

                case 0x4508:
                    { r = GetName.BSFunc(x) + GetName.Param(b.ReadInt16(), b.ReadInt16()); break; }



                case 0x212A: { r = GetName.BSFunc(x); break; }
                case 0x212B: { r = GetName.BSFunc(x); break; }

                //bouncy (n) = bouncy node
                case 0x4309: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(GetName.Bouncy(b16)); Bouncy = 1; Name += "_BouncyBaddy"; break; }


                case 0x4301: { r = GetName.BSFunc(x); break; }


                case 0x2120: // V_REGISTER
                case 0x2125:

                    r = GetName.BSFunc(x) + " " + b.ReadInt16();
                    break;

                // just self
                case 0x2132:
                case 0x212C:
                    r = GetName.BSFunc(x); break;

                case 0x2127: { r = GetName.BSFunc(x) + GetName.Param(b.ReadInt16().ToString() + ", " + b.ReadInt16().ToString() + ", " + b.ReadInt16().ToString()); break; }
                case 0x2128: { r = GetName.BSFunc(x) + GetName.Param(b.ReadInt16().ToString() + ", " + b.ReadInt16().ToString() + ", " + b.ReadInt16().ToString()); break; }

                case 0x2129: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; }

                case 0x2134: { r = GetName.BSFunc(x) + GetName.Param(b.ReadInt16().ToString() + ", " + b.ReadInt16().ToString() + ", " + b.ReadInt16().ToString()); break; }

                // V_SENT_DEATH_PULSE
                case 0x2138: { b16 = b.ReadInt16(); r = GetName.BSFunc(x) + GetName.Param(b16); break; ; }

                //assigns model by checksum
                case 0x212F:
                        b.Pad(4); 
                        b32 = b.ReadInt32(); 
                        r = GetName.BSFunc(x) + GetName.Param("0x" + b32.ToString("X"));
                        break;

                default: r = "0x" + x.ToString("X"); break;
            }

            return r;
        }


        public string ReadBaddyScript(BinaryReader b)
        {
            short x = 0;
            string res = "";

            while (/*x != 0x4100 && */b.BaseStream.Position < Offset+Size)
            {
                x = b.ReadInt16();
                res = res + DoBaddyAction(b, x) +"\r\n";
            }

            return res;
        }



        public override string ToString() => Name;

        public string ToStringLong()
        {
            string res = "\r\n" + Number + " " + Number.ToString("X4") + " 0x" + Offset.ToString("X") + " (" + Size.ToString() + " bytes [" + Type + "])";

            //everything here should migrate to TNode class

            if (IsTrickObCluster)
            {
                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";

                if (TrickObCluster != null)
                    foreach (uint checksum in TrickObCluster) 
                        res += " " + checksum.ToString("X8") + "\r\n";
            }

            if (IsPoint)
            {
                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";
                res += "  pos(" + Position.X.ToString() + ", " + Position.Y.ToString() + ", " + Position.Z.ToString() + ")";
            }

            if (IsScriptPoint)
            {
                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";
                res += "  pos(" + Position.X.ToString() + ", " + Position.Y.ToString() + ", " + Position.Z.ToString() + ")";
                res += "  ang(" + Rotation.X.ToString() + ", " + Rotation.Y.ToString() + ", " + Rotation.Z.ToString() + ")";

                res += "\r\n" + BaddyScript;
            }

            //case rail
            if (IsRail)
            {

                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";

                res += "  pos(" + Position.X.ToString() + ", " + Position.Y.ToString() + ", " + Position.Z.ToString() + ")";
                res += " flags[" + Flags.ToString() + "]";

                if (Alert) res += " ALERT!!! Node size mismatch! " + SizeDiff.ToString(); ;
            }

            //case trickobject or crate, same output
            if (IsTrickOb || IsCrate || IsGoalOb)
            {
                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";

                res += "    checksum(0x" + Checksum.ToString("X") + ")";

                if (Alert) res += " ALERT!!! Node size mismatch! " + SizeDiff.ToString(); ;
            }

            //case powerup
            if (IsPowerUp)
            {
                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";

                res += "  pos(" + Position.X.ToString() + ", " + Position.Y.ToString() + ", " + Position.Z.ToString() + ")";

                res += "  bonus[" + GetName.PowerUp(PowerUpType) + "] " +
                    GetName.CreationTime(CreationTime) + " " +
                    GetName.DropTime(DropTime) + " LifeTime = ";
                if (LifeTime != -1)
                { res += LifeTime.ToString(); }
                else { res += "infinite"; }

                if (Alert) res += " ALERT!!! Node size mismatch! " + SizeDiff.ToString();
            }

            //case trickobject or crate, same output
            if (IsCamPt)
            {
                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";

                res += "  pos(" + Position.X.ToString() + ", " + Position.Y.ToString() + ", " + Position.Z.ToString() + ")";

                res += " radius = " + CamPtRadius.ToString() + " type = " + CamPtType.ToString();

                if (Alert) res += " ALERT!!! Node size mismatch! " + SizeDiff.ToString(); ;
            }

            if (IsBaddy)
            {
                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";
                res += " type = " + GetName.BaddyType(BaddyType) + " Priority = " + Priority.ToString() + " creation = "
                    + Creation.ToString("X");
                res += "  pos(" + Position.X.ToString() + ", " + Position.Y.ToString() + ", " + Position.Z.ToString() + ")";
                res += "  ang(" + Rotation.X.ToString() + ", " + Rotation.Y.ToString() + ", " + Rotation.Z.ToString() + ")";

                res += "\r\n"+BaddyScript;
            }

            if (IsCommand)
            {
                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";

                res += " checksum = 0x" + Checksum.ToString("X");

                res += "\r\n" + CommandList;

            }

            if (IsAutoExec)
            {
                res += "\r\n" + CommandList;
            }

            if (IsRestart)
            {
                res += " - [" + LinkCount + "]";
                foreach (short x in Links) res += (" " + x.ToString()) + "; ";

                res += "  pos(" + Position.X.ToString() + ", " + Position.Y.ToString() + ", " + Position.Z.ToString() + ")";
                res += "  ang(" + Rotation.X.ToString() + ", " + Rotation.Y.ToString() + ", " + Rotation.Z.ToString() + ")";

                res += " Name = " + RestartName;

                res += "\r\n" + CommandList;

            }

            return res;
        }


        public TNode GetParentRail(List<TNode> nodes)
        {
            foreach (var node in nodes)
                if (node.IsRail)
                    if (node.LinkedNodes.Contains(this))
                    // it is assumed that thps rails never have more than one linked parent 
                        return node;

            return null;
        }

        public bool GotChildrenRails()
        {
            // ignore rails with links
            foreach (var link in LinkedNodes)
                if (link.IsRail)
                    return true;

            return false;
        }


        public string Decompile()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"@{Name}");
            sb.AppendLine($"\t-word:{Type}");

            if (LinkedNodes.Count > 0)
            {
                sb.AppendLine("\t-<Links>");

                foreach (var link in LinkedNodes)
                    sb.AppendLine($"\t\t{link.Name}");
            }


            if (IsRail)
            {
                sb.AppendLine($"\t-vec3: {Position.X} {Position.Y} {Position.Z}");
                sb.AppendLine($"\t-word: {Flags.ToString()}");
            }
            else if (IsCrate || IsGoalOb || IsTrickOb)
            {
                sb.AppendLine($"\t-checksum: {Checksum.ToString("X8")}");
                sb.AppendLine($"\t-word: 0xFFFF");
            }
            else
            {
                sb.AppendLine($";\tnot implemented yet!");
            }

            return sb.ToString();
        }
    }
}