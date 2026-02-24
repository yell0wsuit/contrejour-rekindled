using System;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SpringBridgeHint : FadeHint
    {
        public SpringBridgeHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            sucker = (SpringSuckerBodyClip)FarseerUtil.Query(builder.World, builder.ToIPhoneVec(clip.Position), QUERY_RADIUS, typeof(SpringSuckerBodyClip));
            Restart();
        }

        private void RemoveListeners()
        {
            sucker.ContactEvent.RemoveListenerSelector(new Action(OnContact));
            sucker.FinishDragEvent.RemoveListenerSelector(new Action(OnContact));
            sucker.RemoveEvent.RemoveListenerSelector(new Action(OnContact));
        }

        private new void Restart()
        {
            base.Restart();
            RemoveListeners();
            sucker.ContactEvent.AddListenerSelector(new Action(OnContact));
            sucker.FinishDragEvent.AddListenerSelector(new Action(OnContact));
            sucker.RemoveEvent.AddListenerSelector(new Action(OnContact));
        }

        private void OnContact()
        {
            Hide();
            RemoveListeners();
        }

        public override bool HasToHide()
        {
            return false;
        }

        private static readonly float QUERY_RADIUS = 200f * Box2DConfig.DefaultConfig.SizeMultiplier;

        protected SpringSuckerBodyClip sucker;
    }
}
