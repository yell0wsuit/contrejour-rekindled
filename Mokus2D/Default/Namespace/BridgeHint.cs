using System;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class BridgeHint : SuckerHintBase
    {
        public BridgeHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            sucker.FinishDragEvent.AddListenerSelector(new Action(OnFinishDrag));
        }

        public override void Restart()
        {
            if (hiding)
            {
                sucker.FinishDragEvent.AddListenerSelector(new Action(OnFinishDrag));
            }
            base.Restart();
        }

        private void OnFinishDrag()
        {
            sucker.FinishDragEvent.RemoveListenerSelector(new Action(OnFinishDrag));
            Hide();
        }

        public override bool HasToHide()
        {
            return false;
        }
    }
}
