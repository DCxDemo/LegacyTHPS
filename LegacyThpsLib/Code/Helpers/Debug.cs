using System;

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
            if (Verbose)
                Console.WriteLine(text);
        }

        public static void Print(params string[] lines)
        {
            foreach (var line in lines)
                Console.WriteLine(line);
        }
    }
}