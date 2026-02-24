using Microsoft.Xna.Framework;

namespace Mokus2D.Visual
{
    public abstract class AnchorNode : SpriteBatchNode
    {
        public virtual Vector2 Size
        {
            get
            {
                return size;
            }
            protected set
            {
                size = value;
            }
        }

        public Vector2 Anchor
        {
            get
            {
                return anchor;
            }
            set
            {
                anchor = value;
            }
        }

        public float AnchorX
        {
            get
            {
                return Anchor.X;
            }
            set
            {
                Anchor = new Vector2(value, Anchor.Y);
            }
        }

        public float AnchorY
        {
            get
            {
                return Anchor.Y;
            }
            set
            {
                Anchor = new Vector2(Anchor.X, value);
            }
        }

        public Vector2 AnchorInPixels
        {
            get
            {
                return anchor * Size;
            }
            set
            {
                anchor = value / Size;
            }
        }

        private Vector2 anchor = new(0.5f, 0.5f);

        private Vector2 size;
    }
}
