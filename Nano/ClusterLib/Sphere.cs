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
        public Magnetic MagneticField { get; set; }

        readonly public double Radius;

        readonly public Vector MagneticVector;

        readonly public List<Result> Result = new List<Result> ();

        readonly List<Atom> atoms = new List<Atom> ();

        double Hd;

        /// <summary>
        /// Distanation matrixes.
        /// </summary>
        double[,] mat11, mat12, mat13, mat22, mat23, mat33;

        public Sphere (double radius, Vector magneticVector)
        {
            Radius = radius;
            MagneticVector = magneticVector;
        }

        public Sphere (double radius)
        {
            Radius = radius;

            var R = new Random ();
            var randVector = new Vector (2 * (R.NextDouble () - 0.5), 
                                 2 * (R.NextDouble () - 0.5), 
                                 2 * (R.NextDouble () - 0.5));
            MagneticVector = randVector;
        }

        void createRandromAtoms (Material material, int count)
        {
            var R = new Random ();

            for (int i = 0; i < count; i++) {
                var atom = new Atom (material);

                var randPosition = new Vector ((R.NextDouble () - 0.5) * Radius, 
                                       (R.NextDouble () - 0.5) * Radius, 
                                       (R.NextDouble () - 0.5) * Radius);
                atom.Position = randPosition;

                bool isIntersected = atoms.Any (atom.isIntersected);
                if (isIntersected) {
                    i--;
                    continue;
                }

                atoms.Add (atom);
                atom.GenNormalVector ();
                atom.MagneticVector = MagneticVector;
            }
        }

        public void GeneretaAtoms (Material material, int count)
        {
            createRandromAtoms (material, count);

            Hd = material.Volume * material.Ms / Math.Pow (Radius, 3);

            var atomsCount = atoms.Count;
            mat11 = new double[atomsCount, atomsCount];
            mat12 = new double[atomsCount, atomsCount];
            mat13 = new double[atomsCount, atomsCount];
            mat22 = new double[atomsCount, atomsCount];
            mat23 = new double[atomsCount, atomsCount];
            mat33 = new double[atomsCount, atomsCount];

            atoms.ForEach (a => a.Position = a.Position / Radius);

            for (int i = 0; i < atoms.Count; i++) {
                for (int j = 0; j < atoms.Count; j++) {
                    if (i == j) {
                        continue;
                    }

                    var atom1 = atoms [i];
                    var atom2 = atoms [j];

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
                var atom1 = new Atom (material);
                atom1.Position = new Vector (0.5, 0.25, 0.25);
                atom1.NormalVector = new Vector (1 / Math.Sqrt (2), 1 / Math.Sqrt (2), 0);
                atom1.MagneticVector = new Vector (1) / Math.Sqrt (3);
                atoms.Add (atom1);

                var atom2 = new Atom (material);
                atom2.Position = new Vector (-0.25, 0.5, 0.25);
                atom2.NormalVector = new Vector (0, 0, 1);
                atom2.MagneticVector = new Vector (1) / Math.Sqrt (3);
                atoms.Add (atom2);

                var atom3 = new Atom (material);
                atom3.Position = new Vector (-0.5, -0.25, 0.25);
                atom3.NormalVector = new Vector (1) / Math.Sqrt (3);
                atom3.MagneticVector = new Vector (1) / Math.Sqrt (3);
                atoms.Add (atom3);

                var atom4 = new Atom (material);
                atom4.Position = new Vector (-0.25, -0.25, -0.25);
                atom4.NormalVector = new Vector (0, 0, 1);
                atom4.MagneticVector = new Vector (1) / Math.Sqrt (3);
                atoms.Add (atom4);
            }

            Hd = material.Volume * material.Ms / Math.Pow (Radius, 3);

            var atomsCount = atoms.Count;
            mat11 = new double[atomsCount, atomsCount];
            mat12 = new double[atomsCount, atomsCount];
            mat13 = new double[atomsCount, atomsCount];
            mat22 = new double[atomsCount, atomsCount];
            mat23 = new double[atomsCount, atomsCount];
            mat33 = new double[atomsCount, atomsCount];

            for (int i = 0; i < atoms.Count; i++) {
                for (int j = 0; j < atoms.Count; j++) {
                    if (i == j) {
                        continue;
                    }

                    var atom1 = atoms [i];
                    var atom2 = atoms [j];

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
            var atom1 = new Atom (material);
            atom1.Position = new Vector (0.5, 0.25, 0.25);
            atom1.NormalVector = new Vector (1, 0, 0);
            atom1.MagneticVector = new Vector (1) / Math.Sqrt (3);
            atoms.Add (atom1);
        }

        public void calculateOne (double h, double step)
        {
            for (double i = h; i >= -h; i = i - step) {
                double sum = 0;

                var H0cur = MagneticVector * i;

                double rave;
                while (true) {
                    CountH (H0cur);
                    MakeStep ();
                    rave = CountForce ();

                    if (rave <= MagneticField.EpsR) {
                        break;
                    }
                }

                atoms.ForEach (a => {
                    var v = a.MagneticVector * MagneticVector;
                    sum += v.X + v.Y + v.Z;
                });

                Result.Add (new Result (i, sum / atoms.Count));
            }           
        }

        public void calculate (double H, double step)
        {
            const int maxIterCount = 10000;

            for (double i = H; i >= -H; i = i - step) {
                double sum = 0;
             
                var H0cur = MagneticVector * i;

                double rave;
                int iterCount = 0;

                while (true) {
                    CountH (H0cur);
                    MakeStep ();
                    rave = CountForce ();

                    if (++iterCount >= maxIterCount) {
                        break;
                    }

                    if (rave <= MagneticField.EpsR) {
                        break;
                    }
                }

                atoms.ForEach (a => {
                    var v = a.MagneticVector * MagneticVector;
                    sum += v.X + v.Y + v.Z;
                });

                Result.Add (new Result (i, sum / atoms.Count));
            }           
        }

        public Vector CountHdip (int j)
        {
            var HM = new Vector ();
                
            for (int i = 0; i < atoms.Count; i++) {
                if (i != j) {
                    var atom = atoms [j];
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
            for (int i = 0; i < atoms.Count; i++) {
                var atom = atoms [i];

                var sp = (atom.MagneticVector * atom.NormalVector).square ();

                var Ha = atom.NormalVector * sp * atom.Material.Hk;

                var HM = CountHdip (i);

                var Hi = HM * -Hd;

                atom.Hr = Ha + Hi + h0cur;
            }
        }

        public void MakeStep ()
        {
            var Hrs = atoms.ConvertAll (a => a.Hr.mod ());
            double maxHr = Hrs.Max ();

            var dt = MagneticField.stabkoeff * Math.PI /
                     (30 * maxHr * (1 + MagneticField.Stc) * (1 + MagneticField.kappa * MagneticField.kappa));

            foreach (var atom in atoms) {
                Vector Calc;
                Calc = atom.MagneticVector;

                var H = atom.Hr + Calc * maxHr * MagneticField.Stc;

                var at = 1 / (1 + Math.Pow (dt * H.mod (), 2));

                var x = H * atom.MagneticVector * dt * dt;

                var v = new Vector ();
                v.X = dt * (H.Y * Calc.Z - H.Z * Calc.Y);
                v.Y = dt * (H.Z * Calc.X - H.X * Calc.Z);
                v.Z = dt * (H.X * Calc.Y - H.Y * Calc.X);

                Calc = (Calc + x * H + v) * at;

                atom.MagneticVector = Calc / Calc.mod ();
            }
        }

        public double CountForce ()
        {         
            double Rmax = 0;

            double rave = 0;

            foreach (var atom in atoms) {
                var v = new Vector ();
                v.X = atom.Hr.Y * atom.MagneticVector.Z - atom.Hr.Z * atom.MagneticVector.Y;
                v.Y = atom.Hr.Z * atom.MagneticVector.X - atom.Hr.X * atom.MagneticVector.Z;
                v.Z = atom.Hr.X * atom.MagneticVector.Y - atom.Hr.Y * atom.MagneticVector.X;

                var x = v.mod () / atom.Hr.mod ();
                if (Rmax < x) {
                    Rmax = x;
                }

                rave += Math.Abs (x);
            }
            rave = rave / atoms.Count;

            return rave;
        }
    }
}
