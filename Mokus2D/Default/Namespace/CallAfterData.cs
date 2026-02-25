using System;

namespace Default.Namespace
{
    public class CallAfterData(Action _selector, float _time)
    {
        public float TimeLeft
        {
            get => _time; set => _time = value;
        }

        public Action Selector => _selector;

        public bool Skip
        {
            get => skip; set => skip = value;
        }

        public virtual void Execute()
        {
            _selector.Invoke();
        }

        private bool skip;
    }
}
