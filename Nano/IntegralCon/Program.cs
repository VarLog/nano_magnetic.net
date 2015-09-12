using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPI;
using MPIUtils;
namespace IntegralCon
{
    class Program
    {
        static void Main(string[] args)
        {

            double R=0;

            using (new MPI.Environment(ref args))
            {
                Intracommunicator world = Communicator.world;
                Random random = new Random(5 * world.Rank);
                int dartsInCircle = 0;
                for (long i = 0; i < dartsPerProcessor; ++i)
                {
                    double x = (random.NextDouble() - 0.5) * 2;
                    double y = (random.NextDouble() - 0.5) * 2;
                    if (x * x + y * y <= 1.0)
                        ++dartsInCircle;
                }

                if (world.Rank == 0)
                {
                    int totalDartsInCircle = world.Reduce<int>(dartsInCircle, Operation<int>.Add, 0);
                    System.Console.WriteLine("Pi is approximately {0:F15}.",
                        4 * (double)totalDartsInCircle / (world.Size * (double)dartsPerProcessor));
                }
                else
                {
                    double r = Integrate(0, 1, 1, 0.0000001);
                    world.Reduce<int>(dartsInCircle, Operation<int>.Add, 0);
                }
            }


            Console.WriteLine(R);


 

        }

        double f(double x)
        {
            // you should specify your own function here
            return x * x * Math.Sin(x);
        }
        double Integrate(double x1, double x2, double step, double precision)
        {
            double x = x1;
            double I1 = 0, I2 = 0; // integral value
            double s;
            do
            {
                I1 = I2;
                s = (f(x1) - f(x2)) / 2;
                x = x1 + step;
                while (x < x2)
                {
                    s += 2 * f(x) + f(x + step);
                    x += 2 * step;
                }
                I2 = 2 * step * s / 3;
                step /= 2.0;  // try once more using less step
            } while (Math.Abs(I1 - I2) > precision);
            return I2;
        }
    }
}
