using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Default.Namespace
{
    public class Portal : ParticleSystem
    {
        public float TargetScale
        {
            get
            {
                return targetScale;
            }
            set
            {
                if (targetScale != value)
                {
                    targetScale = value;
                    if (targetScale != itemsScale)
                    {
                        Visible = true;
                    }
                }
            }
        }

        public float SpeedValue
        {
            get
            {
                Satellite satellite = parts[0];
                return satellite.SpeedValue;
            }
            set
            {
                for (int i = 0; i < 5; i++)
                {
                    Satellite satellite = parts[i];
                    satellite.SpeedValue = value;
                }
            }
        }

        public float ItemsScale
        {
            get
            {
                return itemsScale;
            }
            set
            {
                if (Maths.FuzzyNotEquals(itemsScale, value, 0.0001f))
                {
                    itemsScale = value;
                    targetScale = value;
                }
            }
        }

        public float ScaleStep
        {
            get
            {
                return scaleStep;
            }
            set
            {
                scaleStep = value;
            }
        }

        public Portal(ContreJourGame game, Vector2 position)
            : this(game, position, "McFinishPart.png")
        {
        }

        public Portal(ContreJourGame game, Vector2 position, string textureName)
            : base(ClipFactory.GetAnchorConfig(textureName))
        {
            parts = new List<Satellite>();
            Blend = BlendState.Additive;
            scaleStep = 0.05f;
            for (int i = 0; i < 5; i++)
            {
                Particle particle = AddParticle();
                float num = 1.2566371f * i;
                Satellite satellite = new(game, particle, null, num, position);
                parts.Add(satellite);
            }
            itemsScale = 1f;
            targetScale = 2f;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (Maths.FuzzyNotEquals(itemsScale, targetScale, 0.0001f))
            {
                itemsScale = Maths.StepToTargetMaxStep(itemsScale, targetScale, scaleStep * time * 30f);
            }
            Visible = itemsScale > 0f;
        }

        public override void UpdateParticleTime(Particle particle, float time)
        {
            base.UpdateParticleTime(particle, time);
            particle.Scale = itemsScale;
        }

        private const int PARTS_COUNT = 5;

        protected List<Satellite> parts;

        protected float targetScale;

        protected float itemsScale;

        protected float scaleStep;
    }
}
