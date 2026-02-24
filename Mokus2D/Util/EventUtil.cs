using System;

namespace Mokus2D.Util
{
    public static class EventUtil
    {
        public static void Dispatch(this EventHandler value, object sender)
        {
            if (value != null)
            {
                value.Invoke(sender, EventArgs.Empty);
            }
        }

        public static void Dispatch<T>(this Action<T> value, T argument)
        {
            if (value != null)
            {
                value.Invoke(argument);
            }
        }

        public static void Dispatch(this Action value)
        {
            if (value != null)
            {
                value.Invoke();
            }
        }
    }
}
