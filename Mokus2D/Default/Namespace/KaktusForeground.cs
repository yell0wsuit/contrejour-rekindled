using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class KaktusForeground : ForegroundBase
    {
        public KaktusForeground(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            string text = _builder.ContreJour.ChooseSide("Black", null, "_5", null, "_6");
            if (text != null)
            {
                string text2 = _clip.Texture.Name + text;
                _clip = (Sprite)_builder.ReplaceClipWith(_clip, text2);
                _builder.ChangeChildLayer(_clip, -2);
            }
        }
    }
}
