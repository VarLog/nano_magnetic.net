
namespace ClusterLib
{
    public class Result
    {
        public double U;
        public double R;

        public static Result operator -(Result R)
        {
            var RN = new Result();
            RN.R = -R.R;
            RN.U = -R.U;
            return RN;            
        }
    }
}
