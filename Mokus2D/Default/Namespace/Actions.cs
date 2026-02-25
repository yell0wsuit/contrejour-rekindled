using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;

namespace Default.Namespace
{
    public class Actions
    {
        public static NodeAction CreateNeonEffect(float minOpacity, float maxOpacity)
        {
            Sequence sequence = new([]);
            for (int i = 0; i < 2; i++)
            {
                int num = (int)Maths.randRange(4f, 8f);
                sequence.Add(CreateMultipleFadeOutIn(minOpacity, maxOpacity, 1.5f, 3f, num));
                num = (int)Maths.randRange(4f, 6f);
                sequence.Add(CreateMultipleFadeOutIn(minOpacity, maxOpacity, 0.05f, 0.2f, num));
            }
            return new RepeatForever(sequence);
        }

        private static NodeAction CreateMultipleFadeOutIn(float minOpacity, float maxOpacity, float minTime, float maxTime, int count)
        {
            Sequence sequence = new([]);
            for (int i = 0; i < count; i++)
            {
                sequence.Add(CreateFadeOutIn(minOpacity, maxOpacity, minTime, maxTime));
            }
            return sequence;
        }

        private static NodeAction CreateFadeOutIn(float minOpacity, float maxOpacity, float minTime, float maxTime)
        {
            Sequence sequence = new([]);
            sequence.Add(new FadeTo(Maths.randRange(minTime, maxTime), minOpacity));
            sequence.Add(new FadeTo(Maths.randRange(minTime, maxTime), maxOpacity));
            return sequence;
        }

        public static NodeAction ShakeWithDurationPositionOffsetCountScaleDiff(float time, Vector2 position, float offset, int count, float scaleDiff)
        {
            NodeAction nodeAction = null;
            for (int i = 0; i < count; i++)
            {
                bool flag = i == count - 1;
                Vector2 vector = flag ? Vector2.Zero : new Vector2(Maths.randRange(-offset, offset), Maths.randRange(-offset, offset));
                MoveTo moveTo = new(time / count, position + vector);
                NodeAction nodeAction2 = moveTo;
                if (scaleDiff != 0f)
                {
                    float num = 1f;
                    if (!flag)
                    {
                        num += i / (float)count * scaleDiff;
                        num += (i % 2 != 0) ? 0.05f : (-0.05f);
                    }
                    ScaleTo scaleTo = new(time / count, num);
                    nodeAction2 = new Spawn([moveTo, scaleTo]);
                }
                if (nodeAction == null)
                {
                    nodeAction = nodeAction2;
                }
                else
                {
                    nodeAction = new Sequence([nodeAction, nodeAction2]);
                }
            }
            return nodeAction;
        }

        public static NodeAction ShakeWithDurationOffsetCount(float d, float offset, int count)
        {
            return ShakeWithDurationPositionOffsetCountScaleDiff(d, Vector2.Zero, offset, count, 0.2f);
        }
    }
}
