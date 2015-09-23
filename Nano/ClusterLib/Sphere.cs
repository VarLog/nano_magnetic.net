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
        /// Initializes a new instance of the <see cref="ClusterLib.Sphere"/> class.
        /// </summary>
        /// <param name="radius">Radius.</param>
        public Sphere( double radius )
        {
            Radius = radius;
        }

        /// <summary>
        /// Calculate the Landau–Lifshitz–Gilbert equation.
        /// </summary>
        /// <param name="externalMagneticField">The external magnetic field.</param>
        /// <param name="dt">dt.</param>
        /// <param name="epsillon">Epsillon.</param>
        public Vector Calculate( Vector externalMagneticField, double dt, double epsillon )
        {
            if( Particles == null || !Particles.Any() )
            {
                throw new Exception( "There are not any particles to calculate" );
            }

            var volume = ( 4.0 / 3.0 ) * Math.PI * Math.Pow( Radius, 3 );
            ParticlesDensity = ParticlesMaterial.Volume * ParticlesCount / volume;

            Utils.Debug( "Particles density h == " + ParticlesDensity );

            AnisotropyCoefficient = ParticlesMaterial.MagneticAnisotropy / Math.Pow( ParticlesMaterial.MagneticSaturation, 2 );

            Utils.Debug( "Anisotropy coefficient == " + AnisotropyCoefficient );

            bool isFirstIteration = true;
            bool isDone = false;
            while( !isDone )
            {
                isDone = true;
                foreach( var particle in Particles )
                {
                    if( !isFirstIteration )
                    {
                        if( particle.MagneticVector.Cross( particle.EffectiveMagneticField ).Mod() <= epsillon )
                        {
                            continue;
                        }
                    }

                    isDone = false;

                    var Ha = particle.EasyAnisotropyAxis * particle.MagneticVector.Dot( particle.EasyAnisotropyAxis )
                             * particle.Material.MagneticDamping;

                    var HM = new Vector();

                    foreach( var p in Particles.Where(p => p != particle) )
                    {
                        var diff = particle.RadiusVector - p.RadiusVector;
                        var diffNorm = diff.Norm();

                        var numerator = p.MagneticVector - diffNorm * 3 * diffNorm.Dot( p.MagneticVector );
                        var denumerator = Math.Pow( diff.Mod(), 3 );

                        HM = HM + ( numerator / denumerator );
                    }

                    HM = HM * particle.Material.MagneticSaturation * particle.Material.Volume;

                    particle.EffectiveMagneticField = Ha + HM + externalMagneticField;

                    var hr = particle.EffectiveMagneticField - particle.EffectiveMagneticField.Cross( particle.MagneticVector )
                             * particle.Material.MagneticDamping;
                 
                    var at = 1 / ( 1 + Math.Pow( dt * hr.Mod(), 2 ) );
                    var aNext = particle.MagneticVector + hr.Cross( particle.MagneticVector ) * dt
                                + hr * Math.Pow( dt, 2 ) * hr.Dot( particle.MagneticVector );

                    particle.MagneticVector = aNext * at;
                }
                isFirstIteration = false;
            }

            var magneticMomentAverage = new Vector();

            Particles.ForEach( a => {
                var m = a.MagneticVector * ParticlesMaterial.MagneticSaturation * ParticlesMaterial.Volume;
                magneticMomentAverage = magneticMomentAverage + m;
            } );

            magneticMomentAverage = magneticMomentAverage / Particles.Count;

            return magneticMomentAverage;
        }

    }
}
