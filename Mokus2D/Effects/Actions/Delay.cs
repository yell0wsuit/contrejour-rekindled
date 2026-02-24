using System;

namespace Mokus2D.Effects.Actions
{
    public class Delay(TimeSpan interval) : IntervalActionBase(interval)
    {
        public Delay(float seconds)
            : this(TimeSpan.FromSeconds((double)seconds))
        {
        }
    }
}
