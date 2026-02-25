using System;

using Mokus2D.Default.Namespace;

using Microsoft.Xna.Framework;

namespace Mokus2D.Effects.Actions
{
    public class FadeColor(Color targetColor, TimeSpan interval) : IntervalActionBase(interval)
    {
        public FadeColor(float seconds, Color targetColor)
            : this(targetColor, seconds)
        {
        }

        public FadeColor(Color targetColor, float seconds)
            : this(targetColor, TimeSpan.FromSeconds((double)seconds))
        {
        }

        internal override void Start(float time)
        {
            base.Start(time);
            startColor = Target.Color;
        }

        internal override void UpdateNode(float ratio)
        {
            base.UpdateNode(ratio);
            Target.Color = CocosUtil.ccc4Mix(startColor, targetColor, 1f - ratio);
        }

        private Color startColor;
    }
}
