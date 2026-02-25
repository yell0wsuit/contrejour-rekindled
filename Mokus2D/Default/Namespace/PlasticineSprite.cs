using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class PlasticineSprite : PrimitivesNode
    {
        public PlasticineSprite()
        {
            Color = COLOR;
        }

        public VertexPositionColorTexture[] Vertices { get; private set; }

        public override Color Color
        {
            get => base.Color;
            set
            {
                if (Color != value)
                {
                    base.Color = value;
                    RefreshColor();
                }
            }
        }

        private void RefreshColor()
        {
            if (Vertices != null)
            {
                GraphUtil.SetColor(Vertices, Color);
            }
        }

        protected override void DrawPrimitives()
        {
            GraphUtil.FillTrianglesStrip(Vertices, null);
        }

        public void InitVertices(int verticesCount)
        {
            Vertices = new VertexPositionColorTexture[verticesCount];
            RefreshColor();
        }

        private static readonly Color COLOR = new(0, 0, 0, 255);
    }
}
