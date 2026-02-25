using Default.Namespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Data;

namespace Mokus2D.Visual
{
    public class Sprite : AnchorNode, ITextureNode, IAnchorNode, ISizeNode
    {
        protected virtual Rectangle? GetTileRectangle()
        {
            return default(Rectangle?);
        }

        public Texture2D Texture => texture;

        public ClipData Config => config;

        public Sprite(TextureData data)
            : this(data.Texture)
        {
            config = data.Config;
            Size = config.Size;
            Anchor = config.Anchor;
        }

        public Sprite(string name)
            : this(ClipFactory.GetAnchorConfig(name))
        {
        }

        public Sprite(Texture2D texture)
        {
            this.texture = texture;
            Size = new Vector2(texture.Width, texture.Height);
            if (!string.IsNullOrEmpty(texture.Name))
            {
                Mokus2DGame.SharedContent.IncreaseReferenceCount(texture.Name);
            }
        }

        protected override void DrawSprite(VisualState state, Color color)
        {
            batch.Draw(texture, Vector2.Zero, GetTileRectangle(), color, 0f, AnchorInPixels, state.SpritesScaleFactor, SpriteEffects.None, 0f);
        }

        protected override void OnRemovedFromStage()
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!string.IsNullOrEmpty(texture.Name))
            {
                Mokus2DGame.SharedContent.DecreaseReferenceCount(texture.Name);
            }
        }

        protected Texture2D texture;

        protected ClipData config;
    }
}
