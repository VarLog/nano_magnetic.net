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
<<<<<<< HEAD
        #region Var

        readonly public Magnetic MagneticField;

        internal double Radius;

        readonly public List<Result> Result = new List<Result> ();

        readonly List<Atom> atoms = new List<Atom> ();

        double Hd;
        
=======
        /// <summary>
        /// The external magnetic field.
        /// </summary>
        readonly public Magnetic MagneticField;

        /// <summary>
        /// The radius of the sphere.
        /// \f$R_{cl}\f$
        /// </summary>
        readonly public double Radius;

>>>>>>> 8a9d90f46e8abcce2dfbba29f4e262324e6c59c9
        /// <summary>
        /// The results.
        /// </summary>
        readonly public List<Result> Result = new List<Result>();

<<<<<<< HEAD
        #endregion

        #region Constructors
        public Sphere (double radius, Magnetic magneticField)
        {
            Radius = radius;
            MagneticField = magneticField;
        }

        #endregion

        #region Logik
        void createRandromAtoms (Material material, int count)
        {
            var R = new Random ();

            for (int i = 0; i < count; i++)
=======
        /// <summary>
        /// Gets or sets the particles.
        /// </summary>
        /// <value>The particles.</value>
        public List<Particle> Particles { 
            get { return particles; }
            set
>>>>>>> 8a9d90f46e8abcce2dfbba29f4e262324e6c59c9
            {
                particles = value; 

                if( particles != null )
                {
                    if( particles.Any() )
                    {
                        ParticlesMaterial = particles.First().Material;

                        if( particles.Any( p => p.Material != ParticlesMaterial ) )
                        {
                            throw new Exception( "All particles must have the same material" );
                        }
                    }

                    particles.ForEach( p => p.MagneticVector = MagneticField.MagneticVector );
                }
            }
        }

<<<<<<< HEAD
        /// <summary>
        /// Создает случайное количество атомов в кластере
        /// </summary>
        /// <param name="material">Material</param>
        /// <param name="count">Количество атомов</param>
        public void GeneretaAtoms (Material material, int count)
        {
            GenerateAtomsList(material, count);
        }

        /// <summary>
        /// Создает случайное количество атомов в кластере по параметру плотности
        /// </summary>
        /// <param name="material">Материал</param>
        /// <param name="ETA">Плотность клайстера</param>
        public void GeneretaAtoms(Material material, int count, double ETA)
        {
            this.Radius = count * material.Radius / ETA;
            GenerateAtomsList(material, count);
        }

        private void GenerateAtomsList(Material material, int count)
        {
            createRandromAtoms(material, count);

            Hd = material.Volume * material.Ms / Math.Pow(Radius, 3);

            var atomsCount = atoms.Count;
            mat11 = new double[atomsCount, atomsCount];
            mat12 = new double[atomsCount, atomsCount];
            mat13 = new double[atomsCount, atomsCount];
            mat22 = new double[atomsCount, atomsCount];
            mat23 = new double[atomsCount, atomsCount];
            mat33 = new double[atomsCount, atomsCount];

            atoms.ForEach(a => a.Position = a.Position / Radius);

            for (int i = 0; i < atoms.Count; i++)
            {
                for (int j = 0; j < atoms.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var atom1 = atoms[i];
                    var atom2 = atoms[j];
=======
        List<Particle> particles = new List<Particle>();

        /// <summary>
        /// Gets or sets the particles material.
        /// </summary>
        /// <value>The particles material.</value>
        Material ParticlesMaterial { get; set; }
>>>>>>> 8a9d90f46e8abcce2dfbba29f4e262324e6c59c9

        /// <summary>
        /// Gets the particules count.
        /// \f$N_{p}\f$
        /// </summary>
        /// <value>The particules count.</value>
        public int ParticlesCount { get { return Particles.Count; } }

<<<<<<< HEAD
                    var distance = diff.mod();
                    var distanceCube = Math.Pow(distance, 3);
=======
        /// <summary>
        /// The particles density.
        /// \f$h=\frac{VN_{p}}{V_{cl}}\f$
        /// </summary>
        /// <value>The particles density.</value>
        public double ParticlesDensity { get; private set; }
>>>>>>> 8a9d90f46e8abcce2dfbba29f4e262324e6c59c9

        /// <summary>
        /// Distanation matrixes.
        /// </summary>
        double[,] mat11, mat12, mat13, mat22, mat23, mat33;

<<<<<<< HEAD
                    mat11[i, j] = (1 - 3 * nR.X * nR.X) / distanceCube;
                    mat12[i, j] = -3 * nR.X * nR.Y / distanceCube;
                    mat13[i, j] = -3 * nR.X * nR.Z / distanceCube;
                    mat22[i, j] = (1 - 3 * nR.Y * nR.Y) / distanceCube;
                    mat23[i, j] = -3 * nR.Y * nR.Z / distanceCube;
                    mat33[i, j] = (1 - 3 * nR.Z * nR.Z) / distanceCube;

                    mat11[j, i] = mat11[i, j];
                    mat12[j, i] = mat12[i, j];
                    mat13[j, i] = mat13[i, j];
                    mat22[j, i] = mat22[i, j];
                    mat23[j, i] = mat23[i, j];
                    mat33[j, i] = mat33[i, j];
                }
            }
        }

        public void AddDetermAtoms(Material material)
        {
            {
                var atom1 = new Atom(material);
                atom1.Position = new Vector(0.5, 0.25, 0.25);
                atom1.NormalVector = new Vector(1 / Math.Sqrt(2), 1 / Math.Sqrt(2), 0);
                atom1.MagneticVector = new Vector(1) / Math.Sqrt(3);
                atoms.Add(atom1);

                var atom2 = new Atom(material);
                atom2.Position = new Vector(-0.25, 0.5, 0.25);
                atom2.NormalVector = new Vector(0, 0, 1);
                atom2.MagneticVector = new Vector(1) / Math.Sqrt(3);
                atoms.Add(atom2);

                var atom3 = new Atom(material);
                atom3.Position = new Vector(-0.5, -0.25, 0.25);
                atom3.NormalVector = new Vector(1) / Math.Sqrt(3);
                atom3.MagneticVector = new Vector(1) / Math.Sqrt(3);
                atoms.Add(atom3);

                var atom4 = new Atom(material);
                atom4.Position = new Vector(-0.25, -0.25, -0.25);
                atom4.NormalVector = new Vector(0, 0, 1);
                atom4.MagneticVector = new Vector(1) / Math.Sqrt(3);
                atoms.Add(atom4);
            }   
            Hd = material.Volume * material.Ms / Math.Pow (Radius, 3);
=======
        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterLib.Sphere"/> class.
        /// </summary>
        /// <param name="radius">Radius.</param>
        /// <param name="magneticField">Magnetic field.</param>
        public Sphere( double radius, Magnetic magneticField )
        {
            Radius = radius;
            MagneticField = magneticField;
        }

        /// <summary>
        /// Calculates the distinations between particles.
        /// </summary>
        void calculateDistinations()
        {
            // TODO: Old implementation. It sould be discussed. 
            ParticlesDensity = ParticlesMaterial.Volume * ParticlesMaterial.MagneticSaturation / Math.Pow( Radius, 3 );

            //var volume = (4.0 / 3.0) * Math.PI * Math.Pow (Radius, 3);
            //ParticlesDensity = ParticlesMaterial.Volume * ParticlesCount / volume;
>>>>>>> 8a9d90f46e8abcce2dfbba29f4e262324e6c59c9

            Console.WriteLine( "Particles density h == " + ParticlesDensity );

            // TODO: Improve mechanism to store and to calculate destinations
            mat11 = new double[ParticlesCount, ParticlesCount];
            mat12 = new double[ParticlesCount, ParticlesCount];
            mat13 = new double[ParticlesCount, ParticlesCount];
            mat22 = new double[ParticlesCount, ParticlesCount];
            mat23 = new double[ParticlesCount, ParticlesCount];
            mat33 = new double[ParticlesCount, ParticlesCount];

            for( int i = 0; i < ParticlesCount; i++ )
            {
                for( int j = 0; j < ParticlesCount; j++ )
                {
                    if( i == j )
                    {
                        continue;
                    }

                    var p1 = Particles[ i ];
                    var p2 = Particles[ j ];

                    var diff = p1.Position - p2.Position;

                    var distance = diff.mod();
                    var distanceCube = Math.Pow( distance, 3 );

                    var nR = diff / distance;

                    mat11[ i, j ] = ( 1 - 3 * nR.X * nR.X ) / distanceCube;
                    mat12[ i, j ] = -3 * nR.X * nR.Y / distanceCube;
                    mat13[ i, j ] = -3 * nR.X * nR.Z / distanceCube;
                    mat22[ i, j ] = ( 1 - 3 * nR.Y * nR.Y ) / distanceCube;
                    mat23[ i, j ] = -3 * nR.Y * nR.Z / distanceCube;
                    mat33[ i, j ] = ( 1 - 3 * nR.Z * nR.Z ) / distanceCube;

                    mat11[ j, i ] = mat11[ i, j ];
                    mat12[ j, i ] = mat12[ i, j ];
                    mat13[ j, i ] = mat13[ i, j ];
                    mat22[ j, i ] = mat22[ i, j ];
                    mat23[ j, i ] = mat23[ i, j ];
                    mat33[ j, i ] = mat33[ i, j ];
                }
            }
        }

        /// <summary>
        /// Calculate the Landau–Lifshitz–Gilbert equation.
        /// </summary>
        /// <param name="magneticIntensityRange">Magnetic intensity range.</param>
        /// <param name="step">Step.</param>
        public void calculate( IEnumerable<int> magneticIntensityRange, double step )
        {
            if( Particles == null || !Particles.Any() )
            {
<<<<<<< HEAD
                double sum = 0;

                var H0cur = MagneticField.MagneticVector * i;

                double rave;
                while (true)
                {
                    {   //CountH
                        var atom = atoms[0];
                        var sp = atom.MagneticVector.dot(atom.NormalVector);
                        var Ha = atom.NormalVector * sp * atom.Material.Hk;
                        atom.Hr = Ha + H0cur; 
                    }

                    MakeStep ();
                    rave = CountForce ();

                    if (rave <= MagneticField.EpsR)
                    {
                        break;
                    }
                }

                atoms.ForEach (a => {
                    var v = a.MagneticVector * MagneticField.MagneticVector;
                    sum += v.X + v.Y + v.Z;
                });
=======
                throw new Exception( "There are not any particles to calculate" );
            }
>>>>>>> 8a9d90f46e8abcce2dfbba29f4e262324e6c59c9

            calculateDistinations();

<<<<<<< HEAD
        /// <summary>
        /// решения уравнения Ландау-Лифшеца-Гильберта
        /// </summary>
        /// <param name="H">Максимальное магнитное поле</param>
        /// <param name="step">Шаг по полю</param>
        public void calculate (double H, double step)
        {
=======
>>>>>>> 8a9d90f46e8abcce2dfbba29f4e262324e6c59c9
            const int maxIterCount = 10000;

            var array = magneticIntensityRange.ToArray();
            var min = array.ElementAt( 0 );
            var max = array.ElementAt( 1 );

            for( double i = max; i >= min; i = i - step )
            {
                double sum = 0;
             
                var H0cur = MagneticField.MagneticVector * i;

                double rave;
                int iterCount = 0;

                while( true )
                {
                    CountH( H0cur );
                    MakeStep();
                    rave = CountForce();

                    if( ++iterCount >= maxIterCount )
                    {
                        break;
                    }

                    if( rave <= MagneticField.EpsR )
                    {
                        break;
                    }
                }

                Particles.ForEach( a => {
                    var v = a.MagneticVector * MagneticField.MagneticVector;
                    sum += v.X + v.Y + v.Z;
                } );

                Result.Add( new Result( i, sum / Particles.Count ) );
            }           
        }       


        public Vector CountHdip( int j )
        {
            var HM = new Vector();

            for( int i = 0; i < Particles.Count; i++ )
            {
                if( i != j )
                {
                    var particle = Particles[ i ];
                    var magneticVector = particle.MagneticVector;

                    HM.X = HM.X + mat11[ j, i ] * magneticVector.X +
                    mat12[ j, i ] * magneticVector.Y +
                    mat13[ j, i ] * magneticVector.Z;
                    
                    HM.Y = HM.Y + mat12[ j, i ] * magneticVector.X +
                    mat22[ j, i ] * magneticVector.Y +
                    mat23[ j, i ] * magneticVector.Z;
                    
                    HM.Z = HM.Z + mat13[ j, i ] * magneticVector.X +
                    mat23[ j, i ] * magneticVector.Y +
                    mat33[ j, i ] * magneticVector.Z;
                }
            }
            return HM;        
        }

        public void CountH( Vector h0cur )
        {
            for( int i = 0; i < Particles.Count; i++ )
            {
                var particle = Particles[ i ];

                var sp = particle.MagneticVector.dot( particle.NormalVector );

                var Ha = particle.NormalVector * sp * particle.Material.MagneticDamping;

                var HM = CountHdip( i );

                var Hi = HM * -ParticlesDensity;

                particle.Hr = Ha + Hi + h0cur;
            }
        }

        public void MakeStep()
        {
            var Hrs = Particles.ConvertAll( p => p.Hr.mod() );
            double maxHr = Hrs.Max();

            var dt = MagneticField.stabkoeff * Math.PI /
                     ( 30 * maxHr * ( 1 + MagneticField.Stc ) * ( 1 + MagneticField.kappa * MagneticField.kappa ) );

            foreach( var particle in Particles )
            {
                Vector Calc;
                Calc = particle.MagneticVector;

                var H = particle.Hr + Calc * maxHr * MagneticField.Stc;

                var at = 1 / ( 1 + Math.Pow( dt * H.mod(), 2 ) );

                var x = H.dot( particle.MagneticVector ) * dt * dt;

                var v = new Vector();
                v.X = dt * ( H.Y * Calc.Z - H.Z * Calc.Y );
                v.Y = dt * ( H.Z * Calc.X - H.X * Calc.Z );
                v.Z = dt * ( H.X * Calc.Y - H.Y * Calc.X );

                Calc = ( Calc + H * x + v ) * at;

                particle.MagneticVector = Calc / Calc.mod();
            }
        }

        public double CountForce()
        {         
            double Rmax = 0;

            double rave = 0;

            foreach( var particle in Particles )
            {
                var v = new Vector();
                v.X = particle.Hr.Y * particle.MagneticVector.Z - particle.Hr.Z * particle.MagneticVector.Y;
                v.Y = particle.Hr.Z * particle.MagneticVector.X - particle.Hr.X * particle.MagneticVector.Z;
                v.Z = particle.Hr.X * particle.MagneticVector.Y - particle.Hr.Y * particle.MagneticVector.X;

                var x = v.mod() / particle.Hr.mod();
                if( Rmax < x )
                {
                    Rmax = x;
                }

                rave += Math.Abs( x );
            }
            rave = rave / ParticlesCount;

            return rave;
        }
        #endregion
     
    }
}
