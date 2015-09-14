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
using System.Diagnostics;

using ClusterLib;

namespace Cluster
{
    class Program
    {
        static void Main( string[] args )
        {
            const double anisotropy = 40000;
            const double saturation = 500;
            const double particleRadius = 20e-7;
            var material = new Material( anisotropy, saturation, particleRadius );


            var magnetic = new Magnetic();
            magnetic.kappa = 0.2;
            magnetic.Stc = 30;
            magnetic.stabkoeff = 30;
            magnetic.EpsR = 1e-12;

            // Random MagneticVector
//            {
//                var R = new Random ();
//                var randVector = new Vector (2 * (R.NextDouble () - 0.5), 
//                    2 * (R.NextDouble () - 0.5), 
//                    2 * (R.NextDouble () - 0.5));
//                magnetic.MagneticVector = randVector;
//            }
            magnetic.MagneticVector = new Vector( 1 ) / Math.Sqrt( 3 );


            const double clusterRadius = 80e-7;

            var cluster = new Sphere( clusterRadius, magnetic );
            cluster.Particles = Utils.GetDetermParticles( material );

            //const int particlesCount = 20;
            //cluster.Particles = Utils.GenerateRandromParticlesInSphere (material, clusterRadius, particlesCount);

            var rangeH = new [] { -1500, 1500 };
            const int step = 300;
            cluster.calculate( rangeH, step );

            Console.WriteLine( "Results:" );
            cluster.Result.ForEach( r => Console.WriteLine( "U: " + r.U + " R: " + r.R ) );
        }
    }
}
