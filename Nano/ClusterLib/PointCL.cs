
namespace ClusterLib
{
    public struct PointCL 
    {
        public double X { set; get; }
        public double Y { set; get; }
        public double Z { set; get; }

        public PointCL()
        {
            X = default(double);
            Y = default(double);
            Z = default(double);
        }

        public PointCL(double _X, double _Y, double _Z)
        {
            X = _X;
            Y = _Y;
            Z = _Z;
        }
    }
}
