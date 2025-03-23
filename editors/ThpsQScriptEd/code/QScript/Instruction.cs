using LegacyThps.QScript.Helpers;
using System.Collections.Generic;

namespace LegacyThps.QScript
{
    public class Instruction
    {
        public bool stop_parsing = false;
        public bool unimplemented = false;

        public QBcode code;

        public uint offset;


        public int data_int = 0;
        public uint data_uint = 0;
        public float data_float = 0;
        public string data_string = "";
        public Vector3f data_vector = Vector3f.Zero;

        public Instruction(BinaryReaderEx br)
        {
            offset = (uint)br.BaseStream.Position;
            ReadData(br);
        }


        //strings like 'Razor's Blade' wouldn't work otherwise -> 'Razor\'s Blade'
        public string EscapedString()
        {
            if (code == QBcode.val_string) return data_string.Replace("\"", "\\\"");
            if (code == QBcode.val_string_param) return data_string.Replace("'", "\\'");
            return data_string;
        }


        public void ReadData(BinaryReaderEx br)
        {
            code = (QBcode)br.ReadByte();

            if (code == QBcode.endfile)
            {
                if (br.BaseStream.Length - offset > 4)
                {
                    ThpsQScriptEd.MainForm.WarnUser("got terminator at: 0x" + offset.ToString("X8"));
                }
            }

            switch (code)
            {
                case QBcode.endfile:
                case QBcode.script:
                case QBcode.endscript:
                case QBcode.array:
                case QBcode.endarray:
                case QBcode.structure:
                case QBcode.endstructure:
                case QBcode.newline1:
                case QBcode.qbif:
                case QBcode.qbelse:
                case QBcode.qbelseif:
                case QBcode.qbendif:
                case QBcode.math_eq:
                case QBcode.global:
                case QBcode.globalall:
                case QBcode.roundopen:
                case QBcode.repeat:
                case QBcode.repeatend:
                case QBcode.repeatbreak:
                case QBcode.qbadd:
                case QBcode.qbsub:
                case QBcode.qbdiv:
                case QBcode.qbmul:
                case QBcode.property:
                case QBcode.member:
                case QBcode.unk_43:
                case QBcode.unk_44:
                case QBcode.unk_46:
                case QBcode.qbnot:
                case QBcode.qband:
                case QBcode.qbor:
                case QBcode.qbswitch:
                case QBcode.qbcase:
                case QBcode.qbendswitch:
                case QBcode.qbdefault:
                case QBcode.qbreturn:
                case QBcode.greater:
                case QBcode.greatereq:
                case QBcode.less:
                case QBcode.lesseq:
                case QBcode.comma:
                case QBcode.roundclose:

                    break;




                case QBcode.val_float:
                    {
                        data_float = br.ReadSingle();
                        break;
                    }

                case QBcode.randomjump:
                case QBcode.newline2:
                case QBcode.val_int:
                    {
                        data_int = br.ReadInt32();
                        break;
                    }

                case QBcode.symboldef:
                    {
                        data_uint = br.ReadUInt32();
                        data_string = br.ReadNTString();
                        break;
                    }


                case QBcode.symbol:
                    {
                        data_uint = br.ReadUInt32();
                        break;
                    }

                case QBcode.randomrange:
                case QBcode.randomrange2:
                    {
                        byte c = br.ReadByte();
                        if (c == (byte)QBcode.val_vector2)
                        {
                            data_vector = br.ReadVector2();
                        }
                        else
                        {
                            ThpsQScriptEd.MainForm.WarnUser("RandomRange got no vector2. much wow");
                        }

                        break;
                    }

                case QBcode.val_vector2:
                    {
                        data_vector = br.ReadVector2();
                        break;
                    }

                case QBcode.val_vector3:
                    {
                        data_vector = br.ReadVector3();
                        break;
                    }

                case QBcode.random:
                case QBcode.random2:
                case QBcode.randomnorepeat:
                case QBcode.randompermute:
                    {
                        int numEntries = br.ReadInt32();

                        //now we have to determine whether we're thps or thug
                        //we could do this with a user flag
                        //but we don't want to fail due to incorrect user settings, right?

                        long pos = br.BaseStream.Position;
                        bool isthugrandom = true;

                        for (int i = 0; i < numEntries; i++)
                        {
                            short s = br.ReadInt16();
                            if (s <= 0 || s >= 9) isthugrandom = false;

                            //dafuq? 
                            //we basically assume that thug vals are never zero or above 10. 
                            //this might not be always true.
                            //this might be totally wrong
                            //ask someone
                        }

                        //if failed, go back
                        if (!isthugrandom) br.Jump(pos);
                        else QB.SetFormat(QBFormat.THUG1);


                        //then read pointers to code blocks 
                        var jumpTable = new List<int>();

                        for (int i = 0; i < numEntries; i++)
                            jumpTable.Add(br.ReadInt32());


                        //so now let's jump the last entry and get the closing address for this random
                        //i wonder if it works for random in random. it actually should

                        long f = br.BaseStream.Position;

                        br.Skip(jumpTable[numEntries - 1] - 5); //5 is the size of randomjump opcode

                        if (br.ReadByte() == (byte)QBcode.randomjump)
                        {
                            int x = br.ReadInt32();
                            requestBracketAt = (int)(br.BaseStream.Position + x);
                        }
                        br.Jump(f);

                        break;
                    }


                case QBcode.val_string:
                case QBcode.val_string_param:
                    {
                        data_int = br.ReadInt32();
                        data_string = br.ReadFixedString(data_int);
                        break;
                    }

                default:
                    {
                        unimplemented = true;

                        ThpsQScriptEd.MainForm.WarnUser(
                            "Unknown opcode: " + ((byte)code).ToString("X2") +
                            " at 0x" + (br.BaseStream.Position - 1).ToString("X8") +
                            "\r\nParsing failed."
                        );

                        QB.HaltParsing();

                        break;
                    }
            }
        }


