using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Data;

namespace Mokus2D.Visual
{
    public class IntLabel(SpriteFont font) : LabelBase(font)
    {
        public int TextValue { get; set; }

        protected override void DrawSprite(VisualState state, Color color)
        {
            batch.DrawInt32(Font, TextValue, Vector2.Zero, color, 0f, Vector2.Zero, state.SpritesScaleFactor, SpriteEffects.None, 0);
        }

        public override Vector2 Size => default(Vector2);
    }
}
