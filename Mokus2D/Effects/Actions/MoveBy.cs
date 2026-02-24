using System;

using Microsoft.Xna.Framework;

namespace Mokus2D.Effects.Actions
{
    public class MoveBy(Vector2 offset, TimeSpan interval) : MoveTo(Vector2.Zero, interval)
    {
        public MoveBy(Vector2 offset, float seconds)
            : this(Vector2.Zero, TimeSpan.FromSeconds((double)seconds))
        {
            this.offset = offset;
        }

        internal override void Start(float time)
        {
            base.Start(time);
            targetPosition = initialPosition + offset;
        }

        private Vector2 offset;
    }
}
