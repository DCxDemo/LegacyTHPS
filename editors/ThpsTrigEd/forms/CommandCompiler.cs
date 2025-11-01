using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LegacyThps.Thps2.Triggers;

namespace ThpsTrigEd
{
    public partial class CommandCompiler : Form
    {
        public CommandCompiler()
        {
            InitializeComponent();

            textBox1.Select(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // test restarts
            try
            {
                var cmd = new CommandList();

                cmd.Parse(textBox1.Text);

                var ms = new MemoryStream();

                var bw = new BinaryWriter(ms);
                if (checkBox1.Checked)
                    bw.BaseStream.Position = 2;

                cmd.Write(bw);
                bw.BaseStream.SetLength(bw.BaseStream.Position);

                textBox2.Text = "";

                using (var br = new BinaryReader(ms))
                {
                    br.BaseStream.Position = 0;
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        textBox2.Text += br.ReadByte().ToString("X2") + " ";
                    }
                }

                textBox2.Text += "\r\n\r\nFormatted source:\r\n";

                textBox2.Text += cmd.ToString();

            }
            catch (Exception ex)
            {
                textBox2.Text = $"Error!\r\n{ex.Message}\r\n\r\n{ex.ToString()}";
            }
        }
    }
}
