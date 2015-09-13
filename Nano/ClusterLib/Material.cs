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

        public Material (double _K1, double _Ms, double _Radius)
        {
            K1 = _K1;
            Ms = _Ms;
            Radius = _Radius;
            Diametr = 2 * _Radius;
            Hk = 2 * K1 / Ms;
            Volume = 4 * Math.PI * Radius * Radius * Radius / 3;
        }
    }
}