        public int requestBracketAt = -1;


        //formatting params
        public bool spaceAfter = true;
        public bool increaseIndent = false;
        public bool decreaseIndent = false;
        public bool newlineBefore = false;
        public bool nextGlobal = false;

        public string GetSyntax()
        {
            switch (code)
            {
                case QBcode.endfile: return "";

                case QBcode.script: newlineBefore = true; increaseIndent = true; return "script";
                case QBcode.endscript: decreaseIndent = true; return "endscript";
                case QBcode.array: increaseIndent = true; return "[";
                case QBcode.endarray: decreaseIndent = true; return "]";
                case QBcode.structure: increaseIndent = true; return "{";
                case QBcode.endstructure: decreaseIndent = true; return "}";
                case QBcode.newline1: spaceAfter = false; return "\r\n";
                case QBcode.newline2: spaceAfter = false; return "\r\n";

                case QBcode.roundopen: return "(";
                case QBcode.roundclose: return ")";

                case QBcode.comma: return ",";

                case QBcode.qbadd: return "+";
                case QBcode.qbsub: return "-";
                case QBcode.qbdiv: return "/";
                case QBcode.qbmul: return "*";
                case QBcode.greater: return ">";
                case QBcode.greatereq: return ">=";
                case QBcode.less: return "<";
                case QBcode.lesseq: return "<=";

                case QBcode.property: return ".";

                case QBcode.member: return ":";

                case QBcode.qbnot: return "not";
                case QBcode.qband: return "and";
                case QBcode.qbor: return "or";

                case QBcode.qbreturn: return "return";

                case QBcode.qbswitch: return "switch";
                case QBcode.qbcase: return "case";
                case QBcode.qbendswitch: return "endswitch";
                case QBcode.qbdefault: return "default";

                case QBcode.random: return "Random (@";
                case QBcode.random2: return "Random2 (@";
                case QBcode.randomnorepeat: return "RandomNoRepeat (@";
                case QBcode.randompermute:  return "RandomPermute (@";
                case QBcode.randomrange: return "RandomRange" + data_vector.ToString();
                case QBcode.randomrange2: return "RandomRange2" + data_vector.ToString();
                case QBcode.randomjump: return "@";

                case QBcode.repeat: increaseIndent = true; return "begin";
                case QBcode.repeatend: decreaseIndent = true; return "repeat";
                case QBcode.repeatbreak: return "break";

                case QBcode.val_vector2: return data_vector.ToString();
                case QBcode.val_vector3: return data_vector.ToString();

                case QBcode.globalall: return "<...>";

                case QBcode.qbif: increaseIndent = true; return "if";
                case QBcode.qbelse: increaseIndent = true; decreaseIndent = true; return "else";
                case QBcode.qbelseif: return "elseif";
                case QBcode.qbendif: decreaseIndent = true; return "endif";

                case QBcode.global: spaceAfter = false; nextGlobal = true; return "";

                case QBcode.math_eq:
                    //if (!Settings.Default.equalsWantsSpace) spaceAfter = false; 
                    return "=";

                case QBcode.val_float: return data_float.ToString("0.0#####");
                case QBcode.val_int: return data_int.ToString();
                case QBcode.symbol:
                    {
                        string res = SymbolCache.GetSymbolName(data_uint);

                        bool isKeyword = false;

                        foreach (QToken q in QBuilder.tokens)
                        {
                            if (res.ToLower() == q.Name.ToLower())
                                isKeyword = true;
                        }

                        if (
                                res.Contains(" ") ||
                                res.Contains("'") ||
                                res.Contains("\"") ||
                                isKeyword
                            )
                        {
                            return "#\"" + res + '"';
                        }
                        else
                        {
                            return res;
                        }
                    }
                case QBcode.symboldef: spaceAfter = false; return ""; //do nothing really as it build up automatically

                case QBcode.val_string: return "\"" + EscapedString() + "\"";
                case QBcode.val_string_param: return "\'" + EscapedString() + "\'"; //check ns code



                default:
                    {
                        unimplemented = true;
                        ThpsQScriptEd.MainForm.WarnUser(code.ToString() + " " + offset.ToString("X8"));
                        return "[unk_" + ((byte)code).ToString("X2") + "]";
                    }
            }
        }


        public bool isSymbolDef()
        {
            return code == QBcode.symboldef;
        }

        public bool isNewLine()
        {
            return code == QBcode.newline1 || code == QBcode.newline2;
        }

        public bool isTerminator()
        {
            return code == QBcode.endfile;
        }


        public override string ToString()
        {
            return
                code.ToString().PadRight(16, ' ') + "\t" +
                data_int + "\t" +
                "0x" + data_uint.ToString("X8") + "\t" +
                data_float + "\t" +
                data_string.Replace("\0", "") + "\r\n";
        }

    }
}
