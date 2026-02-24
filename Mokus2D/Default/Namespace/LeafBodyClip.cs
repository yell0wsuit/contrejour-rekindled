using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class LeafBodyClip : ForegroundBase
    {
        public LeafBodyClip(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            float @float = _config.GetFloat("angleOffset", 0f);
            float num = _config.GetFloat("rotationSpeed") / 30f;
            rotationChanger = new CosChanger(-@float, @float, num);
            initialRotation = clip.Rotation;
        }

        public override void Update(float time)
        {
            base.Update(time);
            rotationChanger.Update(time);
            clip.Rotation = initialRotation + rotationChanger.Value;
        }

        protected CosChanger rotationChanger;

        protected float initialRotation;
    }
}
