using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace thps_ddx_gui
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void discordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://discord.gg/vTWucHS");
        }

        private void githubToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/DCxDemo/LegacyTHPS");
        }
    }
}