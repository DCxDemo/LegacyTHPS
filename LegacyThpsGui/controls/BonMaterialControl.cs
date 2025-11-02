using LegacyThps.Thps2;
using System;
using System.Windows.Forms;

namespace bon_tool
{
    public partial class BonMaterialControl : UserControl
    {
        public BonMaterial _material;
        public BonMaterial Material
        {
            get
            {
                return _material;
            }
            set
            {
                _material = value;

                if (Material != null)
                {
                    propertyGrid1.SelectedObject = Material;
                    propertyGrid2.SelectedObject = Material.Texture;
                    pictureBox1.Image = _material.Texture.GetBitmap();
                }
            }
        }

        public BonMaterialControl()
        {
            InitializeComponent();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "DirectX DDS surface (*.dds)|*.dds";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _material.Replace(ofd.FileName);
                pictureBox1.Image = _material.Texture.GetBitmap();
            }
        }
    }
}