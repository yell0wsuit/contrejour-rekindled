using System;

namespace Default.Namespace
{
    public class Updater : IUpdatable
    {
        public bool Paused
        {
            get => paused; set => paused = value;
        }

        public Updater()
        {
            callAfters = new ArrayList();
            pendingCallAfters = new ArrayList();
        }

        public CallAfterData<T> CallAfterSelectorDelayParameter<T>(Action<T> selector, float time, T parameter)
        {
            CallAfterData<T> callAfterData = new(selector, time, parameter);
            AddCallAfter(callAfterData);
            return callAfterData;
        }

        public CallAfterData CallAfterSelectorDelay(Action selector, float time)
        {
            CallAfterData callAfterData = new(selector, time);
            AddCallAfter(callAfterData);
            return callAfterData;
        }

        public void AddCallAfter(CallAfterData data)
        {
            if (!inCallAfterLoop)
            {
                callAfters.Add(data);
                return;
            }
            pendingCallAfters.Add(data);
        }

        public void Update(float time)
        {
            if (!paused)
            {
                ExecuteCallAfters(time);
            }
        }

        public void ExecuteCallAfters(float time)
        {
            inCallAfterLoop = true;
            ArrayList arrayList = new();
            foreach (object obj in callAfters)
            {
                CallAfterData callAfterData = (CallAfterData)obj;
                callAfterData.TimeLeft -= time;
                if (callAfterData.TimeLeft <= 0f)
                {
                    if (!callAfterData.Skip)
                    {
                        callAfterData.Execute();
                    }
                    arrayList.Add(callAfterData);
                }
            }
            foreach (object obj2 in arrayList)
            {
                callAfters.Remove(obj2);
            }
            callAfters.AddRange(pendingCallAfters);
            pendingCallAfters.Clear();
            inCallAfterLoop = false;
        }

        protected ArrayList callAfters;

        protected ArrayList pendingCallAfters;

        protected bool inCallAfterLoop;

        protected float totalTime;

        protected bool paused;
    }
}
