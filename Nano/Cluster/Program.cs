using ClusterLib;

namespace Cluster
{
    class Program
    {
        static void Main(string[] args)
        {
            var material = new Material(40000, 500, 20e-7);

            var cluster = new Sphere();

            //cluster.AddAtomList(atom, 20);
            cluster.AddDetermList(material);

            var magnetic = new Magnetic ();
            magnetic.kappa = 0.2;
            magnetic.Stc = 30;
            magnetic.stabkoeff = 30;
            magnetic.EpsR = 1e-12;

            cluster.MagneticField = magnetic;

            cluster.calculate(1500, 300);
        }
    }
}
