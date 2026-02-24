using System;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class RotatorHint : FadeHint
    {
        public RotatorHint(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            rotator = (RotatorBodyClip)FarseerUtil.Query(builder.World, builder.ToIPhoneVec(clip.Position), 3f, typeof(RotatorBodyClip));
            clip.Parent.ChangeChildLayer(clip, 12);
        }

        public override bool HasToHide()
        {
            return false;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!hiding && rotator != null && (double)builder.Game.TotalTime > 0.5 && Math.Abs(Maths.PeriodicOffset(rotator.Body.Rotation, 6.2831855f)) > 0.7853982f)
            {
                Hide();
            }
        }

        protected RotatorBodyClip rotator;
    }
}
