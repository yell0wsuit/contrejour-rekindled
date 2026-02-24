using System;

using Default.Namespace;

using Microsoft.Xna.Framework;

namespace Mokus2D.Util.MathUtils
{
    public static class MathExtensions
    {
        public static float Abs(this float value)
        {
            return Math.Abs(value);
        }

        public static float Sign(this float value)
        {
            return Math.Sign(value);
        }

        public static float Sign(this int value)
        {
            return Math.Sign(value);
        }

        public static bool Between(this float value, float min, float max)
        {
            return Maths.Between(value, min, max);
        }

        public static float Clamp(this float value, float min, float max)
        {
            return MathHelper.Clamp(value, min, max);
        }

        public static float Clamp(this double value, float min, float max)
        {
            return MathHelper.Clamp((float)value, min, max);
        }

        public static float ToRadians(this float value)
        {
            return MathHelper.ToRadians(value);
        }

        public static float ToRadians(this int value)
        {
            return MathHelper.ToRadians(value);
        }

        public static float ToDegrees(this float value)
        {
            return MathHelper.ToDegrees(value);
        }

        public static float ToDegrees(this int value)
        {
            return MathHelper.ToDegrees(value);
        }

        public static float StepTo(this float value, float target, float step)
        {
            return Math.Abs(target - value) <= step ? target : value + Math.Sign(target - value) * step;
        }

        public static T RandomItem<T>(this T[] array)
        {
            return array[Maths.RandomGenerator.Next(array.Length)];
        }

        public static float Range(this Random random, float min, float max)
        {
            return MathHelper.Lerp(min, max, (float)random.NextDouble());
        }

        public static float Range01(this Random random)
        {
            return MathHelper.Lerp(0f, 1f, (float)random.NextDouble());
        }
    }
}
