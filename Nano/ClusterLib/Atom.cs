using System;
using System.Collections.Generic;

namespace ClusterLib
{
    public class Atom
    {
        #region Parametrs

        public PointCL Position;
        public double Radius;

        public PointCL MagneticVector;
            
        public PointCL NormalVector;

        public PointCL HDman;

        public Material material;

        public double Hrx, Hry, Hrz;

        #endregion

        #region Constructors
        public Atom()
        { 
        }

        public Atom(Material _material)
        {
            material = _material;
        }

        #endregion

        public void GenNormalVector(Random R)
        {
            NormalVector.X = (R.NextDouble() - 0.5);
            NormalVector.Y = (R.NextDouble() - 0.5);
            NormalVector.Z = (R.NextDouble() - 0.5);

            double norm = Math.Sqrt(NormalVector.X * NormalVector.X + 
                NormalVector.Y * NormalVector.Y + 
                NormalVector.Z * NormalVector.Z);
            
            NormalVector.X = NormalVector.X / norm;
            NormalVector.Y = NormalVector.Y / norm;
            NormalVector.Z = NormalVector.Z / norm; 
        }
        public static bool isIntersected(Atom A1, Atom A2)
        {
            var distance = Math.Sqrt ((A1.Position.X - A2.Position.X) * (A1.Position.X - A2.Position.X) +
                           (A1.Position.Y - A2.Position.Y) * (A1.Position.Y - A2.Position.Y) +
                           (A1.Position.Z - A2.Position.Z) * (A1.Position.Z - A2.Position.Z));
            return distance < 2 * A1.Radius;
        }

        public static double operator -(Atom A1, Atom A2)
        {
            double L = (A1.Position.X - A2.Position.X) * (A1.Position.X - A2.Position.X) + 
                (A1.Position.Y - A2.Position.Y) * (A1.Position.Y - A2.Position.Y) + 
                (A1.Position.Z - A2.Position.Z) * (A1.Position.Z - A2.Position.Z);
            return Math.Abs (L) > 1e-6 ? Math.Sqrt (L) : 0;
        }
    }
}
