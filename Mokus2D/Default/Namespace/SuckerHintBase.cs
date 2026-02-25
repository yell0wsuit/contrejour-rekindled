using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class SuckerHintBase : FadeHint
    {
        public SuckerHintBase(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            sucker = (SuckerBodyClip)FarseerUtil.Query(builder.World, builder.ToIPhoneVec(clip.Position), QUERY_RADIUS, typeof(SuckerBodyClip));
        }

        protected SuckerBodyClip sucker;

        private static readonly float QUERY_RADIUS = 200f * Box2DConfig.DefaultConfig.SizeMultiplier;
    }
}
