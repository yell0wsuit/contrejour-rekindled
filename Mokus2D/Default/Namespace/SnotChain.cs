namespace Default.Namespace
{
    public class SnotChain(SnotBodyClip _snot, float _distance)
    {
        public SnotBodyClip Snot
        {
            get
            {
                return snot;
            }
        }

        public float Distance
        {
            get
            {
                return distance;
            }
        }

        public float Diff
        {
            get
            {
                return diff;
            }
            set
            {
                diff = value;
            }
        }

        public static object CreateWithSnotDistance(SnotBodyClip _snot, float _distance)
        {
            return new SnotChain(_snot, _distance);
        }

        protected SnotBodyClip snot = _snot;

        protected float distance = _distance;

        protected float diff;
    }
}
