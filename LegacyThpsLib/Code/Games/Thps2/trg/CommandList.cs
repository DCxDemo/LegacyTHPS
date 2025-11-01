using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LegacyThps.Shared;

namespace LegacyThps.Thps2.Triggers
{
    public class CFuncDef
    {
        public ushort Value;
        public string Name;
        public string Description;

        public List<EParamType> ParamTypes;
        public int NumParams => ParamTypes == null ? 0 : ParamTypes.Count;

        static List<EParamType> empty = new List<EParamType> { };
        static List<EParamType> oneString = new List<EParamType> { EParamType.String };
        static List<EParamType> stringList = new List<EParamType> { EParamType.StringList };
        static List<EParamType> oneHash = new List<EParamType> { EParamType.Hash };
        static List<EParamType> oneBool = new List<EParamType> { EParamType.BoolWord };
        static List<EParamType> oneColor = new List<EParamType> { EParamType.Color };
        static List<EParamType> oneWord = new List<EParamType> { EParamType.Word };
        static List<EParamType> twoWords = new List<EParamType> { EParamType.Word, EParamType.Word };
        static List<EParamType> threeWords = new List<EParamType> { EParamType.Word, EParamType.Word, EParamType.Word };

        static List<EParamType> mhpbGap = new List<EParamType> { EParamType.Word, EParamType.Word, EParamType.String };

        static List<EParamType> setVisBoxParams = new List<EParamType> { 
            EParamType.Word, EParamType.Word, EParamType.BboxList
        };

        public static List<CFuncDef> Definitions = new List<CFuncDef>()
        {
            
            new CFuncDef { Value =   2, Name = "SetCheatRestarts",  ParamTypes = stringList },
            new CFuncDef { Value =   3, Name = "SendPulse",         ParamTypes = empty },
            new CFuncDef { Value =   4, Name = "SendActivate",      ParamTypes = empty },
            new CFuncDef { Value =   5, Name = "SendSuspend",       ParamTypes = empty },

            new CFuncDef { Value =  10, Name = "SendSignal",        ParamTypes = empty },
            new CFuncDef { Value =  11, Name = "SendKill",          ParamTypes = empty },
            new CFuncDef { Value =  12, Name = "SendKillLoudly",    ParamTypes = empty },
            new CFuncDef { Value =  13, Name = "SendVisible",       ParamTypes = oneBool },

            new CFuncDef { Value = 104, Name = "SetFoggingParams",  ParamTypes = threeWords },

            new CFuncDef { Value = 119, Name = "DebugText",         ParamTypes = oneString },

            new CFuncDef { Value = 126, Name = "SpoolIn",           ParamTypes = oneString },
            new CFuncDef { Value = 127, Name = "SpoolOut",          ParamTypes = oneString },
            new CFuncDef { Value = 128, Name = "SpoolEnv",          ParamTypes = oneString },

           new CFuncDef { Value = 131, Name = "BackgroundOn", ParamTypes = oneBool },
           new CFuncDef { Value = 132, Name = "BackgroundOff", ParamTypes = oneBool },

            // guess param is linked node index or -1 for all linked nodes?
            new CFuncDef { Value = 134, Name = "SendInitialPulses", ParamTypes = oneWord },

            new CFuncDef { Value = 140, Name = "SetRestart",        ParamTypes = oneString },

            
            new CFuncDef { Value = 141, Name = "SetVisibilityInBox", ParamTypes = setVisBoxParams },
            new CFuncDef { Value = 142, Name = "SetObjFile",        ParamTypes = oneString },

            new CFuncDef { Value = 147, Name = "SetGameLevel",      ParamTypes = oneWord },

            
            new CFuncDef { Value = 151, Name = "SetDualBufferSize", ParamTypes = oneWord },
            new CFuncDef { Value = 152, Name = "KillBruce",         ParamTypes = empty },

            new CFuncDef { Value = 155, Name = "MidiFadeIn",        ParamTypes = empty },
            new CFuncDef { Value = 156, Name = "MidiFadeOut",       ParamTypes = empty },
            new CFuncDef { Value = 157, Name = "SetReverbType",     ParamTypes = oneWord },
            new CFuncDef { Value = 158, Name = "EndLevel",          ParamTypes = empty },
           

            new CFuncDef { Value = 166, Name = "SetOTPushback",     ParamTypes = oneWord },

            new CFuncDef { Value = 167, Name = "SetCamZoom",        ParamTypes = twoWords },

            new CFuncDef { Value = 169, Name = "SetOTPushback2",    ParamTypes = oneWord },

            new CFuncDef { Value = 171, Name = "BackgroundCreate",  ParamTypes = new List<EParamType>() { EParamType.Hash, EParamType.Word, EParamType.Word, EParamType.Word } },
            
            new CFuncDef { Value = 178, Name = "SetRestart2",       ParamTypes = oneString },


            new CFuncDef { Value = 200, Name = "SetFadeColor",      ParamTypes = oneColor },
            new CFuncDef { Value = 201, Name = "GapPolyHit",        ParamTypes = new List<EParamType>() { EParamType.Hash, EParamType.Word } },
            new CFuncDef { Value = 202, Name = "SetSkyColor",       ParamTypes = oneColor },

            new CFuncDef { Value = 203, Name = "SetCareerFlag",     ParamTypes = oneWord },
            new CFuncDef { Value = 204, Name = "IfCareerFlag",      ParamTypes = mhpbGap },


            new CFuncDef { Value = 302, Name = "Assumed_CreateCareerItems",         ParamTypes = empty },

            new CFuncDef { Value = 65535, Name = "ListTerminator",  ParamTypes = empty },
            

        };

