using System;
using System.Collections.Generic;

using Default.Namespace;

namespace Mokus2D.Util
{
    public class Scheduler
    {
        public void AddUpdatable(IUpdatable updatable)
        {
            updatables.Add(updatable);
        }

        public void RemoveUpdatable(IUpdatable updatable)
        {
            updatables.Remove(updatable);
        }

        public void Schedule(Action action)
        {
            action.Invoke();
        }

        public IDisposable Schedule(Action action, float seconds)
        {
            Task task = tasksPool.New();
            task.Initialize(this, seconds, action);
            tasks.Add(task);
            return task;
        }

        public void Update(float time)
        {
            totalGameTime += time;
            foreach (IUpdatable updatable in updatables)
            {
                updatable.Update(time);
            }
            for (int i = tasks.Count - 1; i >= 0; i--)
            {
                Task task = tasks[i];
                task.Update(time);
                if (task.TimeLeft <= 0f)
                {
                    toRun.Add(task.Action);
                    tasks.RemoveAt(i);
                    tasksPool.Delete(task);
                    task.RemoveReferences();
                }
            }
            for (int j = 0; j < toRun.Count; j++)
            {
                toRun[j].Invoke();
            }
            toRun.Clear();
        }

        private Pool<Task> tasksPool = new(64, true, false, null, null);

        private float totalGameTime;

        private List<Task> tasks = new();

        private List<Action> toRun = new();

        private List<IUpdatable> updatables = new();

        public class Task : IUpdatable, IDisposable
        {
            public Action Action { get; private set; }

            public float TimeLeft => timeLeft;

            public void Initialize(Scheduler scheduler, float timeLeft, Action action)
            {
                this.scheduler = scheduler;
                this.timeLeft = timeLeft;
                Action = action;
            }

            public void RemoveReferences()
            {
                scheduler = null;
                Action = null;
            }

            public void Update(float time)
            {
                timeLeft -= time;
            }

            public void Dispose()
            {
                RemoveReferences();
                scheduler.tasks.Remove(this);
            }

            private float timeLeft;

            private Scheduler scheduler;
        }
    }
}
