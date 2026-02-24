using System;

namespace Mokus2D.Effects.Actions
{
    public abstract class IntervalActionBase(TimeSpan interval) : NodeAction
    {
        public TimeSpan Interval
        {
            get
            {
                return interval;
            }
        }

        internal override void Start(float time)
        {
            base.Start(time);
            elapsed = 0f;
        }

        public override void Update(float time)
        {
            base.Update(time);
            elapsed += time;
            float num = elapsed / (float)interval.TotalSeconds;
            UpdateNode(Math.Min(num, 1f));
            if (num >= 1f)
            {
                DoFinish();
            }
        }

        protected void DoFinish()
        {
            finished = true;
            Finish();
        }

        protected virtual void Finish()
        {
        }

        internal virtual void UpdateNode(float ratio)
        {
        }

        protected TimeSpan interval = interval;

        private float elapsed;
    }
}
