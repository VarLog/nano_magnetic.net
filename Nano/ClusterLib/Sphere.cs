using System;
using System.Collections.Generic;
using System.Linq;

namespace ClusterLib
{
    public class Sphere
    {
        readonly List<Atom> Atoms = new List<Atom>();
       
        public Magnetic MagneticField { get; set; }

        public double Radius { get; set; }

        PointCL MagneticVector = new PointCL();

        public List<Result> Result { get; } = new List<Result>();

        double Hd_num;   //Hd_num = Ms*Volume/RCl3;

        //Матрицы
        double[,] M11, M12, M13, M22, M23, M33;

        public Sphere(double _Radius)
        {
            Radius = _Radius;
            var R = new Random();

            var randVector = new PointCL (2 * (R.NextDouble () - 0.5), 
                                 2 * (R.NextDouble () - 0.5), 
                                 2 * (R.NextDouble () - 0.5));
            MagneticVector = randVector;
        }

        void createRandromAtoms(Material material, long count)
        {
            var R = new Random();

            for (int i = 0; i < count; i++)
            {
                var atom = new Atom(material);

                var randPosition = new PointCL ((R.NextDouble() - 0.5) * Radius, 
                    (R.NextDouble() - 0.5) * Radius, 
                    (R.NextDouble() - 0.5) * Radius);

                atom.Position = randPosition;

                bool isIntersected = Atoms.Any (atom.isIntersected);
                if (isIntersected)
                {
                    i--;
                    continue;
                }

                Atoms.Add(atom);
                atom.GenNormalVector();
                atom.MagneticVector = MagneticVector;
            }
        }

        public void GeneretaAtoms(Material material, long count)
        {
            createRandromAtoms(material, count);

            Hd_num = material.Volume * material.Ms / Math.Pow(Radius, 3);

            M11 = new double[Atoms.Count, Atoms.Count];
            M12 = new double[Atoms.Count, Atoms.Count];
            M13 = new double[Atoms.Count, Atoms.Count];
            M22 = new double[Atoms.Count, Atoms.Count];
            M23 = new double[Atoms.Count, Atoms.Count];
            M33 = new double[Atoms.Count, Atoms.Count];

            for (int i = 0; i < Atoms.Count; i++ )
            {
                Atoms[i].Position = Atoms[i].Position / Radius;
            }

            for (int i = 0; i < Atoms.Count; i++)
            {
                for (int j = 0; j < Atoms.Count; j++)
                {
                    if (i == j) {
                        continue;
                    }

                    var atom1 = Atoms [i];
                    var atom2 = Atoms [j];

                    var diff = atom1.Position - atom2.Position;

                    var distance = diff.mod();
                    var distanceCube = Math.Pow (distance, 3);

                    var nR = diff / distance;

                    M11[i, j] = (1 - 3 * nR.X * nR.X) / distanceCube;
                    M12[i, j] = -3 * nR.X * nR.Y / distanceCube;
                    M13[i, j] = -3 * nR.X * nR.Z / distanceCube;
                    M22[i, j] = (1 - 3 * nR.Y * nR.Y) / distanceCube;
                    M23[i, j] = -3 * nR.Y * nR.Z / distanceCube;
                    M33[i, j] = (1 - 3 * nR.Z * nR.Z) / distanceCube;

                    M11[j, i] = M11[i, j]; M12[j, i] = M12[i, j]; M13[j, i] = M13[i, j];
                    M22[j, i] = M22[i, j]; M23[j, i] = M23[i, j];
                    M33[j, i] = M33[i, j];
                }
            }
        }

