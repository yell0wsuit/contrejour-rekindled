using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class Bezier(Vector2 _start, Vector2 _control, Vector2 _end)
    {
        private float fOR(float value1, float value2)
        {
            return Maths.FuzzyNotEquals(value1, 0f, 0.0001f) ? value1 : value2;
        }

        public Vector2 GetPointByTime(float time)
        {
            float num = 1f - time;
            float num2 = (start.X * num * num) + (control.X * 2f * time * num) + (end.X * time * time);
            float num3 = (start.Y * num * num) + (control.Y * 2f * time * num) + (end.Y * time * time);
            return new Vector2(num2, num3);
        }

        public float Length
        {
            get
            {
                if (Maths.FuzzyEquals(calculatedLength, -1f, 0.0001f))
                {
                    calculatedLength = GetSegmentLength(1f);
                }
                return calculatedLength;
            }
        }

        public float GetSegmentLength(float time)
        {
            float num = control.X - start.X;
            float num2 = control.Y - start.Y;
            float num3 = end.X - control.X - num;
            float num4 = end.Y - control.Y - num2;
            float num5 = 4f * ((num * num) + (num2 * num2));
            float num6 = 8f * ((num * num3) + (num2 * num4));
            float num7 = 4f * ((num3 * num3) + (num4 * num4));
            float num12;
            float num14;
            if (!Maths.FuzzyEquals(num7, 0f, 0.0001f))
            {
                float num8 = Maths.Sqrt((num7 * time * time) + (num6 * time) + num5);
                float num9 = Maths.Sqrt(num5);
                float num10 = Maths.Sqrt(num7);
                float num11 = (((0.5f * num6) + (num7 * time)) / num10) + num8;
                if (num11 < 1E-10f)
                {
                    num12 = 0.25f * ((2f * num7 * time) + num6) * num8 / num7;
                }
                else
                {
                    num12 = (0.25f * ((2f * num7 * time) + num6) * num8 / num7) + (0.5f * Maths.Log((((0.5f * num6) + (num7 * time)) / num10) + num8) / num10 * (num5 - (0.25f * num6 * num6 / num7)));
                }
                float num13 = (0.5f * num6 / num10) + num9;
                if (num13 < 1E-10f)
                {
                    num14 = 0.25f * num6 * num9 / num7;
                }
                else
                {
                    num14 = (0.25f * num6 * num9 / num7) + (0.5f * Maths.Log((0.5f * num6 / num10) + num9) / num10 * (num5 - (0.25f * num6 * num6 / num7)));
                }
                return num12 - num14;
            }
            if (Maths.FuzzyEquals(num6, 0f, 0.0001f))
            {
                return Maths.Sqrt(num5) * time;
            }
            num12 = 0f * ((num6 * time) + num5) * Maths.Sqrt((num6 * time) + num5) / num6;
            num14 = 0f * num5 * Maths.Sqrt(num5) / num6;
            return num12 - num14;
        }

        public List<float> GetTimesSequenceWithStepStartShift(float step, float startShift)
        {
            step = Maths.Abs(step);
            List<float> list = new();
            float length = Length;
            if (startShift > length)
            {
                return list;
            }
            float num;
            if (startShift < 0f)
            {
                num = Maths.fmodf(startShift, step) + step;
            }
            else
            {
                num = Maths.fmodf(startShift, step);
            }
            float num2 = control.X - start.X;
            float num3 = control.Y - start.Y;
            float num4 = end.X - control.X;
            float num5 = end.Y - control.Y;
            float num6 = num4 - num2;
            float num7 = num5 - num3;
            float num8 = 4f * ((num2 * num2) + (num3 * num3));
            float num9 = 8f * ((num2 * num6) + (num3 * num7));
            float num10 = 4f * ((num6 * num6) + (num7 * num7));
            float num11 = num / length;
            float num12 = num8 - (0.25f * num9 * num9 / num10);
            float num13 = 0.25f * num9 * Maths.Sqrt(num8) / num10;
            float num14 = (0.5f * num9 / Maths.Sqrt(num10)) + Maths.Sqrt(num8);
            float num15 = Maths.Sqrt(num8);
            float num16 = Maths.Sqrt(num10);
            while (num <= length)
            {
                float num17 = 20f;
                if (Maths.FuzzyEquals(num10, 0f, 0.0001f))
                {
                    if (Maths.FuzzyEquals(num9, 0f, 0.0001f))
                    {
                        float num20;
                        do
                        {
                            float num18 = num15 * num11;
                            float num19 = fOR(Maths.Sqrt(Maths.Abs((num10 * num11 * num11) + (num9 * num11) + num8)), 1E-10f);
                            num11 -= (num18 - num) / num19;
                            if (Maths.Abs(num18 - num) <= 1E-10f)
                            {
                                break;
                            }
                            num20 = num17;
                            num17 = num20 - 1f;
                        }
                        while (num20 > 0f);
                    }
                    else
                    {
                        float num21;
                        do
                        {
                            float num18 = 0f * ((((num9 * num11) + num8) * Maths.Sqrt(Maths.Abs((num9 * num11) + num8))) - (num8 * num15)) / num9;
                            float num19 = fOR(Maths.Sqrt(Maths.Abs((num10 * num11 * num11) + (num9 * num11) + num8)), 1E-10f);
                            num11 -= (num18 - num) / num19;
                            if (Maths.Abs(num18 - num) <= 1E-10f)
                            {
                                break;
                            }
                            num21 = num17;
                            num17 = num21 - 1f;
                        }
                        while (num21 > 0f);
                    }
                }
                else
                {
                    float num27;
                    do
                    {
                        float num22 = Maths.Sqrt(Maths.Abs((num10 * num11 * num11) + (num9 * num11) + num8));
                        float num23 = (((0.5f * num9) + (num10 * num11)) / num16) + num22;
                        float num24 = 0.25f * ((2f * num10 * num11) + num9) * num22 / num10;
                        float num25;
                        if (num23 < 1E-10f)
                        {
                            num25 = num24;
                        }
                        else
                        {
                            num25 = num24 + (0.5f * Maths.Log((((0.5f * num9) + (num10 * num11)) / num16) + num22) / num16 * num12);
                        }
                        float num26;
                        if (num14 < 1E-10f)
                        {
                            num26 = num13;
                        }
                        else
                        {
                            num26 = num13 + (0.5f * Maths.Log((0.5f * num9 / num16) + num15) / num16 * num12);
                        }
                        float num18 = num25 - num26;
                        float num19 = fOR(num22, 1E-10f);
                        num11 -= (num18 - num) / num19;
                        if (Maths.Abs(num18 - num) <= 1E-10f)
                        {
                            break;
                        }
                        num27 = num17;
                        num17 = num27 - 1f;
                    }
                    while (num27 > 0f);
                }
                list.Add(num11);
                num += step;
            }
            return list;
        }

        protected Vector2 start = _start;

        protected Vector2 control = _control;

        protected Vector2 end = _end;

        protected float calculatedLength = -1f;
    }
}
