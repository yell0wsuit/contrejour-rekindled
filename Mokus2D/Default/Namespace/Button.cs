using System;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class Button : TouchSprite
    {
        public Button(string backgroundFile, string _pressedName, string _iconName)
            : base(backgroundFile)
        {
            realScale = 1f;
            enabled = true;
            if (_pressedName != null)
            {
                pressed = ClipFactory.CreateWithAnchor(_pressedName);
                AddChild(pressed);
                pressed.Visible = false;
                pressed.Opacity = 0;
            }
            if (_iconName != null)
            {
                icon = ClipFactory.CreateWithAnchor(_iconName);
                AddChild(icon);
            }
        }

        public Button(string backgroundFile, string _iconName)
            : this(backgroundFile, "McButtonPressed", _iconName)
        {
        }

        public Button(string _iconName)
            : this("McButtonBackground.png", _iconName)
        {
        }

        public float RealScale
        {
            get => realScale;
            set
            {
                realScale = value;
                Scale = value;
            }
        }

        public bool Enabled
        {
            get => enabled; set => enabled = value;
        }

        public Sprite Icon => icon;

        public static Button ButtonBigWithIcon(string _iconName)
        {
            return new Button("McButtonBackgroundBig.png", "McButtonPressedBig", _iconName);
        }

        private void SetIconRotation(float value)
        {
            icon.Rotation = value;
        }

        private float IconRotation()
        {
            return icon.Rotation;
        }

        public override void TouchBegan(Touch touch)
        {
            if (!enabled)
            {
                return;
            }
            if (StopEventPropagation)
            {
                Mokus2DGame.TouchController.StopPropagation(touch);
            }
            base.TouchBegan(touch);
            touching = true;
            if (pressed != null)
            {
                pressed.Visible = true;
                pressed.StopAllActions();
                pressed.Run(new FadeTo(0.1f, 1f));
            }
            Run(new ScaleTo(0.1f, realScale * 1.1f));
        }

        public override void TouchOut(Touch touch)
        {
            if (!enabled)
            {
                return;
            }
            base.TouchOut(touch);
            HidePressed();
        }

        public override void TouchEnd(Touch touch)
        {
            if (!enabled)
            {
                return;
            }
            base.TouchEnd(touch);
            HidePressed();
        }

        public override void Click(Touch touch)
        {
            if (!enabled)
            {
                return;
            }
            base.Click(touch);
            Mokus2DGame.SoundManager.PlaySound("click", 0.8f, 0f, 0f);
        }

        public void HidePressed()
        {
            touching = false;
            Schedule(new Action(DoHidePressed), 0.2f);
        }

        private void DoHidePressed()
        {
            if (!touching)
            {
                Run(new ScaleTo(0.1f, realScale));
                if (pressed != null)
                {
                    pressed.StopAllActions();
                    pressed.Run(new Sequence(
                    [
                        new FadeTo(0.3f, 0f),
                        new Hide()
                    ]));
                }
            }
        }

        private const float PRESS_SCALE = 1.1f;

        public bool StopEventPropagation;

        protected bool enabled;

        protected Sprite icon;

        protected Sprite pressed;

        protected float realScale;

        protected bool touching;
    }
}
