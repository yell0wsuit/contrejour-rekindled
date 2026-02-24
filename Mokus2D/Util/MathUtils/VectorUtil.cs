using System;

using Default.Namespace;

using Microsoft.Xna.Framework;

namespace Mokus2D.Util.MathUtils
{
    public static class VectorUtil
    {
        public static Vector2 ToVector(float module, float angle)
        {
            return new Vector2((float)((double)module * Math.Cos((double)angle)), (float)((double)module * Math.Sin((double)angle)));
        }

        public static Vector2 Center(Vector2 first, Vector2 second)
        {
            return (first + second) / 2f;
        }

        public static bool FuzzyEquals(this Vector2 a, Vector2 b, float delta = 0.0001f)
        {
            return Maths.FuzzyEquals(a.X, b.X, delta) && Maths.FuzzyEquals(a.Y, b.Y, delta);
        }

        public static Vector2 Clamp(this Vector2 position, Vector2 minValue, Vector2 maxValue)
        {
            return new Vector2(position.X.Clamp(minValue.X, maxValue.X), position.Y.Clamp(minValue.Y, maxValue.Y));
        }
    }
}
