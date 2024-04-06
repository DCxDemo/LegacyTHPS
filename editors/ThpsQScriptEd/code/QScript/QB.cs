using LegacyThps.QScript.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Settings = ThpsQScriptEd.Properties.Settings;

namespace LegacyThps.QScript
{
    public class QB
    {

        public string filename = "";

        List<Instruction> inst = new List<Instruction>();
        string text = "";


        public List<string> scripts = new List<string>();



        private static QBFormat qbformat = QBFormat.THPS3;
        public static void SetFormat(QBFormat qb) { qbformat = qb; }


        public QB()
        {
            QBuilder.Init();
            QB.SetFormat(QBFormat.THPS3);
            SymbolCache.Create();
        }

        private static bool halt = false;

        public static void HaltParsing() { halt = true; }
        public static void ResetHalt() { halt = false; }
        public static bool ShouldHalt() { return halt; }


        public QB(string s)
        {
            ResetHalt();

            filename = s;

            //create cache
            SymbolCache.Create();

            //check if maybe we have .sym.qb file alongside
            string symqb = Path.ChangeExtension(s, "sym.qb");

            if (File.Exists(symqb))
            {
                QB q = new QB(symqb);
                q = null;
            }


            byte[] data = File.ReadAllBytes(s);
            MemoryStream ms = new MemoryStream(data);
            BinaryReaderEx br = new BinaryReaderEx(ms);

            Instruction i;
            bool unimplemented = false;

            do
            {
                i = new Instruction(br);

                if (i.unimplemented) unimplemented = true;

                if (i.isSymbolDef()) SymbolCache.Add(i);

                if (!ShouldHalt()) inst.Add(i);
                else return;
            }
            while (!i.isTerminator() || ShouldHalt()); //i'll be back

            if (unimplemented) System.Windows.Forms.MessageBox.Show("Warning! This file contains unimplemented commands.");
        }


        public string Decompile()
        {
            //inst -> text
            bool unimplemented = false;

            StringBuilder sb = new StringBuilder();

            int indent = 0;
            int indentStep;
            char padding;

            if (Settings.Default.useTab)
            {
                indentStep = 1;
                padding = '\t';
            }
            else
            {
                indentStep = 2;
                padding = ' ';
            }

            bool gotNewLine = true;
            bool gotGlobal = false;


            int randomclose = -1;

            scripts.Clear();

            bool dumpScriptName = false;
            bool isEndScript = false;



            foreach (Instruction i in inst)
            {
                if (dumpScriptName)
                {
                    scripts.Add(SymbolCache.GetSymbolName(i.data_uint));
                    dumpScriptName = false;
                }

                if (i.code == QBcode.script) dumpScriptName = true;

                string syntax = i.GetSyntax();

                if (i.requestBracketAt != -1)
                {
                    randomclose = i.requestBracketAt;
                }
                else
                {
                    if (randomclose == i.offset)
                    {
                        syntax = ")\r\n";
                    }
                }


                if (i.decreaseIndent)
                    indent -= indentStep;

                if (indent < 0)
                {
                    System.Windows.Forms.MessageBox.Show("negative indent detected. something is wrong here:\r\n" + i.code.ToString() + " at " + i.offset.ToString("X8"));
                    indent = 0;
                }


                if (i.isNewLine() || gotGlobal) gotNewLine = true;

                string result =
                    ((i.newlineBefore) ? "\r\n" : "") +
                    ((gotNewLine) ? "".PadLeft(indent, padding) : "") +
                    ((gotGlobal) ? "<" : "") +
                    syntax +
                    ((i.code == QBcode.endscript) ? "  //" + scripts[scripts.Count - 1] : "") +
                    ((gotGlobal) ? ">" : "") +
                    ((i.spaceAfter) ? " " : "")
                    ;


                // System.Windows.Forms.MessageBox.Show("[" + result + "]");

                if (result.Replace(" ", "") != "")
                    sb.Append(result);


                gotGlobal = false;
                if (i.nextGlobal) gotGlobal = true;


                if (i.increaseIndent)
                    indent += indentStep;


                if (!i.isNewLine() && gotNewLine) gotNewLine = false;

                if (i.unimplemented) unimplemented = true;
            }




            if (unimplemented) System.Windows.Forms.MessageBox.Show("This file contains unimplemented commands.\r\n" + filename, "Warning");


            text = sb.ToString();

            return text;
        }


