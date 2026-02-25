using Microsoft.Xna.Framework;

using Mokus2D.Util;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class Chapter4(int _index, MainMenu _menu) : Chapter2(_index, _menu)
    {
        protected override void CreateSprites()
        {
            background = ClipFactory.CreateWithAnchor("McPlanet4Background");
            blurBackground = ClipFactory.CreateWithAnchor("McChapter4Blur");
            if (!HardwareCapabilities.IsLowMemoryDevice)
            {
                shadow = ClipFactory.CreateWithAnchor("McPlanet4Shadow");
                shadow.Scale = 2.2304688f;
                container.AddChild(shadow);
                AddAlphaItem(shadow);
            }
            container.AddChild(background);
            BouncingSprite bouncingSprite = CreateBouncingSprite("McPlanet4Spring0", 40, CocosUtil.ccpIPad(-41f, 49f), 1f);
            bouncingSprite.Step = Maths.randRange(0.05f, 0.08f);
            bouncingSprite = CreateBouncingSprite("McPlanet4Spring1", -70, CocosUtil.ccpIPad(69f, 24f), 1f);
            bouncingSprite.Step = Maths.randRange(0.05f, 0.08f);
            CreateSmoke();
        }

        public override string SmokeSprite()
        {
            return "McWhiteSmoke.png";
        }

        public override Vector2 SmokeCoords()
        {
            return CocosUtil.ccpIPad(0f, 32f);
        }

        protected override void CreateBackLight()
        {
        }

        protected Sprite shadow;
    }
}
