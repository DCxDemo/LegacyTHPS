using System;
using System.Text;

namespace LegacyThps.QScript.Helpers
{
    public class TextProcessor
    {
        /// <summary>
        /// Splits the source code in lines and trims line edges.
        /// </summary>
        /// <param name="sourceText">Arbitrary text string.</param>
        /// <returns>Normalized text string.</returns>
        public static string Normalize(string sourceText)
        {
            var sb = new StringBuilder();

            // get all lines separated by environment set new line symbol
            string[] lines = sourceText.Split(
                new[] { Environment.NewLine }, 
                StringSplitOptions.None
            );

            // add a single new line symbol as we scan chars and convert it later to newline opcode
            foreach (string line in lines)
            {
                sb.Append(line.Trim());
                sb.Append("\n"); // why \n here: we only need 1 char for a newline to make it easier to parse.
            }

            // basically remove last added newline
            sb.Length -= 1;

            return sb.ToString();
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
    }
}
