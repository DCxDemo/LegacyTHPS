using FastColoredTextBoxNS;
using LegacyThps.QScript;
using LegacyThps.QScript.Helpers;
using LegacyThps.QScript.Nodes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Settings = ThpsQScriptEd.Properties.Settings;

namespace ThpsQScriptEd
{
    public partial class MainForm : Form
    {
        AutocompleteMenu autocomplete;

        SettingsForm sf;

        public List<string> exefuncs = new List<string>();
        public List<string> scrfuncs = new List<string>();


        public MainForm()
        {
            InitializeComponent();

            checksumHelper.Text = "";

            //this is here to force dot floating point format.
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            LoadThemes();

            //load from settings
            //Location = Settings.Default.formLocation;

            Size = Settings.Default.formSize;
            scriptList.Font = Settings.Default.editorFont;
            splitContainer1.Panel1Collapsed = Settings.Default.hideScriptsList;
            hideScriptsListToolStripMenuItem.Checked = Settings.Default.hideScriptsList;
            if (Settings.Default.wordWrap) WrapOn(); else WrapOff();


            codeBox.Text = "ThpsQScriptEd\r\n2018, DCxDemo*.";
            codeBox.Font = Settings.Default.editorFont;

            SetTitle("");
            //SetTabWidth(codeBox, 4);

            QBuilder.Init();

            //XmlDocument xml = new XmlDocument();
            //xml.LoadXml(File.ReadAllText("qScriptSyntexHighlighter.xml"));

            //codeBox.SyntaxHighlighter.AddXmlDescription("qScriptSyntax", xml);

            string exefuncslist = "data\\exefuncs.txt";

            if (File.Exists(exefuncslist))
                exefuncs.AddRange(File.ReadAllLines(exefuncslist));

            string scrfuncslist = "data\\scriptfuncs_th3.txt";

            if (File.Exists(scrfuncslist))
                scrfuncs.AddRange(File.ReadAllLines(scrfuncslist));

            /*
            styleKeyword = new TextStyle(Brushes.DarkBlue, null, FontStyle.Bold);
            styleText = new TextStyle(Brushes.Brown, null, FontStyle.Regular);
            styleComment = new TextStyle(Brushes.DarkGreen, null, FontStyle.Regular);
            styleGlobal = new TextStyle(Brushes.Magenta, null, FontStyle.Bold);
            styleNumber = new TextStyle(Brushes.Red, null, FontStyle.Bold);
            styleFunction = new TextStyle(Brushes.Teal, null, FontStyle.Regular);
            */

            autocomplete = new AutocompleteMenu(codeBox);
            autocomplete.Font = codeBox.Font;
            BuildAutocompleteMenu();
        }

        private void UpdateTheme(string name)
        {
            Settings.Default.chosenTheme = name;

            var theme = themes[Settings.Default.chosenTheme];

            theme.Apply(codeBox, scriptList);
        }

        private void BuildAutocompleteMenu()
        {
            if (!File.Exists("data\\exefuncs.txt")) return;
            string[] snippets = File.ReadAllLines("data\\exefuncs.txt");

            var items = new List<AutocompleteItem>();

            foreach (var item in snippets)
                items.Add(new SnippetAutocompleteItem($"{item} " /* a shtupid extra space hack here */ ) { ImageIndex = 1 });

            //set as autocomplete source
            autocomplete.Items.SetAutocompleteItems(items);
        }

        string path = "";
        QB qb = new QB();

        public static bool alertChanges = false;

        //opens binary qb file
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Neversoft QScript files (*.q, *.qb)|*.qb;*.q";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                switch (Path.GetExtension(ofd.FileName).ToUpper())
                {
                    case ".Q": LoadQSource(ofd.FileName, true); break;
                    case ".QB": LoadQBinary(ofd.FileName); break;
                }
            }

