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
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib
{
    public class Sphere
    {
        /// <summary>
        /// The radius of the sphere.
        /// \f$R_{cl}\f$
        /// </summary>
        readonly public double Radius;

        /// <summary>
        /// Gets or sets the particles.
        /// </summary>
        /// <value>The particles.</value>
        public List<Particle> Particles { 
            get { return particles; }
            set
            {
                particles = value; 

                if( particles != null )
                {
                    if( particles.Any() )
                    {
                        ParticlesMaterial = particles.First().Material;

                        if( particles.Any( p => p.Material != ParticlesMaterial ) )
                        {
                            throw new Exception( "All particles must have the same material" );
                        }
                    }
                }
            }
        }

        List<Particle> particles = new List<Particle>();

        /// <summary>
        /// Gets or sets the particles material.
        /// </summary>
        /// <value>The particles material.</value>
        Material ParticlesMaterial { get; set; }

        /// <summary>
        /// Gets the particules count.
        /// \f$N_{p}\f$
        /// </summary>
        /// <value>The particules count.</value>
        public int ParticlesCount { get { return Particles.Count; } }

        /// <summary>
        /// The particles density.
        /// \f$h=\frac{VN_{p}}{V_{cl}}\f$
        /// </summary>
        /// <value>The particles density.</value>
        public double ParticlesDensity { get; private set; }

        /// <summary>
        /// The anisotropy coefficient.
        /// \f\frac{K}{M_{s}^2}\f$
        /// </summary>
        /// <value>The anisotropy coefficient.</value>
        public double AnisotropyCoefficient { get; private set; }

        /// <summary>
        /// Distanation matrixes.
        /// </summary>
        double[,] mat11, mat12, mat13, mat22, mat23, mat33;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterLib.Sphere"/> class.
        /// </summary>
        /// <param name="radius">Radius.</param>
        public Sphere( double radius )
        {
            Radius = radius;
        }

        /// <summary>
        /// Calculates the distinations between particles.
        /// </summary>
        void calculateDistinations()
        {
            // TODO: Improve mechanism to store and to calculate destinations
            mat11 = new double[ParticlesCount, ParticlesCount];
            mat12 = new double[ParticlesCount, ParticlesCount];
            mat13 = new double[ParticlesCount, ParticlesCount];
            mat22 = new double[ParticlesCount, ParticlesCount];
            mat23 = new double[ParticlesCount, ParticlesCount];
            mat33 = new double[ParticlesCount, ParticlesCount];

            for( int i = 0; i < ParticlesCount; i++ )
            {
                for( int j = 0; j < ParticlesCount; j++ )
                {
                    if( i == j )
                    {
                        continue;
                    }

                    var p1 = Particles[ i ];
                    var p2 = Particles[ j ];

                    var diff = p1.RadiusVector - p2.RadiusVector;

                    var distance = diff.Mod();
                    var distanceCube = Math.Pow( distance, 3 );

                    var nR = diff / distance;

                    mat11[ i, j ] = ( 1 - 3 * nR.X * nR.X ) / distanceCube;
                    mat12[ i, j ] = -3 * nR.X * nR.Y / distanceCube;
                    mat13[ i, j ] = -3 * nR.X * nR.Z / distanceCube;
                    mat22[ i, j ] = ( 1 - 3 * nR.Y * nR.Y ) / distanceCube;
                    mat23[ i, j ] = -3 * nR.Y * nR.Z / distanceCube;
                    mat33[ i, j ] = ( 1 - 3 * nR.Z * nR.Z ) / distanceCube;

                    mat11[ j, i ] = mat11[ i, j ];
                    mat12[ j, i ] = mat12[ i, j ];
                    mat13[ j, i ] = mat13[ i, j ];
                    mat22[ j, i ] = mat22[ i, j ];
                    mat23[ j, i ] = mat23[ i, j ];
                    mat33[ j, i ] = mat33[ i, j ];
                }
            }
        }

        /// <summary>
        /// Calculate the Landau–Lifshitz–Gilbert equation.
        /// </summary>
        /// <param name="magneticField">The external magnetic field.</param>
        /// <param name="epsillon">Epsillon.</param>
        public double Calculate( Magnetic magneticField, double epsillon )
        {
            if( Particles == null || !Particles.Any() )
            {
                throw new Exception( "There are not any particles to calculate" );
            }

            particles.ForEach( p => p.MagneticVector = magneticField.MagneticVector.Norm() );

            var volume = ( 4.0 / 3.0 ) * Math.PI * Math.Pow( Radius, 3 );
            ParticlesDensity = ParticlesMaterial.Volume * ParticlesCount / volume;

            Utils.Debug( "Particles density h == " + ParticlesDensity );

            AnisotropyCoefficient = ParticlesMaterial.MagneticAnisotropy / Math.Pow( ParticlesMaterial.MagneticSaturation, 2 );

            Utils.Debug( "Anisotropy coefficient == " + AnisotropyCoefficient );


            calculateDistinations();

            double sum = 0;
         
            while( true )
            {
                CountH( magneticField.MagneticVector );
                MakeStep( magneticField );
                var isDone = CountForce( epsillon );

                // FIXME: Potential infinity loop here:
                if( isDone )
                {
                    break;
                }
            }

            Particles.ForEach( a => {
                var v = a.MagneticVector * magneticField.MagneticVector;
                sum += v.X + v.Y + v.Z;
            } );

            return sum / Particles.Count;
        }


        public Vector CountHdip( int j )
        {
            var HM = new Vector();

            for( int i = 0; i < Particles.Count; i++ )
            {
                if( i != j )
                {
                    var particle = Particles[ i ];
                    var magneticVector = particle.MagneticVector;

                    HM.X = HM.X + mat11[ j, i ] * magneticVector.X +
                    mat12[ j, i ] * magneticVector.Y +
                    mat13[ j, i ] * magneticVector.Z;
                    
                    HM.Y = HM.Y + mat12[ j, i ] * magneticVector.X +
                    mat22[ j, i ] * magneticVector.Y +
                    mat23[ j, i ] * magneticVector.Z;
                    
                    HM.Z = HM.Z + mat13[ j, i ] * magneticVector.X +
                    mat23[ j, i ] * magneticVector.Y +
                    mat33[ j, i ] * magneticVector.Z;
                }
            }
            return HM;        
        }

        public void CountH( Vector h0cur )
        {
            for( int i = 0; i < Particles.Count; i++ )
            {
                var particle = Particles[ i ];

                var sp = particle.MagneticVector.Dot( particle.EasyAnisotropyAxis );

                var Ha = particle.EasyAnisotropyAxis * sp * particle.Material.MagneticDamping;

                var HM = CountHdip( i );

                var Hi = HM * -ParticlesDensity;

                particle.EffectiveMagneticField = Ha + Hi + h0cur;
            }
        }

        public void MakeStep( Magnetic magneticField )
        {
            var Hrs = Particles.ConvertAll( p => p.EffectiveMagneticField.Mod() );
            double maxHr = Hrs.Max();

            // TODO: Why dt isn't a constant?? Why maxHr??
            var dt = magneticField.StabFactor * Math.PI /
                     ( 30 * maxHr * ( 1 + magneticField.Stc ) * ( 1 + magneticField.Kappa * magneticField.Kappa ) );

            foreach( var particle in Particles )
            {
                var magnetic = particle.MagneticVector;

                var H = particle.EffectiveMagneticField + magnetic * maxHr * magneticField.Stc;

                var at = 1 / ( 1 + Math.Pow( dt * H.Mod(), 2 ) );

                var x = H.Dot( particle.MagneticVector ) * dt * dt;

                var v = H.Cross( magnetic );
                v = v * dt;

                magnetic = ( magnetic + H * x + v ) * at;

                particle.MagneticVector = magnetic / magnetic.Mod();
            }
        }

        public bool CountForce( double epsillon )
        {
            bool isDone = true;
            foreach( var particle in Particles )
            {
                // [ particle.MagneticVector, particle.MagneticVector ] <= epsillon

                var v = particle.EffectiveMagneticField.Cross( particle.MagneticVector );

                if( v.Mod() >= epsillon )
                {
                    isDone = false;
                }
            }

            return isDone;
        }

    }
}
