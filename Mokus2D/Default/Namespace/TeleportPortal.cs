namespace Mokus2D.Default.Namespace
{
    public class TeleportPortal(Portal _portal) : ParticleSystem(ClipFactory.GetAnchorConfig("McTeleportPartBlack.png"), 5)
    {
        public override void Update(float time)
        {
            base.Update(time);
            for (int i = 0; i < 5; i++)
            {
                Particle particle = portal.Particles[i];
                Particle particle2 = particles[i];
                particle2.Position = particle.Position;
                particle2.Scale = particle.Scale;
            }
        }

        private const int PARTS_COUNT = 5;

        protected Portal portal = _portal;
    }
}
