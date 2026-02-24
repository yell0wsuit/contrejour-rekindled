using System;

namespace Mokus2D.Effects.Actions
{
    public class EaseInOut(IntervalActionBase action, float rate) : EaseAction(action, rate)
    {
        internal override void UpdateNode(float ratio)
        {
            int num = 1;
            int num2 = (int)rate;
            if (num2 % 2 == 0)
            {
                num = -1;
            }
            ratio *= 2f;
            if (ratio < 1f)
            {
                base.UpdateNode((float)(0.5 * Math.Pow((double)ratio, rate)));
                return;
            }
            base.UpdateNode((float)((double)(num * 0.5f) * (Math.Pow((double)(ratio - 2f), rate) + num * 2)));
        }
    }
}
