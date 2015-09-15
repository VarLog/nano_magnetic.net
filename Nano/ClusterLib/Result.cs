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

namespace ClusterLib
{
    public struct Result
    {
        public double U { get; set; }

        public double R { get; set; }

<<<<<<< HEAD
        public Result(double u, double r)
            : this()
=======
        public Result( double u, double r )
>>>>>>> 8a9d90f46e8abcce2dfbba29f4e262324e6c59c9
        {
            U = u;
            R = r;
        }

        public static Result operator -( Result that )
        {
            return new Result( -that.R, -that.U );            
        }
    }
}
