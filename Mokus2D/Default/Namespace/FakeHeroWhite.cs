using ContreJourMono.ContreJour.Menu.LevelComplete;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class FakeHeroWhite : FakeHeroBlack
    {
        protected override Color TailColor
        {
            get
            {
                return ContreJourConstants.WHITE_TAIL_COLOR;
            }
        }

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
