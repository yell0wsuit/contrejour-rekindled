using System;

namespace Mokus2D.Effects.Actions
{
    public class RotateBy : RotateTo
    {
        public RotateBy(float seconds, float angle)
            : base(seconds, angle)
        {
            diff = angle;
        }

        public RotateBy(TimeSpan interval, float angle)
            : base(interval, angle)
        {
            diff = angle;
        }

        internal override void Reset()
        {
            base.Reset();
            targetAngle = initialAngle + diff;
        }

        internal override void Start(float time)
        {
            base.Start(time);
            Reset();
        }

        private readonly float diff;
    }
}
