using System;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class SnotReleaseHint : SnotLinkHint
    {
        public SnotReleaseHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            hasToRun = false;
        }

        public override void Restart()
        {
            base.Restart();
            hasToRun = false;
            used = false;
        }

        public override bool HasToHide()
        {
            return false;
        }

        public override void CheckHeroDistance()
        {
        }

        public override void OnSnotLink()
        {
            if (!used)
            {
                used = true;
                Show();
                snot.ReleaseEvent.AddListenerSelector(new Action(OnSnotRelease));
            }
        }

        private void OnSnotRelease()
        {
            snot.ReleaseEvent.RemoveListenerSelector(new Action(OnSnotRelease));
            Hide(0.5f);
        }

        protected bool used;
    }
}
