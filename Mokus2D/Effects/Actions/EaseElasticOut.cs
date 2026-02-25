using System;

namespace Mokus2D.Effects.Actions
{
    public class EaseElasticOut(IntervalActionBase action, float rate) : EaseElastic(action, rate)
    {
        protected override float CalculateNewRatio(float ratio)
        {
            float num = rate / 4f;
            return (float)((Math.Pow(2.0, (double)(-10f * ratio)) * Math.Sin((double)((ratio - num) * 6.2831855f / rate))) + 1.0);
        }
    }
}
