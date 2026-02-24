using Microsoft.Xna.Framework;

using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class TexturedSuckerNeck : SuckerNeckSprite
    {
        public TexturedSuckerNeck(string textureName)
        {
            Texture = ClipFactory.CreateWithoutConfig(textureName);
            textureColor = ContreJourConstants.WHITE_COLOR_3;
        }

        public override void CreateVectors(int _allPointsSize)
        {
            base.CreateVectors(_allPointsSize);
            int num = _allPointsSize / 2;
            GraphUtil.CreateTextureCoordsVerticesStep(num - 1, vertices, 0.75f);
        }

        public override void Bounce()
        {
            base.LightBounce();
        }

        public override void DrawBorder()
        {
        }

        public override void DrawPolygons()
        {
            GraphUtil.FillTrianglesList(vertices, Texture);
        }

        private const float TEXTURE_STEP = 0.75f;

        protected Color textureColor;
    }
}
