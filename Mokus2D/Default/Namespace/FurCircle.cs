using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class FurCircle : ParticleSystem
    {
        public float Radius
        {
            get => radius;
            set
            {
                if (Maths.FuzzyNotEquals(radius, value, 0.0001f))
                {
                    radius = value;
                    for (int i = 0; i < particles.Count; i++)
                    {
                        float itemAngle = GetItemAngle(i);
                        Particle particle = particles[i];
                        particle.Rotation = MathHelper.ToDegrees(itemAngle) - 90f;
                        particle.Position = Maths.toPoint(value, itemAngle);
                    }
                }
            }
        }

        public float AngleStep => angleStep;

        public FurCircle(string textureName, int maxParticles, float _radius)
            : base(textureName, maxParticles)
        {
            angleStep = 1f / maxParticles * 2f * 3.1415927f;
            Radius = _radius;
        }

        public float GetItemAngle(int i)
        {
            return i * angleStep;
        }

        protected float radius;

        protected float angleStep;
    }
}
