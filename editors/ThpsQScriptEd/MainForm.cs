using FastColoredTextBoxNS;
using LegacyThps.QScript;
using LegacyThps.QScript.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
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

            scriptList.Font = Settings.Default.editorFont;
            splitContainer1.Panel1Collapsed = Settings.Default.hideScriptsList;
            hideScriptsListToolStripMenuItem.Checked = Settings.Default.hideScriptsList;

            WindowState = (FormWindowState)Settings.Default.windowState;

            // we don't need it to be minimized by default
            // may happen if user kills the minimized app
            if (WindowState == FormWindowState.Minimized )
                WindowState = FormWindowState.Normal;

            // only apply size if we're windowed
            if (WindowState == FormWindowState.Normal)
                Size = Settings.Default.formSize;

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


            scriptList.BeginUpdate();

            scriptList.Items.Clear();

            var scriptLines = codeBox.FindLines("script ", System.Text.RegularExpressions.RegexOptions.IgnoreCase).Distinct();

            foreach (var i in scriptLines)
            {
                var list = codeBox.Lines[i].Trim().Split(' ');

                if (list.Length >= 2)
                    if (list[0] == "script")
                        scriptList.Items.Add(list[1]);
            }

            scriptList.EndUpdate();
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

            if (File.Exists(qpath) && (Settings.Default.alwaysLoadSource || MainForm.AskUser(Strings.SourceFileDetected) == DialogResult.Yes))
            {
                LoadQSource(qpath, true);
                return;
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
            // external editor functionality
            // the idea is to save entire source text to a temporary file
            // lock the form and open the temp file in the external editor
            // then wait for the external editor to close
            // reload temporary file and unlock the form

            // downsides are:
            // cursor position is lost
            // must close external editor
            // wont work properly with multiple instances (only tracks the first one)

            try
            {
                if (Settings.Default.extrenalEditor.Trim() == "")
                {
                    WarnUser(Strings.MissingEditor);
                    return;
                }

                string pth = (path != null) ? Path.GetFileNameWithoutExtension(path) : "";
                string tmp = Path.Combine(Path.GetTempPath(), $"temp_{pth}_{Checksum.Calc(DateTime.Now.ToString())}.q");

                File.WriteAllText(tmp, codeBox.Text);

                var process = Process.Start(Settings.Default.extrenalEditor, tmp);
                process = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Settings.Default.extrenalEditor))[0];


                LockForm();

                // wait for the editor to exit
                while (!process.HasExited)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100); // stay awhile and listen...
                }

                // import text
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
            var fd = new FontDialog()
            {
                // existing font
                FontMustExist = true,
                // monospace font
                FixedPitchOnly = true,
                // load selected font from settings
                Font = Settings.Default.editorFont
            };

            if (fd.ShowDialog() == DialogResult.OK)
            {
                // save selection to settings
                Settings.Default.editorFont = fd.Font;
                // set editor font
                codeBox.Font = fd.Font;
                // set scripts list font
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

            var sfd = new SaveFileDialog()
            {
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

        private void wordWrapToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Settings.Default.wordWrap = wordWrapToolStripMenuItem.Checked;
            if (Settings.Default.wordWrap) WrapOn(); else WrapOff();
        }

        private void dumpHashInQBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SymbolCache.DumpSymbolCache();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.formLocation = this.Location;
            Settings.Default.formSize = this.Size;
            Settings.Default.windowState = (int)this.WindowState;
            Settings.Default.Save();
        }


        private void settingsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            sf = new SettingsForm();

            sf.Sync();
            sf.ShowDialog(this);
        }

        private void scriptList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                var x = codeBox.FindLines($"script {scriptList.SelectedItem}", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                codeBox.SetSelectedLine(x[0] + 1);
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
            {
                if (MessageBox.Show("No scripts folder selected, would you like to choose one?", "Scripts folder", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    var fbd = new FolderBrowserDialog();

                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        Settings.Default.scriptsPath = fbd.SelectedPath;
                    }
                }
            }
        }

        private void codeBox_MouseUp_1(object sender, MouseEventArgs e)
        {
            // on mouse up we want to calculate a checksum for whatever is currently selected
            // and put in the bottom right box. you can doubleclick that label to copy the checksum

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

        #region [Links]
        private void legacyThpsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://discord.gg/vTWucHS");
        }

        private void legacyThpsGithubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/DCxDemo/LegacyThps");
        }

        private void sortOfManualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/DCxDemo/LegacyThps/wiki/ThpsQScriptEd");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("QScripted.\r\nMany codes.\r\nSuch wow.\r\n\r\n2018, DCxDemo*.");
        }
        #endregion
    }
}