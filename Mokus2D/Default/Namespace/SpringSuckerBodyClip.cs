using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SpringSuckerBodyClip : SuckerBodyClip
    {
        public SpringSuckerBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            AutoCreate();
        }

        protected override float BounceVolume => 0.4f;

        protected override string BounceSound => "spring";

        private void AutoCreate()
        {
            if (touch == null && config.Exists("auto") && config.GetString("auto") == "true")
            {
                createPosition = Body.Position;
                createPosition -= new Vector2(maxDistance, 0f);
                CreateBodies();
            }
        }

        protected override SuckerNeckSprite CreateNeck()
        {
            return new SuckerNeckSprite();
        }

        public override Node CreatePimpa()
        {
            return ClipFactory.CreateWithAnchor("McSuckerBody");
        }

        public void CreateLegs()
        {
        }

        public override void CreateBodies()
        {
            base.CreateBodies();
            parallel = FarseerUtil.toVec(1f, bounceAngle);
            normal = parallel.Rotate90();
        }

        public override void OnCollisionStartPoint(Body body2, Contact point)
        {
            base.OnCollisionStartPoint(body2, point);
            HeroBodyClip heroBodyClip = body2.UserData as HeroBodyClip;
            if (end != null && (point.FixtureA == middleFixture || point.FixtureB == middleFixture) && heroBodyClip != null)
            {
                Vector2 vector = heroBodyClip.Body.LinearVelocity;
                float num = Maths.atan2Vec(vector);
                num = 3.1415927f + bounceAngle - (num - bounceAngle - 3.1415927f);
                vector = FarseerUtil.toVec(vector.Length(), num);
                Vector2 vectorProjectionTarget = FarseerUtil.GetVectorProjectionTarget(vector, parallel);
                Vector2 vectorProjectionTarget2 = FarseerUtil.GetVectorProjectionTarget(vector, normal);
                vector = vectorProjectionTarget2;
                vector *= SPEED_MULT;
                vector += vectorProjectionTarget;
                heroBodyClip.Body.LinearVelocity = vector;
                float num2 = Maths.atan2Vec(Body.Position, body2.Position) - bounceAngle;
                num2 = Maths.SimplifyAngleRadiansStartValue(num2, -3.1415927f);
                float num3 = JUMP_IMPULSE + heroBodyClip.SnotJoinedCount * SNOT_JUMP_IMPULSE;
                if (num2 < 0f)
                {
                    num3 *= -1f;
                }
                Vector2 vector2 = FarseerUtil.rotate(new Vector2(0f, num3), bounceAngle);
                body2.ApplyLinearImpulse(vector2, body2.WorldCenter);
                neck.Bounce();
                ContactEvent.SendEvent();
                PlayBounceSound();
            }
        }

        public readonly EventSender ContactEvent = new();

        protected Vector2 parallel;

        protected Vector2 normal;

        private float JUMP_IMPULSE = 1f;

        private float SNOT_JUMP_IMPULSE = 0.2f;

        private float SPEED_MULT = 0.75f;
    }
}
