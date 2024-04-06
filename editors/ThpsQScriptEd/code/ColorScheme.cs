using FastColoredTextBoxNS;
using System.Drawing;
using System.Windows.Forms;

namespace ThpsQScriptEd
{
    class Theme
    {
        public string Name;

        public Color TextColor;
        public Color BackgroundColor;

        public Theme()
        {
        }

        public Theme(string name, Color back, Color front)
        {
            Name = name;
            TextColor = front;
            BackgroundColor = back;
        }

        public void Apply(FastColoredTextBox t, ListBox l)
        {
            t.BackColor = BackgroundColor;
            t.ForeColor = TextColor;

            l.BackColor = BackgroundColor;
            l.ForeColor = TextColor;
        }
    }
}