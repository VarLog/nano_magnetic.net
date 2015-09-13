using ClusterLib;
using System.Diagnostics;
using System;

namespace Cluster
{
    class Program
    {
        static void Main (string[] args)
        {
            var material = new Material (40000, 500, 20e-7);

            const double radius = 80e-7;
            var magneticVector = new Vector (1) / Math.Sqrt (3);

            //var cluster = new Sphere (radius, magneticVector);
            //cluster.AddAtomList(material, 20);

            var cluster = new Sphere (radius, magneticVector);
            cluster.AddDetermList (material);

            var magnetic = new Magnetic ();
            magnetic.kappa = 0.2;
            magnetic.Stc = 30;
            magnetic.stabkoeff = 30;
            magnetic.EpsR = 1e-12;

            cluster.MagneticField = magnetic;

            cluster.calculate (1500, 300);

            cluster.Result.ForEach (r => Debug.WriteLine ("U: " + r.U + " R: " + r.R));
        }
    }
}
