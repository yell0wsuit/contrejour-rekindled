namespace Default.Namespace
{
    public class ParticlesTail(BodyClip _clip) : IUpdatable
    {
        public void Update(float time)
        {
        }

        protected BodyClip clip = _clip;
    }
}
