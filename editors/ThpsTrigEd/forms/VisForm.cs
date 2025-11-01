using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using LegacyThps.Thps2.Triggers;

namespace ThpsTrigEd
{
    public partial class TriggerFileDrawerForm : Form
    {
        TriggerFile rg;
        TriggerFileDrawer drawer;
        Camera cam;     //so called "camera"

        public TriggerFileDrawerForm(TriggerFile t)
        {
            InitializeComponent();

            rg = t;
            cam = new Camera(0, 0, 0.04f);

            drawer = new TriggerFileDrawer(new TriggerFileDrawerContext()
            {
                Camera = cam,
                TriggerFile = rg
            });

            this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //load trg
            // rg = new TRG(@"SkMar_T.trg");
            cam.sx = ClientRectangle.Width;
            cam.sy = ClientRectangle.Height;
        }
       

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            if (rg == null) return;

            drawer.Context.Graphics = e.Graphics;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

            drawer.Draw();
            DrawHelper(e.Graphics);
        }


        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        private bool isDown(Keys key) { return GetAsyncKeyState((int)key) != 0; }

        private void HandleInput()
        {
            if (isDown(Keys.OemMinus)) cam.ZoomOut(test);
            if (isDown(Keys.Oemplus)) cam.ZoomIn(test);

            if (isDown(Keys.Add)) cam.ZoomIn(test);
            if (isDown(Keys.Subtract)) cam.ZoomOut(test);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            HandleInput();
            Invalidate();
            cam.sx = Width;
        }


        Point test = new Point();


        //temporary points
        Point mp = new Point();
        Point mp2 = new Point();

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            //store mouse and camera position on mousewheel down
            if (e.Button == MouseButtons.Middle)
            {
                mp.X = e.X;
                mp.Y = e.Y;

                mp2.X = cam.X;
                mp2.Y = cam.Y;
            }
        }

        int del = 0;

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            //move camera aka everything on the form
            if (e.Button == MouseButtons.Middle)
                cam.ResetCamera(mp2.X-mp.X + e.X, mp2.Y-mp.Y + e.Y, cam.Scale);

            test = new Point(e.X, e.Y);

            this.Text = ((e.X - cam.X) / cam.Scale / 2.833).ToString("0.00000") + "; " + ((e.Y - cam.Y) / cam.Scale / 2.833).ToString("0.00000");
        }

        private void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0) cam.ZoomIn(test); 
            if (e.Delta < 0) cam.ZoomOut(test); 
            }

        private void Form2_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.R) cam.ResetCamera(500, 500, 0.04f);  

            if (e.KeyCode == Keys.D1) cam.RailCaps = !cam.RailCaps;
            if (e.KeyCode == Keys.D2) cam.Nums = !cam.Nums;
            if (e.KeyCode == Keys.D3) cam.Links = !cam.Links;
            if (e.KeyCode == Keys.D4) cam.PowerUps = !cam.PowerUps;
            if (e.KeyCode == Keys.D5) cam.CamPts = !cam.CamPts;
            if (e.KeyCode == Keys.D6) cam.SplitScreenRails = !cam.SplitScreenRails;
            if (e.KeyCode == Keys.D7) cam.Points = !cam.Points;
            if (e.KeyCode == Keys.D8) cam.Baddies = !cam.Baddies;
        }

        private void Form2_Resize(object sender, EventArgs e)
        {
            cam.sx = ClientRectangle.Width;
            cam.sy = ClientRectangle.Height;
        }



        List<string> lines = new List<string>()
        {
            "Move camera: hold mousewheel",
            "Reset camera: R",
            "Zoom In/Out: mouse wheel or +/-",
            "",
            "Toggle rail caps: 1",
            "Toggle rail indices: 2",
            "Toggle rail links: 3",
            "Toggle powerups: 4",
            "Toggle camera points: 5",
            "Toggle 2P rails: 6",
            "Toggle points: 7",
            "Toggle baddies: 8",
            "",
            "Gray rails - concrete",
            "Red rails - wood",
            "Blue rails - metal",
            "",
            "Green rail caps - rail cluster start",
            "Blue rail caps - rail cluster end"
        };


        //draw helper text
        public void DrawHelper(Graphics e)
        {
            int y = 10;

            foreach (var line in lines)
            {
                e.DrawString(line, SystemFonts.DefaultFont, Brushes.Black, new Point(10, y));
                y += 10;
            }
        }
    }
}
