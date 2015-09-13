using System;
using System.Collections.Generic;

namespace ClusterLib
{
    public class Atom
    {
        #region Parametrs

        public Vector Position { get; set; }

        public Vector MagneticVector { get; set; }

        public Vector NormalVector { get; set; }

        public Material Material { get; private set; }

        public Vector Hr { get; set; }

        #endregion

        #region Constructors

        public Atom (Material _material)
        {
            Material = _material;
        }

        #endregion

        public void GenNormalVector ()
        {
            var R = new Random ();

            var randVector = new Vector ((R.NextDouble () - 0.5),
                                 (R.NextDouble () - 0.5),
                                 (R.NextDouble () - 0.5));

            NormalVector = randVector;
            NormalVector = NormalVector / NormalVector.mod ();
        }

        public bool isIntersected (Atom that)
        {
            var diff = Position - that.Position;
            var distance = diff.mod ();
            return distance < (Material.Radius + that.Material.Radius);
        }
    }
}
