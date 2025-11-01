using FastColoredTextBoxNS;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Settings = ThpsQScriptEd.Properties.Settings;
using Resources = ThpsQScriptEd.Properties.Resources;
using System.Drawing.Imaging;

namespace ThpsQScriptEd
{
    partial class MainForm
    {
        //additional helper mainform functions

        public void SetTitle(string filename)
        {
            this.Text = $"{Resources.AppName}";
            if (filename != "")
                this.Text += $" - {filename}";
        }

        public static void WarnUser(string msg, string title = "") => MessageBox.Show(msg, title == "" ? Strings.Warning : title);

        public static DialogResult AskUser(string msg) => MessageBox.Show(msg, Strings.Warning, MessageBoxButtons.YesNo);

        private void WrapOn()
        {
            codeBox.WordWrap = true;
            codeBox.ShowScrollBars = true;
            codeBox.SelectionLength = 0;
            codeBox.SelectionStart = 0;
            wordWrapToolStripMenuItem.Checked = true;
            codeBox.WordWrap = true;
        }

        private void WrapOff()
        {
            codeBox.WordWrap = false;
            codeBox.ShowScrollBars = true;
            codeBox.SelectionLength = 0;
            codeBox.SelectionStart = 0;
            wordWrapToolStripMenuItem.Checked = false;
            codeBox.WordWrap = false;
        }

        private void FillTextBox(FastColoredTextBox t, string text)
        {
            t.Text = text;
            t.SelectionLength = 0;
            t.SelectionStart = 0;
        }

        private void AppendTextBox(FastColoredTextBox t, string text)
        {
            t.Text += "\r\n" + text;
            t.SelectionLength = 0;
            t.SelectionStart = 0;
        }

        private void LockForm()
        {
            codeBox.Enabled = false;
            menuStrip1.Enabled = false;
        }

        private void UnlockForm()
        {
            codeBox.Enabled = true;
            menuStrip1.Enabled = true;
        }

        //move to form code maybe
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
                e.Effect = DragDropEffects.All;
        }


        // ==================color themes======================================

        Dictionary<string, Theme> themes = new Dictionary<string, Theme>();

        private void LoadThemes()
        {
            //move to an external file
            themes.Add("Default", new Theme("Default", Color.White, Color.Black));
            themes.Add("Papyrus", new Theme("Papyrus", Color.OldLace, Color.Sienna));
            themes.Add("Inverted", new Theme("Inverted", Color.FromArgb(30, 30, 30), Color.FromArgb(220, 220, 220)));
            themes.Add("Night", new Theme("Night", Color.Black, Color.Gray));
            themes.Add("Danger", new Theme("Danger", Color.DarkRed, Color.Yellow));
            themes.Add("Pascal", new Theme("Pascal", Color.DarkBlue, Color.Yellow));
            themes.Add("Navy", new Theme("Navy", Color.MidnightBlue, Color.LightSkyBlue));
            themes.Add("Rose", new Theme("Rose", Color.DarkMagenta, Color.Pink));
            themes.Add("Forest", new Theme("Forest", Color.DarkGreen, Color.LimeGreen));
            themes.Add("Matrix", new Theme("Matrix", Color.Black, Color.Lime));

            foreach (var theme in themes.Values)
                toolStripComboBox1.Items.Add(theme.Name);

            bool gotTheme = false;
            int i = 0;

            foreach (var kvp in themes)
            {
                if (kvp.Value.Name == Settings.Default.chosenTheme)
                {
                    toolStripComboBox1.SelectedIndex = i;
                    gotTheme = true;
                }
                i++;
            }

            if (!gotTheme) toolStripComboBox1.SelectedIndex = 0;

            themes[Settings.Default.chosenTheme].Apply(codeBox, scriptList);
        }

        // ==================color themes======================================


    }
}