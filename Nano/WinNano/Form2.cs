using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinNano
{
    
    public partial class Form2 : Form
    {
        List<Cluster> LC = new List<Cluster>();
        List<Parametr> LP=new List<Parametr>();
        Atoms atoms = new Atoms();
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Open the file to read from.

            List<Parametr> _LP = new List<Parametr>();
            #region Считывание с файла
            string path=@"C:\Users\roman_000\Desktop\ПРГ\Nano\WinNano\Params.txt";
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string stroka = s;
                    Parametr param = new Parametr();                   
                    Regex R = new Regex(";");
                    string[] k = R.Split(stroka);
                    param.ETA = Convert.ToDouble(k[1]);
                    param.kolvoatom = Convert.ToDouble(k[2]);
                    param.radiusatomov = Convert.ToDouble(k[3]);
                    param.kolvopovtorenii = Convert.ToDouble(k[4]);
                    _LP.Add(param);
                }
            }
            #endregion
            LP = _LP;
           
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int k = Convert.ToInt16(textBox1.Text);
            Cluster cluster = new Cluster(LP[k]);
            LC.Add(cluster);
            dataGridView2.Tag = LC;
            dataGridView2.DataSource = LC;
            dataGridView1.DataSource = cluster.atoms.LA;
        }

        public void CountHdip()
        {
            double HMx = 0, HMy = 0, HMz = 0;
            for(int i=0; i<LC[0].atoms.LA.Count; i++)
            {
               // HMx=HMx+LC[0].atoms.L[j,i]*LC[0].atoms.LA[i].Mx
               // HMy=HMy+
                //HMz=HMz+
            }
        }

        public void CountH(int k)
        {
            double sp;
            for(int i=0; i<LC[k].atoms.LA.Count; i++)
            {
                //sp=LC[k].atoms.LA[i].Mx*
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = @"MyTest.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Hello");
                    sw.WriteLine("And");
                    sw.WriteLine("Welcome");
                }
            }
        }
    }
}
