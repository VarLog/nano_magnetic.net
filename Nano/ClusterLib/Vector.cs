using System;

namespace ClusterLib
{
    public struct Vector
    {
        public double X { set; get; }

        public double Y { set; get; }

        public double Z { set; get; }

        public Vector ()
        {
            X = default(double);
            Y = default(double);
            Z = default(double);
        }

        public Vector (double value)
        {
            X = Y = Z = value;
        }

        public Vector (double _X, double _Y, double _Z)
        {
            X = _X;
            Y = _Y;
            Z = _Z;
        }

        public double mod ()
        {
            return Math.Sqrt (X * X + Y * Y + Z * Z);
        }

        public double square ()
        {
            return X * X + Y * Y + Z * Z;
        }

        public static Vector operator * (Vector v1, Vector v2)
        {
            return new Vector (v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
        }

        public static Vector operator * (Vector v, double scalar)
        {
            return new Vector (v.X * scalar, v.Y * scalar, v.Z * scalar);
        }

        public static Vector operator / (Vector v1, Vector v2)
        {
            return new Vector (v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z);
        }

        public static Vector operator / (Vector v, double scalar)
        {
            return v * (1 / scalar);
        }

        public static Vector operator + (Vector v1, Vector v2)
        {
            return new Vector (v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector operator - (Vector v1, Vector v2)
        {
            return new Vector (v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

    }
}
