using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyThps.Helpers
{
    public class DebugLog
    {
        public static bool Verbose = false;
        public static void Write(string text = "")
        {
            if (Verbose) Console.Write(text);
        }

        public static void WriteLine(string text = "")
        {
            if (Verbose) Console.WriteLine(text);
        }

        public static void Print(string[] lines)
        {
            foreach (var line in lines)
                Console.WriteLine(line);
        }
    }
}