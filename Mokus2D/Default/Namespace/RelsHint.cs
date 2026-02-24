using System;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class RelsHint : FadeHint
    {
        public RelsHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            rels = (DragableBodyClip)FarseerUtil.Query(builder.World, builder.ToVec(clip.Position), 6.6666665f, typeof(DragableBodyClip));
            rels.DragStartEvent.AddListenerSelector(new Action(OnDragStart));
        }

        public override void Restart()
        {
            base.Restart();
            used = false;
        }

        private void OnDragStart()
        {
            if (!used)
            {
                used = true;
                hiding = true;
                Hide(0.5f * clip.Opacity / 255f);
            }
        }

        private const float QUERY_RADIUS = 6.6666665f;

        protected DragableBodyClip rels;

        protected bool used;
    }
}
