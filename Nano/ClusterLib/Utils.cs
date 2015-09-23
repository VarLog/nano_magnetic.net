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

                particle.RadiusVector = new Vector( ( rand.NextDouble() - 0.5 ) * radius, 
                    ( rand.NextDouble() - 0.5 ) * radius, 
                    ( rand.NextDouble() - 0.5 ) * radius );

                // TODO: Improve algorithm to avoid intersections
                bool isIntersected = res.Any( particle.isIntersected );
                if( isIntersected )
                {
                    i--;
                    continue;
                }

                res.Add( particle );
            }

            return res;
        }
    }
}

