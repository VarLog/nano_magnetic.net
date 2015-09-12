using System;
using System.Collections.Generic;

namespace ClusterLib
{
    public class Sphere
    {
        public List<Atom> Atoms = new List<Atom>();
       
        public Magnetic MagneticField = new Magnetic();

        public double Radius;

        //public double Hk;
        //public double Hdip; //Ms*etaVCl
        public double Qint;     //Hdip/Hk;

        public PointCL MagneticVector = new PointCL();

        public List<Result> Result1 = new List<Result>();
        public List<Result> Result2 = new List<Result>();

        public double Hd_num;   //Hd_num = Ms*Volume/RCl3;
        public double omH0Cur; 
        public double psiH0Cur;

        public double nhx; 
        public double nhy; 
        public double nhz;

        public double H0xcur, H0ycur, H0zcur;

        public double Volume;
        
        public double etaVcl;
        //Матрицы
        public double[,] M11, M12, M13, M21, M22, M23, M31, M32, M33;
        
        public double r;

        public double Rave; 

        public double koef;

        public double Sum;
        public Sphere()
        {
            var R = new Random();
            MagneticVector.X = 2 * (R.NextDouble() - 0.5);
            MagneticVector.Y = 2 * (R.NextDouble() - 0.5);
            MagneticVector.Z = 2 * (R.NextDouble() - 0.5);

            omH0Cur = Math.PI * R.NextDouble() / 2;
            psiH0Cur = Math.PI * R.NextDouble();
        }

        public Sphere(double _Radius)
        {
            Radius = _Radius;
            var R = new Random();
            MagneticVector.X = 2 * (R.NextDouble() - 0.5);
            MagneticVector.Y = 2 * (R.NextDouble() - 0.5);
            MagneticVector.Z = 2 * (R.NextDouble() - 0.5);
            Hd_num = Atoms [0].material.Volume * Atoms [0].material.Ms / Radius;
        }
        
