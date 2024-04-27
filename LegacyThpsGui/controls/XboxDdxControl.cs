using System;
using System.Windows.Forms;
using LegacyThps.Thps2;

namespace thps_ddx_gui
{
    public partial class XboxDdxControl : UserControl
    {
        public XboxDdxControl()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "THPS2x DDX texture library (*.ddx)|*.ddx";

            if (ofd.ShowDialog() == DialogResult.OK)
                ddxPath.Text = ofd.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
                texturesPath.Text = fbd.SelectedPath;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (texturesPath.Text != "" && ddxPath.Text != "")
                XboxDdx.Build(texturesPath.Text, ddxPath.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (texturesPath.Text != "" && ddxPath.Text != "")
                XboxDdx.Extract(ddxPath.Text, texturesPath.Text);
        }
    }
}