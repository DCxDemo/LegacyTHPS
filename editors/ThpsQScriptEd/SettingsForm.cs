using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Settings = ThpsQScriptEd.Properties.Settings;

namespace ThpsQScriptEd
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        public void Sync()
        {
            useTabCb.Checked = Settings.Default.useTab;
            applyCosmeticsCb.Checked = Settings.Default.applyCosmetics;
            alwaysLoadSourceCb.Checked = Settings.Default.alwaysLoadSource;
            useSymFileCb.Checked = Settings.Default.useSymFile;
            useOldLineCb.Checked = Settings.Default.useShortLine;
            extEditor.Text = Settings.Default.extrenalEditor;
            scriptsPathBox.Text = Settings.Default.scriptsPath;
            enableBackupsCb.Checked = Settings.Default.enableBackups;
            preferSymbolicCb.Checked = Settings.Default.preferSymbolic;
            useCapsCb.Checked = Settings.Default.useCaps;
            useDegreesCb.Checked = Settings.Default.useDegrees;
            removeTrailNewlinesCb.Checked = Settings.Default.removeTrailNewlines;

            roundAnglesCb.Checked = Settings.Default.roundAngles;
            roundAnglesCb.Enabled = useDegreesCb.Checked;
            fixIncorrectChecksumsCb.Checked = Settings.Default.fixIncorrectChecksums;


            minQBlevelCb.SelectedIndex = 0;
            minQBlevelCb.Enabled = false;
        }

        private void okButton_Click(object sender, EventArgs e)
        {

        }

        private void useTabCb_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.useTab = useTabCb.Checked;
        }

        private void equalsWantsSpaceCb_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.applyCosmetics = applyCosmeticsCb.Checked;
        }

        private void alwaysLoadSourceCb_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.alwaysLoadSource = alwaysLoadSourceCb.Checked;
        }

        private void useOldLineCb_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.useShortLine = useOldLineCb.Checked;
        }

        private void useSymFileCb_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.useSymFile = useSymFileCb.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog() {
                Filter = "Executables (*.exe)|*.exe",
                InitialDirectory = extEditor.Text == "" ? "" : Path.GetDirectoryName(extEditor.Text)
            };

            if (ofd.ShowDialog() == DialogResult.OK)
                extEditor.Text = ofd.FileName;
        }

        private void extEditor_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.extrenalEditor = extEditor.Text;
        }

        private void enableBackupsCb_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.enableBackups = enableBackupsCb.Checked;
        }

        private void preferSymbolicCb_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.preferSymbolic = preferSymbolicCb.Checked;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }

        private void useCaps_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.useCaps = useCapsCb.Checked;
        }

        private void roundAnglesCb_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.roundAngles = roundAnglesCb.Checked;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.useDegrees = useDegreesCb.Checked;
            roundAnglesCb.Enabled = useDegreesCb.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.removeTrailNewlines = removeTrailNewlinesCb.Checked;
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            Settings.Default.fixIncorrectChecksums = fixIncorrectChecksumsCb.Checked;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                scriptsPathBox.Text = fbd.SelectedPath;
            }
        }

        private void scriptsPathBox_TextChanged(object sender, EventArgs e)
        {
            Settings.Default.scriptsPath = scriptsPathBox.Text;
        }
    }
}