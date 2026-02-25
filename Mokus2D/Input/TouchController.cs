using System.Collections.Generic;

using Default.Namespace;

using Mokus2D.Visual.GameDebug;
using Mokus2D.win.PlatformSupport.Input;

namespace Mokus2D.Input
{
    public class TouchController : IUpdatable
    {
        public void Update(float time)
        {
            newTouches.Clear();
            toBegin.Clear();
            toMove.Clear();
            List<CursorPoint> cursorPoints = CursorPoints.GetCursorPoints();
            for (int i = 0; i < cursorPoints.Count; i++)
            {
                CursorPoint cursorPoint = cursorPoints[i];
                Touch touch = null;
                foreach (KeyValuePair<Touch, List<ITouchListener>> keyValuePair in touches)
                {
                    if (keyValuePair.Key.Id == cursorPoint.Id)
                    {
                        touch = keyValuePair.Key;
                        break;
                    }
                }
                if (touch == null)
                {
                    touch = touchesPool.New();
                    touch.Initialize(cursorPoint.Position, cursorPoint.Id);
                    toBegin.Add(touch);
                }
                else
                {
                    if (touch.Position != cursorPoint.Position)
                    {
                        toMove.Add(touch);
                    }
                    touch.Refresh(cursorPoint.Position);
                }
                newTouches.Add(touch);
            }
            toEnd.Clear();
            foreach (Touch touch2 in touches.Keys)
            {
                if (!newTouches.Contains(touch2))
                {
                    toEnd.Add(touch2);
                    touch2.Active = false;
                }
            }
            stoped.Clear();
            SendBegin(toBegin);
            SendMove(toMove);
            SendEndAndRemove(toEnd);
            listenersCopy.Clear();
        }

        public void StopPropagation(Touch touch)
        {
            if (!stoped.Contains(touch))
            {
                stoped.Add(touch);
            }
        }

        public void AddListener(ITouchListener listener, int priority = 0)
        {
            GetListeners(priority).Add(listener);
            priorities[listener] = priority;
        }

        public void RemoveListener(ITouchListener listener)
        {
            int num = priorities[listener];
            _ = priorities.Remove(listener);
            foreach (KeyValuePair<Touch, List<ITouchListener>> keyValuePair in touches)
            {
                if (keyValuePair.Value.Contains(listener))
                {
                    _ = keyValuePair.Value.Remove(listener);
                }
            }
            _ = GetListeners(num).Remove(listener);
        }

        private List<ITouchListener> GetListeners(int priority)
        {
            if (!listeners.ContainsKey(priority))
            {
                listeners[priority] = new List<ITouchListener>();
                prioritiesList.Add(priority);
                prioritiesList.Sort();
            }
            return listeners[priority];
        }

        private void SendEndAndRemove(List<Touch> toEnd)
        {
            foreach (Touch touch in toEnd)
            {
                List<ITouchListener> list = touches[touch];
                _ = touches.Remove(touch);
                touchesPool.Delete(touch);
                if (!stoped.Contains(touch))
                {
                    SendEnd(touch, list);
                }
                list.Clear();
                confirmedListenerPool.Delete(list);
            }
        }

        private void SendEnd(Touch touch, List<ITouchListener> listeners)
        {
            foreach (ITouchListener touchListener in listeners)
            {
                touchListener.TouchEnd(touch);
                if (stoped.Contains(touch))
                {
                    break;
                }
            }
        }

        private void SendMove(List<Touch> toMove)
        {
            foreach (Touch touch in toMove)
            {
                if (!stoped.Contains(touch))
                {
                    List<ITouchListener> list = touches[touch];
                    listenersCopy.Clear();
                    SendMove(touch, list);
                    list.Clear();
                    list.AddRange(listenersCopy);
                }
            }
        }

        private void SendMove(Touch touch, List<ITouchListener> listeners)
        {
            foreach (ITouchListener touchListener in listeners)
            {
                using (new GarbageTracer(touchListener.GetType(), true))
                {
                    if (touchListener.TouchMove(touch))
                    {
                        listenersCopy.Add(touchListener);
                    }
                    if (stoped.Contains(touch))
                    {
                        break;
                    }
                }
            }
        }

        private void SendBegin(List<Touch> toBegin)
        {
            if (toBegin.Count == 0)
            {
                return;
            }
            foreach (Touch touch in toBegin)
            {
                if (!stoped.Contains(touch))
                {
                    List<ITouchListener> list = confirmedListenerPool.New();
                    list.Clear();
                    touches[touch] = list;
                    SendBegin(touch, list);
                }
            }
        }

        private void SendBegin(Touch touch, List<ITouchListener> confirmedListeners)
        {
            foreach (int num in prioritiesList)
            {
                listenersCopy.Clear();
                listenersCopy.AddRange(listeners[num]);
                foreach (ITouchListener touchListener in listenersCopy)
                {
                    if (touchListener.TouchBegin(touch))
                    {
                        confirmedListeners.Add(touchListener);
                    }
                    if (stoped.Contains(touch))
                    {
                        return;
                    }
                }
            }
        }

        private const int MAX_TOUCHES = 16;

        private readonly Pool<List<ITouchListener>> confirmedListenerPool = new(16, true, false, null, null);

        private readonly List<ITouchListener> listenersCopy = new(64);

        private readonly List<Touch> newTouches = new(16);

        private readonly List<int> prioritiesList = new(64);

        private readonly List<Touch> stoped = new(16);

        private readonly List<Touch> toBegin = new(16);

        private readonly List<Touch> toEnd = new(16);

        private readonly List<Touch> toMove = new(16);

        private readonly Pool<Touch> touchesPool = new(16, true, false, null, null);

        private readonly Dictionary<int, List<ITouchListener>> listeners = new();

        private readonly Dictionary<ITouchListener, int> priorities = new(64);

        private readonly Dictionary<Touch, List<ITouchListener>> touches = new(16);
    }
}
