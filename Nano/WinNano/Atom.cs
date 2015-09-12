using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinNano
{
    public class Atoms
    {
        public List<Atom> LA = new List<Atom>();
        public double[,] L;

        public Atoms() { }
        public void getLenght()
        {
            L = new double[LA.Count, LA.Count];
            for(int i=0; i<LA.Count; i++)
            {
                for(int j=0; j<LA.Count; j++)
                {
                    this.L[i, j] = this.LA[i] - this.LA[j];
                }
            }

            
        }
    }
    public class Atom
    {
        public double R { set; get; }
        public double X { set; get; }
        public double Y { set; get; }
        public double Z { set; get; }
        public double V { set; get; }

        public double Mx { set; get; }
        public double My { set; get; }
        public double Mz { set; get; }


       
        public Atom() { }
        public Atom(Parametr P)
        {
            this.R = P.radiusatomov;
            this.V = this.R *this.R*this.R* 4 * Math.PI / 3;
        }
        public void randomMagneticVector(Random R, Parametr P)
        {
            double norma = 0;            
            this.Mx = (R.NextDouble() - 0.5);
            this.My = (R.NextDouble() - 0.5);
            this.Mz = (R.NextDouble() - 0.5);
            norma = Math.Sqrt(this.Mx * this.Mx + this.My * this.My + this.Mz * this.Mz);
            this.Mx = this.Mx / norma;
            this.Mx = this.My / norma;
            this.Mx = this.Mz / norma;           
        }

        public static bool operator ==(Atom A1, Atom A2)
        {
            if (Math.Sqrt((A1.X - A2.X) * (A1.X - A2.X) + (A1.Y - A2.Y) * (A1.Y - A2.Y) + (A1.Z - A2.Z) * (A1.Z - A2.Z)) < 2 * A1.R) return true;
            else return false;
        }
        public static bool operator !=(Atom A1, Atom A2)
        {
            if (Math.Sqrt((A1.X - A2.X) * (A1.X - A2.X) + (A1.Y - A2.Y) * (A1.Y - A2.Y) + (A1.Z - A2.Z) * (A1.Z - A2.Z)) >= 2 * A1.R) return false;
            else return true;
        }

        public static double operator -(Atom A1, Atom A2)
        {
            double L = (A1.X - A2.X) * (A1.X - A2.X) + (A1.Y - A2.Y) * (A1.Y - A2.Y) + (A1.Z - A2.Z) * (A1.Z - A2.Z);
            if (L != 0) return Math.Sqrt(L);
            else return 0;            
        }
    }
   
}
