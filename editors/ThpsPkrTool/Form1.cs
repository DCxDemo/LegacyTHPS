using System;
using System.IO;
using System.Windows.Forms;
using LegacyThps.Containers;
using ThpsPkrTool.Properties;

namespace ThpsPkrTool
{
    public partial class Form1 : Form
    {
        PKR pkr;

        public Form1()
        {
            InitializeComponent();

            pkr = new PKR();

            UpdateList();
        }

        int currentindex = 0;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fileListBox.Items.Clear();

            var pf = folderListBox.SelectedItem as PKRFolder;

            int index = 0;

            switch (pkr.Version)
            {
                case PkrVersion.PKR2: index = (int)((pf.offset - (12 + pkr.folders.Count * 40)) / (32 + 16));  break;
                case PkrVersion.PKR3: index = Convert.ToInt32(pf.offset); break;//(int)((pf.offset - (pkr.offsettofolders + 12 + pkr.folderCount * 40)) / (32 + 20)); break;
            }

            currentindex = index;

            for (int i = index; i < index + pf.count; i++)
                fileListBox.Items.Add(pkr.files[i]);
        }

        private void listBox2_DoubleClick(object sender, EventArgs e)
        {
            if (pkr == null) return;
            if (fileListBox.SelectedIndex == -1) return;

            var file = fileListBox.SelectedItem as PKRFile;

            if (file != null)
            {
                var sfd = new SaveFileDialog();
                sfd.FileName = file.name;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    file.Save(sfd.FileName);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = Resources.FileDialogPkrFilter;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pkr = PKR.FromFile(ofd.FileName);

                UpdateList();

                Text = $"PKRTool - {ofd.FileName}";
            }

        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
                pkr.Export(fbd.SelectedPath);
        }

        private void UpdateList()
        {
            folderListBox.Items.Clear();
            fileListBox.Items.Clear();

            foreach (var pf in pkr.folders)
                folderListBox.Items.Add(pf);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewPKR(PkrVersion.PKR2);
        }

        private void importFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pkr == null) return;

            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                pkr.AddSubfolders(fbd.SelectedPath);
                UpdateList();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pkr == null) return;

            var sfd = new SaveFileDialog();
            sfd.Filter = Resources.FileDialogPkrFilter;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                pkr.Save(sfd.FileName);
                Text = $"{Resources.AppName} - {sfd.FileName}";
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (pkr == null)
                CreateNewPKR();

            var files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach ( var f in files )
                if (Directory.Exists(f))
                {
                    pkr.AddFolder(f);
                    UpdateList();
                }
                else
                {
                    if (Path.GetExtension(f).ToLower() == ".pkr")
                    {
                        pkr = new PKR(f);
                        UpdateList();
                        this.Text = $"{Resources.AppName} - " + f;
                    }
                }
        }

        public void CreateNewPKR(PkrVersion version = PkrVersion.PKR2)
        {
            if (pkr != null)
                if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

            pkr = new PKR() { Version = version };
            UpdateList();

            Text = $"{Resources.AppName} - <new PKR file>";
        }

        private void fileListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void newPKR2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewPKR(PkrVersion.PKR3);
        }
    }
}