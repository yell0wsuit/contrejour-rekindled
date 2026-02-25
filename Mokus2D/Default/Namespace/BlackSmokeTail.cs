using System;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class BlackSmokeTail(Body _body, LevelBuilderBase _builder) : IUpdatable
    {
        public float StartScale
        {
            get => startScale; set => startScale = value;
        }

        public string ClipName
        {
            get => clipName; set => clipName = value;
        }

        public void Update(float time)
        {
            ArrayList arrayList = new();
            foreach (object obj in items)
            {
                Sprite sprite = (Sprite)obj;
                sprite.Scale -= 0.05f * startScale;
                sprite.Opacity -= 8;
                if (sprite.Opacity <= 0 || sprite.Scale <= 0f)
                {
                    arrayList.Add(sprite);
                    builder.RemoveChild(sprite);
                }
            }
            foreach (object obj2 in arrayList)
            {
                _ = items.Remove(obj2);
            }
            if (body.BodyType == BodyType.Static || body.LinearVelocity.Length() > 0.1f)
            {
                Vector2 vector = builder.ToPoint(body.Position);
                if (initialized)
                {
                    float num = (vector - previousPosition).Length();
                    int num2 = (int)Math.Min((float)Math.Ceiling((double)(num / 2f)), 15f);
                    float num3 = num / num2;
                    for (int i = 0; i < num2; i++)
                    {
                        Sprite sprite2 = ClipFactory.CreateWithAnchor(clipName);
                        sprite2.Position = Maths.StepToPointTargetMaxStep(previousPosition, vector, num3 * i);
                        float num4 = 1f - (i / (float)num2);
                        sprite2.Scale *= startScale;
                        sprite2.Scale -= 0.05f * num4 * startScale;
                        sprite2.Opacity = (int)(150f - (8f * num4));
                        builder.Add(sprite2, 3);
                        items.Add(sprite2);
                    }
                }
                previousPosition = vector;
                initialized = true;
            }
        }

        private const float START_OPACITY = 150f;

        private const float MAX_DISTANCE = 2f;

        private const float MIN_SPEED = 0.1f;

        private const int OPACITY_DIFF = 8;

        private const float SCALE_DIFF = 0.05f;

        protected Body body = _body;

        protected LevelBuilderBase builder = _builder;

        protected ArrayList items = new();

        protected Vector2 previousPosition;

        protected bool initialized;

        protected float startScale = 1f;

        protected string clipName = "McTailPart";
    }
}
