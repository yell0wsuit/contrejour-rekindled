using System;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public abstract class TouchEffect(Node node)
    {
        public float EffectTime
        {
            get => effectTime; set => effectTime = value;
        }

        public TouchEffect(TouchSprite sprite)
            : this((Node)sprite)
        {
            sprite.TouchBeganEvent.AddListenerSelector(new Action(OnTouchBegan));
            sprite.TouchEndEvent.AddListenerSelector(new Action(OnTouchEnd));
            sprite.TouchOutEvent.AddListenerSelector(new Action(OnTouchEnd));
        }

        public bool IsOn
        {
            get => isOn;
            set
            {
                if (value != isOn)
                {
                    isOn = value;
                    if (value)
                    {
                        Node.Run(OnAction());
                    }
                    else
                    {
                        Node.Run(OffAction());
                    }
                    ChangeEvent.SendEvent();
                }
            }
        }

        public abstract NodeAction OnAction();

        public abstract NodeAction OffAction();

        public void OnTouchBegan()
        {
            IsOn = true;
        }

        public void OnTouchEnd()
        {
            IsOn = false;
        }

        protected Node Node = node;

        protected float effectTime = 0.1f;

        protected bool isOn;

        public readonly EventSender ChangeEvent = new();
    }
}
