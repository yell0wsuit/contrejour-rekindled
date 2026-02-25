using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public struct CGSize
    {
        public CGSize(Point pt)
        {
            width = pt.X;
            height = pt.Y;
        }

        public CGSize(Vector2 v)
        {
            width = v.X;
            height = v.Y;
        }

        public CGSize(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public static CGSize operator +(CGSize sz1, CGSize sz2)
        {
            return Add(sz1, sz2);
        }

        public static CGSize operator -(CGSize sz1, CGSize sz2)
        {
            return Subtract(sz1, sz2);
        }

        public static bool operator ==(CGSize sz1, CGSize sz2)
        {
            return sz1.Width == sz2.Width && sz1.Height == sz2.Height;
        }

        public static bool operator !=(CGSize sz1, CGSize sz2)
        {
            return !(sz1 == sz2);
        }

        public readonly bool IsEmpty => width == 0f && height == 0f;

        public float Width
        {
            readonly get => width; set => width = value;
        }

        public float Height
        {
            readonly get => height; set => height = value;
        }

        public static implicit operator Vector2(CGSize size)
        {
            return new Vector2(size.Width, size.Height);
        }

        public static implicit operator CGSize(Vector2 size)
        {
            return new CGSize(size.X, size.Y);
        }

        public readonly Vector2 ToVector2()
        {
            return new Vector2(width, height);
        }

        public readonly CGSize Negg()
        {
            return new CGSize(-width, -height);
        }

        public static CGSize Add(CGSize sz1, CGSize sz2)
        {
            return new CGSize(sz1.Width + sz2.Width, sz1.Height + sz2.Height);
        }

        public static CGSize Subtract(CGSize sz1, CGSize sz2)
        {
            return new CGSize(sz1.Width - sz2.Width, sz1.Height - sz2.Height);
        }

        public override readonly bool Equals(object obj)
        {
            if (!(obj is CGSize))
            {
                return false;
            }
            CGSize cgsize = (CGSize)obj;
            return cgsize.width == width && cgsize.height == height;
        }

        public override readonly string ToString()
        {
            return string.Concat(new string[]
            {
                "{Width=",
                width.ToString(),
                ", Height=",
                height.ToString(),
                "}"
            });
        }

        public static readonly CGSize Empty = default(CGSize);

        private float width;

        private float height;
    }
}
