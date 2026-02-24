using System;

using Microsoft.Xna.Framework;

namespace Mokus2D.Effects.Actions
{
    public class ScaleTo(TimeSpan interval, Vector2 targetScale) : IntervalActionBase(interval)
    {
        public ScaleTo(float seconds, float targetScale)
            : this(TimeSpan.FromSeconds((double)seconds), new Vector2(targetScale))
        {
        }

        public ScaleTo(float seconds, Vector2 targetScale)
            : this(TimeSpan.FromSeconds((double)seconds), targetScale)
        {
        }

        internal override void Start(float time)
        {
            initialScale = Target.ScaleVec;
            base.Start(time);
        }

        internal override void UpdateNode(float ratio)
        {
            Target.ScaleVec = Vector2.Lerp(initialScale, targetScale, ratio);
        }

        private Vector2 initialScale;
    }
}
