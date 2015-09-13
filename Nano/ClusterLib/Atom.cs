using System;
using System.Collections.Generic;

namespace ClusterLib
{
    public class Atom
    {
        #region Parametrs

        public PointCL Position;

        public PointCL MagneticVector;
            
        public PointCL NormalVector;

        public Material Material;

        public PointCL Hr;

        #endregion

        #region Constructors
        public Atom()
        { 
        }

        public Atom(Material _material)
        {
            Material = _material;
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

        public bool isIntersected(Atom that)
        {
            var diff = Position - that.Position;
            var distance = diff.mod();
            return distance < (Material.Radius + that.Material.Radius);
        }
    }
}
