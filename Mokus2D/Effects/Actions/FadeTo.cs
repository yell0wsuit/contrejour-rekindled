using System;

using Microsoft.Xna.Framework;

namespace Mokus2D.Effects.Actions
{
    public class FadeTo(TimeSpan interval, float opacity) : IntervalActionBase(interval)
    {
        public FadeTo(float seconds, float opacity)
            : this(TimeSpan.FromSeconds((double)seconds), opacity)
        {
        }

        internal override void Start(float time)
        {
            initialOpacity = Target.OpacityFloat;
            base.Start(time);
        }

        internal override void UpdateNode(float ratio)
        {
            Target.OpacityFloat = MathHelper.Lerp(initialOpacity, opacity, ratio);
        }

        private float initialOpacity;
    }
}
