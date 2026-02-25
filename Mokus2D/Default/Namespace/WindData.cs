namespace Mokus2D.Default.Namespace
{
    public class WindData
    {
        public float MinAngle => minAngle;

        public float MaxAngle => maxAngle;

        public float WindOffset
        {
            get => windOffset; set => windOffset = value;
        }

        public float Diff => diff;

        public WindData(float angle)
        {
            minAngle = Maths.RandRangeMinMax(-angle, 0f);
            maxAngle = Maths.RandRangeMinMax(0f, angle);
            diff = maxAngle - minAngle;
            windOffset = Maths.RandRangeMinMax(-0.7f, 0.7f);
        }

        public float GetAngle(float wind)
        {
            return minAngle + (diff * wind);
        }

        protected float minAngle;

        protected float maxAngle;

        protected float windOffset;

        protected float diff;
    }
}
