using Microsoft.Xna.Framework.Graphics;

namespace Mokus2D.Visual
{
    public abstract class LabelBase : AnchorNode
    {
        public SpriteFont Font { get; set; }

        protected LabelBase(SpriteFont font)
        {
            Font = font;
        }

        protected LabelBase()
        {
        }
    }
}
