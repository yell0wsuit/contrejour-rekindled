using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class PlasticineHighliteBorder : PrimitivesNode
    {
        public PlasticineHighliteBorder(PlasticineItem firstItem, PlasticineWideBorder _border)
        {
            PlasticineItem plasticineItem = firstItem;
            game = (ContreJourGame)plasticineItem.BodyClip.Builder.Game;
            int num = 0;
            do
            {
                PlasticinePartHighlite plasticinePartHighlite = new(plasticineItem.BodyClip, this, (num * 2 * 2) + 2);
                parts.Add(plasticinePartHighlite);
                plasticinePartHighlite.SetDirty();
                plasticineItem = plasticineItem.NextItem;
                num++;
            }
            while (plasticineItem != firstItem);
            border = _border;
            vertices = new VertexPositionColorTexture[border.OutBorder.Length];
        }

        public VertexPositionColorTexture[] Vertices => vertices;

        public VertexPositionColorTexture[] InBorder => border.InBorder;

        public Color MainColor => border.Color;

        public VertexPositionColorTexture[] OutBorder()
        {
            return border.OutBorder;
        }

        public override void Update(float time)
        {
            foreach (object obj in parts)
            {
                PlasticinePartHighlite plasticinePartHighlite = (PlasticinePartHighlite)obj;
                plasticinePartHighlite.Update(time);
            }
            foreach (object obj2 in parts)
            {
                PlasticinePartHighlite plasticinePartHighlite2 = (PlasticinePartHighlite)obj2;
                plasticinePartHighlite2.TryRefresh();
            }
        }

        protected override void DrawPrimitives()
        {
            GraphUtil.FillTrianglesStrip(vertices, null);
        }

        public const int HIGHLITE_PART_VERTICES_COUNT = 4;

        protected VertexPositionColorTexture[] vertices;

        protected ArrayList parts = new();

        protected ContreJourGame game;

        protected PlasticineWideBorder border;
    }
}
