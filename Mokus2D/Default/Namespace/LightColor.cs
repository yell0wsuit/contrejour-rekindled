using Microsoft.Xna.Framework;

using Mokus2D.Extensions;

namespace Mokus2D.Default.Namespace
{
    public class LightColor
    {
        public LightColor()
        {
        }

        public LightColor(Color lightInColor, Color lightOutColor)
        {
            LightInColor = lightInColor;
            LightOutColor = lightOutColor;
            LightBorderColor = lightOutColor.ChangeAlpha(0);
        }

        public LightColor Clone()
        {
            return (LightColor)MemberwiseClone();
        }

        public Color LightInColor;

        public Color LightOutColor;

        public Color LightBorderColor;
    }
}
