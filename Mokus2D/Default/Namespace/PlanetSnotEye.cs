using ContreJourMono.ContreJour.Game.Eyes;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class PlanetSnotEye : PlanetEye
    {
        public PlanetSnotEye(ContreJourGame _game, bool _visible, Vector2 position)
            : base(_game, _visible, position)
        {
            UpdateEnabled = true;
        }

        protected override void CreateDefaultView()
        {
            background = ClipFactory.CreateWithAnchor("McEyeMonster");
            eyeBall = ClipFactory.CreateWithAnchor("McEyeBallMonster");
        }

        protected override EyeAnimation[] Animations => SNOT_ANIMATIONS;

        protected override float ViewRadius => CocosUtil.iPadValue(7f);

        protected override float MaxAngle()
        {
            return 3.1415927f;
        }
    }
}
