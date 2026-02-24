using System;

namespace Mokus2D.Effects.Actions
{
    public class FadeOut : FadeTo
    {
        public FadeOut(float seconds)
            : base(seconds, 0f)
        {
        }

        public FadeOut(TimeSpan interval)
            : base(interval, 0f)
        {
        }
    }
}
