using LegacyThps.QScript.Helpers;
using ThpsQScriptEd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Settings = ThpsQScriptEd.Properties.Settings;

namespace LegacyThps.QScript
{
    public class SymbolCache
    {
        public static List<string> Scripts = new List<string>();

        private static Dictionary<uint, string> Entries;

        public static void Create()
        {
            if (Entries == null)
                Entries = new Dictionary<uint, string>();

            if (File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}\\cache.sym.qb"))
            {
                QBuilder.Init();
                QBuilder.LoadCompiledScript($"{AppDomain.CurrentDomain.BaseDirectory}\\cache.sym.qb");
            }
        }

        public static void Clear()
        {
            Entries = null;
            Create();
        }

        public static int Count() => Entries.Count;

        public static void Add(Instruction i)
        {
            if (!SymbolCache.Entries.ContainsKey(i.data_uint))
            {
                Entries.Add(i.data_uint, i.data_string);
            }
            else
            {
                //okay so here we're checking whether we have a version of existing symbol that is not all lowercase
                //we prefer TrickSlack over trickslack any other day
                //maybe should add caps counter, since trickSlack will overwrite TrickSlack?

                string kek = SymbolCache.Entries[i.data_uint];

                if (kek != i.data_string)
                    if (i.data_string.ToLower() == kek.ToLower())
                        if (i.data_string.ToLower() != i.data_string)
                        {
                            Entries.Remove(i.data_uint);
                            Entries.Add(i.data_uint, i.data_string);
                        }
            }
        }


        public static void Add(string symbol)
        {
            uint crc = Checksum.Calc(symbol);

            if (!SymbolCache.Entries.ContainsKey(crc))
                Entries.Add(crc, symbol);
        }

        public static void Add(uint crc, string s)
        {
            if (!SymbolCache.Entries.ContainsKey(crc))
                Entries.Add(crc, s);

            //if (Checksum.Calc(s) != crc)
            //MainForm.Warn($"{s} != {crc.ToString("X8")}");
        }

        public static string Find(uint crc)
        {
            if (Entries.ContainsKey(crc))
                return Entries[crc];

            return null;
        }

        public static uint GetSymbolHash(string symbol)
        {
            //first we check if it's a hex string
            if (symbol[0] == '0' && (symbol[1] == 'x' || symbol[1] == 'X'))
            {
                try
                {
                    return Convert.ToUInt32(symbol, 16);
                }
                catch (Exception ex)
                {
                    ThpsQScriptEd.MainForm.WarnUser($"failed to parse hex number {symbol}\r\n" + ex.Message);
                }
            }

            //then try fast without casing first due to performance
            foreach (var entry in SymbolCache.Entries)
            {
                if (symbol == entry.Value)
                    return entry.Key;
            }

            //now we got no match, try with uppercase
            foreach (var v in SymbolCache.Entries)
            {
                if (symbol.ToUpper() == v.Value.ToUpper())
                    return v.Key;
            }

            //if everything failed, calculate it
            return Checksum.Calc(symbol);
        }


        public static void MaybeFix()
        {
            int errors = 0;

            foreach (var v in SymbolCache.Entries.ToDictionary(x => x.Key, x => x.Value))
            {
                if (v.Key != Checksum.Calc(v.Value))
                {
                    if (Settings.Default.fixIncorrectChecksums)
                    {
                        //MainForm.Warn($"fixing {v.Value}: {v.Key.ToString("X8")} vs {Checksum.Calc(v.Value).ToString("X8")}");
                        SymbolCache.Entries.Remove(v.Key);
                        SymbolCache.Add(v.Value);
                    }
                    else
                    {
                        if (v.Key != Checksum.Calc(v.Value))
                            errors++;
                    }
                }
            }

            if (!Settings.Default.fixIncorrectChecksums)
                if (errors > 0)
                    ThpsQScriptEd.MainForm.WarnUser($"{errors} incorrect checksums found.");
        }

        public static string GetSymbolName(uint hash)
        {
            if (SymbolCache.Entries.ContainsKey(hash))
                return SymbolCache.Entries[hash];

            //if key not found, return 0x prefixed hex string of the key itself
            return $"0x{hash.ToString("X8")}";
        }

        public static void DumpText(string path)
        {
            var sb = new StringBuilder();

            foreach (var kvp in Entries)
            {
                sb.AppendLine($"\"{kvp.Key.ToString("X8")}\",\"{kvp.Key.ToString()}\",\"{kvp.Value}\"");
                if (kvp.Value.Contains(',')) ThpsQScriptEd.MainForm.WarnUser("Comma found in symbol: " + kvp.Value);
            }

            File.WriteAllText(path, sb.ToString());
        }

        public static void DumpQB()
        {
            byte[] finalbytes = new byte[0];

            using (var stream = new MemoryStream())
            {
                using (var bw = new BinaryWriter(stream))
                {
                    foreach (var kvp in Entries)
                    {
                        bw.Write((byte)QBcode.symboldef);
                        bw.Write(kvp.Key);
                        bw.Write(System.Text.Encoding.Default.GetBytes(kvp.Value));
                        bw.Write((byte)0);
                    }

                    bw.Write((byte)QBcode.endfile);

                    stream.Flush();
                    finalbytes = stream.GetBuffer();

                    Array.Resize(ref finalbytes, (int)bw.BaseStream.Position);
                }
            }

            File.WriteAllBytes("cache.sym.qb", finalbytes);
        }

        //validates checksum cache, returns list of errors, empty if no errors 
        public static string Validate()
        {
            var sb = new StringBuilder();

            //for each hash in cache
            foreach (var hash in SymbolCache.Entries)
            {
                uint crc = Checksum.Calc(hash.Value);  //calculate string checksum

                //if doesnt match, report
                if (hash.Key != crc)
                    sb.AppendLine($"#\"{hash.Value}\" = {crc.ToString("X8")}, but {hash.Key.ToString("X8")} in cache");
            }

            return sb.ToString();
        }
    }
}