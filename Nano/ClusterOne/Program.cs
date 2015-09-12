using ClusterLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClusterOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите параметры эта, радиус шара, радиус частицы, материал частицы MS");

            //Logik      

            Material material = new Material(100000, 500, 20e-7);
            Atom atom = new Atom(material);

            Sphere cluster = new Sphere();
            //cluster.AddAtomList(atom, 20, 0.25, 20);

            cluster.AddDetermListOne(atom, 20, 0.25, 20);


            cluster.MagneticField.kappa = 0.2;
            cluster.MagneticField.Stc = 30;
            cluster.MagneticField.stabkoeff = 30;
            cluster.MagneticField.EpsR = 1e-8;
            

            cluster.calculateOne(1500, 300);
            //Вывод файл
            StreamWriter SW = new StreamWriter("result.txt");
            for (int i = 0; i < cluster.Result1.Count; i++)
            {
                SW.WriteLine(cluster.Result1[i].U + "   " + cluster.Result1[i].R);
            }
            for (int i = 0; i < cluster.Result2.Count; i++)
            {
                SW.WriteLine(cluster.Result2[i].U + "   " + cluster.Result2[i].R);
            }
            SW.Close();
        }
    }
}
