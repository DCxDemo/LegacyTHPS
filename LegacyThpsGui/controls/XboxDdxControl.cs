using LegacyThps.Thps2;
using System;
using System.Windows.Forms;

namespace thps_ddx_gui
{
    public partial class XboxDdxControl : UserControl
    {
        public XboxDdxControl()
        {
            InitializeComponent();
        }

        private void openDdxButton_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "THPS2x DDX texture library (*.ddx)|*.ddx";

            if (ofd.ShowDialog() == DialogResult.OK)
                ddxPath.Text = ofd.FileName;
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
                texturesPath.Text = fbd.SelectedPath;
        }

        private void buildButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(texturesPath.Text) || String.IsNullOrEmpty(ddxPath.Text))
                MessageBox.Show("Please select both input and output values.");

            XboxDdx.Build(texturesPath.Text, ddxPath.Text);
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(texturesPath.Text) || String.IsNullOrEmpty(ddxPath.Text))
                MessageBox.Show("Please select both input and output values.");

            XboxDdx.Extract(ddxPath.Text, texturesPath.Text);
        }
    }
}