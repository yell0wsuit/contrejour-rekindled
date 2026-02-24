using System;

using Mokus2D.Visual;

namespace Mokus2D.Effects.Actions
{
    public class IntervalAction(TimeSpan interval, Action<Node, float> updateAction) : IntervalActionBase(interval)
    {
        internal override void UpdateNode(float ratio)
        {
            updateAction.Invoke(Target, Math.Min(ratio, 1f));
        }
    }
}
