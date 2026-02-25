using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class PlasticineBorder : PrimitivesNode, IOpacity
    {
        public override float OpacityFloat
        {
            set
            {
                if (OpacityFloat != value)
                {
                    base.OpacityFloat = value;
                    CreateColors();
                }
            }
        }

        public PlasticineBorder(List<Vector2> initialPolygon)
        {
            List<Vector2> list = new();
            GraphUtil.CreateBezierSurfaceSurfaceSegments(initialPolygon, ref list, 3);
            polygonSize = list.Count;
            outBorder = new VertexPositionColorTexture[list.Count * 6];
            inBorder = new VertexPositionColorTexture[list.Count * 6];
            GraphUtil.CreateGradientBorderWidthVertices(list, BorderWidth(), inBorder);
            GraphUtil.CreateGradientBorderWidthVertices(list, -BorderWidth(), outBorder);
            CreateColors();
            OpacityFloat = 0f;
        }

        public virtual float BorderWidth()
        {
            return WIDTH / 2f;
        }

        public virtual Color OutColor()
        {
            return OUT_COLOR;
        }

        public virtual Color InColor()
        {
            return IN_COLOR;
        }

        public virtual Color CenterColor()
        {
            return CENTER_COLOR;
        }

        public void CreateColors()
        {
            Color color = CenterColor();
            GraphUtil.CreateGradientColorsList(polygonSize, color, InColor(), inBorder);
            GraphUtil.CreateGradientColorsList(polygonSize, color, OutColor(), outBorder);
        }

        protected override void DrawPrimitives()
        {
            if (OpacityFloat > 0f)
            {
                GraphUtil.FillTrianglesList(inBorder, null);
                GraphUtil.FillTrianglesList(outBorder, null);
            }
        }

        protected VertexPositionColorTexture[] outBorder;

        protected VertexPositionColorTexture[] inBorder;

        protected int polygonSize;

        private static readonly Color OUT_COLOR = new Color(255, 255, 255) * 0f;

        private static readonly Color IN_COLOR = new(0, 0, 0, 0);

        private static readonly Color CENTER_COLOR = new(127, 127, 127);

        private readonly float WIDTH = 4f;
    }
}
