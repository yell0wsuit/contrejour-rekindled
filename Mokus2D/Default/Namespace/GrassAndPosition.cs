using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class GrassAndPosition(Particle particle, Vector2 position)
    {
        public Particle Particle => particle;

        public Vector2 Position
        {
            get => position; set => position = value;
        }

        protected Particle particle = particle;

        protected Vector2 position = position;
    }
}
