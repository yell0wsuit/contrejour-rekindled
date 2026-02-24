using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class Chapter6(int _index, MainMenu _menu) : ChapterItem(_index, _menu)
    {
        protected override void CreateSprites()
        {
            ParticleSystem particleSystem = new("McGreenPlanetFly");
            container.AddChild(particleSystem);
            AddAlphaItem(particleSystem);
            PlanetSurround planetSurround = new(particleSystem);
            AddUpdating(planetSurround);
            background = ClipFactory.CreateWithAnchor("McChapter6Background");
            blurBackground = ClipFactory.CreateWithAnchor("McChapter6Background");
            container.AddChild(background);
            ParticleSystem particleSystem2 = new("McEnergyBall.png");
            container.AddChild(particleSystem2);
            particleSystem2.Scale = 2f;
            alphaItems.Add(particleSystem2);
            PlanetEnergy planetEnergy = new(particleSystem2, Vector2.Zero, new Range(0.16f, 0.06f));
            AddUpdating(planetEnergy);
            Sprite sprite = new("McChapter6Foreground");
            container.AddChild(sprite);
        }
    }
}
