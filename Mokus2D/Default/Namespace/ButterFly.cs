using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class ButterFly(Particle _particle, float _scale) : FlyBase(_particle, _scale)
    {
        public new void Update(float time)
        {
            horizontalStep += CocosUtil.iPadValue(step);
            targetPosition = ChooseTarget();
            base.Update(time);
        }

        private Vector2 ChooseTarget()
        {
            return new Vector2(initialPosition.X + 5f * (Maths.Sin(horizontalStep) + 1f), initialPosition.Y + 3.5f * Maths.Sin(verticalStep));
        }

        private float horizontalStep;

        private float step = Maths.randRange(0.01f, 0.03f);
    }
}