        public void AddDetermList(Material material)
        {
            MagneticVector = new PointCL(1) / Math.Sqrt(3);

            {
                var atom1 = new Atom (material);
                atom1.Position = new PointCL (0.5, 0.25, 0.25);
                atom1.NormalVector = new PointCL (1 / Math.Sqrt (2), 1 / Math.Sqrt (2), 0);
                atom1.MagneticVector = new PointCL (1) / Math.Sqrt (3);
                Atoms.Add (atom1);

                var atom2 = new Atom (material);
                atom2.Position = new PointCL (-0.25, 0.5, 0.25);
                atom2.NormalVector = new PointCL (0, 0, 1);
                atom2.MagneticVector = new PointCL (1) / Math.Sqrt (3);
                Atoms.Add (atom2);

                var atom3 = new Atom (material);
                atom3.Position = new PointCL (-0.5, -0.25, 0.25);
                atom3.NormalVector = new PointCL (1) / Math.Sqrt (3);
                atom3.MagneticVector = new PointCL (1) / Math.Sqrt (3);
                Atoms.Add (atom3);

                var atom4 = new Atom (material);
                atom4.Position = new PointCL (-0.25, -0.25, -0.25);
                atom4.NormalVector = new PointCL (0, 0, 1);
                atom4.MagneticVector = new PointCL (1) / Math.Sqrt (3);
                Atoms.Add (atom4);
            }

            Hd_num = material.Volume * material.Ms / Math.Pow(Radius, 3);

            M11 = new double[Atoms.Count, Atoms.Count];
            M12 = new double[Atoms.Count, Atoms.Count];
            M13 = new double[Atoms.Count, Atoms.Count];
            M22 = new double[Atoms.Count, Atoms.Count];
            M23 = new double[Atoms.Count, Atoms.Count];
            M33 = new double[Atoms.Count, Atoms.Count];

            for (int i = 0; i < Atoms.Count; i++)
            {
                for (int j = 0; j < Atoms.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var atom1 = Atoms [i];
                    var atom2 = Atoms [j];

                    var diff = atom1.Position - atom2.Position;

                    var distance = diff.mod();
                    var distanceCube = Math.Pow (distance, 3);

                    var nR = diff / distance;

                    M11[i, j] = (1 - 3 * nR.X * nR.X) / distanceCube;
                    M12[i, j] = -3 * nR.X * nR.Y / distanceCube;
                    M13[i, j] = -3 * nR.X * nR.Z / distanceCube;
                    M22[i, j] = (1 - 3 * nR.Y * nR.Y) / distanceCube;
                    M23[i, j] = -3 * nR.Y * nR.Z / distanceCube;
                    M33[i, j] = (1 - 3 * nR.Z * nR.Z) / distanceCube;

                    M11[j, i] = M11[i, j]; M12[j, i] = M12[i, j]; M13[j, i] = M13[i, j];
                    M22[j, i] = M22[i, j]; M23[j, i] = M23[i, j];
                    M33[j, i] = M33[i, j];
                }
            }
        }


        public void AddDetermListOne(Material material)
        {
            MagneticVector = new PointCL(1) / Math.Sqrt(3);

            var atom1 = new Atom (material);
            atom1.Position = new PointCL (0.5, 0.25, 0.25);
            atom1.NormalVector = new PointCL (1, 0, 0);
            atom1.MagneticVector = new PointCL (1) / Math.Sqrt (3);
            Atoms.Add (atom1);
        }

        public void calculateOne(double h, double step)
        {
            for (double i = h; i >= -h; i = i - step)
            {
                double sum = 0;

                var H0cur = MagneticVector * i;

                double rave;
                while(true)
                {
                    CountH(H0cur);
                    MakeStep();
                    rave = CountForce();

                    if (rave <= MagneticField.EpsR)
                    {
                        break;
                    }
                }

                Atoms.ForEach (a => {
                    var v = a.MagneticVector * MagneticVector;
                    sum += v.X + v.Y + v.Z;
                });

                Result.Add(new Result(i, sum / Atoms.Count));
            }           
        }

