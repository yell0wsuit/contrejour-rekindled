using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class BlueLights : GravityParticleSystem
    {
        public BlueLights()
            : base("McGroundPartBlack.png")
        {
            CGSize cgsize = ScreenConstants.W7FromIPhoneSize;
            HorizontalPosition = new Range(cgsize.Width / 2f, cgsize.Width / 2f);
            VerticalPosition = new Range(0f, 0f);
            Speed = new Range(0.5f, 0f);
            Angle = new Range(90f, 15f);
            AngularSpeed = new Range(0f, 0f);
            ParticlesScale = new Range(1.3f, 1f);
            bottomLeftBound = new Vector2(0f, 0f);
            topRightBound = new Vector2(cgsize.Width, cgsize.Height);
        }

        public override void initParticle(GravityParticle gravityParticle)
        {
            base.initParticle(gravityParticle);
            gravityParticle.Opacity = (int)(40f + (20f * gravityParticle.Scale));
            float num = 20f * gravityParticle.Scale;
            gravityParticle.Speed *= num;
        }
    }
}
