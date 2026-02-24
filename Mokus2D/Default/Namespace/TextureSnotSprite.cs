using Microsoft.Xna.Framework;

using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class TextureSnotSprite : SpringSnotSprite
    {
        public TextureSnotSprite(ContreJourGame _game, SnotBodyClipBase _snot, float _startWidth, float _centerWidth, float _endWidth, string textureFile)
            : base(_game, _snot, _startWidth, _centerWidth, _endWidth)
        {
            Texture = ClipFactory.CreateWithoutConfig(textureFile);
            targetOpacity = 255f;
            opacity = 255f;
            textureColor = new Color(255, 255, 255);
        }

        public TextureSnotSprite(ContreJourGame _game, SnotBodyClipBase _snot, float _startWidth, float _centerWidth, float _endWidth)
            : this(_game, _snot, _startWidth, _centerWidth, _endWidth, _game.ChooseSide("blackStrongSnotTexture.png", "whiteStrongSnotTexture.png", "strongSnotTexture.png", "strongSnotTexture.png", "greenStrongSnotTexture.png"))
        {
        }

        public float TargetOpacity
        {
            get
            {
                return targetOpacity;
            }
            set
            {
                targetOpacity = value;
            }
        }

        public Color TextureColor
        {
            get
            {
                return textureColor;
            }
            set
            {
                if (textureColor != value)
                {
                    textureColor = value;
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        vertices[i].Color = textureColor;
                    }
                }
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            opacity = Maths.StepToTargetMaxStep(opacity, targetOpacity, 10f);
        }

        public override void CreateVectors(int _allPointsSize)
        {
            base.CreateVectors(_allPointsSize);
            int num = _allPointsSize / 2;
            GraphUtil.CreateTextureCoordsVerticesStep(num - 1, vertices, 0.1f);
        }

        public override void DrawCircles()
        {
        }

        public override void DrawBorder()
        {
        }

        public override void DrawPolygons()
        {
            GraphUtil.FillTrianglesList(vertices, Texture);
        }

        private const float TEXTURE_STEP = 0.1f;

        private const float OPACITY_STEP = 10f;

        protected float opacity;

        protected float targetOpacity;

        protected Color textureColor;
    }
}