        public void calculate(double H, double step)
        {
            const int maxIterCount = 10000;

            for (double i = H; i >= -H; i=i-step )
            {
                double sum = 0;
             
                var H0cur = MagneticVector * i;

                double rave;
                int iterCount = 0;

                while(true)
                {
                    CountH(H0cur);
                    MakeStep();
                    rave = CountForce();

                    if (++iterCount >= maxIterCount)
                    {
                        break;
                    }

                    if (rave <= MagneticField.EpsR)
                    {
                        break;
                    }
                }

                Atoms.ForEach (a => {
                    var v = a.MagneticVector * MagneticVector;
                    sum += v.X + v.Y + v.Z;
                });

                Result.Add(new Result(i, sum / Atoms.Count));
            }           
        }

        //проблема 
        public PointCL CountHdip(int j)
        {
            var HM = new PointCL ();
                
            for (int i = 0; i < Atoms.Count; i++)
            {
                if (i != j)
                {
                    var atom = Atoms [j];
                    var magneticVector = atom.MagneticVector;

                    HM.X = HM.X + M11[j, i] * magneticVector.X + 
                        M12[j, i] * magneticVector.Y + 
                        M13[j, i] * magneticVector.Z;
                    
                    HM.Y = HM.Y + M12[j, i] * magneticVector.X + 
                        M22[j, i] * magneticVector.Y + 
                        M23[j, i] * magneticVector.Z;
                    
                    HM.Z = HM.Z + M13[j, i] * magneticVector.X + 
                        M23[j, i] * magneticVector.Y + 
                        M33[j, i] * magneticVector.Z;
                }
            }
            return HM;        
        }
       
        public void CountH(PointCL h0cur)
        {
            for (int i = 0; i < Atoms.Count; i++)
            {
                var atom = Atoms[i];

                //скалярное произведение
                var sp = (atom.MagneticVector * atom.NormalVector).square();

                var Ha = atom.NormalVector * sp * atom.Material.Hk;

                var HM = CountHdip(i);

                var Hi = HM * -Hd_num;

                atom.Hr = Ha + Hi + h0cur;
            }
        }

        public void MakeStep()
        {
            var Hrs = Atoms.ConvertAll (a => a.Hr.mod());
            double maxHr = Hrs.Max ();

            var dt = MagneticField.stabkoeff * Math.PI / 
                (30 * maxHr * (1 + MagneticField.Stc) * (1 + MagneticField.kappa * MagneticField.kappa));

            foreach (var atom in Atoms)
            {
                PointCL Calc;
                Calc = atom.MagneticVector;

                var H = atom.Hr + Calc * maxHr * MagneticField.Stc;

                var at = 1 / (1 + Math.Pow(dt * H.mod(), 2));

                var x = H * atom.MagneticVector * dt * dt;

                var v = new PointCL ();
                v.X = dt * (H.Y * Calc.Z - H.Z * Calc.Y);
                v.Y = dt * (H.Z * Calc.X - H.X * Calc.Z);
                v.Z = dt * (H.X * Calc.Y - H.Y * Calc.X);

                Calc = (Calc + x * H + v) * at;

                atom.MagneticVector = Calc / Calc.mod();
            }
        }

        public double CountForce()
        {         
            double Rmax = 0;

            double rave = 0;

            foreach (var atom in Atoms)
            {
                var v = new PointCL ();
                v.X = atom.Hr.Y * atom.MagneticVector.Z - atom.Hr.Z * atom.MagneticVector.Y;
                v.Y = atom.Hr.Z * atom.MagneticVector.X - atom.Hr.X * atom.MagneticVector.Z;
                v.Z = atom.Hr.X * atom.MagneticVector.Y - atom.Hr.Y * atom.MagneticVector.X;
                var x = v.mod () / atom.Hr.mod ();
                if (Rmax < x) {
                    Rmax = x;
                }
                rave += Math.Abs (x);
            }
            rave = rave / Atoms.Count;

            return rave;
        }       
    }
}