        List<uint> hashesToDump;

        void Dump(uint i)
        {
            if (!hashesToDump.Contains(i))
                hashesToDump.Add(i);
        }

        public void Compile()
        {
            hashesToDump = new List<uint>();
            //text -> inst

            string kext = text;
            kext = kext.Replace("\t", " ");
            string kextold = kext;

            /*
            do
            {
                kextold = kext;
                kext = kext.Replace("  ", " ");
            }
            while (kextold != kext);
            */

            kext = kext.Replace("\r\n", "\n");

            do
            {
                kextold = kext;
                kext = kext.Replace(" \n", "\n").Replace("\n ", "\n");
            }
            while (kextold != kext);





            byte[] finalbytes = new byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(stream))
                {
                    string buf = "";

                    bool instring = false;
                    bool inparamstring = false;

                    bool maybeComment = false;
                    bool isComment = false;
                    bool ishash = false;

                    bool isEscape = false;

                    bool newlineExists = false;


                    for (int i = 0; i < kext.Length; i++)
                    {
                        char c = kext[i];

                        if (isComment)
                        {
                            if (c == '\n') isComment = false;
                        }
                        else if (instring)
                        {
                            if (c != '"')
                            {
                                buf += c;
                            }
                            else
                            {
                                if (!ishash)
                                {
                                    instring = false;
                                    WriteString(buf, bw);
                                }
                                else
                                {
                                    instring = false;
                                    ishash = false;

                                    bw.Write((byte)QBcode.symbol);

                                    uint crc = Checksum.Calc(buf);
                                    Dump(crc);

                                    bw.Write(crc);
                                    SymbolCache.Add(buf);
                                }

                                buf = "";
                            }
                        }


                        else if (inparamstring)
                        {
                            if (!isEscape)
                            {
                                if (c != '\'')
                                {
                                    if (c == '\\')
                                    {
                                        isEscape = true;
                                    }
                                    else
                                    {
                                        buf += c;
                                    }
                                }
                                else
                                {
                                    inparamstring = false;
                                    WriteParamString(buf, bw);
                                    buf = "";
                                }

                            }
                            else
                            {
                                if (isEscape) isEscape = false;
                                buf += c;
                            }
                        }


                        else
                        {
                            if (c == '\n' && newlineExists)
                            {
                                //do nothing 
                            }
                            else
                            {
                                newlineExists = false;
                                switch (c)
                                {
                                    case '\n': if (buf != "") AnalyzeWord(buf, bw); buf = ""; WriteNewLine(bw); newlineExists = true; break;
                                    case '=': if (buf != "") AnalyzeWord(buf, bw); buf = ""; WriteInst(bw, QBcode.math_eq); break;
                                    case ' ': if (buf != "") AnalyzeWord(buf, bw); buf = ""; break;
                                    case '{': if (buf != "") AnalyzeWord(buf, bw); buf = ""; WriteInst(bw, QBcode.structure); break;
                                    case '}': if (buf != "") AnalyzeWord(buf, bw); buf = ""; WriteInst(bw, QBcode.endstructure); break;
                                    case '[': if (buf != "") AnalyzeWord(buf, bw); buf = ""; WriteInst(bw, QBcode.array); break;
                                    case ']': if (buf != "") AnalyzeWord(buf, bw); buf = ""; WriteInst(bw, QBcode.endarray); break;
                                    case '!': if (buf != "") AnalyzeWord(buf, bw); buf = ""; WriteInst(bw, QBcode.qbnot); break;
                                    case '#': ishash = true; break;//we're in string
                                    case '"': instring = true; break;//we're in string
                                    case '\'': inparamstring = true; break;//we're in string
                                    case '/':
                                        {
                                            if (buf != "") AnalyzeWord(buf, bw);

                                            if (!maybeComment)
                                            {
                                                maybeComment = true;
                                            }
                                            else
                                            {
                                                maybeComment = false;
                                                isComment = true;
                                            }
                                            break;
                                        }
                                    case ';': if (buf != "") AnalyzeWord(buf, bw); isComment = true; break;

                                    default: buf += c; break;
                                }
                            }
                        }
                    }

                    if (buf != "") AnalyzeWord(buf, bw);


                    if (!Settings.Default.useSymFile)
                    {
                        foreach (uint i in hashesToDump)
                            WriteSymbolDef(i, SymbolCache.GetSymbolName(i), bw);
                    }
                    else
                    {
                        byte[] finalsymbytes = new byte[0];

                        using (MemoryStream streamsym = new MemoryStream())
                        {
                            using (BinaryWriter bwsym = new BinaryWriter(streamsym))
                            {
                                foreach (uint i in hashesToDump)
                                    WriteSymbolDef(i, SymbolCache.GetSymbolName(i), bwsym);

                                bwsym.Write((byte)0);

                                streamsym.Flush();
                                finalsymbytes = streamsym.GetBuffer();
                                Array.Resize(ref finalsymbytes, (int)bwsym.BaseStream.Position);

                                File.WriteAllBytes(Path.ChangeExtension(filename, ".sym.qb"), finalsymbytes);
                                ThpsQScriptEd.MainForm.WarnUser(Path.ChangeExtension(filename, ".sym.qb"));
                            }
                        }
                    }

                    bw.Write((byte)0);

                    stream.Flush();
                    finalbytes = stream.GetBuffer();

                    //removing trailing zeroes here
                    Array.Resize(ref finalbytes, (int)bw.BaseStream.Position);
                }
            }

