using System;

using Default.Namespace;

using Microsoft.Xna.Framework;

namespace Mokus2D.Util.MathUtils
{
    public class CosChanger
    {
        public float Value { get; private set; }

        public bool IsMax => Math.Abs(Progress % 6.283185307179586) < Step;

        public CosChanger(float minValue, float maxValue, float step)
        {
            Progress = (float)(Maths.RandomGenerator.NextDouble() * 3.141592653589793);
            Step = step;
            MinValue = minValue;
            MaxValue = maxValue;
            Update(0f);
        }

        public CosChanger(float minStep, float maxStep)
            : this(0f, 1f, Maths.randRange(minStep, maxStep))
        {
        }

        public CosChanger(float step)
            : this(0f, 1f, step)
        {
        }

        public void Update(float time)
        {
            Progress += Step * time * 30f;
            Value = GetValue(MinValue, MaxValue, Progress);
        }

        public float GetValue(float min, float max, float p)
        {
            return MathHelper.Lerp(max, min, (float)(1.0 + Math.Cos((double)p)) / 2f);
        }

        public void SetMiddleProgress()
        {
            Progress = 1.5707964f;
        }

        public float Progress;

        public float Step;

        public float MinValue;

        public float MaxValue;
    }
}
