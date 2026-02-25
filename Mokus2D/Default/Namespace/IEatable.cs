using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public interface IEatable : IBodyClip
    {
        void EatSpeedPauseScaleTime(Vector2 targetPosition, float _finishSpeed, float pause, float scale, float time);

        float DeadEyeScale();

        bool CanDie();
    }
}
