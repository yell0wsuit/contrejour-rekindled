using Microsoft.Xna.Framework;

namespace Mokus2D.Visual
{
    public abstract class AnchorNode : SpriteBatchNode
    {
        public virtual Vector2 Size
        {
            get => size; protected set => size = value;
        }

        public Vector2 Anchor
        {
            get => anchor; set => anchor = value;
        }

        public float AnchorX
        {
            get => Anchor.X; set => Anchor = new Vector2(value, Anchor.Y);
        }

        public float AnchorY
        {
            get => Anchor.Y; set => Anchor = new Vector2(Anchor.X, value);
        }

        public Vector2 AnchorInPixels
        {
            get => anchor * Size; set => anchor = value / Size;
        }

        private Vector2 anchor = new(0.5f, 0.5f);

        private Vector2 size;
    }
}
