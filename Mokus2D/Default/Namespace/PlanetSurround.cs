using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class PlanetSurround : IUpdatable
    {
        public PlanetSurround(ParticleSystem system)
        {
            system.Position = new Vector2(system.Position.X + 6f, system.Position.Y - 6f);
            for (int i = 0; i < 128; i++)
            {
                float num = Maths.RandRangeMinMax(-6.2831855f, 6.2831855f);
                Vector2 vector = new(orbit.GetValueInRange() * (float)Math.Cos((double)num), orbit.GetValueInRange() * (float)Math.Sin((double)num));
                float valueInRange = startScale.GetValueInRange();
                Particle particle = system.AddParticle(vector);
                ButterFly butterFly = new(particle, valueInRange);
                parts.Add(butterFly);
            }
        }

        public void Update(float time)
        {
            foreach (ButterFly butterFly in parts)
            {
                butterFly.Update(time);
            }
        }

        private const int PARTS_COUNT = 128;

        private readonly List<ButterFly> parts = new(128);

        private readonly Range orbit = new(115f, 5f);

        private readonly Range startScale = new(0.3f, 0.15f);
    }
}
