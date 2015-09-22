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

namespace ClusterLib
{
    public class Particle
    {
        /// <summary>
        /// Gets or sets the radius vector in the cluster.
        /// \f$r\f$
        /// </summary>
        /// <value>The radius vector in the cluster.</value>
        public Vector RadiusVector { get; set; }

        /// <summary>
        /// Gets or sets the unit magnetic vector.
        /// \f$a\f$
        /// </summary>
        /// <value>The unit magnetic vector.</value>
        public Vector MagneticVector { get; set; }

        /// <summary>
        /// Gets or sets the easy anisotropy axis.
        /// \f$n\f$
        /// </summary>
        /// <value>The easy anisotropy axis.</value>
        public Vector EasyAnisotropyAxis { get; set; }

        /// <summary>
        /// Gets the material of the particle.
        /// </summary>
        /// <value>The material of the particle.</value>
        public Material Material { get; private set; }

        /// <summary>
        /// Gets or sets the effective magnetic field vector.
        /// \f$H_{ef}\f$
        /// </summary>
        /// <value>The effective magnetic field vector.</value>
        public Vector EffectiveMagneticField { get; set; }

        public Particle( Material material )
        {
            Material = material;
        
            var rand = new Random();

            var randVector = new Vector( ( rand.NextDouble() - 0.5 ),
                                 ( rand.NextDouble() - 0.5 ),
                                 ( rand.NextDouble() - 0.5 ) );

            EasyAnisotropyAxis = randVector;
            EasyAnisotropyAxis = EasyAnisotropyAxis.Norm();
        }

        public bool isIntersected( Particle that )
        {
            var diff = RadiusVector - that.RadiusVector;
            var distance = diff.Mod();
            return distance < ( Material.Radius + that.Material.Radius );
        }
    }
}
