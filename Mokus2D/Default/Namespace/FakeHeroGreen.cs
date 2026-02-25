using Mokus2D.ContreJourMono.ContreJour.Menu.LevelComplete;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;

namespace Mokus2D.Default.Namespace
{
    public class FakeHeroGreen : FakeHeroBlack
    {
        protected override Color TailColor => ContreJourConstants.GreenTail.ChangeAlpha(byte.MaxValue);

        protected override string ProcessName(string name)
        {
            return name + "_6";
        }

        protected override FakeHeroEye CreateEye()
        {
            return new FakeHeroEyeGreen();
        }
    }
}
