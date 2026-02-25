using System;

namespace Mokus2D.Default.Namespace
{
    public class CallAfterData(Action _selector, float _time)
    {
        public float TimeLeft
        {
            get => _time; set => _time = value;
        }

        public Action Selector => _selector;

        public bool Skip { get; set; }

        public virtual void Execute()
        {
            _selector.Invoke();
        }
    }
}
