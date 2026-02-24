using System;

using Microsoft.Xna.Framework;

namespace Mokus2D.Effects.Actions
{
    public class RotateToDegrees : RotateTo
    {
        public RotateToDegrees(TimeSpan interval, float angle)
            : base(interval, MathHelper.ToRadians(angle))
        {
        }

        public RotateToDegrees(float seconds, float angle)
            : base(seconds, MathHelper.ToRadians(angle))
        {
        }
    }
}
