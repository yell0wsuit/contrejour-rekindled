using System;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class FlyBase : IUpdatable
    {
        public FlyBase(Particle _particle, float _scale)
        {
            particle = _particle;
            particle.Scale = _scale;
            initialPosition = _particle.Position;
            targetPosition = initialPosition;
            opacityChanger = new CosOpacityChanger(particle, 0f, Maths.randRange(0.5f, 0.6f), Maths.RandRangeMinMax(0.01f, 0.07f));
            initParams();
        }

        public void Update(float time)
        {
            float num = Math.Min(Maths.Abs((targetPosition.X - particle.Position.X) / (targetPosition.Y - particle.Position.Y) * stepY), 1f);
            Vector2 vector = new(CocosUtil.iPadValue(num), stepY);
            particle.Position = Maths.StepToPointTargetMaxStep(particle.Position, targetPosition, vector.Length());
            verticalStep += verticalStepDiff;
            opacityChanger.Update(time);
        }

        private void initParams()
        {
            stepY = CocosUtil.iPadValue(Maths.RandRangeMinMax(0.25f, 0.75f));
            verticalStep = Maths.RandRangeMinMax(-6.2831855f, 6.2831855f);
            verticalStepDiff = CocosUtil.iPadValue(Maths.RandRangeMinMax(0.01f, 0.02f));
        }

        private CosOpacityChanger opacityChanger;

        protected Vector2 initialPosition;

        protected Particle particle;

        protected float stepY;

        protected Vector2 targetPosition;

        protected float verticalStep;

        protected float verticalStepDiff;
    }
}
