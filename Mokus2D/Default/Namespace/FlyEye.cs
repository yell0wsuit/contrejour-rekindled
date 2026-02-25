using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class FlyEye(ContreJourGame _game, bool _visible, Vector2 position) : MonsterEye(_game, _visible, position)
    {
        protected override void CreateDefaultView()
        {
            base.CreateDefaultView();
            eyeBall.Scale = 1.5f;
        }

        protected override float ViewRadius => CocosUtil.iPadValue(4f);
    }
}