            alertChanges = false;
        }

        //opens text q file
        private void LoadQSource(string filename, bool updatePath)
        {
            codeBuilder.Clear();
            codeBuilder.Append(File.ReadAllText(filename));

            if (updatePath)
            {
                path = filename;
                SetTitle(path);
            }

            FillTextBox(codeBox, codeBuilder.ToString());

            qb = new QB();
            qb.UpdateText(codeBox.Text);
            qb.filename = filename;


            scriptList.Items.Clear();
            foreach (string scr in qb.scripts)
            {
                scriptList.Items.Add(scr);

                if (!SymbolCache.Scripts.Contains(scr))
                    SymbolCache.Scripts.Add(scr);
            }
        }

        //load rollback file
        private void rollbackClick(object sender, EventArgs e)
        {
            string fn = (sender as ToolStripMenuItem).Name;

            if (File.Exists(fn))
            {
                LoadQSource(fn, false);
                MainForm.WarnUser(fn + " loaded");
            }
            else
            {
                MainForm.WarnUser("Missing rollback file, maybe removed manually.");
                UpdateRollbacks(path);
            }
        }

        //populate rollback file list based on available files
        private void UpdateRollbacks(string s)
        {
            string filename = Path.GetFileNameWithoutExtension(s);

            string[] backups = Directory.GetFiles(Path.GetDirectoryName(s), filename + ".*.bkp.q");

            rollbackToolStripMenuItem.Enabled = false;
            rollbackToolStripMenuItem.DropDownItems.Clear();

            if (backups.Length > 0)
            {
                foreach (string b in backups)
                {
                    ToolStripMenuItem t = new ToolStripMenuItem(GetBackupDate(b), null, new EventHandler(rollbackClick), b);
                    rollbackToolStripMenuItem.DropDownItems.Add(t);
                }

                rollbackToolStripMenuItem.Enabled = true;
            }
        }
        
        //gets backup date, much wow
        private string GetBackupDate(string s)
        {
            string[] kek = Path.GetFileName(s).Split('.');

            if (kek.Length >= 3)
                return kek[kek.Length - 3];

            return "";
        }


        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(Keys vKey);


        private void LoadQBinary(string filename)
        {
            string qpath = Path.ChangeExtension(filename, "q");

            UpdateRollbacks(qpath);


            if (File.Exists(qpath))
            {
                if (Settings.Default.alwaysLoadSource)
                {
                    LoadQSource(qpath, true);
                    return;
                }
                else
                {
                    if (MainForm.AskUser(Strings.SourceFileDetected) == DialogResult.Yes)
                    {
                        LoadQSource(qpath, true);
                        return;
                    }
                }
            }

            QBuilder.Init();
            QBuilder.LoadCompiledScript(filename);

            if (ctrlState)
            {
                //MainForm.Warn("control is pressed");
                AppendTextBox(codeBox, "//" + qpath);
                AppendTextBox(codeBox, QBuilder.GetSource(false));

                codeBuilder.Append("\r\n;" + qpath + "\r\n");
                codeBuilder.Append(QBuilder.GetSource(false));
            }
            else
            {
                //MainForm.Warn("control not pressed");
                FillTextBox(codeBox, QBuilder.GetSource(false));
                codeBuilder.Append(QBuilder.GetSource(false));
            }

            path = filename;
            SetTitle(filename);

            scriptList.Items.Clear();

            foreach (string scr in QBuilder.GetScriptsList())
            {
                scriptList.Items.Add(scr);

                if (!SymbolCache.Scripts.Contains(scr))
                    SymbolCache.Scripts.Add(scr);
            }


            //SymbolCache.Validate();


            QBlevelbox.Text = "QB mode: " + QBuilder.currentQBlevel.ToString();

        }


        StringBuilder codeBuilder = new StringBuilder();


        private bool ctrlState = false;



        List<string> textExt = new List<string> { ".txt", ".q", ".bak", ".lst", ".qn", ".ini", ".xml", ".json", ".csv", ".htm" };

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            ctrlState = GetAsyncKeyState(Keys.ControlKey) != 0;

            codeBox.Text = "";
            codeBuilder.Clear();

            foreach (string filename in files)
            {

                if (!ctrlState)
                {

                    if (codeBox.Text.Trim() != "" && alertChanges)
                    {
                        if (MainForm.AskUser(Strings.UnsavedChanges) != DialogResult.Yes)
                        {
                            FillTextBox(codeBox, codeBuilder.ToString());
                            return;
                        }
                    }

                    string ext = Path.GetExtension(filename).ToLower();

                    if (ext == ".qb")
                    {
                        alertChanges = false;
                        LoadQBinary(filename);
                        FillTextBox(codeBox, codeBuilder.ToString());
                        return;
                    }

                    if (textExt.Contains(ext))
                    {
                        alertChanges = false;
                        LoadQSource(filename, true);
                        FillTextBox(codeBox, codeBuilder.ToString());
                        return;
                    }

                    WarnUser("Unsupported file.\r\n" + filename);
                    return;
                }
                else
                {
                    string ext = Path.GetExtension(filename).ToLower();

                    if (ext == ".qb")
                    {
                        alertChanges = false;
                        LoadQBinary(filename);
                        continue;
                    }

                    if (textExt.Contains(ext))
                    {
                        alertChanges = false;
                        LoadQSource(filename, true);
                        continue;
                    }

                    WarnUser("Unsupported file.\r\n" + filename);
                }
                //if (!shiftPressed) break;
            }

            FillTextBox(codeBox, codeBuilder.ToString());
        }







        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            alertChanges = true;

            if (e.KeyCode == Keys.A && e.Control)
                codeBox.SelectAll();
        }

        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.wordWrap = wordWrapToolStripMenuItem.Checked;
            if (Settings.Default.wordWrap) WrapOn(); else WrapOff();
        }


        private bool ChoosePath(string ext)
        {
            if (path == "")
            {
                var sfd = new SaveFileDialog();

                if (ext == "qb") sfd.Filter = "Q Script compiled binary (*.qb)|*.qb";
                if (ext == "q") sfd.Filter = "Q Script source code (*.q)|*.q";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    path = sfd.FileName;
                    SetTitle(path);
                    return true;
                }

                return false;
            }

            return true;
        }


        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path == "")
                if (!ChoosePath("q"))
                    return;

            string qpath = Path.ChangeExtension(path, ".q");
            File.WriteAllText(qpath, codeBox.Text);
            alertChanges = false;
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //wtf
            ChoosePath("q");
            if (!ChoosePath("q")) return;

            string qpath = Path.ChangeExtension(path, ".q");
            File.WriteAllText(qpath, codeBox.Text);
            alertChanges = false;
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // maybe we don't have path
            if (path == "") ChoosePath("qb");

            if (path == "") return;

            // get correct paths for q and qb
            string qpath = Path.ChangeExtension(path, ".q");
            string qbpath = Path.ChangeExtension(path, ".qb");

            // compile and save to binary
            QBuilder.Compile(codeBox.Text);
            QBuilder.SaveChunks(qbpath);

            // maybe backup?
            if (Settings.Default.enableBackups)
                if (File.Exists(qpath))
                {
                    string targetpath = Path.ChangeExtension(qpath, $".{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.bkp.q");

                    //if you hit compile twice a second, there is a file already.
                    if (File.Exists(targetpath))
                        File.Delete(targetpath);

                    File.Copy(qpath, targetpath);
                }

            // save q script
            File.WriteAllText(qpath, codeBox.Text);

            //old code
            //qb.UpdateText(codeBox.Text);
            //qb.Compile();
            //qb.Save();

            UpdateRollbacks(qpath);

            alertChanges = false;

            GC.Collect();
        }


        private void openInNotepadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Settings.Default.extrenalEditor.Trim() == "")
                {
                    WarnUser(Strings.MissingEditor);
                    return;
                }

                string pth = (path != null) ? Path.GetFileNameWithoutExtension(path) : "";
                string tmp = Path.Combine(Path.GetTempPath(), $"temp_{pth}_{Checksum.Calc(DateTime.Now.ToString())}.q");

                //MainForm.Warn(tmp);

                //pass source code text to QB object
                //qb.UpdateText(codeBox.Text);
                //qb.SaveText(tmp);

                File.WriteAllText(tmp, codeBox.Text);

                var process = System.Diagnostics.Process.Start(Settings.Default.extrenalEditor, tmp);
                process = System.Diagnostics.Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Settings.Default.extrenalEditor))[0];


                LockForm();

                //wait for the editor
                while (!process.HasExited)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100); //stay awhile and listen...
                }

                //import text
                if (File.Exists(tmp))
                {
                    codeBox.Text = File.ReadAllText(tmp);
                    File.Delete(tmp);
                }

                UnlockForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTheme(toolStripComboBox1.Text);
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // set up font box properly
            var fd = new FontDialog() {
                // existing font
                FontMustExist = true,
                // monospace font
                FixedPitchOnly = true,
                // load selected font from settings
                Font = Settings.Default.editorFont
            };

            if (fd.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.editorFont = fd.Font;
                codeBox.Font = fd.Font;
                scriptList.Font = fd.Font;
            }
        }

        private void dumpHasValuesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SymbolCache.Count() == 0)
            {
                WarnUser("Nothing to dump!");
                return;
            }

            var sfd = new SaveFileDialog() {
                Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
                SymbolCache.DumpText(sfd.FileName);
            //System.Diagnostics.Process.Start(sfd.FileName);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
            Application.Exit();
        }

        private void qBSpecsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("find good specs or remove");
        }

        private void wordWrapToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Settings.Default.wordWrap = wordWrapToolStripMenuItem.Checked;
            if (Settings.Default.wordWrap) WrapOn(); else WrapOff();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("QScripted.\r\nMany codes.\r\nSuch wow.\r\n\r\n2018, DCxDemo*.");
        }


        private void dumpHashInQBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SymbolCache.DumpSymbolCache();
        }

        private void sortOfManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/DCxDemo/LegacyThps/wiki/ThpsQScriptEd");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.formLocation = this.Location;
            Settings.Default.formSize = this.Size;
            Settings.Default.Save();
        }


        private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            sf = new SettingsForm();

            sf.Sync();
            if (sf.ShowDialog(this) == DialogResult.OK)
            {
            }
            
        }



        private void scriptList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                int pos = CultureInfo.InvariantCulture.CompareInfo.IndexOf(codeBox.Text, "script " + scriptList.SelectedItem.ToString(), 0, CompareOptions.IgnoreCase);
                int nextspace = codeBox.Text.IndexOf(' ', pos);

                this.ActiveControl = codeBox;
                codeBox.SelectionStart = pos;
                codeBox.SelectionLength = nextspace - pos;
                //codeBox.ScrollToCaret();
            }
            catch
            {
                WarnUser("Reached the end of file.");
            }
        }

        private void hideScriptsListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.hideScriptsList = hideScriptsListToolStripMenuItem.Checked;
            splitContainer1.Panel1Collapsed = Settings.Default.hideScriptsList;
        }

        private void clearGlobalCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SymbolCache.Clear();
        }

        private void codeBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (codeBox.SelectionLength != 0)
                if (codeBox.SelectedText.IndexOf("\r\n") == -1)
                {
                    string str = codeBox.SelectedText.Trim();
                    infoText.Text = str + " = " + Checksum.Calc(str).ToString("X8");
                }
        }

        private void parseNodeArrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var nodeArray = QBuilder.GetNodeArray();

            var sb = new StringBuilder();

            foreach (var node in nodeArray)
                sb.Append(node.ToCSV());

            codeBox.Clear();
            FillTextBox(codeBox, sb.ToString());
        }

        bool grepConfirmed = false;

        private void grepEnfgapToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (!grepConfirmed)
                if (MainForm.AskUser(Strings.GrepWarn) == DialogResult.No)
                    return;

            grepConfirmed = true;

            var sb = new StringBuilder();

            foreach (string s in codeBox.Lines)
            {
                if (s.Trim().IndexOf("EndGap") == 0 || s.Trim().IndexOf("StartGap") == 0)
                {
                    sb.Append(s + "\r\n");
                }
            }

            codeBox.Text = sb.ToString();
        }

        private void grepScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!grepConfirmed)
                if (MainForm.AskUser(Strings.GrepWarn) == DialogResult.No)
                    return;

            grepConfirmed = true;

            var sb = new StringBuilder();
            foreach (string s in codeBox.Lines)
            {
                if (s.Trim().IndexOf("script") == 0)
                {
                    sb.Append(s.Split(' ')[1] + "\r\n");
                }
            }

            codeBox.Text = sb.ToString();
        }

        private void infoText_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(infoText.Text);
        }

        private void searchTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            codeBox.ShowFindDialog();
        }

        private void validateSymbolCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string result = SymbolCache.ReportErrors();

            MainForm.WarnUser(result == "" ? $"{SymbolCache.Count()} symbols checked: Symbol cache OK!" : result);
        }

        private void openScriptsFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Settings.Default.scriptsPath))
                Process.Start(Settings.Default.scriptsPath);
            else
                WarnUser("No scripts path found, please choose a valid path in Settings.");
        }

        private void codeBox_MouseUp_1(object sender, MouseEventArgs e)
        {
            string text = codeBox.SelectedText;

            // no nulls
            if (text == "") return;

            // no multilines
            if (text.Contains(Environment.NewLine)) return;

            checksumHelper.Tag = SymbolCache.GetSymbolHash(text);
            checksumHelper.Text = $"{text} = {((uint)checksumHelper.Tag).ToString("X8")}";
        }

        private void checksumHelper_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(((uint)checksumHelper.Tag).ToString("X8"));
        }

        private void dumpScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            foreach (var str in SymbolCache.Scripts)
                sb.AppendLine(str);

            File.WriteAllText("scripts.txt", sb.ToString());
        }

        private void legacyThpsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/vTWucHS");
        }

        private void legacyThpsGithubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/DCxDemo/LegacyThps");
        }
    }
}