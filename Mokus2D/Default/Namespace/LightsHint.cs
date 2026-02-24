using System;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class LightsHint : FadeHint
    {
        public LightsHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            EnergyBodyClip energyBodyClip = (EnergyBodyClip)FarseerUtil.Query(builder.World, builder.ToIPhoneVec(clip.Position), QUERY_RADIUS, typeof(EnergyBodyClip));
            energyBodyClip.CollectEvent.AddListenerSelector(new Action(OnEnergyCollected));
        }

        public override bool HasToHide()
        {
            return false;
        }

        private void OnEnergyCollected()
        {
            if (!hiding)
            {
                hiding = true;
                Hide(0.5f * clip.Opacity / 255f);
            }
        }

        private static readonly float QUERY_RADIUS = CocosUtil.lite(100, 200) * 0.033333335f;
    }
}
