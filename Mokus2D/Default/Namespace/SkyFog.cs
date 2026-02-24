using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class SkyFog : GravityParticleSystem
    {
        public SkyFog(ContreJourGame _game)
            : base("McBackFog.png", 8)
        {
            game = _game;
            Speed = new Range(100f, 0f);
            ParticlesScale = new Range(4f, 1f);
            StartOpacity = new Range(100f, 0f);
            BottomLeftBound = new Vector2(-1000f, 200f);
            TopRightBound = new Vector2(1700f, 768f);
            HorizontalPosition = new Range(-1000f, 0f);
            VerticalPosition = new Range(520f, 280f);
        }

        protected ContreJourGame game;
    }
}
