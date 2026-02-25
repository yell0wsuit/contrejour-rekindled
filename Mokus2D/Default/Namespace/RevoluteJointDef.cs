using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class RevoluteJointDef(RevoluteJoint joint)
    {
        public RevoluteJoint Create(World world)
        {
            return JointFactory.CreateRevoluteJoint(world, BodyA, BodyB, LocalAnchorB);
        }

        public Body BodyA = joint.BodyA;

        public Body BodyB = joint.BodyB;

        public Vector2 LocalAnchorB = joint.LocalAnchorB;
    }
}
