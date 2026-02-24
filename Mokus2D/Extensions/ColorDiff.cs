namespace Mokus2D.Extensions
{
    public struct ColorDiff(int r, int g, int b, int a)
    {
        public static ColorDiff operator *(ColorDiff source, float mult)
        {
            return new ColorDiff((int)(source.R * mult), (int)(source.G * mult), (int)(source.B * mult), (int)(source.A * mult));
        }

        public int R = r;

        public int G = g;

        public int B = b;

        public int A = a;
    }
}
