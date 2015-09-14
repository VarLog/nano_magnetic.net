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
    public class Material
    {
        /// <summary>
        /// Magnetic anisotropy is the directional dependence of a material's magnetic properties. 
        /// The magnetic moment of magnetically anisotropic materials will tend to align with an "easy axis", 
        /// which is an energetically favorable direction of spontaneous magnetization.
        /// \f$M_a\f$
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Magnetic_anisotropy"/>
        /// <value>The magnetic anisotropy.</value>
        public double MagneticAnisotropy { get; }

        /// <summary>
        /// Seen in some magnetic materials, saturation is the state reached when an increase in applied external 
        /// magnetic field H cannot increase the magnetization of the material further, so the total magnetic flux 
        /// density B more or less levels off. (It continues to increase very slowly due to the vacuum permeability.) 
        /// Saturation is a characteristic of ferromagnetic and ferrimagnetic materials, such as 
        /// iron, nickel, cobalt and their alloys.
        /// \f$M_s\f$
        /// </summary>
        /// <seealso href="https://en.wikipedia.org/wiki/Saturation_(magnetic)"/>
        /// <value>The magnetic saturation.</value>
        public double MagneticSaturation { get; }

        /// <summary>
        /// Magnetic damping is a form of damping that occurs when a magnetic field moves through a conductor 
        /// (or vice versa).
        /// \f$H_d\f$
        /// </summary>
        /// <value>The magnetic damping.</value>
        public double MagneticDamping { get; }

        /// <summary>
        /// The radius of the particle of this material.
        /// \f$R\f$
        /// </summary>
        /// <value>The radius.</value>
        public double Radius { get; }

        /// <summary>
        /// The volume of the particle of this material.
        /// \f$V\f$
        /// </summary>
        /// <value>The volume.</value>
        public double Volume { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusterLib.Material"/> class.
        /// </summary>
        /// <param name="anisotropy">Magnetic anisotropy. <see cref="MagneticAnisotropy"/></param>
        /// <param name="saturation">Magnetic saturation. <see cref="MagneticSaturation"/></param>
        /// <param name="radius">Radius. <see cref="Radius"/></param>
        public Material (double anisotropy, double saturation, double radius)
        {
            MagneticAnisotropy = anisotropy;
            MagneticSaturation = saturation;

            MagneticDamping = 2 * MagneticAnisotropy / MagneticSaturation;

            Radius = radius;
            Volume = (4.0 / 3.0) * Math.PI * Math.Pow (Radius, 3);
        }
    }
}
