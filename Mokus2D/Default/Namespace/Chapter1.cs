using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class Chapter1(int _index, MainMenu _menu) : ChapterItem(_index, _menu)
    {
        public override float Depth
        {
            set
            {
                base.Depth = value;
                eye.AnimationsAllowed = Maths.FuzzyEquals(value, 1f, 0.0001f);
            }
        }

        protected override void CreateSprites()
        {
            CreateSnotRotationScale(CocosUtil.ccpIPad(70f, 60f), -30f, 0.53333336f);
            CreateSnotRotationScale(CocosUtil.ccpIPad(83f, 44f), -80f, 0.4f);
            CreateSnotRotationScale(CocosUtil.ccpIPad(74f, -59f), -130f, 0.26666668f);
            CreateSnotRotationScale(CocosUtil.ccpIPad(54f, -72f), -180f, 0.33333334f);
            CreateSnotRotationScale(CocosUtil.ccpIPad(-83f, -35f), 90f, 0.33333334f);
            background = new Sprite("McPlanet1Background");
            foreground = ClipFactory.CreateWithAnchor("McPlanet1Foreground");
            blurBackground = ClipFactory.CreateWithAnchor("McChapter1Blur");
            blurBackground.Scale = 1.1f;
            container.AddChild(background);
            Sprite sprite = ClipFactory.CreateWithAnchor("McPlanetRoseLight");
            container.AddChild(sprite);
            FadeTo fadeTo = new(3f, 0.19607843f);
            FadeTo fadeTo2 = new(3f, 0.60784316f);
            Sequence sequence = new([fadeTo, fadeTo2]);
            sprite.Run(new RepeatForever(sequence));
            Sprite sprite2 = ClipFactory.CreateWithAnchor("McRoseForeground");
            container.AddChild(sprite2);
            eye = new PlanetEye(null, true, Vector2.Zero);
            eye.Scale = 0.9f;
            eye.Position = CocosUtil.ccpIPad(10f, -10f);
            container.AddChild(eye);
            AddSpikes(CocosUtil.ccpIPad(-92f, 8f), 1f, -200f, 4f, 0f);
            AddSpikes(CocosUtil.ccpIPad(-90f, 23f), 0.85f, -300f, 3f, 1.0471976f);
            AddSpikes(CocosUtil.ccpIPad(-88f, 37f), 0.65f, -400f, 2f, 2.0943952f);
            container.AddChild(foreground);
        }

        private void AddSpikes(Vector2 position, float scale, float speed, float amplitude, float progress)
        {
            MovingRotatingSprite movingRotatingSprite = new("McMenuCircleSpikes");
            movingRotatingSprite.Position = position;
            movingRotatingSprite.Scale = scale;
            movingRotatingSprite.Speed = speed;
            movingRotatingSprite.Initialize(amplitude, progress);
            container.AddChild(movingRotatingSprite);
        }

        public void CreateSnotRotationScale(Vector2 position, float rotation, float scale)
        {
            PlanetSnot planetSnot = new();
            planetSnot.Position = position;
            planetSnot.Rotation = rotation;
            planetSnot.Scale = scale;
            container.AddChild(planetSnot);
            depthDependent.Add(planetSnot);
        }

        protected Sprite foreground;

        protected PlanetEye eye;
    }
}