            //System.Windows.Forms.MessageBox.Show("gonna save binary to " + filename);
            File.WriteAllBytes(Path.ChangeExtension(filename, "qb"), finalbytes);

            hashesToDump.Clear();
        }

        public void WriteString(string s, BinaryWriter bw)
        {
            WriteInst(bw, QBcode.val_string);
            bw.Write(s.Length + 1);
            bw.Write(System.Text.Encoding.Default.GetBytes(s));
            bw.Write((byte)0);
        }

        public void WriteParamString(string s, BinaryWriter bw)
        {
            WriteInst(bw, QBcode.val_string_param);
            bw.Write(s.Length + 1);
            bw.Write(System.Text.Encoding.Default.GetBytes(s));
            bw.Write((byte)0);
        }

        public void WriteSymbolDef(uint i, string s, BinaryWriter bw)
        {
            WriteInst(bw, QBcode.symboldef);
            bw.Write(i);
            bw.Write(System.Text.Encoding.Default.GetBytes(s));
            bw.Write((byte)0);
        }

        public void WriteFloat(float f, BinaryWriter bw)
        {
            WriteInst(bw, QBcode.val_float);
            bw.Write(f);
        }

        public void WriteInt(int i, BinaryWriter bw)
        {
            WriteInst(bw, QBcode.val_int);
            bw.Write(i);
        }


        public bool maybeFloat(string w)
        {
            string allowed = "0123456789-.";

            foreach (char c in w)
            {
                if (!allowed.Contains(c.ToString()))
                    return false;
            }

            return true;
        }


        public bool maybeInt(string w)
        {
            //it's a very loose check. "123-456" is a number. much wow.

            string allowed = "0123456789-";

            foreach (char c in w)
            {
                if (!allowed.Contains(c.ToString()))
                    return false;
            }

            return true;
        }




        public void AnalyzeWord(string word, BinaryWriter bw)
        {
            string w = word.ToString().Trim().ToLower();

            if (w == "") return;

            if (maybeInt(w))
            {
                WriteInt(Int32.Parse(w), bw);
                return;
            }

            if (maybeFloat(w))
            {
                //we're sure it's float
                WriteFloat(Single.Parse(w), bw);
                return;
            }

            //keywords

            if (w.ToLower() == "script")
            {
                WriteInst(bw, QBcode.script);
                return;
            }

            if (w.ToLower() == "endscript")
            {
                WriteInst(bw, QBcode.endscript);
                return;
            }

            if (w.ToLower() == "begin")
            {
                WriteInst(bw, QBcode.repeat);
                return;
            }

            if (w.ToLower() == "repeat")
            {
                WriteInst(bw, QBcode.repeatend);
                return;
            }

            if (w.ToLower() == "break")
            {
                WriteInst(bw, QBcode.repeatbreak);
                return;
            }

            if (w.ToLower() == "if")
            {
                WriteInst(bw, QBcode.qbif);
                return;
            }

            if (w.ToLower() == "endif")
            {
                WriteInst(bw, QBcode.qbendif);
                return;
            }

            if (w.ToLower() == "elseif")
            {
                WriteInst(bw, QBcode.qbelseif);
                return;
            }

            if (w.ToLower() == "else") { WriteInst(bw, QBcode.qbelse); return; }
            if (w.ToLower() == "return") { WriteInst(bw, QBcode.qbreturn); return; }
            if (w.ToLower() == "switch") { WriteInst(bw, QBcode.qbswitch); return; }
            if (w.ToLower() == "case") { WriteInst(bw, QBcode.qbcase); return; }
            if (w.ToLower() == "endswitch") { WriteInst(bw, QBcode.qbendswitch); return; }
            if (w.ToLower() == "default") { WriteInst(bw, QBcode.qbdefault); return; }


            if (w.ToLower() == "not" || w == "!")
            {
                WriteInst(bw, QBcode.qbnot);
                return;
            }

            if (w.ToLower() == "and" || w == "&&")
            {
                WriteInst(bw, QBcode.qband);
                return;
            }

            if (w.ToLower() == "or" || w == "||")
            {
                WriteInst(bw, QBcode.qbor);
                return;
            }



            //math stuff

            if (w == "<")
            {
                WriteInst(bw, QBcode.less);
                return;
            }

            if (w == "<=")
            {
                WriteInst(bw, QBcode.lesseq);
                return;
            }

            if (w == ">")
            {
                WriteInst(bw, QBcode.greater);
                return;
            }

            if (w == ">=")
            {
                WriteInst(bw, QBcode.greatereq);
                return;
            }

            if (w == "+")
            {
                WriteInst(bw, QBcode.qbadd);
                return;
            }

            if (w == "-")
            {
                WriteInst(bw, QBcode.qbsub);
                return;
            }

            if (w == "*")
            {
                WriteInst(bw, QBcode.qbmul);
                return;
            }

            if (w == "/")
            {
                WriteInst(bw, QBcode.qbdiv);
                return;
            }

            if (w == "[unk_41]")
            {
                WriteInst(bw, QBcode.unk_41);
                return;
            }

            //this won't work.
            if (w == ":")
            {
                WriteInst(bw, QBcode.member);
                return;
            }

            if (w == ".")
            {
                WriteInst(bw, QBcode.property);
                return;
            }

            if (w == ">")
            {
                WriteInst(bw, QBcode.greater);
                return;
            }

            if (w == "[unk_43]")
            {
                WriteInst(bw, QBcode.unk_43);
                return;
            }

            if (w == "[unk_44]")
            {
                WriteInst(bw, QBcode.unk_44);
                return;
            }

            if (w == "[unk_46]")
            {
                WriteInst(bw, QBcode.unk_46);
                return;
            }

            if (w == "<...>")
            {
                WriteInst(bw, QBcode.globalall);
                return;
            }


            if (w.ToLower() == "random")
            {
                ThpsQScriptEd.MainForm.WarnUser("Yo!\r\nRandom is not implemented yet.\r\nThis file won't work in game.");
                return;
            }

            if (w[0] == '<' && w[w.Length - 1] == '>')
            {
                //got global!
                WriteInst(bw, QBcode.global);
                WriteInst(bw, QBcode.symbol);
                word = word.Trim('<').Trim('>');
                bw.Write(Checksum.Calc(word));
                Dump(Checksum.Calc(word));
                SymbolCache.Add(word);
                return;
            }

            WriteInst(bw, QBcode.symbol);
            bw.Write(Checksum.Calc(word));
            Dump(Checksum.Calc(word));
            SymbolCache.Add(word);
        }

        public void WriteNewLine(BinaryWriter bw)
        {
            if (!Settings.Default.useShortLine)
            {
                bw.Write((byte)QBcode.newline2);
                bw.Write((int)0);
            }
            else
            {
                bw.Write((byte)QBcode.newline1);
            }
        }

        public void WriteInst(BinaryWriter bw, QBcode code)
        {
            bw.Write((byte)code);
        }


        public void UpdateText(string s)
        {
            text = s;
        }


        public void SaveText(string txtname)
        {
            File.WriteAllText(txtname, text);
        }

        public void Save()
        {
            string targetq = Path.ChangeExtension(filename, "q");

            //so if we got q already, make a backup
            if (Settings.Default.enableBackups)
                if (File.Exists(targetq))
                    File.Copy(targetq, Path.ChangeExtension(filename, "." + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".q"));

            SaveText(targetq);
        }
    }

}
