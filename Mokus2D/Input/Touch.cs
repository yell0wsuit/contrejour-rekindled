using Microsoft.Xna.Framework;

namespace Mokus2D.Input
{
    public class Touch
    {
        public Vector2 TotalOffset => position - initialPosition;

        public Vector2 LastFrameOffset => position - previousPosition;

        public bool Active
        {
            get => active; internal set => active = value;
        }

        public int Id => id;

        public Vector2 Position => position;

        public void Initialize(Vector2 position, int id)
        {
            this.id = id;
            initialPosition = position;
            previousPosition = initialPosition;
            Refresh(position);
        }

        public void Refresh(Vector2 newPosition)
        {
            previousPosition = position;
            position = newPosition;
        }

        private bool active = true;

        private int id;

        private Vector2 initialPosition;

        private Vector2 position;

        private Vector2 previousPosition;
    }
}
