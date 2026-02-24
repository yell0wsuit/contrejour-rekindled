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

        public VertexPositionColorTexture[] Vertices
        {
            get
            {
                return vertices;
            }
        }

        public override Color Color
        {
            get
            {
                return base.Color;
            }
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
            if (vertices != null)
            {
                GraphUtil.SetColor(vertices, Color);
            }
        }

        protected override void DrawPrimitives()
        {
            GraphUtil.FillTrianglesStrip(vertices, null);
        }

        public void InitVertices(int verticesCount)
        {
            vertices = new VertexPositionColorTexture[verticesCount];
            RefreshColor();
        }

        private static readonly Color COLOR = new(0, 0, 0, 255);

        private VertexPositionColorTexture[] vertices;
    }
}
