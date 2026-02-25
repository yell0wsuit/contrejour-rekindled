using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class Chapter3(int _index, MainMenu _menu) : ChapterItem(_index, _menu)
    {
        protected override void CreateSprites()
        {
            RotatingSprite rotatingSprite = AddShesterna("McPlanetShesterna", 5, CocosUtil.ccpIPad(-40f, 60f));
            rotatingSprite.Color = new Color(100, 100, 100);
            blurBackground = ClipFactory.CreateWithAnchor("McChapter3Blur");
            background = ClipFactory.CreateWithAnchor("McPlanet3Background");
            container.AddChild(background);
            Sprite sprite = ClipFactory.CreateWithAnchor("McPlanet3Light");
            container.AddChild(sprite);
            FadeTo fadeTo = new(4f, 0.39215687f);
            FadeTo fadeTo2 = new(5f, 0.7058824f);
            Sequence sequence = new([fadeTo, fadeTo2]);
            sprite.Run(new RepeatForever(sequence));
            rotatingSprite = AddShesterna("McPlanetShesterna", 10, CocosUtil.ccpIPad(-27f, -5f));
            rotatingSprite.Scale = 1.8f;
            rotatingSprite.Color = new Color(100, 100, 100);
            _ = AddShesterna("McPlanetShesternaBlur", 90, CocosUtil.ccpIPad(19f, -58f));
            _ = AddShesterna("McPlanetShesterna2", -20, CocosUtil.ccpIPad(40f, 30f));
            _ = AddShesterna("McPlanetShesterna3", -400, CocosUtil.ccpIPad(74f, 25f));
            rotatingSprite = AddShesterna("McPlanetCross", -200, CocosUtil.ccpIPad(104f, 24f));
            alphaItems.Add(rotatingSprite);
            rotatingSprite = AddShesterna("McMenuCircleSpikes", -400, CocosUtil.ccpIPad(50f, 69f));
            alphaItems.Add(rotatingSprite);
            rotatingSprite = AddShesterna("McMenuCircleSpikes", -200, CocosUtil.ccpIPad(67f, 54f));
            alphaItems.Add(rotatingSprite);
            MovieClip movieClip = (MovieClip)ClipFactory.CreateWithAnchor("McPlanetStick");
            movieClip.Position = CocosUtil.ccpIPad(30f, 96f);
            container.AddChild(movieClip);
            movieClip.Rotation = -26f;
            movieClip.Speed = 1.3f;
            AddUpdating(movieClip);
            alphaItems.Add(movieClip);
            Sprite sprite2 = ClipFactory.CreateWithAnchor("McPlanet3Foreground");
            container.AddChild(sprite2);
        }

        private RotatingSprite AddShesterna(string spriteName, int speed, Vector2 position)
        {
            RotatingSprite rotatingSprite = new(spriteName)
            {
                Speed = speed,
                Position = position
            };
            container.AddChild(rotatingSprite);
            AddUpdating(rotatingSprite);
            return rotatingSprite;
        }
    }
}
