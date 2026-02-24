using Microsoft.Xna.Framework;

namespace Mokus2D.Input
{
    public class Touch
    {
        public Vector2 TotalOffset
        {
            get
            {
                return position - initialPosition;
            }
        }

        public Vector2 LastFrameOffset
        {
            get
            {
                return position - previousPosition;
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }
            internal set
            {
                active = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

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
