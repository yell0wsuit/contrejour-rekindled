using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class GravityParticle(ParticleSystem _system) : Particle(_system)
    {
        public Vector2 Speed
        {
            get => speed; set => speed = value;
        }

        public float AngularSpeed
        {
            get => angularSpeed; set => angularSpeed = value;
        }

        protected Vector2 speed;

        protected float angularSpeed;
    }
}
