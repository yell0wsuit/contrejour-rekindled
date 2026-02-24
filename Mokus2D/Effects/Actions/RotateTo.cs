using System;

using Microsoft.Xna.Framework;

namespace Mokus2D.Effects.Actions
{
    public class RotateTo(TimeSpan interval, float angle) : IntervalActionBase(interval)
    {
        public RotateTo(float seconds, float angle)
            : this(TimeSpan.FromSeconds((double)seconds), angle)
        {
        }

        internal override void Reset()
        {
            base.Reset();
            initialAngle = Target.RotationRadians;
        }

        internal override void Start(float time)
        {
            base.Start(time);
            initialAngle = Target.RotationRadians;
        }

        internal override void UpdateNode(float ratio)
        {
            base.UpdateNode(ratio);
            Target.RotationRadians = MathHelper.Lerp(initialAngle, targetAngle, ratio);
        }

        protected float targetAngle = angle;

        protected float initialAngle;
    }
}
