using System;
using System.Collections.Generic;

namespace Mokus2D.Default.Namespace
{
    public class EventSender<T> : EventSender
    {
        public void SendEvent(T eventObject)
        {
            SendObject(eventObject);
            base.SendEvent();
        }

        public static EventSender<T> operator +(EventSender<T> eventSender, Action<T> action)
        {
            eventSender.AddListenerSelector(action);
            return eventSender;
        }

        public static EventSender<T> operator -(EventSender<T> eventSender, Action<T> action)
        {
            eventSender.RemoveListenerSelector(action);
            return eventSender;
        }

        public override void SendEvent()
        {
            base.SendEvent();
            SendObject(default(T));
        }

        private void SendObject(T eventObject)
        {
            parameterListenersCopy.Clear();
            parameterListenersCopy.AddRange(parameterListeners);
            foreach (Action<T> action in parameterListenersCopy)
            {
                action.Invoke(eventObject);
            }
        }

        public void AddListenerSelector(Action<T> action)
        {
            parameterListeners.Add(action);
        }

        public void RemoveListenerSelector(Action<T> action)
        {
            _ = parameterListeners.Remove(action);
        }

        public override void RemoveListeners()
        {
            base.RemoveListeners();
            parameterListeners.Clear();
        }

        protected List<Action<T>> parameterListeners = new(64);

        private readonly List<Action<T>> parameterListenersCopy = new(64);
    }
}
