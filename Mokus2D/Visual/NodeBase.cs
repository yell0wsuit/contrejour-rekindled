using System;

using Default.Namespace;

using Microsoft.Xna.Framework;

using Mokus2D.Util.MathUtils;
using Mokus2D.Util.Resources;

namespace Mokus2D.Visual
{
    public class NodeBase : DisposableBase, IUpdatable
    {
        public NodeBase()
        {
            ScaleVec = Vector2.One;
        }

        public virtual bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        public virtual Vector2 Position { get; set; }

        public virtual Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }

        public int Opacity
        {
            get
            {
                return (int)(OpacityFloat * 255f);
            }
            set
            {
                OpacityFloat = value / 255f;
            }
        }

        public virtual float OpacityFloat
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value.Clamp(0f, 1f);
            }
        }

        public float Scale
        {
            get
            {
                return ScaleVec.X == ScaleVec.Y ? ScaleVec.X : throw new InvalidOperationException("ScaleVec.X differs from ScaleVec.Y");
            }
            set
            {
                ScaleVec = new Vector2(value, value);
            }
        }

        public float ScaleX
        {
            get
            {
                return ScaleVec.X;
            }
            set
            {
                ScaleVec = new Vector2(value, ScaleVec.Y);
            }
        }

        public float ScaleY
        {
            get
            {
                return ScaleVec.Y;
            }
            set
            {
                ScaleVec = new Vector2(ScaleVec.X, value);
            }
        }

        public virtual Vector2 ScaleVec { get; set; }

        public virtual float RotationRadians { get; set; }

        public float Rotation
        {
            get
            {
                return Maths.ToDegrees(RotationRadians);
            }
            set
            {
                RotationRadians = Maths.ToRadians(value);
            }
        }

        public virtual void Update(float time)
        {
        }

        public object Tag;

        public bool IgnoreParentColor;

        public bool IgnoreParentOpacity;

        public bool Test;

        public bool UpdateEnabled = true;

        private Color color = Color.White;

        private float opacity = 1f;

        private bool visible = true;
    }
}
