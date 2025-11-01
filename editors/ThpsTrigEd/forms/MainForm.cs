using LegacyThps.Thps2.Triggers;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using ThpsTrigEd.Properties;

namespace ThpsTrigEd
{
    public partial class MainForm : Form
    {
        public TriggerFile trg;
        TriggerFileDrawerForm visForm;

        public MainForm()
        {
            InitializeComponent();

            parsingModeBox.DataSource = Enum.GetValues(typeof(ParsingMode));
            parsingModeBox.SelectedIndex = 0;
            nodeTypeBox.DataSource = Enum.GetValues(typeof(NodeType));

            propertyGrid1.Visible = false;
            toggleEditButton.Visible = false;

            this.Text = Resources.AppName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            nodeInfo.Select(0, 0);
        }

        private void visButton_Click(object sender, EventArgs e)
        {
            if (visForm != null)
            {
                visForm.Close();
                visForm = null;
            }

            visForm = new TriggerFileDrawerForm(trg);
            visForm.Show();
        }

        private void RefreshData()
        {
            UpdateDataSource(nodesList, trg.Nodes);
            UpdateDataSource(railClusterList, trg.RailClusters);

            nodeInfo.Text = trg.ToString();
        }


        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ofd.Filter = Resources.TriggerFileOpenDialogFilter;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LoadNewFile(ofd.FileName);
            }
        }

        private void LoadNewFile(string filename)
        {
            if (!File.Exists(filename))
            {
                MessageBox.Show("File doesn't exist.");
                return;
            }

            if (visForm != null) visForm.Hide();
            this.Text = $"{Resources.AppName} - {filename}";
            trg = TriggerFile.FromFile(filename);

            RefreshData();
        }

        private void UpdateDataSource(ListBox listbox, object source)
        {
            if (source == null) return;

            if (source is IEnumerable)
            {
                listbox.BeginUpdate();

                listbox.DataSource = null;
                listbox.DataSource = source;
                listbox.SelectedIndex = -1;

                listbox.EndUpdate();
            }
        }

        private void nodesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // we can be null here
            if (nodesList.SelectedItem is null) return;

            var node = (nodesList.SelectedItem as TNode);

            // show children of current node
            UpdateDataSource(linksList, node.LinkedNodes);

            // show parents of current node
            UpdateDataSource(parentLinksList, trg.GetAllParents(node));

            // set node as editable object
            propertyGrid1.SelectedObject = node;

            // show node text dump
            nodeInfo.Text = node.ToStringLong();
        }

        private void linksList_SelectedIndexChanged(object sender, EventArgs e)
        {
            // we can be null here
            if (linksList.SelectedItem is null) return;

            var node = (linksList.SelectedItem as TNode);

            // set node as editable object
            propertyGrid1.SelectedObject = node;

            // show node text dump
            nodeInfo.Text = node.ToStringLong();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string search = Clipboard.GetText(System.Windows.Forms.TextDataFormat.Text);

            string res = "";

            bool first = true;

            if (trg != null)
            foreach (var node in trg.Nodes)
            {
                if (node.Checksum.ToString("X") == search) 
                {
                    res += node.ToString();

                    if (first)
                    {
                        nodesList.SelectedIndex = node.Number;
                        first = false;
                    }
                }
            }

            if (res != "")
            {
                nodeInfo.Text = res;
            }
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            // nodesList.SelectedIndex = trg.Nodes[nodesList.SelectedIndex].Links[linksList.SelectedIndex];
        }




        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // we can be null here
            if (parentLinksList.SelectedItem is null) return;

            var node = (parentLinksList.SelectedItem as TNode);

            // set node as editable object
            propertyGrid1.SelectedObject = node;

            // show node text dump
            nodeInfo.Text = node.ToStringLong();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        /*
        public void SaveProject(string filename)
        {
            var settings = new XmlWriterSettings() { Indent = true };

            using (var sw = XmlWriter.Create(filename, settings))
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                var x = new XmlSerializer(trg.GetType());
                x.Serialize(sw, trg, ns);
                sw.Flush();
                sw.Close();
            }
        }

        public void LoadProject(string filename)
        {
            var trig = new TriggerFile();

            using (var reader = new StreamReader(File.OpenRead(filename)))
            {
                try
                {
                    var x = new XmlSerializer(trg.GetType());
                    trg = (TriggerFile)x.Deserialize(reader);
                    reader.Close();

                    RefreshData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Load failed.\r\n{ex.Message}");
                }
            }

        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadProject("temp.th2");
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject("temp.th2");
        }
        */


        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            if (Path.GetExtension(FileList[0]).ToLower() != ".trg")
            {
                MessageBox.Show("Not a Neversoft trigger file.");
                return;
            }

            LoadNewFile(FileList[0]);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            nodeInfo.Text = (railClusterList.SelectedItem as RailCluster).ToStringLong();
        }

        private void decompileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sfd.Filter = Resources.TriggerFileSaveDialogFilter;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, trg.Decompile());
            }
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            if (trg == null) return;
            UpdateDataSource(nodesList, trg.GetAllOfType((NodeType)nodeTypeBox.SelectedValue));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (trg == null) return;

            RefreshData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            propertyGrid1.Visible = !propertyGrid1.Visible;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerFile.ParsingMode = (ParsingMode)parsingModeBox.SelectedValue;
        }

        private void commandListEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CommandCompiler();
            form.Show();
        }

        private void legacyTHPSDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/vTWucHS");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("201x-2015 ... 2025 DCxDemo*.\r\n\r\n" +
                "THPS2 TRG files reader and maybe editor in far future.\r\n" +
                "Who would need that anyways.");
        }
    }
}