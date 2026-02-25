using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Util;

namespace Mokus2D.Default.Namespace
{
    public class SpringSnotSprite(ContreJourGame _game, SnotBodyClipBase _snot, float _startWidth, float _centerWidth, float _endWidth) : SnotSprite(_snot, _startWidth, _centerWidth, _endWidth)
    {
        public bool Active
        {
            get => active; set => active = value;
        }

        public override void Update(float time)
        {
            base.Update(time);
            activeProgress = Maths.StepToTargetMaxStep(activeProgress, active ? 1 : 0, 0.05f);
            if (Maths.FuzzyNotEquals(activeProgress, previousActiveProgress, 0.0001f))
            {
                SetCirclesColors();
                previousActiveProgress = activeProgress;
                SetBorderColors();
            }
            Vector2 startPosition = snot.StartPosition;
            Vector2 vector = snot.EndPosition();
            GraphUtil.GetCircleRadiusSegmentsResult(CocosUtil.ccp2Point(Box2DConfig.DefaultConfig.ToPoint(startPosition)), CocosUtil.r(startWidthPixels / 2f), 16, ref baseCircleSurface);
            GraphUtil.GetCircleRadiusSegmentsResult(CocosUtil.ccp2Point(Box2DConfig.DefaultConfig.ToPoint(vector)), CocosUtil.r((endWidthPixels / 2f) + 1f), 12, ref endCircleSurface);
            GraphUtil.CreateGradientBorderWidthVertices(new VertexPositionColorTextureListAdapter(baseCircleSurface), -borderWidth, baseCircle);
            GraphUtil.CreateGradientBorderWidthVertices(new VertexPositionColorTextureListAdapter(endCircleSurface), -borderWidth, endCircle);
        }

        public virtual Color BaseCircleColor()
        {
            return NeckColor;
        }

        public virtual Color EndCircleColor()
        {
            return NeckColor;
        }

        protected override void DrawPrimitives()
        {
            DrawCircles();
            base.DrawPrimitives();
        }

        public virtual void DrawCircles()
        {
            GraphUtil.FillTrianglesList(baseCircle, null);
            if (!game.BlackSide)
            {
                GraphUtil.FillTrianglesList(endCircle, null);
            }
        }

        public void SetCirclesColors()
        {
            GraphUtil.CreateGradientColorsList(16, BaseCircleColor(), EndColor(), baseCircle);
            GraphUtil.CreateGradientColorsList(12, EndCircleColor(), EndColor(), endCircle);
        }

        public override void SetBorderColors()
        {
            GraphUtil.CreateGradientColorsList(allPointsSize, NeckColor, EndColor(), border);
        }

        public override Color EndColor()
        {
            int num = (int)(200f * activeProgress);
            return new Color(num, num, num, 0);
        }

        private const float ACTIVE_STEP = 0.05f;

        private const int CIRLCE_SEGMENTS_END = 12;

        private const int CIRLCE_SEGMENTS_BASE = 16;

        protected ContreJourGame game = _game;

        protected VertexPositionColorTexture[] baseCircleSurface = new VertexPositionColorTexture[16];

        protected VertexPositionColorTexture[] endCircleSurface = new VertexPositionColorTexture[12];

        protected VertexPositionColorTexture[] baseCircle = new VertexPositionColorTexture[96];

        protected VertexPositionColorTexture[] endCircle = new VertexPositionColorTexture[72];

        protected bool active;

        protected float activeProgress = 0f;

        protected float previousActiveProgress = 1f;
    }
}
