using Mokus2D.Visual;

namespace Mokus2D.Effects.Actions
{
    public abstract class NodeAction
    {
        public bool Finished
        {
            get
            {
                return finished;
            }
        }

        internal virtual void Reset()
        {
            started = false;
            finished = false;
        }

        internal virtual void Start(float time)
        {
        }

        public virtual void Update(float time)
        {
            if (!started)
            {
                Start(time);
                started = true;
            }
        }

        public bool Test;

        public Node Target;

        protected bool finished;

        private bool started;
    }
}
