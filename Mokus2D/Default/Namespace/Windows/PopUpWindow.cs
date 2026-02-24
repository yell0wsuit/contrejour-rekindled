using System;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace.Windows
{
    public class PopUpWindow : Node
    {
        public PopUpWindow()
        {
            AddChild(container);
            clickableLayer.Enabled = false;
            container.AddChild(clickableLayer, 1);
            LayerColor layerColor = new(Color.Black);
            AddChild(layerColor);
            howerEffect = new FadeEffect(layerColor);
            layerColor.Visible = false;
            layerColor.Opacity = 0;
            container.Visible = false;
            UpdateEnabled = false;
        }

        public bool Open
        {
            get
            {
                return open;
            }
            set
            {
                if (open != value)
                {
                    open = value;
                    if (action != null)
                    {
                        StopAction(action);
                    }
                    if (open)
                    {
                        OnOpen();
                        Visible = true;
                        howerEffect.IsOn = true;
                        action = Schedule(new Action(ShowItems), howerEffect.EffectTime);
                        container.Visible = false;
                        UpdateEnabled = true;
                    }
                    else
                    {
                        howerEffect.IsOn = true;
                        action = Schedule(new Action(HideItems), howerEffect.EffectTime);
                        clickableLayer.Enabled = false;
                    }
                    OpenChangeEvent.SendEvent();
                }
            }
        }

        protected virtual void OnOpen()
        {
        }

        private void HideItems()
        {
            action = Schedule(new Action(Disable), howerEffect.EffectTime);
            howerEffect.IsOn = false;
            container.Visible = false;
        }

        private void Disable()
        {
            UpdateEnabled = (Visible = false);
        }

        private void ShowItems()
        {
            howerEffect.IsOn = false;
            container.Visible = true;
            clickableLayer.Enabled = true;
        }

        public readonly EventSender OpenChangeEvent = new();

        private FadeEffect howerEffect;

        private bool open;

        protected readonly Node container = new();

        protected readonly ClickableLayer clickableLayer = new(0);

        private NodeAction action;
    }
}
