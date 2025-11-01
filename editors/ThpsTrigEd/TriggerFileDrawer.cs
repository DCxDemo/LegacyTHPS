using System;
using System.Drawing;
using System.Windows.Forms;
using LegacyThps.Thps2.Triggers;

namespace ThpsTrigEd
{
    public class TriggerFileDrawerContext
    {
        public TriggerFile TriggerFile;
        public Graphics Graphics;
        public Camera Camera;
    }

    public class TriggerFileDrawer
    {
        public TriggerFileDrawerContext Context;

        public TriggerFileDrawer (TriggerFileDrawerContext context)
        {
            Context = context;
        }

        //draws nodes on screen
        public void Draw()
        {
            if (Context == null) return;

            TNode nodenow = new TNode();

            if (Context.Camera.Scale > 0.1f) p.Width = 2;

            try
            {
                // draw all but rails
                foreach (var node in Context.TriggerFile.Nodes)
                {
                    nodenow = node;
                    //if (!node.IsRail)
                        DrawNode(node);
                }

                /*
                // draw rails using clusters
                foreach (var cluster in Context.TriggerFile.RailClusters)
                {
                    var start = cluster.Entries[0];

                    foreach (var rail in cluster.Entries)
                        DrawNode(rail);
                }
                */
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\n" + nodenow.Number);
            }
        }

        Pen p = new Pen(Brushes.Black);


        public void DrawNode(TNode node)
        {
            var e = Context.Graphics;
            var c = Context.Camera;
            var trg = Context.TriggerFile;


            if (node.IsRail)
            {
                if (c.Links)
                {
                    // poor rail sound coloring
                    // mask bits 0-3 to get sound
                    switch (node.Flags & 0xF)
                    {
                        case 0: p.Color = Color.Gray; break;
                        case 1: p.Color = Color.Blue; break;
                        case 2: p.Color = Color.Red; break;

                        default: p.Color = Color.Magenta; break;
                    }

                    if ((node.Flags >> 4) == 0 || c.SplitScreenRails)
                    {
                        foreach (var link in node.LinkedNodes)
                        {
                            if (link.IsRail)
                                e.DrawLine(p,
                                    new Point(c.Zoomed(node.Position.X) + c.X, c.Zoomed(-node.Position.Z) + c.Y),
                                    new Point(c.Zoomed(link.Position.X) + c.X, c.Zoomed(-link.Position.Z) + c.Y));
                        }
                    }
                }

                // draw caps on top of lines
                if (c.RailCaps)
                {
                    if ((node.Flags >> 4) == 0 || c.SplitScreenRails)
                    {
                        if (c.Nums) e.DrawString(node.Number.ToString(), SystemFonts.DialogFont, Brushes.Black, new Point(c.Zoomed(node.Position.X) + c.X, c.Zoomed(-node.Position.Z) + c.Y));

                        var color = node.LinkedNodes.Count > 0 ? Brushes.Red : Brushes.Blue;

                        foreach (var cluster in trg.RailClusters)
                            if (cluster.Entries[0] == node)
                                color = Brushes.Green;

                        e.FillEllipse(color, new Rectangle(c.Zoomed(node.Position.X) - 3 + c.X, c.Zoomed(-node.Position.Z) - 3 + c.Y, 6, 6));
                    }
                }
            }

            // points are apparently used for teleports and killers
            if (node.IsPoint && c.Points) 
                e.FillEllipse(Brushes.Green, new Rectangle(c.Zoomed(node.Position.X) - 10 + c.X, c.Zoomed(-node.Position.Z) - 10 + c.Y, 20, 20));

            if (node.IsPowerUp && c.PowerUps)
            {
                e.FillEllipse(Brushes.Brown, new Rectangle(c.Zoomed(node.Position.X) - 10 + c.X, c.Zoomed(-node.Position.Z) - 10 + c.Y, 20, 20));
                e.DrawString(node.Number + " - " + GetName.PowerUp(node.PowerUpType), SystemFonts.DialogFont, Brushes.Black, new Point(c.Zoomed(node.Position.X) + c.X, c.Zoomed(-node.Position.Z) + c.Y));
            }

            if (node.IsCamPt && c.CamPts)
            {
                e.DrawEllipse(Pens.Black, new Rectangle(c.Zoomed(node.Position.X) - 10 + c.X, c.Zoomed(-node.Position.Z) - 10 + c.Y, 20, 20));
            }

            if (node.IsBaddy && c.Baddies)
            {
                e.FillEllipse(Brushes.Red, new Rectangle(c.Zoomed(node.Position.X) - 10 + c.X, c.Zoomed(-node.Position.Z) - 10 + c.Y, 20, 20));
                e.DrawString(node.Number + " - " + GetName.BaddyType(node.BaddyType), SystemFonts.DialogFont, Brushes.Black, new Point(c.Zoomed(node.Position.X) + c.X, c.Zoomed(-node.Position.Z) + c.Y));
            }

            // zero reference lines

            e.DrawLine(Pens.Orange,
                new Point(c.Zoomed(0) + c.X, c.Zoomed(-100000) + c.Y),
                new Point(c.Zoomed(0) + c.X, c.Zoomed(100000) + c.Y)
            );

            e.DrawLine(Pens.Orange,
                new Point(c.Zoomed(-100000) + c.X, c.Zoomed(0) + c.Y),
                new Point(c.Zoomed(100000) + c.X, c.Zoomed(0) + c.Y)
            );

            /*
            if (t.IsBaddy())
            {
                if (t.Bouncy == 1) {
                    e.FillEllipse(Brushes.DarkGreen, new Rectangle(c.Zoomed(t.Pos.X) - 10 + c.X, c.Zoomed(-t.Pos.Z) - 10 + c.Y, 20, 20));
                }
            }*/
        }
    }
}
