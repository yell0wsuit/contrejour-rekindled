using System.Globalization;

using Microsoft.Xna.Framework;

namespace Mokus2D.Extensions
{
    public static class ColorExtension
    {
        public static Color ToColor(this string hexString)
        {
            if (hexString.StartsWith('#'))
            {
                hexString = hexString[1..];
            }
            uint num = uint.Parse(hexString, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            return hexString.Length == 8
                ? num.ToARGBColor()
                : hexString.Length == 6
                ? num.ToRGBColor()
                : throw new System.InvalidOperationException("Invald hex representation of an ARGB or RGB color value.");
        }

        public static Color ToRGBColor(this int hex)
        {
            return ((uint)hex).ToRGBColor();
        }

        public static Color ToRGBColor(this uint hex)
        {
            byte b = (byte)(hex >> 16);
            byte b2 = (byte)(hex >> 8);
            byte b3 = (byte)hex;
            return new Color(b, b2, (int)b3, 255);
        }

        public static Color ToARGBColor(this uint hex)
        {
            byte b = (byte)(hex >> 24);
            byte b2 = (byte)(hex >> 16);
            byte b3 = (byte)(hex >> 8);
            byte b4 = (byte)hex;
            return new Color(b2, b3, b4, (int)b);
        }

        public static Color LerpToWhite(this Color color, float amount)
        {
            return Color.Lerp(color, Color.White, amount);
        }

        public static Color ChangeAlpha(this Color color, byte alpha)
        {
            color.A = alpha;
            return color;
        }

        public static ColorDiff Sub(this Color color, Color colorSub)
        {
            return new ColorDiff(color.R - colorSub.R, color.G - colorSub.G, color.B - colorSub.B, color.A - colorSub.A);
        }

        public static Color Mult(this Color color1, Color color2)
        {
            return new Color(MultColorPart(color1.R, color2.R), MultColorPart(color1.G, color2.G), MultColorPart(color1.B, color2.B), MultColorPart(color1.A, color2.A));
        }

        private static int MultColorPart(float part1, float part2)
        {
            return (int)(part1 * part2 / 255f);
        }

        public static Color Add(this Color color, Color colorSub)
        {
            return new Color(color.R + colorSub.R, color.G + colorSub.G, color.B + colorSub.B, color.A + colorSub.A);
        }

        public static Color Add(this Color color, ColorDiff colorSub)
        {
            return new Color(color.R + colorSub.R, color.G + colorSub.G, color.B + colorSub.B, color.A + colorSub.A);
        }

        public static Color Mult(this Color color, float mult)
        {
            return color * mult;
        }
    }
}