        public static CFuncDef FindByName(string name)
        {
            foreach (var func in Definitions)
                if (func.Name == name) 
                    return func;

            return null;
        }

        public static CFuncDef FindByValue(int value)
        {
            foreach (var func in Definitions)
                if (func.Value == value)
                    return func;

            return null;
        }
    }

    public class CommandList : List<Command>
    {
        public void Read(BinaryReader br)
        {
            while (true)
            {
                var cmd = new Command();
                cmd.Read(br);

                if (cmd.Function.Value == 65535)
                    break;

                this.Add(cmd);
            }
        }

        public void Write(BinaryWriter bw)
        {
            foreach (var cmd in this)
                cmd.Write(bw);
        }
        
        public void Parse(string text)
        {
            var lines = text.Replace("\r", "").Replace(";", "\n").Split('\n');

            foreach (var line in lines)
            {
                var buf = line;

                if (buf.Contains("//"))
                    buf = line.Substring(0, buf.IndexOf("//"));

                var cmd = Command.FromString(buf);
                if (cmd != null)
                    this.Add(cmd);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var cmd in this)
                sb.AppendLine(cmd.ToString());

            return sb.ToString();
        }
    }

    public enum EParamType
    {
        Word,       // a word (short)
        Dword,      // a double word (int)
        String,     // a string
        StringList, // a list of string, ending with 0
        Bbox,       // a bounding box
        BboxList,   // a list of bounding boxes, ending with 0xFF
        Hash,       // a string checksum hash
        BoolWord,   // a word, but limited to 0 and 1
        Color,      // two words in reverse order
    }

    public class Command
    {
        public CFuncDef Function;
        public List<string> Params = new List<string>();

        /// <summary>
        /// Reads a command from a binary stream.
        /// </summary>
        /// <param name="br"></param>
        public void Read(BinaryReader br)
        {
            int index = br.ReadUInt16();

            Function = CFuncDef.FindByValue(index);

            if (Function == null)
                throw new Exception($"not implemented {index} at {br.BaseStream.Position.ToString("X8")}");

            foreach (var entry in Function.ParamTypes)
                ReadParam(br, entry);
        }

