using System;

namespace Mokus2D.Effects.Actions
{
    public class DelayedAction(Action action, float seconds) : NodeAction
    {
        internal override void Start(float time)
        {
            elapsed = 0f;
        }

        public override void Update(float time)
        {
            base.Update(time);
            elapsed += time;
            if (elapsed > seconds)
            {
                action.Invoke();
                finished = true;
            }
        }

        private float elapsed;
    }
}
