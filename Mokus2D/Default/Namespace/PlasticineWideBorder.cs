using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class PlasticineWideBorder : PrimitivesNode
    {
        public void SetSizeBorderColorBorderOutColor(int value, Color _borderColor, Color borderOutColor)
        {
            Color = _borderColor;
            int num = (value * 2 * 2) + 2;
            outBorder = new VertexPositionColorTexture[num];
            inBorder = new VertexPositionColorTexture[num];
            GraphUtil.SetGradientColorsStrip(Color, borderOutColor, outBorder);
            GraphUtil.SetColor(inBorder, Color);
        }

        public VertexPositionColorTexture[] OutBorder => outBorder;

        public VertexPositionColorTexture[] InBorder => inBorder;

        protected override void DrawPrimitives()
        {
            GraphUtil.FillTrianglesStrip(inBorder, null);
            GraphUtil.FillTrianglesStrip(outBorder, null);
        }

        protected VertexPositionColorTexture[] outBorder;

        protected VertexPositionColorTexture[] inBorder;
    }
}
