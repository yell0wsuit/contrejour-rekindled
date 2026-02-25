using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class Explosion : GravityParticleSystem
    {
        public Explosion(string textureName)
            : base(textureName)
        {
            opacityStep = 300f;
            scaleStep = 6f;
            StartOpacity = new Range(220f, 35f);
            Speed = new Range(CocosUtil.iPadValue(140f), CocosUtil.iPadValue(20f));
            ParticlesScale = new Range(2f, 1f);
            Angle = new Range(0f, 3600f);
        }

        public float ScaleStep
        {
            get => scaleStep; set => scaleStep = value;
        }

        public float OpacityStep
        {
            get => opacityStep; set => opacityStep = value;
        }

        public override void initParticle(GravityParticle gravityParticle)
        {
            base.initParticle(gravityParticle);
            if (!Maths.ccpEqual(gravityParticle.Position, Vector2.Zero))
            {
                float num = gravityParticle.Speed.Length();
                float num2 = Maths.atan2(gravityParticle.Position.Y, gravityParticle.Position.X);
                gravityParticle.Speed = Maths.ToPointAngle(num, num2);
            }
        }

        public override void UpdateParticleTime(Particle particle, float time)
        {
            if (particle.Visible)
            {
                base.UpdateParticleTime(particle, time);
                particle.Opacity -= (int)(opacityStep * time);
                particle.Scale += scaleStep * time;
                if (particle.Opacity <= 0 || particle.Scale < 0f)
                {
                    particle.Visible = false;
                }
            }
        }

        protected float opacityStep;

        protected float scaleStep;
    }
}
