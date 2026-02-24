namespace Default.Namespace
{
    public class WindData
    {
        public float MinAngle
        {
            get
            {
                return minAngle;
            }
        }

        public float MaxAngle
        {
            get
            {
                return maxAngle;
            }
        }

        public float WindOffset
        {
            get
            {
                return windOffset;
            }
            set
            {
                windOffset = value;
            }
        }

        public float Diff
        {
            get
            {
                return diff;
            }
        }

        public WindData(float angle)
        {
            minAngle = Maths.RandRangeMinMax(-angle, 0f);
            maxAngle = Maths.RandRangeMinMax(0f, angle);
            diff = maxAngle - minAngle;
            windOffset = Maths.RandRangeMinMax(-0.7f, 0.7f);
        }

        public float GetAngle(float wind)
        {
            return minAngle + diff * wind;
        }

        protected float minAngle;

        protected float maxAngle;

        protected float windOffset;

        protected float diff;
    }
}
