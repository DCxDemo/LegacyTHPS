using System.Collections.Generic;
using System.Linq;

namespace LegacyThps.QScript.Nodes
{
    public class GapNode
    {
        public string Level;
        public string GapID;
        public string Text;
        public int Score;
        public string GapScript;
        public int Found;
        public List<GapFlag> Flags = new List<GapFlag>();

        public GapNode()
        {
        }

        public GapNode(string lp, string gp, string tp, int sp, string ss, params GapFlag[] fl)
        {
            Level = lp;
            GapID = gp;
            Text = tp;
            Score = sp;
            GapScript = ss;
            Found = 0;
            Flags.AddRange(fl.ToList());
        }


        private string FlagsToString()
        {
            string result = "";

            foreach (GapFlag g in Flags)
                result += g.ToString() + " ";

            return "[ " + result + "]";
        }

        private static string GapString = "Gap_{1} = { Level = {0} GapID = {1} text = \"{2}\" score = {3} GapScript = {4} Found = {5} Flags = {6} }";

        public override string ToString()
        {
            return string.Format(GapString, Level, GapID, Text, Score, GapScript, Found, FlagsToString());
        }
    }
}