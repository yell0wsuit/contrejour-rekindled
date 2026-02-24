using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class WhitePlasticineSprite : PlasticineSprite
    {
        public WhitePlasticineSprite()
        {
            Color = PlasticineConstants.WHITE_GROUND_COLOR;
        }

        public override Color Color
        {
            get
            {
                return PlasticineConstants.WHITE_GROUND_COLOR;
            }
        }
    }
}
