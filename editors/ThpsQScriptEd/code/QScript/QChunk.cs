using LegacyThps.QScript.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using ThpsQScriptEd;
using Settings = ThpsQScriptEd.Properties.Settings;

namespace LegacyThps.QScript
{
    public class QChunk
    {
        public QToken code;

        public QBcode QType { get { return (QBcode)code.Code; } }

        public int offset;

        // C++ uses union for this stuff
        public short data_short = 0;
        public int data_int = 0;
        public uint data_uint = 0;
        public float data_float = 0;
        public string data_string = "";
        public Vector3 data_vector = Vector3.Zero;

        public bool isAngle = false;
        public bool isVector2 = false;

        //this is to get float for vector
        public float GetNumericValue()
        {
            if (data_float != 0) return data_float;
            return data_int;
        }

        //data for random
        public List<short> thugstuff = new List<short>();
        public List<int> ptrs = new List<int>();

        public bool randomMarker = false;



        //strings like 'Razor's Blade' wouldn't work otherwise -> 'Razor\'s Blade'
        public string EscapedString()
        {
            if (QType == QBcode.val_string) return data_string.Replace("\"", "\\\"");
            if (QType == QBcode.val_string_param) return data_string.Replace("'", "\\'");
            return data_string;
        }



        public QChunk(QToken c)
        {
            code = c;
        }

        public QChunk(QBcode c)
        {
            code = QBuilder.GetCode(c);
        }

        public static int closeRandomAt = -1;

        public static int maxRandomValue = 0;

        public QChunk(BinaryReaderEx br, QToken c)
        {
            code = c;

            offset = (int)br.BaseStream.Position - 1;
            if (offset == QChunk.closeRandomAt) randomMarker = true;
            //QScripted.MainForm.Warn(c.Code.ToString("X8"));

            //if (QType == QBcode.random) QScripted.MainForm.Warn("random at: " +offset.ToString("X8"));

            switch (c.Group)
            {
                case DataGroup.Unknown:
                case DataGroup.Empty: break;

                case DataGroup.Short: data_short = br.ReadInt16(); break;
                case DataGroup.Int: data_int = br.ReadInt32(); break;
                case DataGroup.Uint:
                    data_uint = br.ReadUInt32();
                    break;
                case DataGroup.Float: data_float = br.ReadSingle(); break;
                case DataGroup.Vector2: data_vector = br.ReadVector2(); isVector2 = true; break;
                case DataGroup.Vector3: data_vector = br.ReadVector3(); isVector2 = false; break;
                case DataGroup.FixedString:
                    {
                        data_int = br.ReadInt32();
                        data_string = br.ReadFixedString(data_int);
                        break;
                    }
                case DataGroup.Random:
                    {
                        data_int = br.ReadInt32();

                        long pos = br.BaseStream.Position;

                        //bool isthugrandom = true;

                        // read extra thug+ table
                        if (Settings.Default.minQBLevel >= (byte)QBFormat.THUG1)
                            for (int i = 0; i < data_int; i++)
                                thugstuff.Add(br.ReadInt16());

                        /*
                        for (int i = 0; i < data_int; i++)
                        {
                            short s = br.ReadInt16();
                            if (s <= 0 || s >= 64) isthugrandom = false;

                            if (maxRandomValue < s) maxRandomValue = s;

                            //dafuq? 
                            //we basically assume that thug vals are never zero or above 10. 
                            //this might not be always true.
                            //this might be totally wrong
                            //ask someone

                            //could it be random chance? like 2 entries with 9 and 1 leads to 90% and 10%?
                        }

                        if (!isthugrandom) br.Jump(pos);
                        else QBuilder.SetQBLevel(QBFormat.THUG1);
                        */

                        // read jump table
                        for (int i = 0; i < data_int; i++)
                            ptrs.Add(br.ReadInt32());

                        long f = br.BaseStream.Position;

                        if (ptrs[data_int - 1] - 5 > 0) //this is a dumb fix for empty random
                        {
                            br.Skip(ptrs[data_int - 1] - 5); //5 is the size of randomjump opcode
                            if (br.ReadByte() == (byte)QBcode.randomjump)
                            {
                                int x = br.ReadInt32();
                                closeRandomAt = (int)(br.BaseStream.Position + x);
                            }
                        }
                        else
                        {
                            closeRandomAt = (int)br.BaseStream.Position;
                        }

                        br.Jump(f);

                        break;
                    }

                case DataGroup.SymbolDef:
                    {
                        data_uint = br.ReadUInt32();
                        data_string = br.ReadNTString();

                        SymbolCache.Add(data_uint, data_string);

                        /*
                         if (SymbolCache.Get(data_uint) != data_string && SymbolCache.Get(data_uint).ToLower() == data_string.ToLower())
                             QScripted.MainForm.Warn("case mismatch!\r\n" + SymbolCache.Get(data_uint) + "\r\n" + data_string);

                         if (SymbolCache.Get(data_uint) != data_string)
                             QScripted.MainForm.Warn("symbol mismatch!\r\n" + SymbolCache.Get(data_uint) + "\r\n" + data_string);
                         */

                        break;
                    }
                default: ThpsQScriptEd.MainForm.WarnUser("unimplemented datagroup!: " + code.Group); break;
            }
        }


