using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class Particle(ParticleSystem _system) : NodeBase
    {
        public Vector2 AnchorInPixels { get; set; }

        public int Frame
        {
            get
            {
                return frame;
            }
            set
            {
                frame = value;
            }
        }

        private int frame;

        protected ParticleSystem system = _system;
    }
}
