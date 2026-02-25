using Mokus2D.Effects.Actions;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class ToggleButton : Button
    {
        public Sprite ToggleIcon => toggleIcon;

        public ToggleButton(string _iconName, string _toggleName)
            : base(_iconName)
        {
            CreateToggle(_toggleName);
        }

        public ToggleButton(string backgroundFile, string _iconName, string _toggleName)
            : base(backgroundFile, "McButtonPressed", _iconName)
        {
            CreateToggle(_toggleName);
        }

        public ToggleButton(string backgroundFile, string _pressedName, string _iconName, string _toggleName)
            : base(backgroundFile)
        {
            CreateToggle(_toggleName);
        }

        public bool Toggle
        {
            get => toggle;
            set
            {
                if (toggle != value)
                {
                    toggle = value;
                    RefreshToggle();
                }
            }
        }

        public void CreateToggle(string _toggleName)
        {
            toggleIcon = ClipFactory.CreateWithAnchor(_toggleName);
            AddChild(toggleIcon);
            toggleIcon.Visible = false;
            toggleIcon.Opacity = 0;
        }

        public override void TouchBegan(Touch touch)
        {
            base.TouchBegan(touch);
            if (enabled)
            {
                RefreshToggle();
            }
        }

        public override void TouchOut(Touch touch)
        {
            base.TouchOut(touch);
            if (enabled)
            {
                RefreshToggle();
            }
        }

        public override void TouchEnd(Touch touch)
        {
            base.TouchEnd(touch);
            if (enabled)
            {
                RefreshToggle();
            }
        }

        public override void Click(Touch touch)
        {
            toggle = !toggle;
            RefreshToggle();
            base.Click(touch);
        }

        public void RefreshToggle()
        {
            toggleIcon.StopAllActions();
            if (toggle || touching)
            {
                toggleIcon.Visible = true;
                toggleIcon.Run(new FadeTo(0.2f, 1f));
                return;
            }
            toggleIcon.Run(new Sequence(
            [
                new FadeTo(0.2f, 0f),
                new Hide()
            ]));
        }

        protected bool toggle;

        protected Sprite toggleIcon;
    }
}
