using LegacyThps.QScript.Helpers;
using LegacyThps.QScript.Nodes;
using ThpsQScriptEd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Settings = ThpsQScriptEd.Properties.Settings;
using System.Reflection.Emit;

namespace LegacyThps.QScript
{
    public class QBuilder
    {
        public static QBFormat currentQBlevel;

        public static void SetQBLevel(QBFormat format)
        {
            if ((byte)format > (byte)currentQBlevel)
            {
                currentQBlevel = format;
            }

            if ((byte)format < (byte)Settings.Default.minQBLevel)
            {
                currentQBlevel = (QBFormat)Settings.Default.minQBLevel;
            }
        }

        public static void ForceQBLevel(QBFormat format)
        {
            currentQBlevel = format;
        }



        public static List<QToken> tokens = new List<QToken>();
        //static Dictionary<byte, string> syntax = new Dictionary<byte, string>();


        static private string path;

        static List<QChunk> chunks = new List<QChunk>();
        static List<QChunk> symbols = new List<QChunk>();

        static List<uint> nodes = new List<uint>();

        /// <summary>
        /// Find QToken by QBcode.
        /// </summary>
        /// <param name="oldcode"></param>
        /// <returns></returns>
        public static QToken GetCode(QBcode oldcode)
        {
            return FindCode((byte)oldcode);
        }

        public static QToken FindCode(byte value)
        {
            foreach (var q in tokens)
            {
                if (q.Code == value)
                    return q;
            }

            MainForm.WarnUser($"FindCode missing token {value} at {lineNumber}");

            return null;
        }

        private static bool initialized = false;

        //basically initializes opcode array from xml file
        public static void Init()
        {
            if (!initialized)
            {
                LoadOpcodes();
                initialized = true;
            }
        }

        /// <summary>
        /// Scan source code for script names. Scans actual chunks, won't work if only text is provided.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetScriptsList()
        {
            var scripts = new List<string>();

            for (int i = 0; i < chunks.Count; i++)
            {
                if (chunks[i].QType == QBcode.script)
                {
                    i++;
                    if (chunks[i].QType != QBcode.symbol)
                        MainForm.WarnUser("something's wrong! no script name.");

                    scripts.Add(SymbolCache.GetSymbolName(chunks[i].data_uint));
                }
            }

            return scripts;
        }

        /// <summary>
        /// Looks for .sym.qb and loads it if found.
        /// </summary>
        /// <param name="filename"></param>
        public static void LoadSymbols(string filename)
        {
            string sympath = Path.ChangeExtension(filename, ".sym.qb");

            if (File.Exists(sympath))
                LoadCompiledScript(sympath);
        }

        /// <summary>
        /// Converts a compiled qb file to am internal list of QChunks.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<QChunk> LoadCompiledScript(string filename)
        {
            ForceQBLevel(QBFormat.THPS3);

            LoadSymbols(filename);

            chunks.Clear();

            try
            {
                using (var br = new BinaryReaderEx(File.OpenRead(filename)))
                {
                    //bool can = true;

                    QToken qcode;
                    QChunk chunk;

                    do
                    {
                        byte x = br.ReadByte();
                        //MainForm.Warn("" + x.ToString("X8"));

                        qcode = FindCode(x);

                        if (qcode is null)
                            MainForm.WarnUser("findcode failed for " + x.ToString("X2"));

                        chunk = new QChunk(br, qcode);

                        // adjust qb format
                        SetProperQBFormat(chunk);

                        chunks.Add(chunk);
                        //if (chunk.code.Code == 0) can = false;
                    }
                    while (chunk.QType != QBcode.endfile);
                }

                SubstituteLinks(true);
                CosmeticFixes();
            }
            catch (Exception ex)
            {
                MainForm.WarnUser($"parse failed: " + ex.Message + "\r\n" + ex.ToString());
            }

            SymbolCache.Validate();

            return chunks;
        }


        private static void SetProperQBFormat(QChunk q)
        {
            switch (q.QType)
            {
                case QBcode.comma:

                case QBcode.qbsub:
                case QBcode.qbadd:
                case QBcode.qbmul:
                case QBcode.qbdiv:

                case QBcode.less:
                case QBcode.lesseq:
                case QBcode.greater:
                case QBcode.greatereq:

                case QBcode.roundopen:
                case QBcode.roundclose:

                case QBcode.member:
                case QBcode.property: //was in thps4?

                case QBcode.qbcase:
                case QBcode.qbswitch:
                case QBcode.qbendswitch:
                case QBcode.qbdefault: SetQBLevel(QBFormat.THPS4); break;

                case QBcode.qbif2:
                case QBcode.qbelse2: SetQBLevel(QBFormat.THUG2); break;
            }
        }

