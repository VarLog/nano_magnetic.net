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
        readonly public Magnetic MagneticField;

        readonly public double Radius;

        readonly public List<Result> Result = new List<Result> ();

        readonly List<Particle> particles = new List<Particle> ();

        public int ParticulesCount { get { return particles.Count; } }

        public double ParticlesDensity { get; private set; }

        /// <summary>
        /// Distanation matrixes.
        /// </summary>
        double[,] mat11, mat12, mat13, mat22, mat23, mat33;

        public Sphere (double radius, Magnetic magneticField)
        {
            Radius = radius;
            MagneticField = magneticField;
        }

        void createRandromParticles (Material material, int count)
        {
            var R = new Random ();

            for (int i = 0; i < count; i++)
            {
                var particle = new Particle (material);

                var randPosition = new Vector ((R.NextDouble () - 0.5) * Radius, 
                                       (R.NextDouble () - 0.5) * Radius, 
                                       (R.NextDouble () - 0.5) * Radius);
                particle.Position = randPosition;

                bool isIntersected = particles.Any (particle.isIntersected);
                if (isIntersected)
                {
                    i--;
                    continue;
                }

                particles.Add (particle);
                particle.GenNormalVector ();
                particle.MagneticVector = MagneticField.MagneticVector;
            }
        }

        public void GenereteParticles (Material material, int count)
        {
            createRandromParticles (material, count);

            ParticlesDensity = material.Volume * material.MagneticSaturation / Math.Pow (Radius, 3);
            Console.WriteLine ("Particles density h == " + ParticlesDensity);

            var atomsCount = particles.Count;
            mat11 = new double[atomsCount, atomsCount];
            mat12 = new double[atomsCount, atomsCount];
            mat13 = new double[atomsCount, atomsCount];
            mat22 = new double[atomsCount, atomsCount];
            mat23 = new double[atomsCount, atomsCount];
            mat33 = new double[atomsCount, atomsCount];

            particles.ForEach (a => a.Position = a.Position / Radius);

            for (int i = 0; i < particles.Count; i++)
            {
                for (int j = 0; j < particles.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var atom1 = particles [i];
                    var atom2 = particles [j];

                    var diff = atom1.Position - atom2.Position;

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

        public void AddDetermList (Material material)
        {
            {
                var atom1 = new Particle (material);
                atom1.Position = new Vector (0.5, 0.25, 0.25);
                atom1.NormalVector = new Vector (1 / Math.Sqrt (2), 1 / Math.Sqrt (2), 0);
                atom1.MagneticVector = new Vector (1) / Math.Sqrt (3);
                particles.Add (atom1);

                var atom2 = new Particle (material);
                atom2.Position = new Vector (-0.25, 0.5, 0.25);
                atom2.NormalVector = new Vector (0, 0, 1);
                atom2.MagneticVector = new Vector (1) / Math.Sqrt (3);
                particles.Add (atom2);

                var atom3 = new Particle (material);
                atom3.Position = new Vector (-0.5, -0.25, 0.25);
                atom3.NormalVector = new Vector (1) / Math.Sqrt (3);
                atom3.MagneticVector = new Vector (1) / Math.Sqrt (3);
                particles.Add (atom3);

                var atom4 = new Particle (material);
                atom4.Position = new Vector (-0.25, -0.25, -0.25);
                atom4.NormalVector = new Vector (0, 0, 1);
                atom4.MagneticVector = new Vector (1) / Math.Sqrt (3);
                particles.Add (atom4);
            }

            ParticlesDensity = material.Volume * material.MagneticSaturation / Math.Pow (Radius, 3);

            var atomsCount = particles.Count;
            mat11 = new double[atomsCount, atomsCount];
            mat12 = new double[atomsCount, atomsCount];
            mat13 = new double[atomsCount, atomsCount];
            mat22 = new double[atomsCount, atomsCount];
            mat23 = new double[atomsCount, atomsCount];
            mat33 = new double[atomsCount, atomsCount];

            for (int i = 0; i < particles.Count; i++)
            {
                for (int j = 0; j < particles.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var atom1 = particles [i];
                    var atom2 = particles [j];

                    var diff = atom1.Position - atom2.Position;

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


        public void AddDetermListOne (Material material)
        {
            var atom1 = new Particle (material);
            atom1.Position = new Vector (0.5, 0.25, 0.25);
            atom1.NormalVector = new Vector (1, 0, 0);
            atom1.MagneticVector = new Vector (1) / Math.Sqrt (3);
            particles.Add (atom1);
        }

        public void calculateOne (double h, double step)
        {
            for (double i = h; i >= -h; i = i - step)
            {
                double sum = 0;

                var H0cur = MagneticField.MagneticVector * i;

                double rave;
                while (true)
                {
                    CountH (H0cur);
                    MakeStep ();
                    rave = CountForce ();

                    if (rave <= MagneticField.EpsR)
                    {
                        break;
                    }
                }

                particles.ForEach (a => {
                    var v = a.MagneticVector * MagneticField.MagneticVector;
                    sum += v.X + v.Y + v.Z;
                });

                Result.Add (new Result (i, sum / particles.Count));
            }           
        }

        public void calculate (double H, double step)
        {
            const int maxIterCount = 10000;

            for (double i = H; i >= -H; i = i - step)
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

                particles.ForEach (a => {
                    var v = a.MagneticVector * MagneticField.MagneticVector;
                    sum += v.X + v.Y + v.Z;
                });

                Result.Add (new Result (i, sum / particles.Count));
            }           
        }

        public Vector CountHdip (int j)
        {
            var HM = new Vector ();
                
            for (int i = 0; i < particles.Count; i++)
            {
                if (i != j)
                {
                    var atom = particles [i];
                    var magneticVector = atom.MagneticVector;

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
            for (int i = 0; i < particles.Count; i++)
            {
                var atom = particles [i];

                var sp = atom.MagneticVector.dot (atom.NormalVector);

                var Ha = atom.NormalVector * sp * atom.Material.MagneticDamping;

                var HM = CountHdip (i);

                var Hi = HM * -ParticlesDensity;

                atom.Hr = Ha + Hi + h0cur;
            }
        }

        public void MakeStep ()
        {
            var Hrs = particles.ConvertAll (a => a.Hr.mod ());
            double maxHr = Hrs.Max ();

            var dt = MagneticField.stabkoeff * Math.PI /
                     (30 * maxHr * (1 + MagneticField.Stc) * (1 + MagneticField.kappa * MagneticField.kappa));

            foreach (var atom in particles)
            {
                Vector Calc;
                Calc = atom.MagneticVector;

                var H = atom.Hr + Calc * maxHr * MagneticField.Stc;

                var at = 1 / (1 + Math.Pow (dt * H.mod (), 2));

                var x = H.dot (atom.MagneticVector) * dt * dt;

                var v = new Vector ();
                v.X = dt * (H.Y * Calc.Z - H.Z * Calc.Y);
                v.Y = dt * (H.Z * Calc.X - H.X * Calc.Z);
                v.Z = dt * (H.X * Calc.Y - H.Y * Calc.X);

                Calc = (Calc + H * x + v) * at;

                atom.MagneticVector = Calc / Calc.mod ();
            }
        }

        public double CountForce ()
        {         
            double Rmax = 0;

            double rave = 0;

            foreach (var atom in particles)
            {
                var v = new Vector ();
                v.X = atom.Hr.Y * atom.MagneticVector.Z - atom.Hr.Z * atom.MagneticVector.Y;
                v.Y = atom.Hr.Z * atom.MagneticVector.X - atom.Hr.X * atom.MagneticVector.Z;
                v.Z = atom.Hr.X * atom.MagneticVector.Y - atom.Hr.Y * atom.MagneticVector.X;

                var x = v.mod () / atom.Hr.mod ();
                if (Rmax < x)
                {
                    Rmax = x;
                }

                rave += Math.Abs (x);
            }
            rave = rave / particles.Count;

            return rave;
        }
    }
}
