using System;
using System.Collections.Generic;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class MoveHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config) : FadeHint(_builder, _body, _clip, _config)
    {
        private void GetPoint()
        {
            List<BodyClip> list = FarseerUtil.QueryAll(builder.World, builder.ToVec(clip.Position), 6.6666665f, typeof(SnotPoint));
            foreach (BodyClip bodyClip in list)
            {
                if (bodyClip is SnotPoint && ((SnotPoint)bodyClip).Used)
                {
                    point = (SnotPoint)bodyClip;
                    point.UnuseEvent.AddListenerSelector(new Action(OnUnuse));
                    break;
                }
            }
        }

        public override bool HasToHide()
        {
            return false;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (point == null)
            {
                GetPoint();
            }
        }

        public override void Restart()
        {
            base.Restart();
            used = false;
        }

        private void OnUnuse()
        {
            if (!used)
            {
                used = true;
                hiding = true;
                Hide(0.5f * clip.Opacity / 255f);
            }
        }

        private const float QUERY_RADIUS = 6.6666665f;

        private SnotPoint point;

        private bool used;
    }
}