        /// <summary>
        /// Applies additional cosmetic fixes to the decompiled chunks.
        /// </summary>
        private static void CosmeticFixes()
        {
            //following loops change actual byte code, hence settings are used

            if (!Settings.Default.applyCosmetics) return;


            // this one adds extra 1 line before script, if no 2 newline codes found 

            for (int i = 2; i < chunks.Count; i++)
            {
                if (chunks[i].QType == QBcode.script)
                {
                    if (chunks[i - 1].code.Logic != OpLogic.Linefeed ||
                        chunks[i - 2].code.Logic != OpLogic.Linefeed)
                        chunks.Insert(i, new QChunk(SelectedNewLine));

                    i++;
                }
            }

            for (int i = 2; i < chunks.Count; i++)
            {
                if (chunks[i].QType == QBcode.script)
                {
                    if (chunks[i - 1].code.Logic != OpLogic.Linefeed ||
                        chunks[i - 2].code.Logic != OpLogic.Linefeed)
                        chunks.Insert(i, new QChunk(SelectedNewLine));

                    i++;
                }
            }

            for (int i = 0; i < chunks.Count - 2; i++)
            {
                if (chunks[i].code.Code != (byte)QBcode.math_eq) continue;
                if (chunks[i + 1].code.Logic != OpLogic.Linefeed) continue;
                if (chunks[i + 2].code.Logic != OpLogic.RegionBegin) continue;

                chunks.RemoveAt(i + 1);

                i++;
            }

            // changes }{ to }\r\n{, applies to all brackets that follow region begin/end logic

            for (int i = 2; i < chunks.Count; i++)
            {
                if (chunks[i].code.Logic == OpLogic.RegionBegin && chunks[i - 1].code.Logic == OpLogic.RegionEnd)
                {
                    chunks.Insert(i, new QChunk(SelectedNewLine));
                    i++;
                }
            }
        }

        /// <summary>
        /// This function essentially replaces array indices with actual node names.
        /// Allows to painfully reorder nodes the way you want.
        /// </summary>
        /// <param name="mode"></param>
        private static void SubstituteLinks(bool mode)
        {
            nodes.Clear();

            bool isNodeArray = false;
            bool readName = false;

            uint nodeArraycrc = Checksum.Calc("NodeArray");
            uint namecrc = Checksum.Calc("Name");
            uint linkscrc = Checksum.Calc("Links");
            uint triggerscriptscrc = Checksum.Calc("TriggerScripts");

            foreach (QChunk qc in chunks)
            {
                if (qc.QType == QBcode.symbol)
                {
                    if (qc.data_uint == nodeArraycrc)
                    {
                        isNodeArray = true;
                    }
                    else
                    {
                        if (isNodeArray)
                        {
                            if (qc.data_uint == namecrc)
                            {
                                readName = true;
                            }
                            else
                            {
                                if (readName)
                                {
                                    nodes.Add(qc.data_uint);
                                    readName = false;
                                }
                            }
                        }
                    }

                }
            }


            bool fixlinks = false;

            if (isNodeArray)
            {

                foreach (QChunk qc in chunks)
                {
                    if (qc.data_uint == linkscrc)
                    {
                        fixlinks = true;
                    }
                    else
                    {
                        if (qc.data_uint == triggerscriptscrc) { fixlinks = false; break; }


                        if (fixlinks)
                        {
                            if (mode)
                            {

                                if (qc.QType == QBcode.val_int)
                                {
                                    //if (qc.data_int >= nodes.Count) QScripted.MainForm.Warn("wow " + qc.data_int);
                                    qc.data_uint = nodes[qc.data_int];
                                    qc.code = QBuilder.GetCode(QBcode.symbol);
                                }
                            }
                            else
                            {
                                if (qc.QType == QBcode.symbol)
                                {
                                    //if (qc.data_int >= nodes.Count) QScripted.MainForm.Warn("wow " + qc.data_int);
                                    qc.data_int = nodes.FindIndex(a => a == qc.data_uint);
                                    qc.code = QBuilder.GetCode(QBcode.val_int);
                                }
                            }
                        }

                        if (qc.QType == QBcode.endarray) fixlinks = false;
                    }
                }
            }

        }


        /// <summary>
        /// Loads available opcodes from XML definition file.
        /// </summary>
        public static void LoadOpcodes()
        {
            string filename = $"{AppDomain.CurrentDomain.BaseDirectory}\\data\\qScript_def.xml";

            if (!File.Exists(filename))
            {
                MainForm.WarnUser($"{filename}\r\nOpcode definition file not found, please check.");
                return;
            }

            var doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(filename));

            try
            {
                tokens.Clear();
                var nodes = doc.GetElementsByTagName("opcode");

                foreach (XmlNode node in nodes)
                {
                    var qc = new QToken(node);
                    tokens.Add(qc);

                    //syntax.Add(qc.Code, qc.Syntax);
                }
            }
            catch (Exception ex)
            {
                MainForm.WarnUser(ex.Message);
            }
        }