        /// <summary>
        /// Converts current chunk to a text string.
        /// </summary>
        /// <param name="debug"></param>
        /// <returns></returns>
        public string ToString(bool debug)
        {
            string result = "[unparsed! " + code.Code.ToString("x2") + "]";

            switch (code.Logic)
            {
                case OpLogic.Reserved:
                case OpLogic.Global:
                case OpLogic.Halt:
                case OpLogic.Linefeed:
                case OpLogic.RegionBegin:
                case OpLogic.RegionEnd:
                case OpLogic.Math:
                case OpLogic.Logic:
                case OpLogic.Relation:
                case OpLogic.Separator:
                    result = code.GetSyntax();
                    break;

                case OpLogic.Keyword:
                    result = code.GetSyntax();
                    if (Settings.Default.useCaps) result = result.ToUpper();
                    break;

                case OpLogic.Numeric:
                    switch (code.Group)
                    {
                        case DataGroup.Int: result = data_int.ToString(); break;
                        case DataGroup.Float: result = data_float.ToString("0.0####"); break;
                        default: result = code.GetSyntax(); MainForm.WarnUser("Unknown numeric entry!\r\n" + result); break;
                    }
                    break;

                case OpLogic.String:
                    result = code.GetSyntax() + EscapedString() + code.GetSyntax();
                    break;

                case OpLogic.Symbol:
                    // get string from global symbol cache
                    result = SymbolCache.GetSymbolName(data_uint);

                    // loop through keywords to find if symbol is a keyword (like "not" or "switch" for example)

                    bool isKeyword = false;

                    foreach (var q in QBuilder.tokens)
                    {
                        if (result.ToLower() == q.Name.ToLower() &&
                            (q.Logic == OpLogic.Keyword || q.Logic == OpLogic.Logic)
                            )
                            isKeyword = true;
                    }

                    // find if symbol is all numbers. without hash it will be treated as int.

                    bool isNumeric = true;
                    string digits = "0123456789";

                    foreach (char c in result.ToCharArray())
                    {
                        if (!digits.Contains(c))
                            isNumeric = false;
                    }

                    // add hash symbol if got one of these cases
                    if (
                            result.Contains(" ") ||
                            result.Contains("'") ||
                            result.Contains("-") ||
                            result.Contains("\"") ||
                            isKeyword ||
                            isNumeric
                        )
                    {
                        result = $"#\"{result}\"";
                    }
                    break;

                case OpLogic.SymbolDef:
                    // this is a syboldef entry, we don't need it to be in the resulting code
                    result = "";
                    // result = "#\"" + data_string + "\"";
                    break;

                case OpLogic.Random:
                    result = code.GetSyntax() + "( @";
                    break;

                case OpLogic.Vector:
                    result = VectorToString();
                    break;

                case OpLogic.Unknown:
                    MainForm.WarnUser("unknown code found: " + code.GetSyntax());
                    result = code.GetSyntax();
                    break;

                default: MainForm.WarnUser("unimplemented logic: " + code.Logic); break;

            }

            return (debug)
                ? "(" + code.Code.ToString("X2") + ") " +
                ((code.Code == 0x02) ? data_int.ToString("X8") : "")
                + result + " -> "
                : result;
        }

