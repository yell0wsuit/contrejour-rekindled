namespace Default.Namespace
{
    public class Range
    {
        public Range(float value, float randomRange)
        {
            Value = value;
            RandomRange = randomRange;
        }

        public Range()
        {
            Value = 0f;
            RandomRange = 0f;
        }

        public float GetValueInRange()
        {
            return Value + Maths.randRange(-1f, 1f) * RandomRange;
        }

        public float Value;

        public float RandomRange;
    }
}
