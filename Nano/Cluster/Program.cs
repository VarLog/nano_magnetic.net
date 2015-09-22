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
using System.Collections.Generic;

using NDesk.Options;

using ClusterLib;

namespace Cluster
{
    class Program
    {
        static class Defaults
        {
            public const double Anisotropy = 4000;
            public const double Saturation = 800;
            public const double ParticleRadius = 20e-7;

            public const double Dt = 0.001;

            public const double Epsillon = 1e-8;

            public const double ClusterRadius = 80e-7;

            public const double MinH = -1500;
            public const double MaxH = 1500;
            public const double StepH = 300;
        }

        static string Version()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo( assembly.Location );
            string version = fvi.FileVersion;

            return version;
        }

        static void Usage( string appName, OptionSet p )
        {
            var version = Version();

            Console.WriteLine( "Usage: " + appName + " [<options>]" );
            Console.WriteLine( "Version: " + version );
            Console.WriteLine();
            Console.WriteLine( "Options:" );
            p.WriteOptionDescriptions( Console.Out );
        }

        static void Main( string[] args )
        {
            var appName = AppDomain.CurrentDomain.FriendlyName;

            bool needShowHelp = false;

            double anisotropy = Defaults.Anisotropy;
            double saturation = Defaults.Saturation;
            double particleRadius = Defaults.ParticleRadius;

            double dt = Defaults.Dt;
            double epsillon = Defaults.Epsillon;

            double clusterRadius = Defaults.ClusterRadius;

            double minH = Defaults.MinH;
            double maxH = Defaults.MaxH;
            double stepH = Defaults.StepH;

            int particlesCount = 10;

            var p = new OptionSet();
            p.Add( "n|particles-count=", "Count of particles.", (int v ) => particlesCount = v );

            p.Add( "a|anisotropy=", "Magnetic anisotropy of the material.", (double v ) => anisotropy = v );
            p.Add( "s|saturation=", "Magnetic saturation of the material.", (double v ) => saturation = v );
            p.Add( "p|particle-radius=", "Radius of a particular of the material.", (double v ) => particleRadius = v );

            p.Add( "d|dt=", "", (double v ) => dt = v );
            p.Add( "e|epsillon=", "", (double v ) => epsillon = v );

            p.Add( "c|cluster-radius=", "Radius of the sphere.", (double v ) => clusterRadius = v );

            p.Add( "min=", "Min value of H.", (double v ) => minH = v );
            p.Add( "max=", "Max value of H.", (double v ) => maxH = v );
            p.Add( "step=", "Step for H.", (double v ) => stepH = v );

            p.Add( "h|help", "Show this message and exit", v => needShowHelp = v != null );
            p.Add( "v", "Increase debug message verbosity",
                v => {
                    if( v != null )
                    {
                        Utils.VerbosityLevel++;
                    }
                } );
            
            try
            {
                p.Parse( args );
            }
            catch( OptionException e )
            {
                Console.Write( appName + ": " );
                Console.WriteLine( e.Message );
                Console.WriteLine( "Try `" + appName + " --help' for more information." );
                return;
            }

            if( needShowHelp )
            {
                Usage( appName, p );
                return;
            }

            if( particlesCount <= 0 )
            {
                Console.Write( appName + ": " );
                Console.WriteLine( "Count of the particles must be positive" );
                Console.WriteLine( "Try `" + appName + " --help' for more information." );
                return;
            }

            var material = new Material( anisotropy, saturation, particleRadius );

            var cluster = new Sphere( clusterRadius );

            // Generate particles
            cluster.Particles = Utils.GenerateRandromParticlesInSphere( material, clusterRadius, particlesCount );

            var results = new Dictionary<double, double>();

            for( var i = minH; i <= maxH; i += stepH )
            {
                var externalMagneticField = new Vector( i, 0, 0 );

                var magneticMomentAverage = cluster.Calculate( externalMagneticField, dt, epsillon );

                results.Add( i, magneticMomentAverage.X );
            }

            Console.WriteLine( "Results:" );
            foreach( var pair in results )
            {
                Console.WriteLine( "ExternalMagneticField: " + pair.Key + " EffectiveMagneticField: " + pair.Value );
            }
        }
    }
}
