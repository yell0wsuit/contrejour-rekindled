using System;

namespace Mokus2D.Effects.Actions
{
    public class EaseIn(IntervalActionBase action, float rate) : EaseAction(action, rate)
    {
        internal override void UpdateNode(float ratio)
        {
            base.UpdateNode((float)Math.Pow((double)ratio, rate));
        }
    }
}
