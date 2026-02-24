using Microsoft.Xna.Framework;

using Mokus2D.Visual.Util;

namespace ContreJourMono.ContreJour.Menu.LevelComplete
{
    public class FakeHeroBlack : FakeHero
    {
        protected override FakeHeroEye CreateEye()
        {
            return new FakeHeroEyeBlack();
        }

        protected override string ProcessName(string name)
        {
            return base.ProcessName(name).Replace("/FakeHero", "/FakeHero/Black") + "Black";
        }

        protected override Color TailColor
        {
            get
            {
                return ColorUtil.CreateColor(26, 164, 222, 255);
            }
        }
    }
}
