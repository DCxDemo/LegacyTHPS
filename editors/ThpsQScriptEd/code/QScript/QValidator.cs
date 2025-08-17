using System;
using System.Collections.Generic;
using System.Text;
using ThpsQScriptEd;

namespace LegacyThps.QScript
{
    /// <summary>
    /// This class supposed to store  additional helper checks one might require to process a list of Q tokens.
    /// These functions are supposed to either evaluate or alter the internals of the passed list.
    /// </summary>
    public static class QValidator
    {
        /// <summary>
        /// Makes sure one does not simply miss a bracket.
        /// Cause it freezes the game and it's annoying to fix.
        /// </summary>
        /// <param name="chunks">List of Q chunks.</param>
        /// <returns>Last successfully processed line, -1 if everything's fine.</returns>
        public static int PerformBracketsCheck(List<QToken> chunks)
        {
            // pretty sure it's super inefficient, but it works just fine
            // it builds a string like "{{[]()}}" by adding every next bracket symbol to stack
            // then it replaces {} () [] combinations with a null string to eliminate properly enclosed chunks

            // if closing bracket was added and there is no opening bracket, it throws an error
            // if string is not empty by the end, there is some error
            // there is also an arbitrary nesting limit of 16, just for sanity


            string stack = "";

            // lines are counted using tokenized new lines, may differ from source lines...
            int line = 0;

            // supposed to remember the line where stack was last seen empty
            int lastemptystackline = -1;

            // breaks the loop early if detected any inconsistency
            bool die = false;


            foreach (var c in chunks) // (int i = 0; i < chunks.Count; i++)
            {
                switch (c.QType)
                {
                    case QBcode.val_string:
                    case QBcode.val_string_param:
                        if (c.data_string.Length > 255) { die = true; }
                        break;

                    case QBcode.newline:
                    case QBcode.newline_debug: line++; break;

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

                    case QBcode.repeat: stack += "l"; break;
                    case QBcode.repeatend: stack += "e"; if (!stack.Contains("l")) die = true; break;
                }

                // inefficiency fest. at least we operate on a relatively short string.
                stack = stack.Replace("{}", "")
                    .Replace("()", "")
                    .Replace("[]", "")
                    .Replace("<>", "")
                    .Replace("\\-/", "")
                    .Replace("\\/", "")
                    .Replace("le", "");

                if (stack == "") lastemptystackline = line;

                if (stack.Length > 32) break;

                if (die) break;
            }

            return stack == "" ? -1 : lastemptystackline;
        }
    }
}