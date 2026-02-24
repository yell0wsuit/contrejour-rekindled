using System;

namespace Default.Namespace
{
    public class CallAfterData<T>(Action<T> _selector, float _time, T parameter) : CallAfterData(null, _time)
    {
        public override void Execute()
        {
            _selector.Invoke(parameter);
        }
    }
}
