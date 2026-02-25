using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public interface IBonusAcceptable : IBodyClip
    {
        void ApplyBonus();

        Vector2 BonusTarget();
    }
}
