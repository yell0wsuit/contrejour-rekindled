using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Extensions;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class ColorSquare : PrimitivesNode
    {
        public CGSize Size
        {
            get
            {
                return size;
            }
            set
            {
                if (size != value)
                {
                    size = value;
                    sizeDirty = true;
                }
            }
        }

        public override float OpacityFloat
        {
            set
            {
                if (value != OpacityFloat)
                {
                    base.OpacityFloat = value;
                    Color = Color.ChangeAlpha((byte)Opacity);
                }
            }
        }

        public override Color Color
        {
            set
            {
                if (Color != value)
                {
                    base.Color = value;
                    RefreshColors();
                }
            }
        }

        public ColorSquare(Color _color, CGSize _size)
        {
            Array.Resize(ref vertices, 4);
            Color = _color;
            Size = _size;
            RefreshColors();
        }

        private void RefreshSize()
        {
            float num = size.Width * 1f;
            float num2 = size.Height * 1f;
            vertices[0].Position = Vector2.Zero.ToVector3();
            vertices[1].Position = new Vector2(num, 0f).ToVector3();
            vertices[2].Position = new Vector2(0f, num2).ToVector3();
            vertices[3].Position = new Vector2(num, num2).ToVector3();
            sizeDirty = false;
        }

        private void RefreshColors()
        {
            GraphUtil.SetColor(vertices, Color);
        }

        protected override void DrawPrimitives()
        {
            if (sizeDirty)
            {
                RefreshSize();
            }
            if (size.Width > 0f && size.Height > 0f && Opacity > 0)
            {
                GraphUtil.FillTrianglesStrip(vertices, null);
            }
        }

        protected CGSize size;

        protected VertexPositionColorTexture[] vertices;

        protected bool sizeDirty;
    }
}
