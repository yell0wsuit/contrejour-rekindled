using System;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class StrongSnotProcessor : SnotProcessor
    {
        public StrongSnotProcessor(LevelBuilderBase _builder)
            : base(_builder, "strongSnot", 0.6666667f)
        {
        }

        public StrongSnotProcessor(LevelBuilderBase _builder, string _type, float _partSize)
            : base(_builder, _type, _partSize)
        {
        }

        public override float GetStartDensity()
        {
            return 10f;
        }

        public override float LinearDamping()
        {
            return 0f;
        }

        public override float GetDensityTotal(int index, int total)
        {
            return 0.13f + ((total - (float)index) / total * 0.13f);
        }

        public override Joint JoinBodiesEndBodyStartEndIndexTotal(Body startBody, Body endBody, Vector2 start, Vector2 end, int index, int total)
        {
            RevoluteJoint revoluteJoint = JointFactory.CreateRevoluteJoint(builder.World, startBody, endBody, endBody.Position - end);
            revoluteJoint.CollideConnected = false;
            revoluteJoint.LimitEnabled = false;
            revoluteJoint.Broke += new Action<Joint, float>(joint_Broke);
            return revoluteJoint;
        }

        private void joint_Broke(Joint arg1, float arg2)
        {
            throw new NotImplementedException();
        }
    }
}
