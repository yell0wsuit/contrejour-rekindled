using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class PlanetEnergy : IUpdatable
    {
        public PlanetEnergy(ParticleSystem system, Vector2 position, Range angleStep = null)
        {
            for (int i = 0; i < 5; i++)
            {
                Particle particle = system.AddParticle(position);
                Satellite satellite = new(null, particle, null, 1.2566371f * i, position);
                parts.Add(satellite);
                if (angleStep != null)
                {
                    satellite.AngleStep = angleStep.GetValueInRange();
                }
            }
        }

        public void Update(float time)
        {
            foreach (Satellite satellite in parts)
            {
                satellite.Update(time);
            }
        }

        private readonly List<Satellite> parts = new();
    }
}
