using System;
using System.Xml;

namespace LegacyThps.QScript
{
    /// <summary>
    /// It's a definition of a token type, obtained from an external XML file.
    /// </summary>
    public class QTokenType
    {
        public byte Code = 0xFF;
        public string Name = "default";
        public string Syntax = "";
        public DataGroup Group = DataGroup.Unknown;
        public OpLogic Logic = OpLogic.Unknown;
        public NestCommand Nesting = NestCommand.None;

        public QTokenType(string name, string syn, OpLogic log)
        {
            Name = name;
            Syntax = syn;
            Logic = log;
        }

        public static QTokenType Space = new QTokenType("space", " ", OpLogic.Reserved);

        // TODO: convert to XML serialization rather than manual node parsing
        public QTokenType(XmlNode n)
        {
            if (n.Attributes["code"] != null)
                Code = Convert.ToByte(n.Attributes["code"].Value, 16);

            if (n.Attributes["name"] != null)
                Name = n.Attributes["name"].Value;

            if (n.Attributes["syntax"] != null)
                Syntax = n.Attributes["syntax"].Value;

            if (n.Attributes["datagroup"] != null)
                Group = (DataGroup)Enum.Parse(typeof(DataGroup), n.Attributes["datagroup"].Value, true);

            if (n.Attributes["logic"] != null)
                Logic = (OpLogic)Enum.Parse(typeof(OpLogic), n.Attributes["logic"].Value, true);

            if (n.Attributes["nesting"] != null)
                Nesting = (NestCommand)Enum.Parse(typeof(NestCommand), n.Attributes["nesting"].Value, true);

            //QScripted.MainForm.Warn(Name + " " + Group + " " + Logic);
        }

        // TODO: convert to getter
        public string GetSyntax()
        {
            if (Syntax == "newline") return Environment.NewLine;
            if (Syntax == "auto") return Name;

            return Syntax;
        }
    }
}