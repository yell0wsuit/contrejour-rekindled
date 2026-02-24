using System;
using System.Collections.Generic;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SnotLinkHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config) : FadeHint(_builder, null, _clip, _config)
    {
        public override bool HasToHide()
        {
            return false;
        }

        public override void Restart()
        {
            base.Restart();
            snotGot = false;
        }

        private void GetSnot()
        {
            List<BodyClip> list = FarseerUtil.QueryBodyClipsCenterRadiusType(builder.World, builder.ToIPhoneVec(clip.Position), 6.6666665f, typeof(SnotBodyClip));
            foreach (BodyClip bodyClip in list)
            {
                SnotBodyClip snotBodyClip = (SnotBodyClip)bodyClip;
                snotBodyClip.LinkEvent.AddListenerSelector(new Action(OnSnotLink));
            }
            snot = (SnotBodyClip)list[0];
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (Maths.FuzzyNotEquals(time, 0f, 0.0001f) && !snotGot)
            {
                GetSnot();
                snotGot = true;
            }
        }

        public virtual void CheckHeroDistance()
        {
        }

        public virtual void OnSnotLink()
        {
            hiding = true;
            Hide(0.5f * clip.Opacity / 255f);
        }

        private const float QUERY_RADIUS = 6.6666665f;

        protected SnotBodyClip snot;

        protected bool snotGot;
    }
}
