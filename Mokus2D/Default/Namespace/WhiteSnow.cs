namespace Mokus2D.Default.Namespace
{
    public class WhiteSnow : SnowFall
    {
        public WhiteSnow()
            : base("McSnowParticle.png")
        {
        }

        protected override void initParams()
        {
            base.initParams();
            ParticlesScale = new Range(1f, 0.4f);
            StartOpacity = new Range(255f, 0f);
        }
    }
}
