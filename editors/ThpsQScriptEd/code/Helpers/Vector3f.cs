using System;
using Settings = QScripted.Properties.Settings;

namespace LegacyThps.QScript.Helpers
{
    public class Vector3f
    {
        //static declarations
        public static Vector3f Zero = new Vector3f(0, 0, 0);
        public static double radian = 180.0 / Math.PI;


        public float X;
        public float Y;
        public float Z;

        bool vector3 = false;
        public bool isAngle = false;

        public Vector3f(float xx, float yy)
        {
            X = xx;
            Y = yy;
            Z = 0;
        }

        public static bool operator !=(Vector3f v1, Vector3f v2)
        {
            if (v1.X != v2.X) return true;
            if (v1.Y != v2.Y) return true;
            if (v1.Z != v2.Z) return true;
            return false;
        }

        public static bool operator ==(Vector3f v1, Vector3f v2)
        {
            if (v1.X != v2.X) return false;
            return true;
        }

        public Vector3f(float xx, float yy, float zz)
        {
            X = xx;
            Y = yy;
            Z = zz;

            vector3 = true;
        }

        public override string ToString()
        {
            if (!isAngle || !Settings.Default.useDegrees)
            {
                return
                    "(" + X.ToString("0.#####") +
                    ", " + Y.ToString("0.#####") +
                    ((vector3) ? (", " + Z.ToString("0.#####")) : "") + ")";
            }
            else
            {
                double fx = X * radian;
                double fy = Y * radian;
                double fz = Z * radian;

                //without it most 180 will be like 180.00001
                if (Settings.Default.roundAngles)
                {
                    fx = Math.Round(fx, 2);
                    fy = Math.Round(fy, 2);
                    fz = Math.Round(fz, 2);
                }

                return
                    "(" + fx.ToString("0.#####") +
                    "°, " + fy.ToString("0.#####") +
                    ((vector3) ? ("°, " + fz.ToString("0.#####")) : "") + "°)";
            }
        }
    }
}
