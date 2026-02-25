using Mokus2D.ContreJourMono.ContreJour.Menu.LevelComplete;

using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class FakeHeroWhite : FakeHeroBlack
    {
        protected override Color TailColor => ContreJourConstants.WHITE_TAIL_COLOR;

        protected override string ProcessName(string name)
        {
            return name + "White";
        }

        protected override FakeHeroEye CreateEye()
        {
            return new FakeHeroEyeWhite();
        }
    }
}
