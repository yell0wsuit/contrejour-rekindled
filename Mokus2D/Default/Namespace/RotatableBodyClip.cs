using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class RotatableBodyClip : BodyClip
    {
        public RotatableBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            scaleDiff = Maths.RandRangeMinMax(0.1f, 0.25f);
            scaleStep = Maths.RandRangeMinMax(0.02f, 0.05f);
            scaleProgress = Maths.RandRangeMinMax(0f, 6.2831855f);
            angleDiff = Maths.RandRangeMinMax(4f, 8f);
            destroying = false;
            rotationDirection = 1;
            scaleSign = 1;
            clip.Rotation = Maths.Random(360);
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!destroying)
            {
                scaleProgress += scaleStep;
                clip.ScaleX = 1f + (Maths.Cos(scaleProgress) * scaleDiff);
                clip.ScaleX = scaleSign * clip.ScaleY;
            }
        }

        public void UpdateRotation(float time)
        {
            clip.Rotation += rotationDirection * angleDiff;
        }

        protected float scaleDiff;

        protected float scaleStep;

        protected float scaleProgress;

        protected float angleDiff;

        protected bool destroying;

        protected int rotationDirection;

        protected int scaleSign;
    }
}
