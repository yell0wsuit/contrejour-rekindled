using System;

using Microsoft.Xna.Framework;

namespace Mokus2D.Effects.Actions
{
    public class MoveTo(Vector2 targetPosition, TimeSpan interval) : IntervalActionBase(interval)
    {
        public MoveTo(float seconds, Vector2 targetPosition)
            : this(targetPosition, seconds)
        {
        }

        public MoveTo(Vector2 targetPosition, float seconds)
            : this(targetPosition, TimeSpan.FromSeconds((double)seconds))
        {
        }

        internal override void Start(float time)
        {
            base.Start(time);
            initialPosition = Target.Position;
        }

        internal override void UpdateNode(float ratio)
        {
            Target.Position = Vector2.Lerp(initialPosition, targetPosition, ratio);
        }

        protected Vector2 targetPosition = targetPosition;

        protected Vector2 initialPosition;
    }
}
