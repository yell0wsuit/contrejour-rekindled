using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class BlackFall : GravityParticleSystem
    {
        public BlackFall()
            : base(ClipFactory.GetAnchorConfig("McFallParticle.png"))
        {
            initParams();
        }

        public BlackFall(string textureName)
            : base(textureName)
        {
            initParams();
        }

        protected virtual void initParams()
        {
            CGSize cgsize = ScreenConstants.W7FromIPhoneSize;
            HorizontalPosition = new Range(cgsize.Width / 2f, cgsize.Width / 2f);
            VerticalPosition = new Range(cgsize.Height + 20f, 0f);
            Speed = new Range(5f, 0f);
            Angle = new Range(-70f, 15f);
            AngularSpeed = new Range(0f, 10f);
            ParticlesScale = new Range(1f, 0.6f);
            bottomLeftBound = new Vector2(-20f, -20f);
            topRightBound = new Vector2(cgsize.Width + 20f, cgsize.Height + 20f);
        }

        public override void initParticle(GravityParticle gravityParticle)
        {
            base.initParticle(gravityParticle);
            float num = SpeedMult * (gravityParticle.Scale - (ParticlesScale.Value - ParticlesScale.RandomRange)) + 1f;
            gravityParticle.Speed *= num;
        }

        public float SpeedMult = 4f;
    }
}
