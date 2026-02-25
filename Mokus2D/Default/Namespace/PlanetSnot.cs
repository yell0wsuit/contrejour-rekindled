using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Util.Data;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class PlanetSnot : LongNeckSprite, IDepthDependent
    {
        public float Depth
        {
            get => depth; set => depth = value;
        }

        public PlanetSnot()
        {
            endInit = CocosUtil.ccp2Point(END);
            middleInit = CocosUtil.ccp2Point(MIDDLE);
            baseSprite = ClipFactory.CreateWithAnchor("McFlowerHead");
            AddChild(baseSprite);
            eye = new PlanetSnotEye(null, true, Vector2.Zero);
            AddChild(eye);
            middle = middleInit;
            end = Vector2.Zero;
            eye.Position = end;
            borderWidth = 4f;
            opacity = 255;
        }

        public override void GetPairs(List<Pair<Vector2>> result)
        {
            Pair<Vector2> pair = ContreDrawUtil.GetPointsPair(Vector2.Zero, Vector2.Zero, middle, CocosUtil.r(10f));
            result.Add(pair);
            pair = ContreDrawUtil.GetPointsPair(middle, Vector2.Zero, middle, CocosUtil.r(5f));
            result.Add(pair);
            pair = ContreDrawUtil.GetPointsPair(end, middle, end, CocosUtil.r(20f));
            result.Add(pair);
        }

        public override void Update(float time)
        {
            if (Maths.FuzzyEquals(depth, 1f, 0.0001f))
            {
                Vector2 vector = Maths.ToPointAngle(eye.ViewDistance * CocosUtil.r(40f), eye.ViewAngle);
                targetEnd = endInit + vector;
                float num = end.DistanceTo(targetEnd);
                middle = Maths.StepToPointTargetMaxStep(middle, middleInit, CocosUtil.iPadValue(1f));
                end = Maths.StepToPointTargetMaxStep(end, targetEnd, CocosUtil.iPadValue(Maths.min(1f, num / 5f)));
            }
            else if (depth > 0.6f)
            {
                targetEnd = CocosUtil.ccp2(-10f, -10f);
                middle = Maths.StepToPointTargetMaxStep(middle, CocosUtil.ccp2(-5f, -5f), CocosUtil.r(10f));
                end = Maths.StepToPointTargetMaxStep(end, targetEnd, CocosUtil.r(10f));
            }
            else
            {
                middle = CocosUtil.ccp2(-5f, -5f);
                targetEnd = CocosUtil.ccp2(-10f, -10f);
                end = targetEnd;
            }
            eye.Position = CocosUtil.fromRetina(end);
            baseSprite.Position = CocosUtil.fromRetina(end);
            base.Update(time);
        }

        private const float RADIUS = 40f;

        private const float MIDDLE_WIDTH = 5f;

        private const float END_WIDTH = 20f;

        private const float START_WIDTH = 10f;

        protected PlanetSnotEye eye;

        protected Sprite baseSprite;

        protected Vector2 middle;

        protected Vector2 end;

        protected Vector2 targetEnd;

        protected ushort opacity;

        protected float depth;

        protected Vector2 endInit;

        protected Vector2 middleInit;

        private static readonly Vector2 END = new(0f, 80f);

        private static readonly Vector2 MIDDLE = new(0f, 30f);
    }
}
