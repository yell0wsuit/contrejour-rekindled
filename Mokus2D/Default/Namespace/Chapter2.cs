using System;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class Chapter2(int _index, MainMenu _menu) : ChapterItem(_index, _menu)
    {
        protected override void CreateSprites()
        {
            background = ClipFactory.CreateWithAnchor("McPlanet2Background");
            blurBackground = ClipFactory.CreateWithAnchor("McChapter2Blur");
            _ = CreateBouncingSprite("McPlanetSpringBack", 45, CocosUtil.ccpIPad(-69f, 26f), 0.8f);
            _ = CreateBouncingSprite("McPlanetSpringBack", -150, CocosUtil.ccpIPad(26f, -73f), 0.7f);
            container.AddChild(background);
            ParticleSystem particleSystem = new("McEnergyBall.png");
            container.AddChild(particleSystem);
            particleSystem.Scale = 0.55f;
            PlanetEnergy planetEnergy = new(particleSystem, CocosUtil.ccpIPad(-136f, -92f), null);
            AddUpdating(planetEnergy);
            alphaItems.Add(particleSystem);
            CreateSmoke();
            _ = CreateBouncingSprite("McPlanetSpringView", -50, CocosUtil.ccpIPad(70f, 35f), 0.9f);
            _ = CreateBouncingSprite("McPlanetSpringView", -190, CocosUtil.ccpIPad(-14f, -74f), 0.9f);
            _ = CreateBouncingSprite("McPlanetSpringView", 90, CocosUtil.ccpIPad(-78f, -3f), 0.8f);
            PlanetSatellite planetSatellite = new();
            container.AddChild(planetSatellite);
            AddUpdating(planetSatellite);
            alphaItems.Add(planetSatellite);
        }

        protected BouncingSprite CreateBouncingSprite(string spriteName, int rotation, Vector2 position, float scale)
        {
            BouncingSprite bouncingSprite = new(spriteName);
            bouncingSprite.Rotation = rotation;
            bouncingSprite.Position = position;
            bouncingSprite.Scale = scale;
            bouncingSprite.MaxBounceEvent += new Action<BouncingSprite>(OnSpringSpit);
            container.AddChild(bouncingSprite);
            AddUpdating(bouncingSprite);
            return bouncingSprite;
        }

        public virtual string SmokeSprite()
        {
            return "McWhiteSmokeBlack.png";
        }

        public void CreateSmoke()
        {
            springSmoke = new WhiteSmoke(SmokeSprite());
            springSmoke.CreateOnStartPosition(20);
            springSmoke.HideAllParticles();
            springSmoke.OpacityStep = 70f;
            springSmoke.ScaleStep = 1f;
            springSmoke.MaxOpacity = 180f;
            springSmoke.Gravity = CocosUtil.ccpIPad(0f, 10f);
            springSmoke.ParticlesScale = new Range(1.2f, 0.3f);
            alphaItems.Add(springSmoke);
            container.AddChild(springSmoke, 100);
        }

        public virtual Vector2 SmokeCoords()
        {
            return CocosUtil.ccpIPad(0f, 43f);
        }

        private void OnSpringSpit(BouncingSprite spring)
        {
            if (Maths.FuzzyEquals(depth, 1f, 0.0001f))
            {
                GravityParticle gravityParticle = (GravityParticle)springSmoke.AddOrGetInvisible();
                gravityParticle.Position = spring.LocalToNode(SmokeCoords(), this, true);
                gravityParticle.Speed = Maths.ToPointAngle(15f, MathHelper.ToRadians(spring.Rotation) + 1.5707964f);
            }
        }

        protected WhiteSmoke springSmoke;
    }
}
