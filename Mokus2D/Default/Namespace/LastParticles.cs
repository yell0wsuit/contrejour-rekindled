namespace Mokus2D.Default.Namespace
{
    public class LastParticles : BlackFall
    {
        public LastParticles()
            : base("Mc5ChapterParticle.png")
        {
            SpeedMult = 5f;
        }

        protected override void initParams()
        {
            base.initParams();
            ParticlesScale = new Range(0.9f, 0.6f);
        }

        public override void initParticle(GravityParticle gravityParticle)
        {
            if (Maths.Rand() < 0.1f)
            {
                gravityParticle.Scale = Maths.randRange(3f, 3.5f);
            }
            base.initParticle(gravityParticle);
            float num = ParticlesScale.Value + ParticlesScale.RandomRange;
            gravityParticle.OpacityFloat = Maths.max((num - gravityParticle.Scale) / num, 0.05f);
        }
    }
}