        public static string GetSource(bool debug)
        {
            QChunk.closeRandomAt = -1;

            StringBuilder sb = new StringBuilder();
            string result = "";
            int i = 0;

            bool globalize = false;
            bool wantSpace = false;
            bool lastwasnewline = false;

            int indent = 0;
            int indentStep;
            char padchar;

            if (Settings.Default.useTab)
            {
                indentStep = 1;
                padchar = '\t';
            }
            else
            {
                indentStep = 2;
                padchar = ' ';
            }

            List<OpLogic> spaceHaters = new List<OpLogic>();
            spaceHaters.AddRange(new OpLogic[] { OpLogic.Linefeed, OpLogic.Relation });


            //changes angle vector format
            try
            {
                bool nextvectorisangle = false;

                for (int ii = 0; ii < chunks.Count - 1; ii++)
                {
                    if (chunks[ii].code.Logic == OpLogic.Symbol)
                    {
                        if (chunks[ii].data_uint == Checksum.Calc("angles"))
                        {
                            nextvectorisangle = true;
                            continue;
                        }
                    }

                    if (chunks[ii].code.Logic == OpLogic.Vector && nextvectorisangle)
                    {
                        chunks[ii].data_vector.isAngle = true;
                        nextvectorisangle = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.WarnUser("Error while fixing angles: " + ex.Message);
            }

            foreach (QChunk c in chunks)
            {
                result = c.ToString(debug);

                if (globalize)
                {
                    // maybe add 0x16 check here?
                    result = "<" + result + ">";
                    globalize = false;
                }

                if (c.code.Code == (byte)QBcode.global) globalize = true;

                if (c.randomMarker) result = " ) " + result;


                switch (c.code.Nesting)
                {
                    case NestCommand.Close: indent -= indentStep; break;
                    case NestCommand.Break: indent -= indentStep; break;
                    default: break;
                }


                if (lastwasnewline)
                {
                    if (indent < 0)
                    {
                        MainForm.WarnUser("Whoops! Negative indent.");
                        indent = 20; //just so you can easily spot the place where this occurs
                    }

                    result = "".PadLeft(indent, padchar) + result;
                    lastwasnewline = false;
                }


                switch (c.code.Nesting)
                {
                    case NestCommand.Open: indent += indentStep; break;
                    case NestCommand.Break: indent += indentStep; break;
                    default: break;
                }


                if (c.code.Logic == OpLogic.Linefeed) lastwasnewline = true;


                if (result != "")
                    if (wantSpace)
                    {
                        if (spaceHaters.Contains(c.code.Logic)) { wantSpace = false; }
                        else result = " " + result;
                    }
                    else
                    {
                        if (!spaceHaters.Contains(c.code.Logic)) wantSpace = true;
                    }

                //implicitly kill some spaces
                if (c.code.Code == (byte)QBcode.randomjump) wantSpace = false;
                if (c.code.Logic == OpLogic.Random) wantSpace = false;
                if (c.code.Code == (byte)QBcode.randomrange) wantSpace = false;

                sb.Append(result);

                i++;
            }

            return sb.ToString();
        }


        /// <summary>
        /// Splits the source code in lines and trims line edges
        /// </summary>
        /// <param name="sourceText"></param>
        /// <returns></returns>
        private static string NormalizeSource(string sourceText)
        {
            var sb = new StringBuilder();

            // get all lines separated by environment set new line symbol
            string[] lines = sourceText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            // add a single new line symbol as we scan chars and convert it later to newline opcode
            foreach (string line in lines)
            {
                sb.Append(line.Trim());
                sb.Append("\n");
            }

            // remove last line? guess there was a reason?
            sb.Length -= 1;

            return sb.ToString();
        }


        static string wordbuf = "";

        /// <summary>
        /// Parses whatever we currently got in the char buffer.
        /// </summary>
        public static void ParseBuf()
        {
            // parse the buffer
            ParseWord(wordbuf);

            // clear the buffer
            wordbuf = "";
        }



        static bool symbolmarker = false;

        private static List<string> localcache = new List<string>();

        public static int lineNumber = 0;


        public static QBcode SelectedNewLine => Settings.Default.useShortLine ? QBcode.newline1 : QBcode.newline2;

        /// <summary>
        /// Converts source Q code string to a list of QChunks.
        /// </summary>
        /// <param name="sourceText"></param>
        public static void Compile(string sourceText)
        {
            lineNumber = 0;

            SymbolCache.Validate();

            //we need no chunks in our list
            chunks.Clear();
            localcache.Clear();

            sourceText = NormalizeSource(sourceText);

            //foreach symbol in our source text
            for (int i = 0; i < sourceText.Length; i++)
            {
                switch (sourceText[i])
                {
                    //detect stop chars. should somehow use xml values probably?
                    case ' ':
                    case '\t': ParseBuf(); break;

                    case '\n': ParseBuf(); chunks.Add(new QChunk(SelectedNewLine)); lineNumber++; break;
                    case '=': ParseBuf(); chunks.Add(new QChunk(QBcode.math_eq)); break;
                    case ',': ParseBuf(); chunks.Add(new QChunk(QBcode.comma)); break;
                    case '{': ParseBuf(); chunks.Add(new QChunk(QBcode.structure)); break;
                    case '}': ParseBuf(); chunks.Add(new QChunk(QBcode.endstructure)); break;
                    case '[': ParseBuf(); chunks.Add(new QChunk(QBcode.array)); break;
                    case ']': ParseBuf(); chunks.Add(new QChunk(QBcode.endarray)); break;
                    case '+': ParseBuf(); chunks.Add(new QChunk(QBcode.qbadd)); break;

                    //minus can be a part of numeric. currently resolved at last point if word is "-". 
                    //case '-': ParseBuf(); chunks.Add(new QChunk(GetCode(QBcode.qbsub))); break;

                    case '*': ParseBuf(); chunks.Add(new QChunk(QBcode.qbmul)); break;
                    case '@': ParseBuf(); chunks.Add(new QChunk(QBcode.randomjump)); break;
                    case '(': ParseBuf(); chunks.Add(new QChunk(QBcode.roundopen)); break;
                    case ')': ParseBuf(); chunks.Add(new QChunk(QBcode.roundclose)); break;
                    case '!': ParseBuf(); chunks.Add(new QChunk(QBcode.qbnot)); break;

                    case '#': ParseBuf(); symbolmarker = true; break;

                    //commenting
                    case ';': ParseBuf(); i = SkipLine(sourceText, i); chunks.Add(new QChunk(SelectedNewLine)); break;
                    case '/':
                        ParseBuf();
                        if (sourceText[i + 1] == '/') { i = SkipLine(sourceText, i); chunks.Add(new QChunk(SelectedNewLine)); }
                        else chunks.Add(new QChunk(QBcode.qbdiv));
                        break;

                    //region based stop symbols
                    case '"': ParseBuf(); i = ReadString(sourceText, i + 1, '"'); PutString(); break;
                    case '\'': ParseBuf(); i = ReadString(sourceText, i + 1, '\''); PutParamString(); break;

                    //oh what a pity, there is nothing to parse yet!
                    default: wordbuf += sourceText[i]; break;
                }
            }


            //one last word to parse
            ParseBuf();

            FinalChecks();
            SubstituteLinks(false);
        }

        /// <summary>
        /// Makes sure one does not simply miss a bracket.
        /// Cause it freezes the game and it's annoying to fix.
        /// </summary>
        private static void CheckBrackets()
        {
            // pretty sure it's super inefficient, but it works just fine
            // it builds a string like "{{[]()}}" by adding every next bracket symbol to stack
            // then it replaces {} () [] combinations with a null string to eliminate properly enclosed chunks
            
            // if closing bracket was added and there is no opening bracket, it throws an error
            // if string is not empty by the end, there is some error
            // there is also an arbitrary nesting limit of 16, just for sanity

            string stack = "";
            int line = 0;
            int lastemptystack = 0;
            bool die = false;


            for (int i = 0; i < chunks.Count; i++)
            {
                switch (chunks[i].QType)
                {
                    case QBcode.val_string:
                    case QBcode.val_string_param:
                        if (chunks[i].data_string.Length > 255) die = true; break;

                    case QBcode.newline1:
                    case QBcode.newline2: line++; break;

                    case QBcode.roundopen: stack += "("; break;
                    case QBcode.roundclose: stack += ")"; if (!stack.Contains("(")) die = true; break;

                    case QBcode.array: stack += "["; break;
                    case QBcode.endarray: stack += "]"; if (!stack.Contains("[")) die = true; break;

                    case QBcode.structure: stack += "{"; break;
                    case QBcode.endstructure: stack += "}"; if (!stack.Contains("{")) die = true; break;

                    case QBcode.qbif: stack += "\\"; break;
                    case QBcode.qbelse: stack += "-"; if (!stack.Contains("\\")) die = true; break;
                    case QBcode.qbendif: stack += "/"; if (!stack.Contains("\\")) die = true; break;

                    case QBcode.script: stack += "<"; break;
                    case QBcode.endscript: stack += ">"; if (!stack.Contains("<")) die = true; break;
                }

                stack = stack.Replace("{}", "").Replace("()", "").Replace("[]", "").Replace("<>", "").Replace("\\-/", "").Replace("\\/", "");

                if (stack == "")
                    lastemptystack = line;

                if (stack.Length > 32)
                    break;

                if (die)
                    break;
            }

            if (stack != "")
                MainForm.WarnUser($"Balance check failed!\r\nCheck below line {lastemptystack}.");
        }

        /// <summary>
        /// This is the second higher level parsing step.
        /// At this step we convert [bracket, number, comma, number, bracket] to pair and such.
        /// </summary>
        private static void FinalChecks()
        {
            // these checks may overflow at borderlines, hence try catch

            // convert pair and vector sequences to opcodes

            try
            {
                for (int i = 0; i < chunks.Count - 1; i++)
                {
                    if (chunks[i].QType == QBcode.roundopen)
                        if (chunks[i + 1].code.Logic == OpLogic.Numeric)
                            if (chunks[i + 2].QType == QBcode.comma)
                                if (chunks[i + 3].code.Logic == OpLogic.Numeric)
                                    if (chunks[i + 4].QType == QBcode.roundclose)
                                    {
                                        //vector2!
                                        QChunk q = new QChunk(QBcode.val_vector2);
                                        q.data_vector = new Vector3f(chunks[i + 1].GetNumericValue(), chunks[i + 3].GetNumericValue());
                                        chunks.RemoveRange(i, 5);
                                        chunks.Insert(i, q);
                                    }
                                    else
                                        if (chunks[i + 4].QType == QBcode.comma)
                                        if (chunks[i + 5].code.Logic == OpLogic.Numeric)
                                            if (chunks[i + 6].QType == QBcode.roundclose)
                                            {
                                                //vector3!
                                                QChunk q = new QChunk(QBcode.val_vector3);
                                                q.data_vector = new Vector3f(chunks[i + 1].GetNumericValue(), chunks[i + 3].GetNumericValue(), chunks[i + 5].GetNumericValue());
                                                chunks.RemoveRange(i, 7);
                                                chunks.Insert(i, q);
                                            }
                }
            }
            catch (Exception ex)
            {
                MainForm.WarnUser("Error while parsing vectors: " + ex.Message);
            }

            // some random line feed cleanup? dont remember

            try
            {
                for (int i = 0; i < chunks.Count - 1; i++)
                {
                    if (chunks[i].QType == QBcode.randomjump)
                    {
                        while (chunks[i + 1].code.Logic == OpLogic.Linefeed)
                        {
                            chunks.RemoveAt(i + 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.WarnUser("Error while removing jump linefeeds: " + ex.Message);
            }


            //fix randoms

            //randoms code here

            try
            {
                for (int i = 0; i < chunks.Count - 1; i++)
                {
                    if (chunks[i].code.Group == DataGroup.Random)
                    {
                        i = CalcRandom(i).pos;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.WarnUser("Error while parsing random: " + ex.Message);
            }

            // make sure we have not ruined anything

            CheckBrackets();

            // optional fix: remove repeating new line opcodes

            if (Settings.Default.removeTrailNewlines)
            {
                try
                {
                    for (int i = 0; i < chunks.Count - 1; i++)
                    {
                        if (chunks[i].code.Logic == OpLogic.Linefeed)
                        {
                            while (chunks[i + 1].code.Logic == OpLogic.Linefeed)
                            {
                                chunks.RemoveAt(i + 1);
                                if (i + 1 == chunks.Count) break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.WarnUser("Error while removing trailing newlines: " + ex.Message);
                }
            }

            // remove commas if we're in THPS3 mode, it wasn't supported yet 

            if (Settings.Default.minQBLevel == (int)QBFormat.THPS3)
            {
                try
                {
                    for (int i = 0; i < chunks.Count - 1; i++)
                    {
                        if (chunks[i].QType == QBcode.comma)
                        {
                            chunks.RemoveAt(i);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.WarnUser("Error while removing commas: " + ex.Message);
                }
            }


            // add qchunk index like in original scripts. doesn't matter really (should be source code line number though)

            for (int i = 0; i < chunks.Count - 1; i++)
            {
                if (chunks[i].QType == QBcode.newline2)
                    if (chunks[i].data_int == 0)
                        chunks[i].data_int = i;
            }


            // dump localcache - only values actually used in current source, hello thqb

            localcache = localcache.Select(x => x).Distinct().ToList();
            symbols.Clear();

            foreach (string lc in localcache)
            {
                var q = new QChunk(QBcode.symboldef);

                q.data_uint = SymbolCache.GetSymbolHash(lc);
                q.data_string = lc;

                symbols.Add(q);
            }

            if (!Settings.Default.useSymFile)
            {
                chunks.AddRange(symbols);
            }
            else
            {
                symbols.Add(new QChunk(QBcode.endfile));
            }

            // the end
            chunks.Add(new QChunk(QBcode.endfile));
        }




        public struct RandomResult
        {
            public int size;
            public int pos;

            public RandomResult(int s, int p)
            {
                size = s; pos = p;
            }
        }

        public static RandomResult CalcRandom(int i)
        {
            int rand = i;

            bool rand_opened = true;

            chunks[rand].ptrs.Clear();

            int offset = 0;
            int jumps = 0;

            List<int> jumpstofix = new List<int>();


            i++;

            do
            {
                if (chunks[i].code.Group == DataGroup.Random)
                {
                    RandomResult rr = CalcRandom(i);
                    offset += rr.size;
                    i = rr.pos;
                }
                else if (chunks[i].QType == QBcode.roundopen)
                {
                    chunks.RemoveAt(i);
                    continue;
                }
                else if (chunks[i].QType == QBcode.roundclose && rand_opened)
                {
                    chunks.RemoveAt(i);
                    rand_opened = false;

                    continue;
                }
                else if (chunks[i].QType == QBcode.randomjump && rand_opened)
                {
                    jumps++;

                    if (jumps == 1)
                    {
                        chunks.RemoveAt(i);
                        chunks[rand].ptrs.Add(offset);
                        continue;
                    }
                    else
                    {
                        offset += chunks[i].GetSize();
                        // QScripted.MainForm.Warn("" + offset);
                        chunks[i].data_int = offset;
                        jumpstofix.Add(i);
                        chunks[rand].ptrs.Add(offset);
                    }
                }
                else
                {
                    offset += chunks[i].GetSize();
                }

                i++;
            }
            while (rand_opened);


            for (int k = 0; k < chunks[rand].ptrs.Count; k++)
            {
                chunks[rand].ptrs[k] += (chunks[rand].ptrs.Count - k - 1) * 4;
            }


            foreach (int f in jumpstofix)
            {
                chunks[f].data_int = offset - chunks[f].data_int;
            }


            RandomResult rres = new RandomResult(offset, i);

            return rres;
        }


        /// <summary>
        /// Seek for the next new line symbol.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static int SkipLine(string src, int pos)
        {
            do
            {
                if (src[pos] == '\n') break;
                pos++;
                if (pos == src.Length) break;
            }
            while (true);

            return pos;
        }


        public static int ReadString(string src, int i, char stopchar)
        {
            do
            {
                if (src[i] == '\\')
                {
                    if (src[i + 1] == '"' || src[i + 1] == '\'' || src[i + 1] == '\\') { i++; }
                    wordbuf += src[i];
                }
                else
                {
                    if (src[i] != stopchar) { wordbuf += src[i]; }
                    else { break; }
                }
                i++;

                //if (i >= src.Length) throw new Exception("Read string out of bounds.");
            }
            while (true);

            return i;
        }


        public static void PutString()
        {
            if (!symbolmarker)
            {
                QChunk q = new QChunk(QBcode.val_string);
                q.data_string = wordbuf;
                wordbuf = "";
                chunks.Add(q);
                return;
            }
            else
            {
                //QScripted.MainForm.Warn("got symbol!" + wordbuf);
                QChunk q = new QChunk(QBcode.symbol);
                q.data_string = wordbuf;
                q.data_uint = Checksum.Calc(q.data_string);

                SymbolCache.Add(wordbuf);
                localcache.Add(wordbuf);

                wordbuf = "";
                chunks.Add(q);
                symbolmarker = false;
                return;
            }
        }


        public static void PutParamString()
        {
            QChunk q = new QChunk(QBcode.val_string_param);
            q.data_string = wordbuf;
            wordbuf = "";
            chunks.Add(q);
            return;
        }


        // TODO: these 3 maybe funcs are pretty much same
        // gotta merge and also make sure minus to be on the left and ° on the right. regexp?

        public static bool maybeFloat(string w)
        {
            string allowed = "0123456789-.";

            int cnt = 0;

            foreach (char c in w)
            {
                if (!allowed.Contains(c.ToString()))
                    return false;

                if (c == '-') cnt++;
            }

            if (cnt > 1) return false;

            return true;
        }

        public static bool maybeInt(string w)
        {
            //it's a very loose check. "123-456" is a number. much wow.

            string allowed = "0123456789-";

            int cnt = 0;

            foreach (char c in w)
            {
                if (!allowed.Contains(c.ToString()))
                    return false;

                if (c == '-') cnt++;
            }

            if (cnt > 1) return false;

            return true;
        }

        public static bool maybeAngle(string w)
        {
            string allowed = "0123456789.-°";

            int cnt = 0;

            foreach (char c in w)
            {
                if (!allowed.Contains(c.ToString()))
                    return false;

                if (c == '-') cnt++;
            }

            if (cnt > 1) return false;

            //QScripted.MainForm.Warn(w + " is angle");
            return true;
        }


        private static void ParseWord(string ss)
        {
            //shouldn't really matter, but still
            string s = ss.ToString().Trim().ToLower();

            //empty word, do nothing
            if (s == "") return;

            if (s == "-")
            {
                QChunk p = new QChunk(QBcode.qbsub);
                //p.data_float = Single.Parse(s);
                chunks.Add(p);
                return;
            }


            if (maybeInt(s))
            {
                QChunk q = new QChunk(QBcode.val_int);
                q.data_int = Int32.Parse(s);
                chunks.Add(q);
                //QScripted.MainForm.Warn(q.data_int + "");
                return;
            }



            if (maybeFloat(s))
            {
                if (s == ".")
                {
                    QChunk p = new QChunk(QBcode.property);
                    //p.data_float = Single.Parse(s);
                    chunks.Add(p);
                    return;
                }

                QChunk q = new QChunk(GetCode(QBcode.val_float));
                q.data_float = Single.Parse(s);
                chunks.Add(q);
                return;
            }



            if (maybeAngle(s))
            {
                QChunk q = new QChunk(QBcode.val_int);
                q.data_float = Single.Parse(s.Replace("°", ""));
                q.data_float = (float)(q.data_float / Vector3f.radian);
                chunks.Add(q);
                //QScripted.MainForm.Warn(q.data_int + "");
                return;
            }


            // TODO: we definitely can make that a loop over a QBcode enum
            // just gotta make sure only a valid subset is used

            if (s == QBuilder.GetCode(QBcode.script).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.script));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.endscript).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.endscript));
                return;
            }


            if (s == QBuilder.GetCode(QBcode.globalall).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.globalall));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbif).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbif));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbelse).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbelse));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbelseif).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbelseif));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbendif).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbendif));
                return;
            }


            if (s == QBuilder.GetCode(QBcode.repeat).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.repeat));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.repeatbreak).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.repeatbreak));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.repeatend).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.repeatend));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbnot).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbnot));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbor).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbor));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qband).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qband));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbswitch).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbswitch));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbendswitch).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbendswitch));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbcase).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbcase));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbdefault).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbdefault));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.qbreturn).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.qbreturn));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.random).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.random));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.randompermute).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.randompermute));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.randomnorepeat).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.randomnorepeat));
                return;
            }

            if (s == QBuilder.GetCode(QBcode.randomrange).GetSyntax())
            {
                chunks.Add(new QChunk(QBcode.randomrange));
                return;
            }

            

            if (s.Contains("."))
            {
                //QScripted.MainForm.Warn(s + " got into .!");
                string[] buf = s.Split('.');

                PutSymbol(buf[0], true);
                chunks.Add(new QChunk(QBcode.property));
                PutSymbol(buf[1], true); // so im suppressing this error, cause apparently it's a feature

                return;
            }

            if (s.Contains(":"))
            {
                string[] buf = s.Split(':');

                PutSymbol(buf[0], true);
                chunks.Add(new QChunk(QBcode.member));
                PutSymbol(buf[1], false); // at least member is always a symbol, i hope?

                return;
            }

            //so nothing worked. let's put a symbol
            PutSymbol(ss, false);
        }

        private static void PutSymbol(string s, bool suppressError)
        {
            //maybeGlobal?
            if (s == "")
            {
                if (!suppressError) MainForm.WarnUser($"wtf null putsymbol called at line {QBuilder.lineNumber}");
                return;
            }

            if (s == "<")
            {
                chunks.Add(new QChunk(QBcode.less));
                return;
            }

            if (s == "<=")
            {
                chunks.Add(new QChunk(QBcode.lesseq));
                return;
            }

            if (s == ">")
            {
                chunks.Add(new QChunk(QBcode.greater));
                return;
            }

            if (s == ">=")
            {
                chunks.Add(new QChunk(QBcode.greatereq));
                return;
            }


            if (s[0] == '<' && s[s.Length - 1] == '>')
            {
                //yes!
                QChunk q = new QChunk(QBcode.global);
                chunks.Add(q);
            }

            s = s.Trim('<', '>');

            if (s[0] == '0')
            {
                if (s[1] == 'x')
                {

                }
                else localcache.Add(s);
            }
            else localcache.Add(s);



            uint crc = SymbolCache.GetSymbolHash(s);

            QChunk q2 = new QChunk(QBcode.symbol);
            q2.data_uint = crc;
            q2.data_string = s;
            chunks.Add(q2);
        }


        public static void SaveChunks(string path)
        {
            SaveChunks(path, chunks);

            if (Settings.Default.useSymFile) 
                SaveChunks(Path.ChangeExtension(path, ".sym.qb"), symbols);
        }


        public static void SaveChunks(string path, List<QChunk> chunks)
        {
            byte[] data = new byte[0];

            using (var stream = new MemoryStream())
            {
                using (var bw = new BinaryWriter(stream))
                {
                    foreach (var chunk in chunks)
                        bw.Write(chunk.ToArray());

                    stream.Flush();
                    data = stream.GetBuffer();

                    //removing trailing zeroes here
                    Array.Resize(ref data, (int)bw.BaseStream.Position);
                }
            }

            File.WriteAllBytes(path, data);
        }









        static uint positioncrc = Checksum.Calc("Position");
        static uint anglescrc = Checksum.Calc("Angles");
        static uint createdcrc = Checksum.Calc("CreatedAtStart");
        static uint trickobcrc = Checksum.Calc("TrickObject");
        static uint namecrc = Checksum.Calc("Name");
        static uint classcrc = Checksum.Calc("Class");
        static uint clustercrc = Checksum.Calc("Cluster");


        static List<Node> nodeArray = new List<Node>();


        public static List<Node> GetNodeArray()
        {

            uint nodeArraycrc = Checksum.Calc("NodeArray");
            uint namecrc = Checksum.Calc("Name");
            uint linkscrc = Checksum.Calc("Links");
            uint triggerscriptscrc = Checksum.Calc("TriggerScripts");


            bool isNodeArray = false;
            bool inNodeArray = false;


            for (int i = 0; i < chunks.Count; i++)
            {
                if (chunks[i].QType == QBcode.symbol)
                    if (chunks[i].data_uint == nodeArraycrc)
                    {
                        //it's node array!
                        i = SkipUntil(i, QBcode.array);

                        while (chunks[i].QType != QBcode.endarray)
                        {
                            i = SkipUntil(i, QBcode.structure) + 1;
                            i = ReadNode(chunks, i);
                            i = SkipUntil(i, QBcode.structure) + 1;
                        }
                    }
            }

            return nodeArray;
        }

        public static int SkipUntil(int from, QBcode qc)
        {
            while (chunks[from].QType != qc)
            {
                if (chunks[from].QType == QBcode.endstructure) return from;
                from++;
            }
            return from;
        }

        public static int ReadNode(List<QChunk> chunks, int from)
        {
            var sb = new StringBuilder();

            //QScripted.MainForm.Warn("reading node from " + from);

            Node n = new Node();

            while (chunks[from].QType != QBcode.endstructure)
            {
                //QScripted.MainForm.Warn("" + from);
                if (chunks[from].QType == QBcode.symbol)
                {
                    if (chunks[from].data_uint == Checksum.Calc("Position"))
                    {
                        from++;
                        from++;
                        n.Position = chunks[from].data_vector;
                        from++;
                        continue;
                    }

                    else if (chunks[from].data_uint == Checksum.Calc("Angles"))
                    {
                        from++;
                        from++;
                        n.Angles = chunks[from].data_vector;
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("Name"))
                    {
                        from++;
                        from++;
                        n.Name = SymbolCache.GetSymbolName(chunks[from].data_uint);
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("RestartName"))
                    {
                        from++;
                        from++;
                        n.RestartName = SymbolCache.GetSymbolName(chunks[from].data_uint);
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("Type"))
                    {
                        from++;
                        from++;
                        try
                        {
                            n.railType = (RailType)Enum.Parse(typeof(RailType), SymbolCache.GetSymbolName(chunks[from].data_uint), true);
                        }
                        catch
                        {
                            ThpsQScriptEd.MainForm.WarnUser("TYPE FOUND: " + SymbolCache.GetSymbolName(chunks[from].data_uint));
                        }
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("Class"))
                    {
                        from++;
                        from++;
                        n.nodeClass = (NodeClass)Enum.Parse(typeof(NodeClass), SymbolCache.GetSymbolName(chunks[from].data_uint), true);
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("Cluster"))
                    {
                        from++;
                        from++;
                        n.Cluster = SymbolCache.GetSymbolName(chunks[from].data_uint);
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("TriggerScript"))
                    {
                        from++;
                        from++;
                        n.TriggerScript = SymbolCache.GetSymbolName(chunks[from].data_uint);
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("SpawnObjScript"))
                    {
                        from++;
                        from++;
                        n.SpawnObjScript = SymbolCache.GetSymbolName(chunks[from].data_uint);
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("TerrainType"))
                    {
                        from++;
                        from++;
                        n.terrainType = (TerrainType)Enum.Parse(typeof(TerrainType), SymbolCache.GetSymbolName(chunks[from].data_uint), true);
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("CreatedAtStart"))
                    {
                        n.CreatedAtStart = true;
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("TrickObject"))
                    {
                        n.TrickObject = true;
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("NetEnabled"))
                    {
                        n.NetEnabled = true;
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("AbsentInNetGames"))
                    {
                        n.AbsentInNetGames = true;
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("Permanent"))
                    {
                        n.Permanent = true;
                        from++;
                        continue;
                    }
                    else if (chunks[from].data_uint == Checksum.Calc("Zone_Multiplier"))
                    {
                        from++;
                        from++;
                        n.Zone_Multiplier = chunks[from].data_int;
                        from++;
                        continue;
                    }


                    else if (chunks[from].data_uint == Checksum.Calc("Links"))
                    {
                        from = SkipUntil(from, QBcode.array);
                        from++;

                        while (chunks[from].QType != QBcode.endarray)
                        {
                            if (chunks[from].code.Logic == OpLogic.Numeric)
                            {
                                n.Links.Add(chunks[from].data_int);
                            }
                            from++;
                        }

                        from++;

                        continue;
                    }
                    else
                    {
                        MainForm.WarnUser(SymbolCache.GetSymbolName(chunks[from].data_uint) + " at " + from);
                        from++;
                    }
                }
                else
                {
                    from++;
                }

            }

            nodeArray.Add(n);

            // QScripted.MainForm.Warn(n.ToString());

            return from++;
        }

    }
}