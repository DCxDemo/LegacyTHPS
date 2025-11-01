using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Numerics;

namespace ThpsTrigEd
{
    public class Camera
    {
        public int X;
        public int Y;
        public double Scale;
        double scaleSpeed = 0.9;

        public bool Links = true;
        public bool RailCaps = true;
        public bool Nums = true;
        public bool PowerUps = true;
        public bool CamPts = true;
        public bool SplitScreenRails = true;
        public bool Points = true;
        public bool Baddies = true;

        public int sx;
        public int sy;

        double oldsc;

        public Camera(int x1, int y1, double s)
        {
            X = x1;
            Y = y1;
            Scale = s;
        }

        public void ResetCamera(int x1, int y1, double s)
        {
            X = x1;
            Y = y1;
            Scale = s;
        }

        public void MoveRel(int x1, int y1)
        {
            X += x1;
            Y += y1;
        }

        public void ZoomIn(Point test)
        {
            oldsc = Scale;
            Scale /= scaleSpeed;
            CorrectScale(test);    
        }

        public void ZoomOut(Point test)
        {
            oldsc = Scale;
            Scale *= scaleSpeed;
            CorrectScale(test);
        }

        public void CorrectScale(Point test)
        {

            if (test.X > X)
            {
                int t1 = Math.Abs(test.X - X);
                double s8 = t1 * (Scale * 1000) / (oldsc * 1000);
                X = Convert.ToInt32(test.X - s8);
            }
            else
            {
                int t1 = Math.Abs(X - test.X);
                double s8 = t1 * (Scale * 1000) / (oldsc * 1000);
                X = Convert.ToInt32(test.X + s8);
            }

            if (test.Y > Y)
            {
                int t1 = Math.Abs(test.Y - Y);
                double s8 = t1 * (Scale * 1000) / (oldsc * 1000);
                Y = Convert.ToInt32(test.Y - s8);
            }
            else
            {
                int t1 = Math.Abs(Y - test.Y);
                double s8 = t1 * (Scale * 1000) / (oldsc * 1000);
                Y = Convert.ToInt32(test.Y + s8);
            }

        }

        public int Zoomed(float x) => Convert.ToInt32(x * Scale);
    }
}