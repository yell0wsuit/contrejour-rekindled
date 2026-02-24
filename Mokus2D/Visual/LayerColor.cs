using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Data;
using Mokus2D.Visual.Util;

namespace Mokus2D.Visual
{
    public class LayerColor : Node
    {
        public LayerColor()
            : this(Color.Black)
        {
        }

        public LayerColor(Color color)
        {
            Color = color;
            CreateVertices();
        }

        public override Color Color
        {
            get
            {
                return base.Color;
            }
            set
            {
                if (base.Color != value)
                {
                    base.Color = value;
                    colorChanged = true;
                }
            }
        }

        private void CreateVertices()
        {
            vertices[0] = new VertexPositionColor(new Vector3(-1.1f, -1.1f, 0f), Color);
            vertices[1] = new VertexPositionColor(new Vector3(-1.1f, 1.1f, 0f), Color);
            vertices[2] = new VertexPositionColor(new Vector3(1.1f, -1.1f, 0f), Color);
            vertices[3] = new VertexPositionColor(new Vector3(1.1f, 1.1f, 0f), Color);
        }

        protected override void UpdateDrawProperties()
        {
            base.UpdateDrawProperties();
            if (colorChanged)
            {
                colorChanged = false;
                for (int i = 0; i < vertices.Length; i++)
                {
                    vertices[i].Color = Color;
                }
            }
        }

        public override void Draw(VisualState state)
        {
            base.Draw(state);
            XNAUtil.LoadIdentityMatrix();
            using (new PrimitivesDrawing(state, Matrix.Identity, null))
            {
                GraphUtil.DrawTriangleStrip(vertices);
            }
        }

        private VertexPositionColor[] vertices = new VertexPositionColor[4];

        private bool colorChanged;
    }
}
