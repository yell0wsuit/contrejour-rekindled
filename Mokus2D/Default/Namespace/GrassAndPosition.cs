using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class GrassAndPosition(Particle particle, Vector2 position)
    {
        public Particle Particle
        {
            get
            {
                return particle;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        protected Particle particle = particle;

        protected Vector2 position = position;
    }
}
