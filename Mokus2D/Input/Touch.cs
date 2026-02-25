using Microsoft.Xna.Framework;

namespace Mokus2D.Input
{
    public class Touch
    {
        public Vector2 TotalOffset => position - initialPosition;

        public Vector2 LastFrameOffset => position - previousPosition;

        public bool Active { get; internal set; } = true;

        public int Id { get; private set; }

        public Vector2 Position => position;

        public void Initialize(Vector2 position, int id)
        {
            Id = id;
            initialPosition = position;
            previousPosition = initialPosition;
            Refresh(position);
        }

        public void Refresh(Vector2 newPosition)
        {
            previousPosition = position;
            position = newPosition;
        }

        private Vector2 initialPosition;

        private Vector2 position;

        private Vector2 previousPosition;
    }
}
