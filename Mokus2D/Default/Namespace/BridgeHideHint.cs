using System;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class BridgeHideHint : SuckerHintBase
    {
        public BridgeHideHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            hasToRun = false;
            sucker.FinishDragEvent.AddListenerSelector(new Action(OnFinishDrag));
        }

        public override void Restart()
        {
            base.Restart();
            hasToRun = false;
            sucker.FinishDragEvent.RemoveListenerSelector(new Action(OnFinishDrag));
            sucker.RemoveEvent.RemoveListenerSelector(new Action(OnRemoveBridge));
            sucker.FinishDragEvent.AddListenerSelector(new Action(OnFinishDrag));
        }

        public override bool HasToHide()
        {
            return false;
        }

        private void OnFinishDrag()
        {
            hasToRun = true;
            sucker.FinishDragEvent.RemoveListenerSelector(new Action(OnFinishDrag));
            sucker.RemoveEvent.AddListenerSelector(new Action(OnRemoveBridge));
        }

        private void OnRemoveBridge()
        {
            sucker.RemoveEvent.RemoveListenerSelector(new Action(OnRemoveBridge));
            Hide();
        }
    }
}
