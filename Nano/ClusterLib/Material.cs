﻿using System;

namespace ClusterLib
{
    public class Material
    {
        public double K1 { get; }   //{uniaxial anisotropy constant}
        public double Ms { get; }   //{500, 1400 emu/cm3}
        public double Hk { get; } 
        public double Diametr { get; }
        public double Radius { get; }
        public double Volume { get; }

        public Material(double _K1, double _Ms, double _Radius)
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
