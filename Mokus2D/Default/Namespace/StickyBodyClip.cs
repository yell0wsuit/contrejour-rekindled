using System.Collections.Generic;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class StickyBodyClip : ContreJourBodyClip
    {
        public StickyBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            Body.BodyType = BodyType.Kinematic;
        }

        private void Join()
        {
            joinedBody = builder.GroundBody;
            builder.Root.ChangeChildLayer(clip, -1);
            CircleShape circleShape = null;
            foreach (Fixture fixture in Body.FixtureList)
            {
                if (fixture.Shape.ShapeType == ShapeType.Circle)
                {
                    circleShape = (CircleShape)fixture.Shape;
                    break;
                }
            }
            Vector2 worldPoint = Body.GetWorldPoint(circleShape.Position);
            List<Fixture> list = FarseerUtil.Query(builder.World, worldPoint, 0.16666667f, 0.16666667f);
            float num = 0f;
            foreach (Fixture fixture2 in list)
            {
                float num2 = joinedBody.Position.DistanceTo(worldPoint);
                if (fixture2.Body != Body && (joinedBody == null || num < num2))
                {
                    joinedBody = fixture2.Body;
                    num = num2;
                }
            }
            offset = Body.Position - joinedBody.Position;
            Body.SleepingAllowed = false;
        }

        public override void Update(float time)
        {
            if (time != 0f && !joined)
            {
                joined = true;
                Join();
            }
            if (joined)
            {
                Body.Position = joinedBody.Position + offset;
            }
            base.Update(time);
        }

        private const float PLAY_SPEED = 3f;

        private const float RADIUS = 0.16666667f;

        protected Body joinedBody;

        protected bool joined;

        private Vector2 offset;
    }
}
