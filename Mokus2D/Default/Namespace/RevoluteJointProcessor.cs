using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class RevoluteJointProcessor : JointProcessorBase
    {
        public RevoluteJointProcessor(LevelBuilderBase _builder)
            : base("revoluteJoint", _builder)
        {
        }

        public RevoluteJointProcessor(string _type, LevelBuilderBase _builder)
            : base(_type, _builder)
        {
        }

        public void CreateRevoluteJointPositionConfig(List<Body> bodies, Vector2 position, Hashtable config)
        {
            if (bodies[0] == null || bodies[1] == null)
            {
                Mokus2D.Util.DebugLog.warn("Revolute joint not initialized", null);
                return;
            }
            if (bodies[0].BodyType == BodyType.Dynamic && bodies[1].BodyType == BodyType.Static)
            {
                Body body = bodies[0];
                bodies[0] = bodies[1];
                bodies[1] = body;
            }
            RevoluteJoint revoluteJoint = new(bodies[0], bodies[1], position);
            if (config.Exists("motorSpeed"))
            {
                float num = 0f;
                for (int i = 0; i < bodies.Count; i++)
                {
                    num += bodies[i].Inertia;
                }
                revoluteJoint.MotorSpeed = config.GetFloat("motorSpeed");
                revoluteJoint.MaxMotorTorque = config.Exists("maxMotorTorque") ? config.GetFloat("maxMotorTorque") : (30f * num);
                revoluteJoint.MotorEnabled = true;
            }
            if (config.Exists("upperAngle"))
            {
                revoluteJoint.LimitEnabled = true;
                revoluteJoint.LowerLimit = MathHelper.ToRadians(config.GetFloat("lowerAngle"));
                revoluteJoint.UpperLimit = MathHelper.ToRadians(config.GetFloat("upperAngle"));
            }
            revoluteJoint.CollideConnected = false;
            _ = CreateJointConfig(revoluteJoint, config);
        }

        private const float MOTOR_TORQUE_MULT = 30f;
    }
}

