using LegacyThps.QScript.Helpers;
using LegacyThps.QScript.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Xml;
using ThpsQScriptEd;
using Settings = ThpsQScriptEd.Properties.Settings;

namespace LegacyThps.QScript
{
    // TODO: convert QBuilder to QTokenCollection : List<QToken>
    // tokenizer should return a collection.
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



        public static List<QTokenType> tokenTypes = new List<QTokenType>();



        static private string path;


        static List<QToken> tokens = new List<QToken>();
        static List<QToken> symbols = new List<QToken>();

        static List<uint> nodes = new List<uint>();

        /// <summary>
        /// Find QToken by QBcode.
        /// </summary>
        /// <param name="oldcode"></param>
        /// <returns></returns>
        public static QTokenType Tokenizer_GetTokenType(QBcode oldcode)
        {
            return Tokenizer_GetTokenType((byte)oldcode);
        }

        public static QTokenType Tokenizer_GetTokenType(byte value)
        {
            foreach (var q in tokenTypes)
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
                Tokenizer_LoadTokenTypes();
                SymbolCache.Create();
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

            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].QType == QBcode.script)
                {
                    i++;
                    if (tokens[i].QType != QBcode.symbol)
                        MainForm.WarnUser("something's wrong! no script name.");

                    scripts.Add(SymbolCache.GetSymbolName(tokens[i].data_uint));
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
                Parse(sympath);
        }

        /// <summary>
        /// Parses a binary Q file to an list of QTokens.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<QToken> Parse(string filename)
        {
            //ForceQBLevel(QBFormat.THPS3);

            LoadSymbols(filename);

            tokens.Clear();

            try
            {
                using (var br = new BinaryReaderEx(File.OpenRead(filename)))
                {
                    //bool can = true;

                    QTokenType qcode;
                    QToken chunk;

                    do
                    {
                        byte x = br.ReadByte();
                        //MainForm.Warn("" + x.ToString("X8"));

                        qcode = Tokenizer_GetTokenType(x);

                        if (qcode is null)
                            MainForm.WarnUser("findcode failed for " + x.ToString("X2"));

                        chunk = new QToken(br, qcode);

                        // adjust qb format
                        SetProperQBFormat(chunk);

                        tokens.Add(chunk);
                        //if (chunk.code.Code == 0) can = false;
                    }
                    while (chunk.QType != QBcode.endfile);

                    if (br.BaseStream.Position < br.BaseStream.Length)
                        MainForm.WarnUser("Unexpected EOF, failed to decompile correctly.\r\nThis file is not currently supported.");
                }

                SubstituteLinks(true);
                ApplyCosmeticFixes(tokens);
            }
            catch (Exception ex)
            {
                MainForm.WarnUser($"parse failed: " + ex.Message + "\r\n" + ex.ToString());
            }

            SymbolCache.Validate();

            return tokens;
        }


        private static void SetProperQBFormat(QToken q)
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
        private static void ApplyCosmeticFixes(List<QToken> tokens)
        {
            //following loops change actual byte code, hence settings are used

            if (!Settings.Default.applyCosmetics) return;


            // this one adds extra 1 line before script, if no 2 newline codes found 

            for (int i = 2; i < tokens.Count; i++)
            {
                if (tokens[i].QType == QBcode.script)
                {
                    if (tokens[i - 1].tokenType.Logic != OpLogic.Linefeed ||
                        tokens[i - 2].tokenType.Logic != OpLogic.Linefeed)
                        tokens.Insert(i, new QToken(SelectedNewLine));

                    i++;
                }
            }

            for (int i = 2; i < tokens.Count; i++)
            {
                if (tokens[i].QType == QBcode.script)
                {
                    if (tokens[i - 1].tokenType.Logic != OpLogic.Linefeed ||
                        tokens[i - 2].tokenType.Logic != OpLogic.Linefeed)
                        tokens.Insert(i, new QToken(SelectedNewLine));

                    i++;
                }
            }

            for (int i = 0; i < tokens.Count - 2; i++)
            {
                if (tokens[i].tokenType.Code != (byte)QBcode.math_eq) continue;
                if (tokens[i + 1].tokenType.Logic != OpLogic.Linefeed) continue;
                if (tokens[i + 2].tokenType.Logic != OpLogic.RegionBegin) continue;

                tokens.RemoveAt(i + 1);

                i++;
            }

            // changes }{ to }\r\n{, applies to all brackets that follow region begin/end logic

            for (int i = 2; i < tokens.Count; i++)
            {
                if (tokens[i].tokenType.Logic == OpLogic.RegionBegin && tokens[i - 1].tokenType.Logic == OpLogic.RegionEnd)
                {
                    tokens.Insert(i, new QToken(SelectedNewLine));
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

            foreach (var qc in tokens)
            {
                if (qc.QType != QBcode.symbol) continue;

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


            bool fixlinks = false;

            if (isNodeArray)
            {

                foreach (QToken qc in tokens)
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
                                    qc.tokenType = QBuilder.Tokenizer_GetTokenType(QBcode.symbol);
                                }
                            }
                            else
                            {
                                if (qc.QType == QBcode.symbol)
                                {
                                    //if (qc.data_int >= nodes.Count) QScripted.MainForm.Warn("wow " + qc.data_int);
                                    qc.data_int = nodes.FindIndex(a => a == qc.data_uint);
                                    qc.tokenType = QBuilder.Tokenizer_GetTokenType(QBcode.val_int);
                                }
                            }
                        }

                        if (qc.QType == QBcode.endarray) fixlinks = false;
                    }
                }
            }

        }


        /// <summary>
        /// Loads available token types from XML definition file.
        /// </summary>
        public static void Tokenizer_LoadTokenTypes()
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
                tokenTypes.Clear();
                var nodes = doc.GetElementsByTagName("opcode");

                foreach (XmlNode node in nodes)
                {
                    var qc = new QTokenType(node);
                    tokenTypes.Add(qc);

                    //syntax.Add(qc.Code, qc.Syntax);
                }
            }
            catch (Exception ex)
            {
                MainForm.WarnUser(ex.Message);
            }
        }


        /// <summary>
        /// Converts a list of tokens to source code.
        /// </summary>
        /// <param name="debug"></param>
        /// <returns></returns>
        public static string GetSource(bool debug)
        {
            QToken.closeRandomAt = -1;

            var sb = new StringBuilder();
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

            var spaceHaters = new List<OpLogic>() {
                OpLogic.Linefeed,
                OpLogic.Relation
            };

            //changes angle vector format
            try
            {
                bool nextvectorisangle = false;

                for (int ii = 0; ii < tokens.Count - 1; ii++)
                {
                    if (tokens[ii].tokenType.Logic == OpLogic.Symbol)
                    {
                        if (tokens[ii].data_uint == Checksum.Calc("angles"))
                        {
                            nextvectorisangle = true;
                            continue;
                        }
                    }

                    if (tokens[ii].tokenType.Logic == OpLogic.Vector && nextvectorisangle)
                    {
                        tokens[ii].isAngle = true;
                        nextvectorisangle = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.WarnUser("Error while fixing angles: " + ex.Message);
            }

            foreach (var c in tokens)
            {
                result = c.ToString(debug);

                if (globalize)
                {
                    // maybe add 0x16 check here?
                    result = $"<{result}>";
                    globalize = false;
                }

                // mark next entry as global
                if (c.tokenType.Code == (byte)QBcode.global) globalize = true;

                // mayve convert randomMarker to compiletime only opcode here
                if (c.randomMarker) result = " ) " + result;

                switch (c.tokenType.Nesting)
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


                switch (c.tokenType.Nesting)
                {
                    case NestCommand.Open: indent += indentStep; break;
                    case NestCommand.Break: indent += indentStep; break;
                    default: break;
                }


                if (c.tokenType.Logic == OpLogic.Linefeed) lastwasnewline = true;


                if (result != "")
                    if (wantSpace)
                    {
                        if (spaceHaters.Contains(c.tokenType.Logic)) { wantSpace = false; }
                        else result = " " + result;
                    }
                    else
                    {
                        if (!spaceHaters.Contains(c.tokenType.Logic)) wantSpace = true;
                    }

                //implicitly kill some spaces
                if (c.tokenType.Code == (byte)QBcode.randomjump) wantSpace = false;
                if (c.tokenType.Logic == OpLogic.Random) wantSpace = false;
                if (c.tokenType.Code == (byte)QBcode.randomrange) wantSpace = false;

                sb.Append(result);

                i++;
            }

            return sb.ToString();
        }





        static string wordbuf = "";

        /// <summary>
        /// Parses whatever we currently got in the char buffer.
        /// </summary>
        public static void Tokenizer_ParseBuffer()
        {
            // parse the buffer
            Tokenizer_ParseWord(wordbuf);

            // clear the buffer
            wordbuf = "";
        }



        static bool symbolmarker = false;

        private static List<string> localcache = new List<string>();

        public static int lineNumber = 0;


        public static QBcode SelectedNewLine => Settings.Default.useShortLine ? QBcode.newline : QBcode.newline_debug;

        /// <summary>
        /// Converts source Q code string to a list of QChunks.
        /// </summary>
        /// <param name="sourceText"></param>
        public static List<QToken> Tokenizer_ParseText(string sourceText)
        {
            // TODO: gotta refactor Tokenizer functions to a separate class that will operate on its own list of token
            // cause now it would require to add list param to all individual functions.
            // var tokens = new List<QToken>();

            lineNumber = 0;

            SymbolCache.Validate();

            //we need no chunks in our list
            tokens.Clear();
            localcache.Clear();

            sourceText = TextProcessor.Normalize(sourceText);

            //foreach symbol in our source text
            for (int i = 0; i < sourceText.Length; i++)
            {
                switch (sourceText[i])
                {
                    //detect stop chars. should somehow use xml values probably?
                    case ' ':
                    case '\t': Tokenizer_ParseBuffer(); break;

                    case '\n': Tokenizer_ParseBuffer(); tokens.Add(new QToken(SelectedNewLine)); lineNumber++; break;
                    case '=': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.math_eq)); break;
                    case ',': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.comma)); break;
                    case '{': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.structure)); break;
                    case '}': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.endstructure)); break;
                    case '[': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.array)); break;
                    case ']': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.endarray)); break;
                    case '+': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.qbadd)); break;

                    //minus can be a part of numeric. currently resolved at last point if word is "-". 
                    //case '-': ParseBuf(); chunks.Add(new QChunk(GetCode(QBcode.qbsub))); break;

                    case '*': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.qbmul)); break;
                    case '@': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.randomjump)); break;
                    case '(': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.roundopen)); break;
                    case ')': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.roundclose)); break;
                    case '!': Tokenizer_ParseBuffer(); tokens.Add(new QToken(QBcode.qbnot)); break;

                    case '#': Tokenizer_ParseBuffer(); symbolmarker = true; break;

                    //commenting
                    case ';': Tokenizer_ParseBuffer(); i = Tokenizer_SkipLine(sourceText, i); tokens.Add(new QToken(SelectedNewLine)); break;
                    case '/':
                        Tokenizer_ParseBuffer();
                        if (sourceText[i + 1] == '/') { i = Tokenizer_SkipLine(sourceText, i); tokens.Add(new QToken(SelectedNewLine)); }
                        else tokens.Add(new QToken(QBcode.qbdiv));
                        break;

                    //region based stop symbols
                    case '"': Tokenizer_ParseBuffer(); i = Tokenizer_ReadString(sourceText, i + 1, '"'); Tokenizer_PutString(); break;
                    case '\'': Tokenizer_ParseBuffer(); i = Tokenizer_ReadString(sourceText, i + 1, '\''); Tokenizer_PutParamString(); break;

                    //oh what a pity, there is nothing to parse yet!
                    default: wordbuf += sourceText[i]; break;
                }
            }


            //one last word to parse
            Tokenizer_ParseBuffer();

            Tokenizer_PostProcess();
            SubstituteLinks(false);

            return tokens;
        }


        /// <summary>
        /// This is the second higher level token parsing step.
        /// At this step we convert a sequence of tokens [bracket, number, comma, number, bracket] to a single pair token and such.
        /// </summary>
        private static void Tokenizer_PostProcess()
        {
            // these checks may overflow at borderlines, hence try catch

            // convert pair and vector sequences to opcodes

            try
            {
                for (int i = 0; i < tokens.Count - 1; i++)
                {
                    if (tokens[i].QType == QBcode.roundopen)
                        if (tokens[i + 1].tokenType.Logic == OpLogic.Numeric)
                            if (tokens[i + 2].QType == QBcode.comma)
                                if (tokens[i + 3].tokenType.Logic == OpLogic.Numeric)
                                    if (tokens[i + 4].QType == QBcode.roundclose)
                                    {
                                        //vector2!
                                        var q = new QToken(QBcode.val_vector2);
                                        q.data_vector = new Vector3(tokens[i + 1].GetNumericValue(), tokens[i + 3].GetNumericValue(), 0);
                                        tokens.RemoveRange(i, 5);
                                        tokens.Insert(i, q);
                                    }
                                    else
                                        if (tokens[i + 4].QType == QBcode.comma)
                                        if (tokens[i + 5].tokenType.Logic == OpLogic.Numeric)
                                            if (tokens[i + 6].QType == QBcode.roundclose)
                                            {
                                                //vector3!
                                                var q = new QToken(QBcode.val_vector3);
                                                q.data_vector = new Vector3(tokens[i + 1].GetNumericValue(), tokens[i + 3].GetNumericValue(), tokens[i + 5].GetNumericValue());
                                                tokens.RemoveRange(i, 7);
                                                tokens.Insert(i, q);
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
                for (int i = 0; i < tokens.Count - 1; i++)
                {
                    if (tokens[i].QType == QBcode.randomjump)
                    {
                        while (tokens[i + 1].tokenType.Logic == OpLogic.Linefeed)
                        {
                            tokens.RemoveAt(i + 1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.WarnUser("Error while removing jump linefeeds: " + ex.Message);
            }


            // fix randoms
            // so at this point we only have random opcode and jump opcode

            // randoms code here

            try
            {
                for (int i = 0; i < tokens.Count - 1; i++)
                {
                    if (tokens[i].tokenType.Group == DataGroup.Random)
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

            int result = QValidator.PerformBracketsCheck(tokens);

            if (result != -1)
                MainForm.WarnUser($"Balance check failed!\r\nCheck below line {result}.");

            // optional fix: remove repeating new line opcodes

            if (Settings.Default.removeTrailNewlines)
            {
                try
                {
                    for (int i = 0; i < tokens.Count - 1; i++)
                    {
                        if (tokens[i].tokenType.Logic == OpLogic.Linefeed)
                        {
                            while (tokens[i + 1].tokenType.Logic == OpLogic.Linefeed)
                            {
                                tokens.RemoveAt(i + 1);
                                if (i + 1 == tokens.Count) break;
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
                    for (int i = 0; i < tokens.Count - 1; i++)
                    {
                        if (tokens[i].QType == QBcode.comma)
                        {
                            tokens.RemoveAt(i);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.WarnUser("Error while removing commas: " + ex.Message);
                }
            }


            // add qchunk index like in original scripts. doesn't matter really (should be source code line number though)

            int line = 0;

            for (int i = 0; i < tokens.Count - 1; i++)
            {
                if (tokens[i].QType == QBcode.newline_debug || tokens[i].QType == QBcode.newline)
                {
                    if (tokens[i].data_int == 0)
                        tokens[i].data_int = line;

                    line++;
                }
            }


            // dump localcache - only values actually used in current source, hello thqb

            localcache = localcache.Select(x => x).Distinct().ToList();
            symbols.Clear();

            foreach (string lc in localcache)
            {
                var q = new QToken(QBcode.symboldef);

                q.data_uint = SymbolCache.GetSymbolHash(lc);
                q.data_string = lc;

                symbols.Add(q);
            }

            if (!Settings.Default.useSymFile)
            {
                tokens.AddRange(symbols);
            }
            else
            {
                symbols.Add(new QToken(QBcode.endfile));
            }

            // the end
            tokens.Add(new QToken(QBcode.endfile));
        }




        // omg this is convoluted

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

            tokens[rand].ptrs.Clear();

            int offset = 0;
            int jumps = 0;

            var jumpstofix = new List<int>();


            i++;

            do
            {
                if (tokens[i].tokenType.Group == DataGroup.Random)
                {
                    RandomResult rr = CalcRandom(i);
                    offset += rr.size;
                    i = rr.pos;
                }
                else if (tokens[i].QType == QBcode.roundopen)
                {
                    tokens.RemoveAt(i);
                    continue;
                }
                else if (tokens[i].QType == QBcode.roundclose && rand_opened)
                {
                    tokens.RemoveAt(i);
                    rand_opened = false;

                    continue;
                }
                else if (tokens[i].QType == QBcode.randomjump && rand_opened)
                {
                    jumps++;

                    if (jumps == 1)
                    {
                        tokens.RemoveAt(i);
                        tokens[rand].ptrs.Add(offset);
                        continue;
                    }
                    else
                    {
                        offset += tokens[i].GetSize();
                        // QScripted.MainForm.Warn("" + offset);
                        tokens[i].data_int = offset;
                        jumpstofix.Add(i);
                        tokens[rand].ptrs.Add(offset);
                    }
                }
                else
                {
                    offset += tokens[i].GetSize();
                }

                i++;
            }
            while (rand_opened);


            for (int k = 0; k < tokens[rand].ptrs.Count; k++)
            {
                tokens[rand].ptrs[k] += (tokens[rand].ptrs.Count - k - 1) * 4;
            }


            foreach (int f in jumpstofix)
            {
                tokens[f].data_int = offset - tokens[f].data_int;
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
        public static int Tokenizer_SkipLine(string src, int pos)
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


        public static int Tokenizer_ReadString(string src, int i, char stopchar)
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


        public static void Tokenizer_PutString()
        {
            if (!symbolmarker)
            {
                QToken q = new QToken(QBcode.val_string);
                q.data_string = wordbuf;
                wordbuf = "";
                tokens.Add(q);
                return;
            }
            else
            {
                //QScripted.MainForm.Warn("got symbol!" + wordbuf);
                QToken q = new QToken(QBcode.symbol);
                q.data_string = wordbuf;
                q.data_uint = Checksum.Calc(q.data_string);

                SymbolCache.Add(wordbuf);
                localcache.Add(wordbuf);

                wordbuf = "";
                tokens.Add(q);
                symbolmarker = false;
                return;
            }
        }

        public static void Tokenizer_PutParamString()
        {
            QToken q = new QToken(QBcode.val_string_param);
            q.data_string = wordbuf;
            wordbuf = "";
            tokens.Add(q);
            return;
        }

        private static void Tokenizer_ParseWord(string ss)
        {
            //shouldn't really matter, but still
            string s = ss.Trim().ToLower();

            //empty word, do nothing
            if (s == "") return;

            if (s == "-")
            {
                QToken p = new QToken(QBcode.qbsub);
                //p.data_float = Single.Parse(s);
                tokens.Add(p);
                return;
            }


            if (TextProcessor.maybeInt(s))
            {
                QToken q = new QToken(QBcode.val_int);
                q.data_int = Int32.Parse(s);
                tokens.Add(q);
                //QScripted.MainForm.Warn(q.data_int + "");
                return;
            }



            if (TextProcessor.maybeFloat(s))
            {
                if (s == ".")
                {
                    QToken p = new QToken(QBcode.property);
                    //p.data_float = Single.Parse(s);
                    tokens.Add(p);
                    return;
                }

                QToken q = new QToken(Tokenizer_GetTokenType(QBcode.val_float));
                q.data_float = Single.Parse(s);
                tokens.Add(q);
                return;
            }



            if (TextProcessor.maybeAngle(s))
            {
                QToken q = new QToken(QBcode.val_int);
                q.data_float = Single.Parse(s.Replace("°", ""));
                q.data_float = (float)(q.data_float / QToken.Radian);
                tokens.Add(q);
                //QScripted.MainForm.Warn(q.data_int + "");
                return;
            }


            // TODO: we definitely can make that a loop over a QBcode enum
            // just gotta make sure only a valid subset is used

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.script).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.script));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.endscript).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.endscript));
                return;
            }


            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.globalall).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.globalall));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbif).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbif));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbelse).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbelse));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbelseif).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbelseif));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbendif).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbendif));
                return;
            }


            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.repeat).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.repeat));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.repeatbreak).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.repeatbreak));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.repeatend).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.repeatend));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbnot).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbnot));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbor).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbor));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qband).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qband));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbswitch).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbswitch));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbendswitch).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbendswitch));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbcase).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbcase));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbdefault).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbdefault));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.qbreturn).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.qbreturn));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.random).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.random));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.random2).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.random2));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.randomnorepeat).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.randomnorepeat));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.randompermute).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.randompermute));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.randomrange).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.randomrange));
                return;
            }

            if (s == QBuilder.Tokenizer_GetTokenType(QBcode.randomrange2).GetSyntax())
            {
                tokens.Add(new QToken(QBcode.randomrange2));
                return;
            }


            if (s.Contains("."))
            {
                //QScripted.MainForm.Warn(s + " got into .!");
                string[] buf = s.Split('.');

                Tokenizer_PutSymbol(buf[0], true);
                tokens.Add(new QToken(QBcode.property));
                Tokenizer_PutSymbol(buf[1], true); // so im suppressing this error, cause apparently it's a feature

                return;
            }

            if (s.Contains(":"))
            {
                string[] buf = s.Split(':');

                Tokenizer_PutSymbol(buf[0], true);
                tokens.Add(new QToken(QBcode.member));
                Tokenizer_PutSymbol(buf[1], false); // at least member is always a symbol, i hope?

                return;
            }

            //so nothing worked. let's put a symbol
            Tokenizer_PutSymbol(ss, false);
        }

        private static void Tokenizer_PutSymbol(string s, bool suppressError)
        {
            //maybeGlobal?
            if (s == "")
            {
                if (!suppressError) MainForm.WarnUser($"wtf null putsymbol called at line {QBuilder.lineNumber}");
                return;
            }

            if (s == "<")
            {
                tokens.Add(new QToken(QBcode.less));
                return;
            }

            if (s == "<=")
            {
                tokens.Add(new QToken(QBcode.lesseq));
                return;
            }

            if (s == ">")
            {
                tokens.Add(new QToken(QBcode.greater));
                return;
            }

            if (s == ">=")
            {
                tokens.Add(new QToken(QBcode.greatereq));
                return;
            }


            if (s[0] == '<' && s[s.Length - 1] == '>')
            {
                //yes!
                QToken q = new QToken(QBcode.global);
                tokens.Add(q);
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

            QToken q2 = new QToken(QBcode.symbol);
            q2.data_uint = crc;
            q2.data_string = s;
            tokens.Add(q2);
        }


        public static void Save(string path)
        {
            // dump the script
            Save(path, tokens);

            // dump a separate symbol file, if required.
            if (Settings.Default.useSymFile)
                Save(Path.ChangeExtension(path, ".sym.qb"), symbols);
        }

        /// <summary>
        /// Dumps QChunk array to disk.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="chunks"></param>
        public static void Save(string filepath, List<QToken> chunks)
        {
            using (var bw = new BinaryWriter(File.Create(filepath)))
            {
                foreach (var chunk in chunks)
                    chunk.Write(bw);

                bw.BaseStream.SetLength(bw.BaseStream.Position);
                bw.Flush();
            }
        }



        // stuff below is a messy node array parser

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


            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].QType == QBcode.symbol)
                    if (tokens[i].data_uint == nodeArraycrc)
                    {
                        //it's node array!
                        i = SkipUntil(i, QBcode.array);

                        while (tokens[i].QType != QBcode.endarray)
                        {
                            i = SkipUntil(i, QBcode.structure) + 1;
                            i = ReadNode(tokens, i);
                            i = SkipUntil(i, QBcode.structure) + 1;
                        }
                    }
            }

            return nodeArray;
        }

        public static int SkipUntil(int from, QBcode qc)
        {
            while (tokens[from].QType != qc)
            {
                if (tokens[from].QType == QBcode.endstructure) return from;
                from++;
            }
            return from;
        }

        public static int ReadNode(List<QToken> chunks, int from)
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
                            if (chunks[from].tokenType.Logic == OpLogic.Numeric)
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