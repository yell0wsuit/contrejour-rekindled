namespace Mokus2D.Default.Namespace
{
    public class SnowFall : BlackFall
    {
        public SnowFall()
            : base("McSnowParticle.png")
        {
            SpeedMult = 2.5f;
        }

        public SnowFall(string textureName)
            : base(textureName)
        {
        }

        protected override void initParams()
        {
            base.initParams();
            AngularSpeed = new Range(0f, 0f);
            ParticlesScale = new Range(1.75f, 0.75f);
        }
    }
}
