using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public static class Maths
    {
        public static Random RandomGenerator { get; private set; } = new((int)DateTime.Now.Ticks);

        public static void Randomize(int seed)
        {
            RandomGenerator = new Random(seed);
        }

        public static int getSign(float value)
        {
            return Math.Sign(value);
        }

        public static Vector2 toPoint(float module, float angle)
        {
            return new Vector2(module * Cos(angle), module * Sin(angle));
        }

        public static float stepTo(float value, float target, float maxStep)
        {
            return Math.Abs(target - value) <= maxStep ? target : value + (getSign(target - value) * maxStep);
        }

        private static bool isnan(float value)
        {
            return value != value;
        }

        public static int min(int a, int b)
        {
            return a > b ? b : a;
        }

        public static float min(float a, float b)
        {
            return a > b ? b : a;
        }

        public static float min(double a, double b)
        {
            return a > b ? (float)b : (float)a;
        }

        public static int max(int a, int b)
        {
            return a < b ? b : a;
        }

        public static float max(float a, float b)
        {
            return a < b ? b : a;
        }

        public static float max(double a, double b)
        {
            return a < b ? (float)b : (float)a;
        }

        public static float randRange01()
        {
            return randRange(0f, 1f);
        }

        public static float randRange(float min, float max)
        {
            return MathHelper.Lerp(min, max, (float)RandomGenerator.NextDouble());
        }

        public static int randRangeInt(int min, int max)
        {
            return RandomGenerator.Next(min, max);
        }

        public static float randRange(double min, double max)
        {
            return randRange((float)min, (float)max);
        }

        public static bool isfinite(float value)
        {
            return !isnan(value) && value != 100100100f && value != -100100100f;
        }

        public static bool ccpEqual(Vector2 first, Vector2 second)
        {
            return first.X == second.X && first.Y == second.Y;
        }

        public static Vector2 randomPoint(Vector2 start, Vector2 end)
        {
            return new Vector2(randRange(start.X, end.X), randRange(start.Y, end.Y));
        }

        public static Vector2 ccpRotateRadians(Vector2 point, float radians)
        {
            Vector2 vector = point;
            float num = Cos(radians);
            float num2 = Sin(radians);
            vector.X = (point.X * num) - (point.Y * num2);
            vector.Y = (point.X * num2) + (point.Y * num);
            return vector;
        }

        private static Vector2 ccpRotate(Vector2 point, float rotation)
        {
            float num = MathHelper.ToRadians(rotation);
            return ccpRotateRadians(point, num);
        }

        public static float atan2(float a, float b)
        {
            return (float)Math.Atan2((double)a, (double)b);
        }

        public static float atan2(Vector2 source)
        {
            return atan2(source.Y, source.X);
        }

        public static float atan2(Vector2 source, Vector2 target)
        {
            return atan2(target - source);
        }

        public static float atan2Vec(Vector2 vec)
        {
            return atan2(vec.Y, vec.X);
        }

        public static float atan2Vec(Vector2 start, Vector2 end)
        {
            return atan2(end.Y - start.Y, end.X - start.X);
        }

        public static float fmodP(float source, float divide)
        {
            float num = fmodf(source, divide);
            if (num < 0f)
            {
                num += divide;
            }
            return num;
        }

        public static float floor(float source, float step)
        {
            float num = fmodP(source, step);
            return source - num;
        }

        public static float ceil(float source, float step)
        {
            float num = fmodP(source, step);
            return source - num + step;
        }

        public static float round(float source, float step)
        {
            float num = fmodP(source, step);
            float num2;
            if (num < step / 2f)
            {
                num2 = source - num;
            }
            else
            {
                num2 = source - num + step;
            }
            return num2;
        }

        public static float PeriodicOffset(float value, float period)
        {
            float num = value % period;
            return num < 0f ? Math.Abs(num) < num + period ? num : num + period : num < Math.Abs(num - period) ? num : num - period;
        }

        public static bool testSegmentsIntersects(Vector2 firstStart, Vector2 firstEnd, Vector2 secondStart, Vector2 secondEnd)
        {
            return testSegmentsIntersects(new CGPointSegment(firstStart, firstEnd), new CGPointSegment(secondStart, secondEnd));
        }

        public static Vector2 stepToPoint(Vector2 source, Vector2 target, float maxStep)
        {
            Vector2 vector = target - source;
            if (vector.Length() < maxStep)
            {
                return target;
            }
            float num = atan2(vector.Y, vector.X);
            source += toPoint(maxStep, num);
            return source;
        }

        public static bool testSegmentsIntersects(CGPointSegment s1, CGPointSegment s2)
        {
            float num = ((s2.B.Y - s2.A.Y) * (s1.B.X - s1.A.X)) - ((s2.B.X - s2.A.X) * (s1.B.Y - s1.A.Y));
            float num2 = ((s2.B.X - s2.A.X) * (s1.A.Y - s2.A.Y)) - ((s2.B.Y - s2.A.Y) * (s1.A.X - s2.A.X));
            float num3 = ((s1.B.X - s1.A.X) * (s1.A.Y - s2.A.Y)) - ((s1.B.Y - s1.A.Y) * (s1.A.X - s2.A.X));
            return num != 0f && num2 / num <= 1f && num2 / num >= 0f && num3 / num <= 1f && num3 / num >= 0f;
        }

        public static void SetLength(ref Vector2 value, float length)
        {
            value.Normalize();
            value *= length;
        }

        public static Vector2 GetCenter(List<Vector2> points)
        {
            Vector2 vector = Vector2.Zero;
            for (int i = 0; i < points.Count; i++)
            {
                vector += points[i];
            }
            return vector * (1f / points.Count);
        }

        public static bool Between(float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        public static float RandRangeMinMax(float min, float max)
        {
            return min + (Rand() * (max - min));
        }

        public static float Random(float max)
        {
            return Rand() * max;
        }

        public static int Random(int max)
        {
            return RandomGenerator.Next(max);
        }

        public static float Rand()
        {
            return randRange01();
        }

        public static float ToDegrees(float value)
        {
            return MathHelper.ToDegrees(value);
        }

        public static float ToRadians(float value)
        {
            return MathHelper.ToRadians(value);
        }

        public static float GetAngleTarget(Vector2 start, Vector2 target)
        {
            Vector2 vector = target - start;
            return atan2(vector.Y, vector.X);
        }

        public static Vector2 GetCenterPointWith(Vector2 first, Vector2 second)
        {
            return new Vector2((first.X + second.X) / 2f, (first.Y + second.Y) / 2f);
        }

        public static Vector2 StepToVecTargetMaxStep(Vector2 source, Vector2 target, float maxStep)
        {
            Vector2 vector = target - source;
            if (vector.Length() < maxStep)
            {
                return target;
            }
            float num = atan2(vector.Y, vector.X);
            source += FarseerUtil.ToVecAngle(maxStep, num);
            return source;
        }

        public static Vector2 StepToPointTargetMaxStep(Vector2 source, Vector2 target, float maxStep)
        {
            return stepToPoint(source, target, maxStep);
        }

        public static float Abs(float value)
        {
            return Math.Abs(value);
        }

        public static float StepToTargetMaxStep(float value, float target, float maxStep)
        {
            return Abs(target - value) <= maxStep ? target : value + (GetSign(target - value) * maxStep);
        }

        public static int StepToTargetMaxStep(int value, int target, int maxStep)
        {
            return Abs(target - value) <= maxStep ? target : value + (GetSign(target - value) * maxStep);
        }

        public static int GetSign(float value)
        {
            return value < 0f ? -1 : value <= 0f ? 0 : 1;
        }

        public static float Length(Vector2 point)
        {
            return Sqrt((point.X * point.X) + (point.Y * point.Y));
        }

        public static void NormalizeLength(ref Vector2 point, float length)
        {
            float num = length / Length(point);
            point.X *= num;
            point.Y *= num;
        }

        public static Vector2 RotateAngle(Vector2 point, float angle)
        {
            float num = Cos(angle);
            float num2 = Sin(angle);
            return new Vector2((point.X * num) - (point.Y * num2), (point.Y * num) + (point.X * num2));
        }

        public static void RotateMinus90(ref Vector2 point)
        {
            float x = point.X;
            point.X = point.Y;
            point.Y = -x;
        }

        public static void Rotate90(ref Vector2 point)
        {
            float x = point.X;
            point.X = -point.Y;
            point.Y = x;
        }

        public static Vector2 MultValue(Vector2 source, float value)
        {
            return new Vector2(source.X * value, source.Y * value);
        }

        public static Vector2 SubtractSubtraction(Vector2 source, Vector2 subtraction)
        {
            return new Vector2(source.X - subtraction.X, source.Y - subtraction.Y);
        }

        public static Vector2 AddAdd(Vector2 source, Vector2 add)
        {
            return new Vector2(source.X + add.X, source.Y + add.Y);
        }

        public static float SimplifyAngleStartValue(float value, float startValue)
        {
            value -= startValue;
            value = fmodf(value, 360f);
            if (value < 0f)
            {
                value += 360f;
            }
            return value + startValue;
        }

        public static float SimplifyAngleRadiansStartValue(float value, float startValue)
        {
            value -= startValue;
            value = fmodf(value, 6.2831855f);
            if (value < 0f)
            {
                value += 6.2831855f;
            }
            return value + startValue;
        }

        public static Vector2 ToPointAngle(float module, float angle)
        {
            return toPoint(module, angle);
        }

        public static void AddBezierPointsPointsSegments(List<Vector2> target, List<Vector2> points, int segments)
        {
            for (int i = 0; i < points.Count - 2; i += 2)
            {
                GetBezierPointsControlDestinationSegmentsInsertLastResult(points[i], points[i + 1], points[i + 2], segments, false, target);
            }
            target.Add(points[points.Count - 1]);
        }

        public static void AddBezierPointsPointsSegmentsVector(List<Vector2> target, List<Vector2> points, List<int> segmentsVector)
        {
            for (int i = 0; i < points.Count - 2; i += 2)
            {
                GetBezierPointsControlDestinationSegmentsInsertLastResult(points[i], points[i + 1], points[i + 2], segmentsVector[i / 2], false, target);
            }
            target.Add(points[points.Count - 1]);
        }

        public static void GetBezierPointsControlDestinationSegmentsInsertLastResult(Vector2 origin, Vector2 control, Vector2 destination, int segments, bool insertLast, List<Vector2> result)
        {
            float num = 0f;
            for (int i = 0; i < segments; i++)
            {
                float num2 = ((float)Math.Pow((double)(1f - num), 2.0) * origin.X) + (2f * (1f - num) * num * control.X) + (num * num * destination.X);
                float num3 = ((float)Math.Pow((double)(1f - num), 2.0) * origin.Y) + (2f * (1f - num) * num * control.Y) + (num * num * destination.Y);
                result.Add(new Vector2(num2, num3));
                num += 1f / segments;
            }
            if (insertLast)
            {
                result.Add(destination);
            }
        }

        public static float SimplifyAngle(float angle)
        {
            return SimplifyAngle(angle, 0f);
        }

        public static float SimplifyAngle(float angle, float startValue)
        {
            angle -= startValue;
            angle = (float)Math.IEEERemainder((double)angle, 6.283185307179586);
            if (angle < 0f)
            {
                angle += 6.2831855f;
            }
            return angle + startValue;
        }

        public static float Sqrt(float f)
        {
            return (float)Math.Sqrt((double)f);
        }

        public static float Cos(float f)
        {
            return (float)Math.Cos((double)f);
        }

        public static float Sin(float f)
        {
            return (float)Math.Sin((double)f);
        }

        public static float Log(float f)
        {
            return (float)Math.Log((double)f);
        }

        public static float Clamp(float value, float min, float max)
        {
            return MathHelper.Clamp(value, min, max);
        }

        public static float fmodf(float value, float min)
        {
            return value % min;
        }

        public static bool FuzzyEquals(float a, float b, float delta = 0.0001f)
        {
            return Math.Abs(a - b) < delta;
        }

        public static bool FuzzyNotEquals(float a, float b, float delta = 0.0001f)
        {
            return !FuzzyEquals(a, b, delta);
        }

        public static float easeInOutSine(float progress, float maxValue)
        {
            return -maxValue / 2f * ((float)Math.Cos(3.141592653589793 * (double)progress) - 1f);
        }

        public const float INFINITY = 100100100f;

        public const float EPSILON = 1E-07f;

        public const float MAXFLOAT = 3.4028235E+38f;

        public const float PI = 3.1415927f;

        public const float PI_X_2 = 6.2831855f;

        public const float PI2 = 1.5707964f;

        public const float PI4 = 0.7853982f;
    }
}
