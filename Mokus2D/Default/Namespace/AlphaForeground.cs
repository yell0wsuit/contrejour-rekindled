using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class AlphaForeground : ForegroundBase, IUpdatable
    {
        public AlphaForeground(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            float @float = _config.GetFloat("alphaStep");
            changer = new CosChanger(@float, @float);
            changer.MaxValue = _config.GetFloat("maximumAlpha");
            changer.MinValue = _config.GetFloat("minimumAlpha");
        }

        public override void Update(float time)
        {
            changer.Update(time);
            clip.OpacityFloat = changer.Value;
        }

        protected CosChanger changer;
    }
}
