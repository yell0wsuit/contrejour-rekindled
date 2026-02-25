using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class LianaProcessor(LevelBuilderBase _builder) : JointProcessorBase("liana", _builder)
    {
        public override object ProcessItem(Hashtable item)
        {
            return null;
        }

        public void JoinBodyTo(Body body1, Body body2)
        {
            _ = FarseerUtil.CreateDistanceJointBody2CollideConnectedFreqDamping(builder.World, body1, body2, false, 4f, 0.2f);
        }

        public Body CreateBodyDynamic(Vector2 position, bool dynamic)
        {
            Body body = FarseerUtil.CreateCircle(builder.World, 0.16666667f, position, 0f, 0.3f, dynamic);
            FarseerUtil.SetSensor(body, true);
            return body;
        }

        private const float DENSITY = 0.3f;

        private const float RADIUS = 0.16666667f;
    }
}
