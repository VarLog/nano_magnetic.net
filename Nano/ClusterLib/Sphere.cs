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

        public List<Result> Result1 { get; } = new List<Result>();
        public List<Result> Result2 { get; } = new List<Result>();

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
        
        public void AddAtomList(Material material, long Num)
        {
            var R = new Random();

            for (int i = 0; i < Num; i++)
            {
                var atom = new Atom();
                atom.Material = material;

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
                atom.GenNormalVector(R);
                atom.MagneticVector = MagneticVector;

            }

            Hd_num = Atoms[0].Material.Volume * Atoms[0].Material.Ms / Math.Pow(Radius, 3);

            M11 = new double[Atoms.Count, Atoms.Count];
            M12 = new double[Atoms.Count, Atoms.Count];
            M13 = new double[Atoms.Count, Atoms.Count];
            M22 = new double[Atoms.Count, Atoms.Count];
            M23 = new double[Atoms.Count, Atoms.Count];
            M33 = new double[Atoms.Count, Atoms.Count];

            for (int i = 0; i < Atoms.Count; i++ )
            {
                Atoms[i].Position.X = Atoms[i].Position.X / Radius;
                Atoms[i].Position.Y = Atoms[i].Position.Y / Radius;
                Atoms[i].Position.Z = Atoms[i].Position.Z / Radius;
            }

            for (int i = 0; i < Atoms.Count; i++)
            {
                for (int j = 0; j < Atoms.Count; j++)
                {
                    if (i != j)
                    {
                        var Rd2 = (Atoms[i].Position.X - Atoms[j].Position.X) * (Atoms[i].Position.X - Atoms[j].Position.X) +
                            (Atoms[i].Position.Y - Atoms[j].Position.Y) * (Atoms[i].Position.Y - Atoms[j].Position.Y) +
                            (Atoms[i].Position.Z - Atoms[j].Position.Z) * (Atoms[i].Position.Z - Atoms[j].Position.Z);
                        var Rd = Math.Sqrt(Rd2);
                        var Rd3 = Rd * Rd2;

                        var nRx = (Atoms[i].Position.X - Atoms[j].Position.X) / Rd;
                        var nRy = (Atoms[i].Position.Y - Atoms[j].Position.Y) / Rd;
                        var nRz = (Atoms[i].Position.Z - Atoms[j].Position.Z) / Rd;

                        M11[i, j] = (1 - 3 * nRx * nRx) / Rd3;
                        M12[i, j] = -3 * nRx * nRy / Rd3;
                        M13[i, j] = -3 * nRx * nRz / Rd3;
                        M22[i, j] = (1 - 3 * nRy * nRy) / Rd3;
                        M23[i, j] = -3 * nRy * nRz / Rd3;
                        M33[i, j] = (1 - 3 * nRz * nRz) / Rd3;

                        M11[j, i] = M11[i, j]; M12[j, i] = M12[i, j]; M13[j, i] = M13[i, j];
                        M22[j, i] = M22[i, j]; M23[j, i] = M23[i, j];
                        M33[j, i] = M33[i, j];
                    }

                }
            }
        }

        public void AddDetermList(Material material)
        {
            MagneticVector.X = 1 / Math.Sqrt(3);
            MagneticVector.Y = 1 / Math.Sqrt(3);
            MagneticVector.Z = 1 / Math.Sqrt(3);

            var atom1 = new Atom();
            var atom2 = new Atom();
            var atom3 = new Atom();
            var atom4 = new Atom();

            atom1.Material = material;
            atom2.Material = material;
            atom3.Material = material;
            atom4.Material = material;

            atom1.Position.X = 0.5;
            atom1.Position.Y = 0.25;
            atom1.Position.Z = 0.25;
            atom1.Material.Radius = 20e-7;
            atom1.Material.Volume = 4 * Math.PI * atom1.Material.Radius * atom1.Material.Radius * atom1.Material.Radius / 3;
            atom1.NormalVector.X = 1/Math.Sqrt(2);
            atom1.NormalVector.Y = 1/Math.Sqrt(2);
            atom1.NormalVector.Z = 0;

            atom1.MagneticVector.X = 1;
            atom1.MagneticVector.Y = 0;
            atom1.MagneticVector.Z = 0;

            atom1.MagneticVector.X = 1 / Math.Sqrt(3);
            atom1.MagneticVector.Y = 1 / Math.Sqrt(3);
            atom1.MagneticVector.Z = 1 / Math.Sqrt(3); 
            Atoms.Add(atom1);

            atom2.Position.X = -0.25;
            atom2.Position.Y = 0.5;
            atom2.Position.Z = 0.25;
            atom2.Material.Radius = 20e-7;
            atom2.Material.Volume = 4 * Math.PI * atom1.Material.Radius * atom1.Material.Radius * atom1.Material.Radius / 3;
            atom2.NormalVector.X = 0;
            atom2.NormalVector.Y = 0;
            atom2.NormalVector.Z = 1;

            atom2.MagneticVector.X = 0;
            atom2.MagneticVector.Y = 1;
            atom2.MagneticVector.Z = 0;

            atom2.MagneticVector.X = 1 / Math.Sqrt(3);
            atom2.MagneticVector.Y = 1 / Math.Sqrt(3);
            atom2.MagneticVector.Z = 1 / Math.Sqrt(3);
            Atoms.Add(atom2);

            atom3.Position.X = -0.5;
            atom3.Position.Y = -0.25;
            atom3.Position.Z = 0.25;
            atom3.Material.Radius = 20e-7;
            atom3.Material.Volume = 4 * Math.PI * atom1.Material.Radius * atom1.Material.Radius * atom1.Material.Radius / 3;
            atom3.NormalVector.X = 1 / Math.Sqrt(3);
            atom3.NormalVector.Y = 1 / Math.Sqrt(3);
            atom3.NormalVector.Z = 1 / Math.Sqrt(3);

            atom3.MagneticVector.X = 1 / Math.Sqrt(3);
            atom3.MagneticVector.Y = 1 / Math.Sqrt(3);
            atom3.MagneticVector.Z = 1 / Math.Sqrt(3);
            Atoms.Add(atom3);

            atom4.Position.X = -0.25;
            atom4.Position.Y = -0.25;
            atom4.Position.Z = -0.25;
            atom4.Material.Radius = 20e-7;
            atom4.Material.Volume = 4 * Math.PI * atom1.Material.Radius * atom1.Material.Radius * atom1.Material.Radius / 3;
            atom4.NormalVector.X = 0;
            atom4.NormalVector.Y = 0;
            atom4.NormalVector.Z = 1;
          

            atom4.MagneticVector.X = 1 / Math.Sqrt(3);
            atom4.MagneticVector.Y = 1 / Math.Sqrt(3);
            atom4.MagneticVector.Z = 1 / Math.Sqrt(3);
            Atoms.Add(atom4);            
        
            Hd_num = Atoms[0].Material.Volume * Atoms[0].Material.Ms / Math.Pow(Radius, 3);

            double nRx, nRy, nRz;
            double Rd, Rd2 = 0, Rd3;

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
                    if (i != j)
                    {
                        Rd2 = (Atoms[i].Position.X - Atoms[j].Position.X) * (Atoms[i].Position.X - Atoms[j].Position.X) +
                            (Atoms[i].Position.Y - Atoms[j].Position.Y) * (Atoms[i].Position.Y - Atoms[j].Position.Y) +
                            (Atoms[i].Position.Z - Atoms[j].Position.Z) * (Atoms[i].Position.Z - Atoms[j].Position.Z);
                        Rd = Math.Sqrt(Rd2);
                        Rd3 = Rd * Rd2;

                        nRx = (Atoms[i].Position.X - Atoms[j].Position.X) / Rd;
                        nRy = (Atoms[i].Position.Y - Atoms[j].Position.Y) / Rd;
                        nRz = (Atoms[i].Position.Z - Atoms[j].Position.Z) / Rd;

                        M11[i, j] = (1 - 3 * nRx * nRx) / Rd3; 
                        M12[i, j] = -3 * nRx * nRy / Rd3; 
                        M13[i, j] = -3 * nRx * nRz / Rd3;
                        M22[i, j] = (1 - 3 * nRy * nRy) / Rd3; 
                        M23[i, j] = -3 * nRy * nRz / Rd3;
                        M33[i, j] = (1 - 3 * nRz * nRz) / Rd3;

                        M11[j, i] = M11[i, j]; M12[j, i] = M12[i, j]; M13[j, i] = M13[i, j];
                        M22[j, i] = M22[i, j]; M23[j, i] = M23[i, j];
                        M33[j, i] = M33[i, j];
                    }

                }
            }
        }


        public void AddDetermListOne(Material material)
        {
            MagneticVector.X = 1 / Math.Sqrt(3);
            MagneticVector.Y = 1 / Math.Sqrt(3);
            MagneticVector.Z = 1 / Math.Sqrt(3);

            var atom1 = new Atom();         

            atom1.Material = material;            

            atom1.Position.X = 0.5;
            atom1.Position.Y = 0.25;
            atom1.Position.Z = 0.25;
            atom1.Material.Radius = 20e-7;
            atom1.Material.Volume = 4 * Math.PI * atom1.Material.Radius * atom1.Material.Radius * atom1.Material.Radius / 3;
            atom1.NormalVector.X = 1;
            atom1.NormalVector.Y = 0;
            atom1.NormalVector.Z = 0;            

            atom1.MagneticVector.X = 1 / Math.Sqrt(3);
            atom1.MagneticVector.Y = 1 / Math.Sqrt(3);
            atom1.MagneticVector.Z = 1 / Math.Sqrt(3);
            Atoms.Add(atom1);
        }

        public void calculateOne(double h, double step)
        {
            double sSum;

            for (double i = h; i >= -h; i = i - step)
            {
                var res = new Result();
                sSum = 0;
                res.U = i;

                PointCL H0cur;
                H0cur.X = i * MagneticVector.X;
                H0cur.Y = i * MagneticVector.Y;
                H0cur.Z = i * MagneticVector.Z;

                double rave;
                do
                {
                    CountH(H0cur);
                    MakeStep();
                    rave = CountForce();
                } while (rave > MagneticField.EpsR);


                for (int j = 0; j < Atoms.Count; j++)
                {
                    sSum = sSum + Atoms[j].MagneticVector.X * MagneticVector.X + Atoms[j].MagneticVector.Y * 
                        MagneticVector.Y + Atoms[j].MagneticVector.Z * MagneticVector.Z;

                }
                res.R = sSum / Atoms.Count;

                Result1.Add(res);
                Result2.Add(-res);
            }           
        }

        public void calculate(double H, double step)
        {
            double SSum;
            const int MaxIter = 10000;

            for (double i = H; i >= -H; i=i-step )
            {
                var res = new Result();
                SSum = 0;
                res.U = i;
             
                PointCL H0cur;
                H0cur.X = i * MagneticVector.X;
                H0cur.Y = i * MagneticVector.Y;
                H0cur.Z = i * MagneticVector.Z;

                double rave;
                int Niter = 0;
                do
                {
                    CountH(H0cur);
                    MakeStep();
                    rave = CountForce();
                    Niter++;
                } while ((rave > MagneticField.EpsR)||(MaxIter!=Niter));


                for (int j = 0; j < Atoms.Count; j++)
                {
                    SSum = SSum + Atoms[j].MagneticVector.X * MagneticVector.X + Atoms[j].MagneticVector.Y * 
                        MagneticVector.Y + Atoms[j].MagneticVector.Z *  MagneticVector.Z;
               
                }
                res.R = SSum / Atoms.Count;            
                
                Result1.Add(res);
                Result2.Add(-res);
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
                    HM.X = HM.X + M11[j, i] * Atoms[j].MagneticVector.X + 
                        M12[j, i] * Atoms[j].MagneticVector.Y + 
                        M13[j, i] * Atoms[j].MagneticVector.Z;
                    HM.Y = HM.Y + M12[j, i] * Atoms[j].MagneticVector.X + 
                        M22[j, i] * Atoms[j].MagneticVector.Y + 
                        M23[j, i] * Atoms[j].MagneticVector.Z;
                    HM.Z = HM.Z + M13[j, i] * Atoms[j].MagneticVector.X + 
                        M23[j, i] * Atoms[j].MagneticVector.Y + 
                        M33[j, i] * Atoms[j].MagneticVector.Z;
                }
            }
            return HM;        
        }
       
        public void CountH(PointCL h0cur)
        {
            double Hxi, Hyi, Hzi;
            double Hxa, Hya, Hza, sp;

            for (int i = 0; i < Atoms.Count; i++)
            {
                //скалярное произведение
                sp = Atoms[i].MagneticVector.X * Atoms[i].NormalVector.X + Atoms[i].MagneticVector.Y * 
                    Atoms[i].NormalVector.Y+Atoms[i].MagneticVector.Z * Atoms[i].NormalVector.Z;
                
                Hxa = Atoms[i].Material.Hk * sp * Atoms[i].NormalVector.X;
                Hya = Atoms[i].Material.Hk * sp * Atoms[i].NormalVector.Y;
                Hza = Atoms[i].Material.Hk * sp * Atoms[i].NormalVector.Z;

                //для одной//CountHdip(i); 
                var HM = CountHdip(i); //для нескольких

                Hxi = -Hd_num * HM.X;
                Hyi = -Hd_num * HM.Y; 
                Hzi = -Hd_num * HM.Z;

                Atoms[i].Hr.X = Hxa + Hxi + h0cur.X;
                Atoms[i].Hr.Y = Hya + Hyi + h0cur.Y;
                Atoms[i].Hr.Z = Hza + Hzi + h0cur.Z;              
            }

        }

        public void MakeStep()
        {
            double H, Hm, vx, vy, vz, dt, at, x, Hx, Hy, Hz;
            Hm = 0;
            for (int i = 0; i < Atoms.Count; i++)
            {
                H = Math.Sqrt(Atoms[i].Hr.X * Atoms[i].Hr.X + Atoms[i].Hr.Y * Atoms[i].Hr.Y + Atoms[i].Hr.Z * Atoms[i].Hr.Z);
                if (H > Hm)
                    Hm = H;
            }
            dt = MagneticField.stabkoeff * Math.PI / (30 * Hm * (1 + MagneticField.Stc) * (1 + MagneticField.kappa * MagneticField.kappa));
            for (int i = 0; i < Atoms.Count; i++)
            {
                PointCL Calc;
                Calc = Atoms[i].MagneticVector;
                Hx = Atoms[i].Hr.X + MagneticField.Stc * Hm * Calc.X;
                Hy = Atoms[i].Hr.Y + MagneticField.Stc * Hm * Calc.Y;
                Hz = Atoms[i].Hr.Z + MagneticField.Stc * Hm * Calc.Z;
                H = Math.Sqrt(Hx * Hx + Hy * Hy + Hz * Hz);

                at = 1 / (1 + Math.Pow(dt * H, 2));
                x = (Hx * Atoms[i].MagneticVector.X + Hy * Atoms[i].MagneticVector.Y + Hz * Atoms[i].MagneticVector.Z) * dt * dt;
                vx = dt * (Hy * Calc.Z - Hz * Calc.Y);
                vy = dt * (Hz * Calc.X - Hx * Calc.Z);
                vz = dt * (Hx * Calc.Y - Hy * Calc.X);

                Calc.X = at * (Calc.X + x * Hx + vx);
                Calc.Y = at * (Calc.Y + x * Hy + vy);
                Calc.Z = at * (Calc.Z + x * Hz + vz);
                at = Math.Sqrt(Calc.X * Calc.X + Calc.Y * Calc.Y + Calc.Z * Calc.Z);

                Atoms[i].MagneticVector.X = Calc.X / at;
                Atoms[i].MagneticVector.Y = Calc.Y / at;
                Atoms[i].MagneticVector.Z = Calc.Z / at;
            }
        }

        public double CountForce()
        {         
            double vx, vy, vz;
            double Hr;
            double x;
            double Rmax = 0;

            double rave = 0;

            for (int i = 0; i < Atoms.Count; i++)
            {
                Hr = Math.Sqrt(Atoms[i].Hr.X * Atoms[i].Hr.X + Atoms[i].Hr.Y * Atoms[i].Hr.Y + Atoms[i].Hr.Z * Atoms[i].Hr.Z);
                vx = Atoms[i].Hr.Y * Atoms[i].MagneticVector.Z - Atoms[i].Hr.Z * Atoms[i].MagneticVector.Y;
                vy = Atoms[i].Hr.Z * Atoms[i].MagneticVector.X - Atoms[i].Hr.X * Atoms[i].MagneticVector.Z;
                vz = Atoms[i].Hr.X * Atoms[i].MagneticVector.Y - Atoms[i].Hr.Y * Atoms[i].MagneticVector.X;
                x = Math.Sqrt(vx * vx + vy * vy + vz * vz) / Hr;
                if (Rmax < x)
                {
                    Rmax = x;
                }
                    rave = rave + Math.Abs(x);
                
            }
            rave = rave / Atoms.Count;

            return rave;
        }       
    }
}
