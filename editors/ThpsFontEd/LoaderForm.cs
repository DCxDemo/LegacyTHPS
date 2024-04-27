using System.Windows.Forms;

namespace ThpsFontEd
{
    public partial class LoaderForm : Form
    {
        public LoaderForm()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        public void ResetBar()
        {
            this.progressBar1.Maximum = 100;
            this.progressBar1.Value = 0;
        }
    }
}
