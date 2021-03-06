﻿//
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
        /// <summary>
        /// Gets or sets the x.
        /// </summary>
        /// <value>The x.</value>
        public double X { set; get; }

        /// <summary>
        /// Gets or sets the y.
        /// </summary>
        /// <value>The y.</value>
        public double Y { set; get; }

        /// <summary>
        /// Gets or sets the z.
        /// </summary>
        /// <value>The z.</value>
        public double Z { set; get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterLib.Vector"/> struct.
        /// </summary>
        /// <param name="value">Value for X, Y and for Z.</param>
        public Vector( double value ) : this()
        {
            X = Y = Z = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterLib.Vector"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public Vector( double x, double y, double z ) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Module (distance) of Vector instance.
        /// </summary>
        public double Mod()
        {
            return Math.Sqrt( Square() );
        }

        /// <summary>
        /// Square of the module (distance) of the Vector instance.
        /// </summary>
        public double Square()
        {
            return X * X + Y * Y + Z * Z;
        }

        /// <summary>
        /// Dot product of two vector: this and that.
        /// </summary>
        /// <param name="that">Result scalar</param>
        public double Dot( Vector that )
        {
            return X * that.X + Y * that.Y + Z * that.Z;
        }

        /// <summary>
        /// Cross product of two vector: [ this x that ].
        /// </summary>
        /// <param name="that">Result vector.</param>
        public Vector Cross( Vector that )
        {
            Vector res;
            res.X = Y * that.Z - Z * that.Y;
            res.Y = Z * that.X - X * that.Z;
            res.Z = X * that.Y - Y * that.X;

            return res;
        }

        /// <summary>
        /// Get the unit normalized vector.
        /// </summary>
        public Vector Norm()
        {
            var mod = Mod();
            if( Math.Abs( mod ) < double.Epsilon )
            {
                throw new Exception( "The module of the vector is equal to zero" );
            }
            return this / Mod();
        }

        /// <summary>
        /// Return true if the vector is equal to zero.
        /// </summary>
        /// <returns><c>true</c>, if the vector is equal to zero, <c>false</c> otherwise.</returns>
        public bool isZero()
        {
            if( Math.Abs( X ) > double.Epsilon )
            {
                return false;
            }
            if( Math.Abs( Y ) > double.Epsilon )
            {
                return false;
            }
            if( Math.Abs( Z ) > double.Epsilon )
            {
                return false;
            }
            return true;
        }

        public static Vector operator *( Vector v1, Vector v2 )
        {
            return new Vector( v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z );
        }

        public static Vector operator *( Vector v, double scalar )
        {
            return new Vector( v.X * scalar, v.Y * scalar, v.Z * scalar );
        }

        public static Vector operator /( Vector v1, Vector v2 )
        {
            return new Vector( v1.X / v2.X, v1.Y / v2.Y, v1.Z / v2.Z );
        }

        public static Vector operator /( Vector v, double scalar )
        {
            return v * ( 1 / scalar );
        }

        public static Vector operator +( Vector v1, Vector v2 )
        {
            return new Vector( v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z );
        }

        public static Vector operator -( Vector v1, Vector v2 )
        {
            return new Vector( v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z );
        }

    }
}
