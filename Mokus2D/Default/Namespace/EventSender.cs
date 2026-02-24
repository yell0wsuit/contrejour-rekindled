using System;
using System.Collections.Generic;

namespace Default.Namespace
{
    public class EventSender
    {
        public bool Enabled { get; set; }

        public EventSender()
        {
            Enabled = true;
        }

        public virtual void SendEvent()
        {
            if (!Enabled)
            {
                return;
            }
            listenersCopy.Clear();
            listenersCopy.AddRange(listeners);
            foreach (Action action in listenersCopy)
            {
                action.Invoke();
            }
        }

        public static EventSender operator +(EventSender eventSender, Action action)
        {
            eventSender.AddListenerSelector(action);
            return eventSender;
        }

        public static EventSender operator -(EventSender eventSender, Action action)
        {
            eventSender.RemoveListenerSelector(action);
            return eventSender;
        }

        public void AddListenerSelector(Action selector)
        {
            listeners.Add(selector);
        }

        public virtual void RemoveListeners()
        {
            listeners.Clear();
        }

        public void RemoveListenerSelector(Action selector)
        {
            listeners.Remove(selector);
        }

        protected List<Action> listeners = new(64);

        private List<Action> listenersCopy = new(64);
    }
}
