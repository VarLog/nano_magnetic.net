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
using System.Diagnostics;

using ClusterLib;

namespace Cluster
{
    class Program
    {
        static void Main (string[] args)
        {
            var material = new Material (4000, 800, 20e-7);

            const double radius = 80e-7;


            var magnetic = new Magnetic ();
            magnetic.kappa = 0.2;
            magnetic.Stc = 30;
            magnetic.stabkoeff = 30;
            magnetic.EpsR = 1e-12;

//            {
//                var R = new Random ();
//                var randVector = new Vector (2 * (R.NextDouble () - 0.5), 
//                    2 * (R.NextDouble () - 0.5), 
//                    2 * (R.NextDouble () - 0.5));
//                magnetic.MagneticVector = randVector;
//            }

            magnetic.MagneticVector = new Vector (1) / Math.Sqrt (3);


            //var cluster = new Sphere (radius, magneticVector);
            //cluster.AddAtomList(material, 20);

            var cluster = new Sphere (radius, magnetic);
            cluster.AddDetermList (material);

                
            cluster.calculate (1500, 300);

            cluster.Result.ForEach (r => Debug.WriteLine ("U: " + r.U + " R: " + r.R));
        }
    }
}
