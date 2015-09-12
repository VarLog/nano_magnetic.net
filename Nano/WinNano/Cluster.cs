using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinNano
{
    class Cluster
    {
        public double R { set; get; }
        public double X { set; get; }
        public double Y { set; get; }
        public double Z { set; get; }
        public double V { set; get; }
        public double ETA { set; get; }   // ETA:=NNp*Volume/VolCl;
        public double Hdip { set; get; } //Hdip:=Ms*ETA; 
        public double Qint { set; get; } //  Qint:=Hdip/Hk;

        public Atoms atoms = new Atoms();

        public Cluster() { }
        public Cluster(Parametr P) 
        {
            this.ETA =  P.ETA;
            this.X = 0;
            this.Y = 0;
            this.Z = 0;
            this.V = P.kolvoatom * 4 * Math.PI * P.radiusatomov * P.radiusatomov * P.radiusatomov / 3/this.ETA;
            double L = P.kolvoatom * P.radiusatomov * P.radiusatomov * P.radiusatomov / this.ETA;
            double p;
            p = 1.0 / 3.0;
            this.R = Math.Pow(L, p);
            Load(P);
            this.atoms.getLenght();
        }

        public void Load(Parametr P)
        {
            Random R=new Random();
            for(int i=0; i<P.kolvoatom; i++)
            {
                Atom A = new Atom(P);
                A.X = (R.NextDouble()-0.5)*2*this.R;
                A.Y = (R.NextDouble()-0.5)*2*this.R;
                A.Z = (R.NextDouble()-0.5)*2*this.R;
                if (atoms.LA.Count == 0)
                {
                    A.randomMagneticVector(R, P);
                    atoms.LA.Add(A);
                }
                else
                {
                    int k = atoms.LA.FindIndex(C => C != A);
                    if (k == 0)
                    {
                        A.randomMagneticVector(R, P);
                        atoms.LA.Add(A);
                    }
                    else
                    {
                        i--;
                    }
                }
               
            }            
        }

       
    }

 

}
