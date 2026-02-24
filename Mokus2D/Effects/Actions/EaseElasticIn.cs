using System;

namespace Mokus2D.Effects.Actions
{
    public class EaseElasticIn(IntervalActionBase action, float rate) : EaseElastic(action, rate)
    {
        protected override float CalculateNewRatio(float ratio)
        {
            float num = rate / 4f;
            ratio -= 1f;
            return (float)(-(float)Math.Pow(2.0, (double)(10f * ratio)) * Math.Sin((double)((ratio - num) * 6.2831855f / rate)));
        }
    }
}
