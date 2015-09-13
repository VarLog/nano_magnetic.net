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

            cluster.MagneticField.kappa = 0.2;
            cluster.MagneticField.Stc = 30;
            cluster.MagneticField.stabkoeff = 30;
            cluster.MagneticField.EpsR = 1e-12;


            cluster.calculate(1500, 300);
        }
    }
}