        public void AddAtomList(Material material, long Num)
        {
            var R = new Random();
            Radius = 80e-7;
            koef = 8e-6;         

            for (int i = 0; i < Num; i++)
            {
                var atom = new Atom();
                atom.material = material;

                atom.Position.X = (R.NextDouble() - 0.5)  * Radius;
                atom.Position.Y = (R.NextDouble() - 0.5)  * Radius;
                atom.Position.Z = (R.NextDouble() - 0.5)  * Radius;

                if (Atoms.Count == 0)
                {
                    Atoms.Add(atom);
                    atom.GenNormalVector(R);
                    atom.MagneticVector = MagneticVector;
                }
                else
                {
                    int k = Atoms.FindIndex(C => C != atom);
                    if (k == 0)
                    {
                        Atoms.Add(atom);
                        atom.GenNormalVector(R);
                        atom.MagneticVector = MagneticVector;
                    }
                    else
                    {
                        i--;
                    }
                }
            }

            Hd_num = Atoms[0].material.Volume * Atoms[0].material.Ms / Math.Pow(Radius, 3);
            double nRx, nRy, nRz;
            double Rd, Rd2=0, Rd3;
            Volume = 4 * Math.PI * Radius *Radius*Radius / 3;
            etaVcl = Atoms.Count * Math.Pow(Atoms[0].material.Radius / Radius, 3);

            var Hdip = Atoms[0].material.Ms * etaVcl;
            var Hk = 2 * Atoms[0].material.K1 / Atoms[0].material.Ms;
            Qint = Hdip / Hk;

            M11 = new double[Atoms.Count, Atoms.Count];
            M12 = new double[Atoms.Count, Atoms.Count];
            M13 = new double[Atoms.Count, Atoms.Count];
            M21 = new double[Atoms.Count, Atoms.Count];
            M22 = new double[Atoms.Count, Atoms.Count];
            M23 = new double[Atoms.Count, Atoms.Count];
            M31 = new double[Atoms.Count, Atoms.Count];
            M32 = new double[Atoms.Count, Atoms.Count];
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

        public void AddDetermList(Material material)
        {
            Radius = 80e-7;

            MagneticVector.X = 1 / Math.Sqrt(3);
            MagneticVector.Y = 1 / Math.Sqrt(3);
            MagneticVector.Z = 1 / Math.Sqrt(3);

            var atom1 = new Atom();
            var atom2 = new Atom();
            var atom3 = new Atom();
            var atom4 = new Atom();

            var R = new Random();          

            atom1.material = material;
            atom2.material = material;
            atom3.material = material;
            atom4.material = material;

            atom1.Position.X = 0.5;
            atom1.Position.Y = 0.25;
            atom1.Position.Z = 0.25;
            atom1.Radius = 20e-7;
            atom1.material.Volume = Volume = 4 * Math.PI * atom1.Radius * atom1.Radius * atom1.Radius / 3;
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
            atom2.Radius = 20e-7;
            atom2.material.Volume = Volume = 4 * Math.PI * atom1.Radius * atom1.Radius * atom1.Radius / 3;
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
            atom3.Radius = 20e-7;
            atom3.material.Volume = Volume = 4 * Math.PI * atom1.Radius * atom1.Radius * atom1.Radius / 3;
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
            atom4.Radius = 20e-7;
            atom4.material.Volume = Volume = 4 * Math.PI * atom1.Radius * atom1.Radius * atom1.Radius / 3;
            atom4.NormalVector.X = 0;
            atom4.NormalVector.Y = 0;
            atom4.NormalVector.Z = 1;
          

            atom4.MagneticVector.X = 1 / Math.Sqrt(3);
            atom4.MagneticVector.Y = 1 / Math.Sqrt(3);
            atom4.MagneticVector.Z = 1 / Math.Sqrt(3);
            Atoms.Add(atom4);            
        

            Hd_num = Atoms[0].material.Volume * Atoms[0].material.Ms / Math.Pow(Radius, 3);
            koef = 8e-6;
            
            double nRx, nRy, nRz;
            double Rd, Rd2 = 0, Rd3;
            Volume = 4 * Math.PI * Radius * Radius * Radius / 3;
            etaVcl = Atoms.Count * Math.Pow(Atoms[0].material.Radius / Radius, 3);
            Hdip = Atoms[0].material.Ms * etaVcl;

            Hk = 2 * Atoms[0].material.K1 / Atoms[0].material.Ms;
            Qint = Hdip / Hk;

            M11 = new double[Atoms.Count, Atoms.Count];
            M12 = new double[Atoms.Count, Atoms.Count];
            M13 = new double[Atoms.Count, Atoms.Count];
            M21 = new double[Atoms.Count, Atoms.Count];
            M22 = new double[Atoms.Count, Atoms.Count];
            M23 = new double[Atoms.Count, Atoms.Count];
            M31 = new double[Atoms.Count, Atoms.Count];
            M32 = new double[Atoms.Count, Atoms.Count];
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
            Radius = 80e-7;

            MagneticVector.X = 1 / Math.Sqrt(3);
            MagneticVector.Y = 1 / Math.Sqrt(3);
            MagneticVector.Z = 1 / Math.Sqrt(3);

            var atom1 = new Atom();         

            atom1.material = material;            

            atom1.Position.X = 0.5;
            atom1.Position.Y = 0.25;
            atom1.Position.Z = 0.25;
            atom1.Radius = 20e-7;
            atom1.material.Volume = Volume = 4 * Math.PI * atom1.Radius * atom1.Radius * atom1.Radius / 3;
            atom1.NormalVector.X = 1;
            atom1.NormalVector.Y = 0;
            atom1.NormalVector.Z = 0;            

            atom1.MagneticVector.X = 1 / Math.Sqrt(3);
            atom1.MagneticVector.Y = 1 / Math.Sqrt(3);
            atom1.MagneticVector.Z = 1 / Math.Sqrt(3);
            Atoms.Add(atom1);
        }

        public void calculateOne(double H, double step)
        {
            double SSum;

            for (double i = H; i >= -H; i = i - step)
            {
                var res = new Result();
                SSum = 0;
                res.U = i;

                H0xcur = i * MagneticVector.X;
                H0ycur = i * MagneticVector.Y;
                H0zcur = i * MagneticVector.Z;

                int Niter = 0;
                do
                {
                    CountH();
                    MakeStep();
                    CountForce();
                    Niter++;
                } while ((Rave > MagneticField.EpsR));


                for (int j = 0; j < Atoms.Count; j++)
                {
                    SSum = SSum + Atoms[j].MagneticVector.X * MagneticVector.X + Atoms[j].MagneticVector.Y * 
                        MagneticVector.Y + Atoms[j].MagneticVector.Z * MagneticVector.Z;

                }
                res.R = SSum / Atoms.Count;

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
             
                H0xcur = i * MagneticVector.X;
                H0ycur = i * MagneticVector.Y;
                H0zcur = i * MagneticVector.Z;

                int Niter = 0;
                do
                {
                    CountH();
                    MakeStep();
                    CountForce();
                    Niter++;
                } while ((Rave > MagneticField.EpsR)||(MaxIter!=Niter));


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
        public void CountHdip(int j)
        {
            double HMx=0, HMy=0, HMz=0;
            for (int i = 0; i < Atoms.Count; i++)
            {
                if (i != j)
                {
                    HMx = HMx + M11[j, i] * Atoms[j].MagneticVector.X + M12[j, i] * Atoms[j].MagneticVector.Y + 
                        M13[j, i] * Atoms[j].MagneticVector.Z;
                    HMy = HMy + M12[j, i] * Atoms[j].MagneticVector.X + M22[j, i] * Atoms[j].MagneticVector.Y + 
                        M23[j, i] * Atoms[j].MagneticVector.Z;
                    HMz = HMz + M13[j, i] * Atoms[j].MagneticVector.X + M23[j, i] * Atoms[j].MagneticVector.Y + 
                        M33[j, i] * Atoms[j].MagneticVector.Z;
                }
            }
            Atoms[j].HDman.X = HMx;
            Atoms[j].HDman.Y = HMy;
            Atoms[j].HDman.Z = HMz;           
        }
            //
       
        public void CountH()
        {
           
            double Hxi, Hyi, Hzi;
            double Hxa, Hya, Hza, sp;

            for (int i = 0; i < Atoms.Count; i++)
            {
                //скалярное произведение
                sp = Atoms[i].MagneticVector.X * Atoms[i].NormalVector.X + Atoms[i].MagneticVector.Y * 
                    Atoms[i].NormalVector.Y+Atoms[i].MagneticVector.Z * Atoms[i].NormalVector.Z;
                
                Hxa = Atoms[i].material.Hk * sp * Atoms[i].NormalVector.X;
                Hya = Atoms[i].material.Hk * sp * Atoms[i].NormalVector.Y;
                Hza = Atoms[i].material.Hk * sp * Atoms[i].NormalVector.Z;

                //для одной//CountHdip(i); 
                CountHdip(i); //для нескольких
                Hxi = -Hd_num * Atoms[i].HDman.X;
                Hyi = -Hd_num * Atoms[i].HDman.Y; 
                Hzi = -Hd_num * Atoms[i].HDman.Z;

                Atoms[i].Hrx = Hxa + Hxi + H0xcur;
                Atoms[i].Hry = Hya + Hyi + H0ycur;
                Atoms[i].Hrz = Hza + Hzi + H0zcur;              
            }

        }

        public void MakeStep()
        {
            double H, Hm, vx, vy, vz, dt, at, x, Hx, Hy, Hz;
            Hm = 0;
            for (int i = 0; i < Atoms.Count; i++)
            {
                H = Math.Sqrt(Atoms[i].Hrx * Atoms[i].Hrx + Atoms[i].Hry * Atoms[i].Hry + Atoms[i].Hrz * Atoms[i].Hrz);
                if (H > Hm)
                    Hm = H;
            }
            dt = MagneticField.stabkoeff * Math.PI / (30 * Hm * (1 + MagneticField.Stc) * (1 + MagneticField.kappa * MagneticField.kappa));
            for (int i = 0; i < Atoms.Count; i++)
            {
                PointCL Calc;
                Calc = Atoms[i].MagneticVector;
                Hx = Atoms[i].Hrx + MagneticField.Stc * Hm * Calc.X;
                Hy = Atoms[i].Hry + MagneticField.Stc * Hm * Calc.Y;
                Hz = Atoms[i].Hrz + MagneticField.Stc * Hm * Calc.Z;
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

        public void CountForce()
        {         
            double vx, vy, vz;
            double Hr;
            double x;
            double Rmax = 0;
            Rave = 0;
            for (int i = 0; i < Atoms.Count; i++)
            {
                Hr = Math.Sqrt(Atoms[i].Hrx * Atoms[i].Hrx + Atoms[i].Hry * Atoms[i].Hry + Atoms[i].Hrz * Atoms[i].Hrz);
                vx = Atoms[i].Hry * Atoms[i].MagneticVector.Z - Atoms[i].Hrz * Atoms[i].MagneticVector.Y;
                vy = Atoms[i].Hrz * Atoms[i].MagneticVector.X - Atoms[i].Hrx * Atoms[i].MagneticVector.Z;
                vz = Atoms[i].Hrx * Atoms[i].MagneticVector.Y - Atoms[i].Hry * Atoms[i].MagneticVector.X;
                x = Math.Sqrt(vx * vx + vy * vy + vz * vz) / Hr;
                if (Rmax < x)
                {
                    Rmax = x;
                }
                    Rave = Rave + Math.Abs(x);
                
            }
            Rave = Rave / Atoms.Count;
        }       
    }
}