        public static double Radian = 180.0 / Math.PI;

        public string VectorToString()
        {
            if (!isAngle || !Settings.Default.useDegrees)
            {
                return
                    "(" + data_vector.X.ToString("0.#####") +
                    ", " + data_vector.Y.ToString("0.#####") +
                    ((!isVector2) ? (", " + data_vector.Z.ToString("0.#####")) : "") + ")";
            }
            else
            {
                double fx = data_vector.X * Radian;
                double fy = data_vector.Y * Radian;
                double fz = data_vector.Z * Radian;

                //without it most 180 will be like 180.00001
                if (Settings.Default.roundAngles)
                {
                    fx = Math.Round(fx, 2);
                    fy = Math.Round(fy, 2);
                    fz = Math.Round(fz, 2);
                }

                return
                    "(" + fx.ToString("0.#####") +
                    "°, " + fy.ToString("0.#####") +
                    ((!isVector2) ? ("°, " + fz.ToString("0.#####")) : "") + "°)";
            }
        }


        /// <summary>
        /// Retrieves the resulting chunk size based on data group.
        /// </summary>
        /// <returns></returns>
        public int GetSize()
        {
            switch (code.Group)
            {
                case DataGroup.Short: return 1 + 2; //single word

                case DataGroup.Uint:
                case DataGroup.Float:
                case DataGroup.Int: return 1 + 4; //single dword

                case DataGroup.Vector2: return 1 + 4 + 4;
                case DataGroup.Vector3: return 1 + 4 + 4 + 4;

                case DataGroup.FixedString:
                case DataGroup.SymbolDef: return 1 + 4 + data_string.Length + 1; //null terminated

                case DataGroup.Random:
                    if (QBuilder.currentQBlevel < QBFormat.THUG1)
                        return 1 + 4 + ptrs.Count * 4;
                    else
                        return 1 + 4 + ptrs.Count * 2 + ptrs.Count * 4;

                case DataGroup.Unknown:
                    MessageBox.Show($"Unknown data group! {code.Group} at {QBuilder.lineNumber}");
                    return 1;

                case DataGroup.Empty:
                default: return 1;
            }
        }

        /// <summary>
        /// Writes opcode data using a provided binary writer.
        /// </summary>
        /// <param name="bw">Binary writer instance.</param>
        public void Write(BinaryWriter bw)
        {
            bw.Write(code.Code);

            switch (code.Group)
            {
                default:
                case DataGroup.Unknown:
                case DataGroup.Empty: break;

                case DataGroup.FixedString:
                    // account for terminating 0
                    bw.Write(data_string.Length + 1);
                    bw.Write(System.Text.Encoding.Default.GetBytes(data_string));
                    bw.Write((byte)0);
                    break;

                case DataGroup.SymbolDef: bw.Write(data_uint); bw.Write(System.Text.Encoding.Default.GetBytes(data_string)); bw.Write((byte)0); break;

                case DataGroup.Short: bw.Write(data_short); break;
                case DataGroup.Uint: bw.Write(data_uint); break;
                case DataGroup.Float: bw.Write(data_float); break;
                case DataGroup.Int: bw.Write(data_int); break;

                case DataGroup.Vector2:
                    bw.Write(data_vector.X);
                    bw.Write(data_vector.Y);
                    break;

                case DataGroup.Vector3:
                    bw.Write(data_vector.X);
                    bw.Write(data_vector.Y);
                    bw.Write(data_vector.Z);
                    break;

                case DataGroup.Random:
                    bw.Write(ptrs.Count());

                    // if we're in thug+ mode
                    if (QBuilder.currentQBlevel >= QBFormat.THUG1)
                        for (int i = 0; i < ptrs.Count; i++)
                            bw.Write((short)1);

                    foreach (int pt in ptrs)
                        bw.Write(pt);

                    break;
            }
        }
    }
}