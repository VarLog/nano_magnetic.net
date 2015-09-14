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
        /// The external magnetic field.
        /// </summary>
        readonly public Magnetic MagneticField;

        /// <summary>
        /// The radius of the sphere.
        /// \f$R_{cl}\f$
        /// </summary>
        readonly public double Radius;

        /// <summary>
        /// The results.
        /// </summary>
        readonly public List<Result> Result = new List<Result> ();

        /// <summary>
        /// Gets or sets the particles.
        /// </summary>
        /// <value>The particles.</value>
        public List<Particle> Particles { 
            get { return particles; }
            set {
                particles = value; 

                if (particles != null)
                {
                    if (particles.Any ())
                    {
                        ParticlesMaterial = particles.First ().Material;

                        if (particles.Any (p => p.Material != ParticlesMaterial))
                        {
                            throw new Exception ("All particles must have the same material");
                        }

                        calculateDistinations ();
                    }

                    particles.ForEach (p => p.MagneticVector = MagneticField.MagneticVector);
                }
            }
        }

        List<Particle> particles = new List<Particle> ();

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
        /// Distanation matrixes.
        /// </summary>
        double[,] mat11, mat12, mat13, mat22, mat23, mat33;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterLib.Sphere"/> class.
        /// </summary>
        /// <param name="radius">Radius.</param>
        /// <param name="magneticField">Magnetic field.</param>
        public Sphere (double radius, Magnetic magneticField)
        {
            Radius = radius;
            MagneticField = magneticField;
        }

        /// <summary>
        /// Calculates the distinations between particles.
        /// </summary>
        void calculateDistinations ()
        {
            // TODO: Old implementation. It sould be discussed. 
            ParticlesDensity = ParticlesMaterial.Volume * ParticlesMaterial.MagneticSaturation / Math.Pow (Radius, 3);

            //var volume = (4 / 3) * Math.PI * Math.Pow (Radius, 3);
            //ParticlesDensity = ParticlesMaterial.Volume * ParticlesCount / volume;

            Console.WriteLine ("Particles density h == " + ParticlesDensity);

            // TODO: Improve mechanism to store and to calculate destinations
            mat11 = new double[ParticlesCount, ParticlesCount];
            mat12 = new double[ParticlesCount, ParticlesCount];
            mat13 = new double[ParticlesCount, ParticlesCount];
            mat22 = new double[ParticlesCount, ParticlesCount];
            mat23 = new double[ParticlesCount, ParticlesCount];
            mat33 = new double[ParticlesCount, ParticlesCount];

            for (int i = 0; i < ParticlesCount; i++)
            {
                for (int j = 0; j < ParticlesCount; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var p1 = Particles [i];
                    var p2 = Particles [j];

                    var diff = p1.Position - p2.Position;

                    var distance = diff.mod ();
                    var distanceCube = Math.Pow (distance, 3);

                    var nR = diff / distance;

                    mat11 [i, j] = (1 - 3 * nR.X * nR.X) / distanceCube;
                    mat12 [i, j] = -3 * nR.X * nR.Y / distanceCube;
                    mat13 [i, j] = -3 * nR.X * nR.Z / distanceCube;
                    mat22 [i, j] = (1 - 3 * nR.Y * nR.Y) / distanceCube;
                    mat23 [i, j] = -3 * nR.Y * nR.Z / distanceCube;
                    mat33 [i, j] = (1 - 3 * nR.Z * nR.Z) / distanceCube;

                    mat11 [j, i] = mat11 [i, j];
                    mat12 [j, i] = mat12 [i, j];
                    mat13 [j, i] = mat13 [i, j];
                    mat22 [j, i] = mat22 [i, j];
                    mat23 [j, i] = mat23 [i, j];
                    mat33 [j, i] = mat33 [i, j];
                }
            }
        }

        /// <summary>
        /// Calculate the Landau–Lifshitz–Gilbert equation.
        /// </summary>
        /// <param name="magneticIntensityRange">Magnetic intensity range.</param>
        /// <param name="step">Step.</param>
        public void calculate (IEnumerable<int> magneticIntensityRange, double step)
        {
            if (Particles == null || !Particles.Any ())
            {
                throw new Exception ("There are not any particles to calculate");
            }

            const int maxIterCount = 10000;

            var array = magneticIntensityRange.ToArray ();
            var min = array.ElementAt (0);
            var max = array.ElementAt (1);

            for (double i = max; i >= min; i = i - step)
            {
                double sum = 0;
             
                var H0cur = MagneticField.MagneticVector * i;

                double rave;
                int iterCount = 0;

                while (true)
                {
                    CountH (H0cur);
                    MakeStep ();
                    rave = CountForce ();

                    if (++iterCount >= maxIterCount)
                    {
                        break;
                    }

                    if (rave <= MagneticField.EpsR)
                    {
                        break;
                    }
                }

                Particles.ForEach (a => {
                    var v = a.MagneticVector * MagneticField.MagneticVector;
                    sum += v.X + v.Y + v.Z;
                });

                Result.Add (new Result (i, sum / Particles.Count));
            }           
        }

        public Vector CountHdip (int j)
        {
            var HM = new Vector ();

            for (int i = 0; i < Particles.Count; i++)
            {
                if (i != j)
                {
                    var particle = Particles [i];
                    var magneticVector = particle.MagneticVector;

                    HM.X = HM.X + mat11 [j, i] * magneticVector.X +
                    mat12 [j, i] * magneticVector.Y +
                    mat13 [j, i] * magneticVector.Z;
                    
                    HM.Y = HM.Y + mat12 [j, i] * magneticVector.X +
                    mat22 [j, i] * magneticVector.Y +
                    mat23 [j, i] * magneticVector.Z;
                    
                    HM.Z = HM.Z + mat13 [j, i] * magneticVector.X +
                    mat23 [j, i] * magneticVector.Y +
                    mat33 [j, i] * magneticVector.Z;
                }
            }
            return HM;        
        }

        public void CountH (Vector h0cur)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                var particle = Particles [i];

                var sp = particle.MagneticVector.dot (particle.NormalVector);

                var Ha = particle.NormalVector * sp * particle.Material.MagneticDamping;

                var HM = CountHdip (i);

                var Hi = HM * -ParticlesDensity;

                particle.Hr = Ha + Hi + h0cur;
            }
        }

        public void MakeStep ()
        {
            var Hrs = Particles.ConvertAll (p => p.Hr.mod ());
            double maxHr = Hrs.Max ();

            var dt = MagneticField.stabkoeff * Math.PI /
                     (30 * maxHr * (1 + MagneticField.Stc) * (1 + MagneticField.kappa * MagneticField.kappa));

            foreach (var particle in Particles)
            {
                Vector Calc;
                Calc = particle.MagneticVector;

                var H = particle.Hr + Calc * maxHr * MagneticField.Stc;

                var at = 1 / (1 + Math.Pow (dt * H.mod (), 2));

                var x = H.dot (particle.MagneticVector) * dt * dt;

                var v = new Vector ();
                v.X = dt * (H.Y * Calc.Z - H.Z * Calc.Y);
                v.Y = dt * (H.Z * Calc.X - H.X * Calc.Z);
                v.Z = dt * (H.X * Calc.Y - H.Y * Calc.X);

                Calc = (Calc + H * x + v) * at;

                particle.MagneticVector = Calc / Calc.mod ();
            }
        }

        public double CountForce ()
        {         
            double Rmax = 0;

            double rave = 0;

            foreach (var particle in Particles)
            {
                var v = new Vector ();
                v.X = particle.Hr.Y * particle.MagneticVector.Z - particle.Hr.Z * particle.MagneticVector.Y;
                v.Y = particle.Hr.Z * particle.MagneticVector.X - particle.Hr.X * particle.MagneticVector.Z;
                v.Z = particle.Hr.X * particle.MagneticVector.Y - particle.Hr.Y * particle.MagneticVector.X;

                var x = v.mod () / particle.Hr.mod ();
                if (Rmax < x)
                {
                    Rmax = x;
                }

                rave += Math.Abs (x);
            }
            rave = rave / ParticlesCount;

            return rave;
        }
    }
}
