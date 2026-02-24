using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class HintBase : BodyClip, IRemovable, IUpdatable
    {
        public HintBase(ContreJourLevelBuilder _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            contreJour = _builder.ContreJour;
            _builder.Game.AddUpdatable(this);
            contreJour.AddTextureToUnload(_clip.Texture.Name);
            AddText(config.GetHashtable("textData"));
            int num = 0;
            while (config.Exists("textData" + num.ToString()))
            {
                Hashtable hashtable = config.GetHashtable("textData" + num.ToString());
                if (hashtable != null)
                {
                    AddText(hashtable);
                }
                num++;
            }
        }

        private void AddText(Hashtable textData)
        {
            Vector2 vector = textData.GetVector("textSize");
            Vector2 vector2 = textData.GetVector("textPosition");
            int @int = textData.GetInt("fontSize");
            string @string = textData.GetString("textAlign");
            float num = 0.5f;
            if (@string == "right")
            {
                num = 1f;
                vector2.X += vector.X / 2f;
            }
            else if (@string == "left")
            {
                num = 0f;
                vector2.X -= vector.X / 2f;
            }
            string string2 = textData.GetString("text");
            MultilineLabel multilineLabel = ContreJourLabel.CreateMultilineLabel(@int / 2, string2.Localize());
            multilineLabel.Anchor = new Vector2(num, 0.5f);
            if (textData.Exists("color"))
            {
                multilineLabel.Color = textData.GetUInt("color").ToRGBColor();
            }
            multilineLabel.Position = CocosUtil.toIPad(vector2);
            multilineLabel.Y += (vector.Y - multilineLabel.Size.Y) / 2f;
            clip.AddChild(multilineLabel);
        }

        public bool HasRemove()
        {
            return false;
        }

        private const string TEXT_DATA = "textData";

        protected ContreJourGame contreJour;
    }
}
