
namespace ClusterLib
{
    public struct Result
    {
        public double U { get; set; }

        public double R { get; set; }

        public Result (double u, double r)
        {
            U = u;
            R = r;
        }

        public static Result operator - (Result that)
        {
            return new Result (-that.R, -that.U);            
        }
    }
}
