using System;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class PortalHint : FadeHint
    {
        public PortalHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            Restart();
        }

        public override void Restart()
        {
            base.Restart();
            portal = (TeleportBodyClip)FarseerUtil.Query(builder.World, builder.ToIPhoneVec(clip.Position), 6.6666665f, typeof(TeleportBodyClip));
            portal.UseEvent.AddListenerSelector(new Action(OnPortalUse));
        }

        public virtual void OnPortalUse()
        {
            portal.UseEvent.RemoveListenerSelector(new Action(OnPortalUse));
            Hide(0.5f * clip.Opacity / 255f);
        }

        public override bool HasToHide()
        {
            return false;
        }

        private const float QUERY_RADIUS = 6.6666665f;

        protected TeleportBodyClip portal;
    }
}
