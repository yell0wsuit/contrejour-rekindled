using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class WhiteSmoke : GravityParticleSystem
    {
        public WhiteSmoke(string textureName)
            : this(textureName, 0)
        {
        }

        public WhiteSmoke(string textureName, int maxParticles)
            : base(textureName, maxParticles)
        {
            Speed = new Range(CocosUtil.iPadValue(250f), CocosUtil.iPadValue(80f));
            ParticlesScale = new Range(2f, 1f);
            maxOpacity = 255f;
        }

        public float OpacityStep
        {
            get => opacityStep; set => opacityStep = value;
        }

        public float ScaleStep
        {
            get => scaleStep; set => scaleStep = value;
        }

        public float MaxOpacity
        {
            get => maxOpacity; set => maxOpacity = value;
        }

        public virtual Vector2 SmokePosition
        {
            get => new Vector2(horizontalPosition.Value, verticalPosition.Value);
            set
            {
                horizontalPosition.Value = value.X;
                verticalPosition.Value = value.Y;
            }
        }

        public override void initParticle(GravityParticle gravityParticle)
        {
            base.initParticle(gravityParticle);
            gravityParticle.Tag = this;
            gravityParticle.Opacity = 1;
        }

        public override void UpdateParticleTime(Particle particle, float time)
        {
            if (particle.Visible)
            {
                base.UpdateParticleTime(particle, time);
                if (particle.Tag != null)
                {
                    particle.Opacity = (int)Maths.StepToTargetMaxStep(particle.Opacity, maxOpacity, maxOpacity / 4f);
                    if (particle.Opacity >= maxOpacity)
                    {
                        particle.Tag = null;
                    }
                }
                else
                {
                    float num = opacityStep * time / 255f;
                    if ((double)particle.OpacityFloat < 0.5 && ScaleDownOnDestroy)
                    {
                        num *= 4f;
                    }
                    particle.OpacityFloat -= num;
                }
                if ((double)particle.OpacityFloat < 0.5 && ScaleDownOnDestroy)
                {
                    particle.Scale -= scaleStep * time;
                }
                else
                {
                    particle.Scale += scaleStep * time;
                }
                if (particle.Opacity <= 0 || (scaleStep < 0f && (particle.Scale < 2f || particle.Opacity > 150)))
                {
                    particle.Visible = false;
                    initParticle((GravityParticle)particle);
                }
            }
        }

        public bool ScaleDownOnDestroy = true;

        protected float maxOpacity;

        protected float opacityStep;

        protected float scaleStep;
    }
}
