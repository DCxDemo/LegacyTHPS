using LegacyThps.Thps2;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace bon_tool
{
    public partial class BonTextureReplacerControl : UserControl
    {
        string lastLoadedPath;

        BonModel model;

        public BonTextureReplacerControl()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "THPS2x BON model (*.bon)|*.bon";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                lastLoadedPath = ofd.FileName;

                Text = $"THPS2x BON tool - {Path.GetFileName(lastLoadedPath)}";

                model = BonModel.FromFile(ofd.FileName);

                materialListBox.Items.Clear();

                foreach (var mat in model.Materials)
                    materialListBox.Items.Add(mat.Name);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastLoadedPath != null && lastLoadedPath != "")
            {
                model.Save(lastLoadedPath);
            }
            else
            {
                SaveAs();
            }
        }

        private void SaveAs()
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "THPS2x BON model (*.bon)|*.bon";
            sfd.FileName = Path.GetFileName(lastLoadedPath);

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                model.Save(sfd.FileName);
            }
        }

        private void extractTexturesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (model == null) return;

            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                model.ExtractTextures(fbd.SelectedPath);
            }
        }

        private void materialListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialListBox.SelectedIndex != -1)
                bonMaterialControl1.Material = model.Materials[materialListBox.SelectedIndex];
        }

        private void replaceFromPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (model == null) return;

            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
                model.ReplaceTextures(fbd.SelectedPath);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }

        private void legacyTHPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://discord.gg/vTWucHS");
        }
    }
}