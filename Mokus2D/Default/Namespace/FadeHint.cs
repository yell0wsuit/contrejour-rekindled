using System;
using System.Collections.Generic;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class FadeHint : HintBase, IRemovable, IUpdatable, IRestartable
    {
        public FadeHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            _builder.ContreJour.AddUpdatable(this);
            _builder.ContreJour.AddTextureToUnload(_clip.Texture.Name);
            clip.Opacity = 0;
            clip.Visible = false;
            hasToRun = true;
        }

        public virtual void Restart()
        {
            clip.StopAllActions();
            clip.Run(new Sequence(
            [
                new FadeTo(0.2f, 0f),
                new Hide()
            ]));
            hasToRun = true;
            hiding = false;
            foreach (DelayedAction delayedAction in callAfters)
            {
                Unschedule(delayedAction);
            }
            callAfters.Clear();
        }

        private DelayedAction CallAfterDelay(Action action, float delay)
        {
            DelayedAction delayedAction = Schedule(action, delay);
            callAfters.Add(delayedAction);
            return delayedAction;
        }

        public virtual bool HasToHide()
        {
            return true;
        }

        public override void Update(float time)
        {
            if (((ContreJourGame)builder.Game).TouchEnabled && hasToRun)
            {
                hasToRun = false;
                _ = CallAfterDelay(new Action(Show), 2f);
            }
        }

        public void Show()
        {
            Show(2f);
        }

        public void Show(float time)
        {
            if (hiding)
            {
                return;
            }
            clip.Visible = true;
            clip.StopAllActions();
            clip.Run(new FadeIn(2f));
            if (HasToHide())
            {
                _ = CallAfterDelay(new Action(Hide), 5f);
            }
        }

        public void Hide(float time)
        {
            clip.StopAllActions();
            clip.Run(new Sequence(
            [
                new FadeTo(time, 0f),
                new Hide()
            ]));
        }

        public void Hide()
        {
            if (!hiding)
            {
                hiding = true;
                Hide(clip.Opacity / 255f);
            }
        }

        public new bool HasRemove()
        {
            return false;
        }

        protected bool hasToRun;

        protected bool hiding;

        protected List<DelayedAction> callAfters = new();
    }
}
