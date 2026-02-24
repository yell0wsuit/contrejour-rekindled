using System;

namespace Mokus2D.Effects.Actions
{
    public class InstantAction(Action action) : InstantActionBase
    {
        protected override void Execute(float time)
        {
            action.Invoke();
        }
    }
}
