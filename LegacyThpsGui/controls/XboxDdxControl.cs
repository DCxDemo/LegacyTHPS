using LegacyThps.Levels.Thps2x;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        private void button5_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/vTWucHS");
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