        /// <summary>
        /// Reads a single param from a binary stream.
        /// </summary>
        /// <param name="br"></param>
        /// <param name="paramType"></param>
        /// <exception cref="Exception"></exception>
        public void ReadParam(BinaryReader br, EParamType paramType)
        {
            string param = "";

            switch (paramType)
            {
                case EParamType.Word:
                    param = br.ReadInt16().ToString();
                    break;

                case EParamType.BoolWord:
                    int value = br.ReadInt16();

                    if (value == 0) param = "false";
                    else if (value == 1) param = "true";
                    else throw new Exception($"Not a boolean param in {Function.Name}");
                    
                    break;

                case EParamType.Hash:
                    br.Pad();
                    param = br.ReadUInt32().ToString("X8");
                    break;

                case EParamType.String:
                    param = br.ReadTrgString();
                    break;

                case EParamType.StringList:

                    while (true)
                    {
                        string res = br.ReadTrgString();

                        if (res == "") return;

                        Params.Add(res);
                    }

                case EParamType.Color:
                    param = ((uint)(br.ReadUInt16() | br.ReadUInt16() << 16)).ToString("X8");
                    break;

                case EParamType.Dword:
                    br.Pad();
                    param = br.ReadInt32().ToString();
                    break;

                case EParamType.Bbox:
                    br.Pad();
                    param = $"({br.ReadInt32()}, {br.ReadInt32()}, {br.ReadInt32()}";
                    break;

                case EParamType.BboxList:

                    while (true)
                    {
                        // a weird way to end the list
                        // sometimes it may have no boxes at all...
                        var test = br.ReadInt16();

                        if (test == 255) break;
                        else
                        {
                            br.BaseStream.Position -= 2;
                            br.Pad();
                            param += $"[({br.ReadInt32()}, {br.ReadInt32()}, {br.ReadInt32()}), ({br.ReadInt32()}, {br.ReadInt32()}, {br.ReadInt32()})]\r\n";
                        }
                    }

                    break;

                default:
                    throw new Exception("unknown param type!");
            }

            Params.Add(param);
        }

        public void Write(BinaryWriter bw)
        {
            if (Function.NumParams != Params.Count)
                throw new Exception("Params count mismatch!");

            bw.Write(Function.Value);

            for (int i = 0; i < Function.NumParams; i++)
                WriteParam(bw, Function.ParamTypes[i], Params[i]);
        }

        public void WriteParam(BinaryWriter bw, EParamType paramType, string param)
        {
            switch (paramType) {
                case EParamType.Word:

                    short word = 0;

                    if (!Int16.TryParse(param, out word))
                        throw new Exception("failed to parse word!");

                    bw.Write(word);

                    break;

                case EParamType.BoolWord:
                    bool value = false;

                    if (!Boolean.TryParse(param, out value))
                        throw new Exception("failed to parse boolean!");

                    bw.Write(value ? (short) 1 : (short)0);

                    break;

                case EParamType.Hash:
                    uint hash = Convert.ToUInt32(param, 16);

                    bw.Pad();
                    bw.Write(hash);

                    break;

                case EParamType.String:
                    bw.WriteTrgString(param);
                    break;

                case EParamType.Color:
                    uint color = Convert.ToUInt32(param, 16);

                    bw.Write((ushort)(color & 0xFFFF));
                    bw.Write((ushort)((color >> 16) & 0xFFFF));

                    break;

                case EParamType.Dword:
                    int dword = Convert.ToInt32(param);
                    bw.Write(dword);
                    break;

                default:
                    throw new Exception("unknown param type!");
            }
        }

        /// <summary>
        /// Parses a source code string.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Command FromString(string text)
        {
            var cmd = new Command();

            text = text.Replace('(', ' ').Replace(')', ' ').Trim();

            if (String.IsNullOrEmpty(text))
                return null;

            var tokens = text.Split(' ', '\t');

            if (tokens.Length == 0)
                throw new Exception("Cmon.");

            cmd.Function = CFuncDef.FindByName(tokens[0]);

            if (cmd.Function is null)
                throw new Exception($"Unimplemented function: {tokens[0]}");

            for (int i = 1; i < tokens.Length; i++)
            {
                var param = tokens[i].Trim();

                if (!String.IsNullOrEmpty(param))
                    cmd.Params.Add(tokens[i]);
            }

            if (cmd.Params.Count != cmd.Function.NumParams)
                throw new Exception($"Passed {cmd.Params.Count} params to {cmd.Function.Name}, but require {cmd.Function.NumParams}");

            return cmd;
        }

        /// <summary>
        /// Generates a source code string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < Params.Count; i++)
            {
                sb.Append(Params[i]);

                if (i == Params.Count - 1)
                    break;

                sb.Append(", ");
            }

            return $"{Function.Name}({sb.ToString()});";
        }
    }
}
