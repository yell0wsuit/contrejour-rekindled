using Mokus2D.Default.Namespace;

using Microsoft.Xna.Framework;

namespace Mokus2D.Extensions
{
    public static class VectorExtensions
    {
        public static float Atan2(this Vector2 vector)
        {
            return (float)System.Math.Atan2(vector.Y, vector.X);
        }

        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.X, vector.Y, 0f);
        }

        public static Vector2 Rotate(this Vector2 vector, float angle)
        {
            return Maths.RotateAngle(vector, angle);
        }

        public static Vector2 Rotate90(this Vector2 vector)
        {
            Vector2 vector2 = vector;
            Maths.Rotate90(ref vector2);
            return vector2;
        }

        public static Vector2 Normalize(this Vector2 vector, float length)
        {
            Vector2 vector2 = vector;
            vector2.Normalize();
            return vector2 * length;
        }

        public static Vector3 Normalize(this Vector3 vector, float length)
        {
            Vector3 vector2 = vector;
            vector2.Normalize();
            return vector2 * length;
        }

        public static Vector2 StepTo(this Vector2 source, Vector2 target, float step)
        {
            Vector2 vector = target - source;
            if (vector.Length() < step)
            {
                return target;
            }
            vector *= step / vector.Length();
            source += vector;
            return source;
        }

        public static float DistanceTo(this Vector2 source, Vector2 target)
        {
            return (target - source).Length();
        }

        public static Vector2 Middle(this Vector2 source, Vector2 target)
        {
            return (source + target) / 2f;
        }

        public static Vector2 Multiply(this Vector2 source, float value)
        {
            return source * value;
        }

        public static string ToStringInt(this Vector2 source)
        {
            return string.Concat(new object[]
            {
                "{",
                (int)source.X,
                ", ",
                (int)source.Y,
                "}"
            });
        }
    }
}
