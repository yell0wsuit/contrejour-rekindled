using Mokus2D.Visual;

namespace Default.Namespace
{
    public class KaktusBodyClip : ContreJourBodyClip
    {
        public KaktusBodyClip(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            string text = _builder.ContreJour.ChooseSide("Black", null, "_5", null, "_6");
            if (text != null)
            {
                string text2 = _clip.Texture.Name;
                text2 += text;
                if (ClipFactory.HasConfig(text2))
                {
                    _clip = (Sprite)_builder.ReplaceClipWith(_clip, text2);
                }
            }
            _clip.Parent.ChangeChildLayer(_clip, -2);
        }
    }
}
