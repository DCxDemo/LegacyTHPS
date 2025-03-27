using LegacyThps.QScript.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ThpsQScriptEd;
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

            var symbolCachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache.sym.qb");

            // if such file exists
            if (File.Exists(symbolCachePath))
            {
                // make sure qbuilder is ready
                QBuilder.Init();

                // load cache file
                QBuilder.LoadCompiledScript(symbolCachePath);
            }

            LoadCFuncs();
        }

        public static void LoadCFuncs()
        {
            LoadCFuncs("data\\exefuncs.txt");
            LoadCFuncs("data\\exefuncs_th4.txt");
            LoadCFuncs("data\\exefuncs_ug1.txt");
        }

        public static void LoadCFuncs(string filename)
        {
            if (File.Exists(filename))
            {
                var list = File.ReadAllLines(filename);

                foreach (var line in list)
                    SymbolCache.Add(line);
            }
        }



        public static void Clear()
        {
            Entries = null;
            Create();
        }

        public static int Count() => Entries.Count;

        public static void Add(QChunk i)
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

        /// <summary>
        /// Add a new symbol by name and calculate hash automatically.
        /// </summary>
        /// <param name="symbol"></param>
        public static void Add(string symbol)
        {
            uint crc = Checksum.Calc(symbol);

            if (!SymbolCache.Entries.ContainsKey(crc))
                Entries.Add(crc, symbol);
        }

        /// <summary>
        /// Add a new symbol with the provided hash without the check.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="symbol"></param>
        public static void Add(uint hash, string symbol)
        {
            if (!SymbolCache.Entries.ContainsKey(hash))
                Entries.Add(hash, symbol);

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
            // first we check if it's a hex string
            if (symbol[0] == '0' && (symbol[1] == 'x' || symbol[1] == 'X'))
            {
                try
                {
                    return Convert.ToUInt32(symbol, 16);
                }
                catch (Exception ex)
                {
                    MainForm.WarnUser($"failed to parse hex number {symbol}\r\n{ex.Message}");
                }
            }

            // then try fast linear lookup without casing first due to performance
            foreach (var entry in SymbolCache.Entries)
            {
                if (symbol == entry.Value)
                    return entry.Key;
            }

            // since we got no match here, try the slow uppercase
            foreach (var entry in SymbolCache.Entries)
            {
                if (symbol.ToUpperInvariant() == entry.Value.ToUpperInvariant())
                    return entry.Key;
            }

            // if everything failed, calculate it. should we maybe add it here too?
            return Checksum.Calc(symbol);
        }




        /// <summary>
        /// Looks for mismatching checksums in the symbol cache.
        /// </summary>
        public static void Validate()
        {
            // it is important to let user have the option to keep invalid checksums
            // cause there is always a possiblity of manually baked keys that don't correspond to actual NS hash
            // this ruins compilation of such files and leads to unknown symbols
            // THQBEditor never fixed the hashes and handled unknown hashes as "pseudo"
            // for files edited from scratch with this tool it's recommended to have it always enabled

            int numErrors = 0;

            // we create a new temp list here, since we'll attempt to remove values from it
            foreach (var entry in SymbolCache.Entries.ToDictionary(x => x.Key, x => x.Value))
            {
                // compare stored symbol hash to a calculated hash
                if (entry.Key != Checksum.Calc(entry.Value))
                {
                    // if user wants to fixup incorrect checksums
                    if (Settings.Default.fixIncorrectChecksums)
                    {
                        //MainForm.Warn($"fixing {v.Value}: {v.Key.ToString("X8")} vs {Checksum.Calc(v.Value).ToString("X8")}");

                        // remove the symbol by hash
                        SymbolCache.Entries.Remove(entry.Key);

                        // add symbol by values, it handles the checksum calculation inside
                        SymbolCache.Add(entry.Value);
                    }
                    else
                    {
                        // otherwise simply count the errors for the report
                        numErrors++;
                    }
                }
            }

            // finally, if we don't fix them up, report the number
            if (!Settings.Default.fixIncorrectChecksums && numErrors > 0)
                MainForm.WarnUser($"{numErrors} incorrect checksums found.");
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

        /// <summary>
        /// Dumps entire symbol cache to disk.
        /// </summary>
        public static void DumpSymbolCache()
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

        /// <summary>
        /// List checksum mismatches in the cache.
        /// </summary>
        /// <returns></returns>
        public static string ReportErrors()
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