using System;

namespace ClusterLib
{
    public struct PointCL 
    {
        public double X { set; get; }
        public double Y { set; get; }
        public double Z { set; get; }

        public PointCL()
        {
            X = default(double);
            Y = default(double);
            Z = default(double);
        }

        public PointCL(double value)
        {
            X = Y = Z = value;
        }

        public PointCL(double _X, double _Y, double _Z)
        {
            X = _X;
            Y = _Y;
            Z = _Z;
        }

        public double mod()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public double square()
        {
            return X * X + Y * Y + Z * Z;
        }

        public static PointCL operator *(PointCL v1, PointCL v2)
        {
            return new PointCL (v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static PointCL operator *(PointCL v, double scalar)
        {
            return new PointCL (v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static PointCL operator /(PointCL v1, PointCL v2)
        {
            return new PointCL (v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static PointCL operator /(PointCL v, double scalar)
        {
            return v * (1 / scalar);
        }

        public static PointCL operator +(PointCL v1, PointCL v2)
        {
            return new PointCL (v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static PointCL operator -(PointCL v1, PointCL v2)
        {
            return new PointCL (v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

    }
}
