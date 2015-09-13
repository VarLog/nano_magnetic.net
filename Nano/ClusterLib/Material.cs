using System;

namespace ClusterLib
{
    public class Material
    {
        public double K1 { get; }

        public double Ms { get; }

        public double Hk { get; }

        public double Diametr { get; }

        public double Radius { get; }

        public double Volume { get; }

        public Material (double k1, double ms, double radius)
        {
            K1 = k1;
            Ms = ms;
            Radius = radius;
            Diametr = 2 * radius;
            Hk = 2 * K1 / Ms;
            Volume = 4 * Math.PI * Math.Pow (Radius, 3) / 3;
        }
    }
}
