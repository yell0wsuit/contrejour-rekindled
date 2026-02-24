using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SpriteFader(Node _target)
    {
        public ushort EnabledOpacity
        {
            get
            {
                return enabledOpacity;
            }
            set
            {
                enabledOpacity = value;
            }
        }

        public ushort DisabledOpacity
        {
            get
            {
                return disabledOpacity;
            }
            set
            {
                disabledOpacity = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    NodeAction nodeAction = new FadeTo(duration, enabled ? enabledOpacity : disabledOpacity);
                    target.Run(nodeAction);
                }
            }
        }

        public float Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }

        protected Node target = _target;

        protected ushort enabledOpacity = 0;

        protected ushort disabledOpacity = 255;

        protected bool enabled;

        protected float duration = 0.15f;
    }
}
