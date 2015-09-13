//
// The MIT License (MIT)
// 
// Copyright (c) 2015 Maxim Fedorenko <varlllog@gmail.com>
// Copyright (c) 2015 Roman Shershnev <LarscoRS@yandex.ru>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 

using System;

namespace ClusterLib
{
    public struct Vector
    {
        public double X { set; get; }

        public double Y { set; get; }

        public double Z { set; get; }

        //public Vector ()   {       }

        public Vector (double value):this()
        {
            X = Y = Z = value;
        }

        public Vector(double x, double y, double z)
            : this()
        {

            X = x;
            Y = y;
            Z = z;
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
