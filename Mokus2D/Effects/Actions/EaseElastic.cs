namespace Mokus2D.Effects.Actions
{
    public abstract class EaseElastic(IntervalActionBase action, float rate) : EaseAction(action, rate)
    {
        internal override void UpdateNode(float ratio)
        {
            float num;
            if (ratio is 0f or 1f)
            {
                num = ratio;
            }
            else
            {
                num = CalculateNewRatio(ratio);
            }
            base.UpdateNode(num);
        }

        protected abstract float CalculateNewRatio(float ratio);
    }
}
