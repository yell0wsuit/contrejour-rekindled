namespace Mokus2D.Default.Namespace
{
    public class GroundFall : GravityParticleSystem
    {
        public GroundFall(ContreJourGame game)
            : base(game.Choose("McGroundPart.png", "McGroundPartBlack.png", "McGroundPartWhite.png", null, "McGroundPart_6"))
        {
            black = game.BlackSide;
            Angle = new Range(270f, 0f);
            Speed = new Range(CocosUtil.iPadValue(40f), CocosUtil.iPadValue(20f));
            AngularSpeed = new Range(0f, 0f);
            ParticlesScale = new Range(1f, 0.3f);
            StartOpacity = new Range(200f, 50f);
        }

        public override void UpdateParticleTime(Particle particle, float time)
        {
            base.UpdateParticleTime(particle, time);
            particle.Scale -= time / 2f;
            if (particle.Scale <= 0f)
            {
                particle.Visible = false;
            }
        }

        private const float SCALE_STEP = 0.015f;

        protected bool black;
    }
}
