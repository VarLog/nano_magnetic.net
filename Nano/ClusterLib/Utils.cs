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
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib
{
    public static class Utils
    {
        /// <summary>
        /// The verbosity level.
        /// </summary>
        public static int VerbosityLevel { get; set; }

        /// <summary>
        /// Debug output using specified format and args.
        /// </summary>
        /// <param name="format">Format.</param>
        /// <param name="args">Arguments.</param>
        public static void Debug( string format, params object[] args )
        {
            if( VerbosityLevel > 0 )
            {
                for( int i = 0; i < VerbosityLevel; i++ )
                {
                    Console.Write( "#" );
                }
                Console.Write( " " );

                Console.WriteLine( format, args );
            }
        }

        /// <summary>
        /// Generates the randrom particles.
        /// </summary>
        /// <param name="material">Particles material.</param>
        /// <param name="radius">The radius of the sphere.</param>
        /// <param name="count">Particles count.</param>
        public static List<Particle> GenerateRandromParticlesInSphere( Material material, double radius, int count )
        {
            var res = new List<Particle>();

            var rand = new Random();

            for( int i = 0; i < count; i++ )
            {
                var particle = new Particle( material );

                var randPosition = new Vector( ( rand.NextDouble() - 0.5 ) * radius, 
                                       ( rand.NextDouble() - 0.5 ) * radius, 
                                       ( rand.NextDouble() - 0.5 ) * radius );
                particle.RadiusVector = randPosition;

                // TODO: Improve algorithm to avoid intersections
                bool isIntersected = res.Any( particle.isIntersected );
                if( isIntersected )
                {
                    i--;
                    continue;
                }

                res.Add( particle );
            }

            res.ForEach( p => p.RadiusVector = p.RadiusVector / radius );

            return res;
        }

        /// <summary>
        /// Gets a list of 4 predefined particles.
        /// </summary>
        /// <returns>The determ particles.</returns>
        /// <param name="material">Material.</param>
        public static List<Particle> GetPredefineParticles( Material material )
        {
            var res = new List<Particle>();

            var p1 = new Particle( material );
            p1.RadiusVector = new Vector( 0.5, 0.25, 0.25 );
            p1.EasyAnisotropyAxis = new Vector( 1 / Math.Sqrt( 2 ), 1 / Math.Sqrt( 2 ), 0 );
            p1.MagneticVector = new Vector( 1 ) / Math.Sqrt( 3 );
            res.Add( p1 );

            var p2 = new Particle( material );
            p2.RadiusVector = new Vector( -0.25, 0.5, 0.25 );
            p2.EasyAnisotropyAxis = new Vector( 0, 0, 1 );
            p2.MagneticVector = new Vector( 1 ) / Math.Sqrt( 3 );
            res.Add( p2 );

            var p3 = new Particle( material );
            p3.RadiusVector = new Vector( -0.5, -0.25, 0.25 );
            p3.EasyAnisotropyAxis = new Vector( 1 ) / Math.Sqrt( 3 );
            p3.MagneticVector = new Vector( 1 ) / Math.Sqrt( 3 );
            res.Add( p3 );

            var p4 = new Particle( material );
            p4.RadiusVector = new Vector( -0.25, -0.25, -0.25 );
            p4.EasyAnisotropyAxis = new Vector( 0, 0, 1 );
            p4.MagneticVector = new Vector( 1 ) / Math.Sqrt( 3 );
            res.Add( p4 );
        
            return res;
        }

        /// <summary>
        /// Gets a single predefined particle.
        /// </summary>
        /// <returns>The determ single particle.</returns>
        /// <param name="material">Material.</param>
        public static List<Particle> GetPredefineSingleParticle( Material material )
        {
            var res = new List<Particle>();

            var p = new Particle( material );
            p.RadiusVector = new Vector( 0.5, 0.25, 0.25 );
            p.EasyAnisotropyAxis = new Vector( 1, 0, 0 );
            p.MagneticVector = new Vector( 1 ) / Math.Sqrt( 3 );
            res.Add( p );

            return res;
        }

    }
}

