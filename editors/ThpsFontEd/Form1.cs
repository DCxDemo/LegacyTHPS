using LegacyThps.Fonts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace ThpsFontEd
{
    public partial class Form1 : Form
    {
        FNT fnt;

        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "THPS Font file (FNT1/2/3)|*.fnt;*fnt.dat;*.fnt.xbx;*.fnt.wpc";

            if (ofd.ShowDialog() == DialogResult.OK)
                LoadFont(ofd.FileName);
        }

        public void LoadFont(string filename)
        {
            try
            {
                fnt = FNT.FromFile(filename);
                versionbox.SelectedIndex = (int)FNT.version;
                RefreshForm();
                SetTitle(filename);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        public void RefreshForm()
        {
            if (fnt != null)
            {
                fontBox.SelectedObject = fnt;

                previewBox.Image = new Bitmap(previewBox.Width, previewBox.Height);
                graphics = Graphics.FromImage(previewBox.Image);
                graphics.DrawImage(fnt.Atlas, 0, 0);

                FillListView(infoBox);
            }
        }


        SolidBrush sb = new SolidBrush(Color.FromArgb(220, 255, 0, 0));
        Graphics graphics;

        public void SelectChar(int c)
        {
            Glyph g = fnt.Glyphs[c];

            graphics = Graphics.FromImage(previewBox.Image);

            graphics.Clear(Color.Transparent);
            graphics.DrawImage(fnt.Atlas, 0, 0);
            graphics.FillRectangle(sb, g.Region);

            foreach (Glyph let in fnt.Glyphs)
                graphics.DrawRectangle(Pens.Black, let.Region);

            graphics.DrawRectangle(Pens.Red, new Rectangle(0, 0, fnt.Atlas.Width, fnt.Atlas.Height));

            previewBox.Image = previewBox.Image;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // SelectChar(listBox1.SelectedIndex);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DrawString();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            DrawString();
        }
        private void DrawString()
        {
            int drawx = 0;

            Bitmap x = new Bitmap(800, 800);
            Graphics xx = Graphics.FromImage(x);

            // xx.FillRectangle(Brushes.Black, new Rectangle(0,0, 800,100));

            string sample = textBox1.Text;

            foreach (char c in sample)
            {
                if (c != ' ')
                {
                    foreach (var glyph in fnt.Glyphs)
                    {
                        if (glyph.Value == (int)c)
                        {
                            if (glyph.bmp != null)
                            { xx.DrawImage(glyph.bmp, drawx, fnt.FontVShift - glyph.VShift, glyph.Region.Width, glyph.Region.Height); }
                            drawx += glyph.Region.Width;
                            break;
                        }
                    }
                }
                else { if (FNT.version != 0) { drawx += fnt.Glyphs[0].Region.Width / 2; } else { drawx += 20; } }

                drawx += trackBar1.Value;

            }

            previewBox.Image = x;
        }


        private string ParseCharset(string s)
        {
            string charset = s;

            try
            {
                string[] lolz = File.ReadAllLines("buttons.txt");

                foreach (string lulz in lolz)
                {
                    string[] z = lulz.Split('=');

                    string code = z[0];
                    int value = Int32.Parse(z[1]);

                    System.Windows.Forms.MessageBox.Show(value.ToString());

                    s = s.Replace("\\" + code, "" + (char)value);
                }
            }
            catch
            {
                MessageBox.Show("Parse charset failed.");
            }

            return s;
        }

        private void importerTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "PNG image (*.png)|*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var bi = new BitmapImporter(ofd.FileName);

                if (textBox1.Text != "") bi.SetCharset(ParseCharset(textBox1.Text));

                bi.Info();

                fnt = bi.fnt;
                fnt.GenerateBitmap(false);
                previewBox.Image = fnt.Atlas;

                fontBox.SelectedObject = fnt;

                if (FNT.version != EVersion.FNT3)
                {
                    FillListView(infoBox);
                }

                var bb = new FastBMP(fnt.Atlas);

                if (bb.GetNumCols() <= 256)
                {
                    fnt.bmpraw = bb.GetRaw();
                    //bb.THUGifyPalette();
                    fnt.palette = bb.GetRawPalette();

                    fnt.Atlas = fnt.RawToBmp(fnt.Width, fnt.Height);

                    fnt.FontHeight = fnt.Glyphs[0].Region.Height;
                    //fnt.fvshift = 300;

                    foreach (Glyph l in fnt.Glyphs)
                    {
                        //l.vshift = fnt.fvshift;
                    }
                }
                else
                {
                    MessageBox.Show(
                        "This image contains more than 256 colors.\r\n" +
                        "FNT files only support 256 colors. Use Color Quantizer tool."
                        );
                }
            }
        }

        private void saveFNT1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt is null) return;

            var sfd = new SaveFileDialog();

            switch (FNT.version)
            {
                case EVersion.FNT1: sfd.Filter = "THPS FNT1 font file (*.fnt)|*.fnt"; break;
                case EVersion.FNT2: sfd.Filter = "THUG2 FNT2 font file (*.fnt.xbx)|*.fnt.xbx"; break;
                default: MessageBox.Show("Can't save this version yet."); return;
            }

            if (sfd.ShowDialog() == DialogResult.OK)
                fnt.Save(sfd.FileName);
        }

        private void exportPNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt is null) return;

            var sfd = new SaveFileDialog();
            sfd.Filter = "PNG image (*.png)|*.png";

            if (sfd.ShowDialog() == DialogResult.OK)
                fnt.ExportAtlas(sfd.FileName);
        }

        private void exportCharsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt is null) return;

            var sfd = new SaveFileDialog();
            sfd.Filter = "PNG image (*.png)|*.png";

            if (sfd.ShowDialog() == DialogResult.OK)
                fnt.GetSourceCharset(2).Save(sfd.FileName);
        }



        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            // textBox1.Text += (char)fnt.Glyphs[listBox1.SelectedIndex].Character;
        }


        [DllImport("Unswizzler.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Unswizzle(IntPtr src, uint depth, uint width, uint height, IntPtr dest);


        private void unswizzleVadruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            byte[] buf = new byte[fnt.bmpraw.Length];

            unsafe
            {
                fixed (byte* d = buf)
                {
                    fixed (byte* s = fnt.bmpraw)
                    {
                        IntPtr src = (IntPtr)s;
                        IntPtr dst = (IntPtr)d;
                        // do you stuff here

                        Unswizzle(src, 1, (uint)fnt.Width, (uint)fnt.Height, dst);
                    }
                }
            }

            fnt.bmpraw = buf;
            fnt.Atlas = fnt.RawToBmp(fnt.Width, fnt.Height);

            previewBox.Image = fnt.Atlas;

            fnt.Recut();
        }


        /*
        public byte[] Unswizzle_Vadru(byte[] buf, uint depth, uint width, uint height)
        {
            byte[] swizzled = buf;

            for (int y = 0; y < height; y++)
            {
                int sy = 0;

                if (y < width)
                {
                    for (int bit = 0; bit < 16; bit++)
                        sy |= ((y >> bit) & 1) << (2 * bit);
                    sy <<= 1; // y counts twice
                }
                else
                {
                    int y_mask = (int)(y % width);
                    for (int bit = 0; bit < 16; bit++)
                        sy |= ((y_mask >> bit) & 1) << (2 * bit);
                    sy <<= 1; // y counts twice
                    sy += (int)((y / width) * width * width);
                }

                //BYTE* d = (BYTE*)dest + y * width * depth;
                int d = (int)(y * width * depth);

                for (int x = 0; x < width; x++)
                {
                    int sx = 0;

                    if (x < height * 2)
                    {
                        for (int bit = 0; bit < 16; bit++)
                            sx |= ((x >> bit) & 1) << (2 * bit);
                    }
                    else
                    {
                        int x_mask = (int)(x % (2 * height));
                        for (int bit = 0; bit < 16; bit++)
                            sx |= ((x_mask >> bit) & 1) << (2 * bit);

                        sx += (int)((x / (2 * height)) * 2 * height * height);
                    }

                    //BYTE* s = (BYTE*)src + (sx + sy) * depth;
                    int s = (int)((sx + sy) * depth);

                    for (int i = 0; i < depth; ++i)
                        //*d++ = *s++;
                        swizzled[d] = buf[s];
                }
            }

            return swizzled;
        }
        */



        private void getCharsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = fnt.GetCharset(false);
        }

        private void getCharsetSpacesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Text = fnt.GetCharset(true);
        }

        private void setButtonsCharsetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < fnt.Glyphs.Count; i++)
            {
                fnt.Glyphs[i].Value = (ushort)(-i - 1);
            }

            //fnt.FillListbox(listBox1);

        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] filePaths = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            LoadFont(filePaths[0]);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (infoBox.SelectedItems.Count > 0)
                SelectChar(infoBox.SelectedIndices[0]);

            if (fnt != null)
            {
                if (infoBox.SelectedIndices.Count > 0)
                    fontBox.SelectedObject = fnt.Glyphs[infoBox.SelectedIndices[0]];
            }
        }

        private void sortByCharToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt != null)
            {
                fnt.Glyphs = fnt.Glyphs.OrderBy(p => p.Value).ToList();
                RefreshForm();
            }
        }

        private void versionbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            FNT.version = (EVersion)versionbox.SelectedIndex;
        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            if (fnt is null) return;

            previewBox.Image = new Bitmap(previewBox.Width, previewBox.Height);
            graphics = Graphics.FromImage(previewBox.Image);
            graphics.DrawImage(fnt.Atlas, 0, 0);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (fnt != null)
                fontBox.SelectedObject = fnt;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "ThpsFontEd v1.0\r\n" +
                "THPS fonts converter.\r\n\r\n" +
                "DCxDemo*.");
        }

        private void addLowercaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<Glyph> x = new List<Glyph>();

            foreach (Glyph g in fnt.Glyphs)
            {
                if (g.Value >= (ushort)'A' && g.Value <= (ushort)'Z')
                {
                    Glyph n = new Glyph();
                    n.Region = g.Region;
                    n.Value = g.Value;
                    n.VShift = g.VShift;
                    n.Value += (ushort)32;

                    x.Add(n);
                }
            }

            fnt.Glyphs.AddRange(x);
            fnt.Recut();
            RefreshForm();
        }

        private void rGBBGRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt != null)
                fnt.GenerateBitmap(true);
        }

        private void keepButtonsOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt is null) return;

            foreach (var glyph in fnt.Glyphs)
            {
                if (glyph.Value <= 0xFFA1 || glyph.Value >= 0xFFAE)
                    fnt.Glyphs.Remove(glyph);
            }

            RefreshForm();
        }

        private void add1pxAlphaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt is null) return;

            foreach (var glyph in fnt.Glyphs)
            {
                var buf = new Bitmap(glyph.bmp.Width + 2, glyph.bmp.Height + 2);
                var graphics = Graphics.FromImage(buf);
                graphics.DrawImage(glyph.bmp, new Point(1, 1));
                glyph.bmp = buf;

                glyph.Region = new Rectangle(0, 0, glyph.bmp.Width, glyph.bmp.Height);
                //g.VShift -= 0;
            }

            //fnt.FontHeight += 2;
            fnt.FontVShift -= 1;

            RefreshForm();
        }

        public void FillListView(ListView x)
        {
            x.Items.Clear();
            foreach (Glyph g in fnt.Glyphs)
            {
                ListViewItem li = new ListViewItem(new[] {
                    (char)g.Value + "",
                    g.Value.ToString("X4") + "",
                    g.Region.X + "," + g.Region.Y,
                    g.Region.Width + "x" + g.Region.Height
                });
                x.Items.Add(li);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        string discordLink = "https://discord.gg/vTWucHS";
        string githubLink = "https://github.com/DCxDemo/LegacyThps";
        string fonthuntLink = "https://github.com/DCxDemo/ThpsFonts";

        private void legacyThpsDiscordToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start(discordLink);

        private void tHPSFontsHuntToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start(fonthuntLink);

        private void githubToolStripMenuItem_Click(object sender, EventArgs e) => Process.Start(githubLink);

        private void SetTitle(string title = "")
        {
            if (title == "") Text = "ThpsFontEd";
            else Text = $"ThpsFontEd - {title}";
        }

        private void exportBitmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt is null) return;

            var ofd = new OpenFileDialog();
            
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var bitmap = (Bitmap)Bitmap.FromFile(ofd.FileName);

                fnt.Atlas = bitmap;
                fnt.Recut();
                fnt.GenerateBitmap(false);
            }
        }

        private void rearrangeGlyphsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt is null) return;

            fnt.RearrangeGlyphs(256);
            fnt.GenerateBitmap(false);

            RefreshForm();
        }

        private void bleed1pxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fnt is null) return;

            foreach (var glyph in fnt.Glyphs)
            {
                glyph.Region = new Rectangle(
                    glyph.Region.X,
                    glyph.Region.Y,
                    glyph.Region.Width + 1,
                    glyph.Region.Height + 1
                    );
            }

            fnt.Recut();
            RefreshForm();
        }

        private void scaleX2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fnt.Width *= 2;
            fnt.Height *= 2;

            foreach (var glyph in fnt.Glyphs)
            {
                glyph.Scale(2);
            }

            fnt.RearrangeGlyphs(fnt.Width);
            fnt.GenerateBitmap(false);
        }

        private void newFromTrueTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fontDialog = new FontDialog();

            if (fontDialog.ShowDialog() == DialogResult.OK)
            {
                fnt = BitmapTrueType.Create("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", fontDialog.Font);
                RefreshForm();
            }
        }

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (MemoryStream pngMemStream = new MemoryStream())
            {
                fnt.Atlas.Save(pngMemStream, System.Drawing.Imaging.ImageFormat.Png);
                Clipboard.SetImage(fnt.Atlas);
                Clipboard.SetData("PNG", pngMemStream);
            }
        }

        private void pasteFromClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Clipboard.ContainsData("PNG"))
            {
                MessageBox.Show("No image in the clipboard!");
                return;
            }

            fnt.Atlas = (Bitmap)Bitmap.FromStream((MemoryStream)Clipboard.GetData("PNG"));
            fnt.Recut();
            fnt.GenerateBitmap(false);
            RefreshForm();
        }
    }
}