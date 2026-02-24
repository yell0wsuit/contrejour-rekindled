using System;

namespace Mokus2D.Effects.Actions
{
    public class FadeIn : FadeTo
    {
        public FadeIn(float seconds)
            : base(seconds, 1f)
        {
        }

        public FadeIn(TimeSpan interval)
            : base(interval, 1f)
        {
        }
    }
}
