using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Visual.Util;

namespace Mokus2D.Default.Namespace
{
    public class TexturedSmoothNeck : TexturedNeck
    {
        private TexturedSmoothNeck(string textureFile, float _width)
            : base(textureFile)
        {
            width = _width;
        }

        public void AddControlPoint(Vector2 point)
        {
            controlPoints.Add(point);
            dirty = true;
        }

        public override void Update(float time)
        {
            if (dirty)
            {
                dirty = false;
                Refresh();
            }
        }

        public void Refresh()
        {
            ClearVertices();
            if (controlPoints.Count > 1)
            {
                List<Vector2> list = GraphUtil.CreateBezierLineBezierMaxBezierStep(controlPoints, 3f);
                vertices = GraphUtil.LineToNeckNeckWidth(list, width);
                RefreshTextureCoords();
            }
        }

        private const float BEZIER_STEP = 3f;

        protected float width;

        protected List<Vector2> controlPoints;

        protected bool dirty;
    }
}
