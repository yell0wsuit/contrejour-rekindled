using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class SnotProcessor(LevelBuilderBase _builder, string _type, float _partSize) : JointProcessorBase(_type, _builder)
    {
        public SnotProcessor(LevelBuilderBase _builder)
            : this(_builder, "snot")
        {
        }

        public SnotProcessor(LevelBuilderBase _builder, string _type)
            : this(_builder, _type, 1.6f)
        {
        }

        public virtual float GetStartDensity()
        {
            return 0.13f;
        }

        public override object ProcessItem(Hashtable item)
        {
            Vector2 vector = item.GetVector("start");
            Vector2 vector2 = item.GetVector("end");
            RopeMetricsWithCoords ropeMetricsEndItem = GetRopeMetricsEndItem(vector, vector2, item);
            Body body = GetBodyByWorld(vector);
            BodyClip bodyClip = body.UserData as BodyClip;
            if (bodyClip is not null and (PlasticinePartBodyClip or EnergyBodyClip))
            {
                body = builder.GroundBody;
            }
            if (bodyClip is ISnotHolder)
            {
                vector = ((ISnotHolder)bodyClip).SnotPosition;
            }
            Body body2 = FarseerUtil.CreateCircle(builder.World, 0.16666667f, vector, 0f, GetStartDensity(), true);
            FarseerUtil.SetSensor(body2, true);
            PlasticineConstants.ApplyActiveBodiesFilter(body2);
            RevoluteJoint revoluteJoint = FarseerUtil.CreateRevoluteJoint(builder.World, body, body2, vector, false);
            Body body3 = body2;
            List<Body> list = new();
            List<Joint> list2 = new();
            for (int i = 0; i < ropeMetricsEndItem.Parts; i++)
            {
                BodyAndJoint bodyAndJoint = default(BodyAndJoint);
                CreatePartStartEndResultIndexTotal(body3, ropeMetricsEndItem.GetPositionByIndex(i), ropeMetricsEndItem.GetPositionByIndex(i + 1), ref bodyAndJoint, i, ropeMetricsEndItem.Parts);
                body3 = bodyAndJoint.Body;
                list.Add(bodyAndJoint.Body);
                list2.Add(bodyAndJoint.Joint);
            }
            return new SnotData(body2, revoluteJoint, body, body.GetLocalPoint(vector), list, list2, ropeMetricsEndItem);
        }

        public virtual RopeMetricsWithCoords GetRopeMetricsEndItem(Vector2 start, Vector2 end, Hashtable item)
        {
            return RopeUtil.GetRopeMetricsEndMaxPartSizeMinParts(start, end, partSize, 3);
        }

        public virtual float LinearDamping()
        {
            return 1f;
        }

        public virtual float GetDensityTotal(int index, int total)
        {
            return index != total - 1 ? 0.13f : 0.221f;
        }

        public void CreatePartStartEndResultIndexTotal(Body previousBody, Vector2 start, Vector2 end, ref BodyAndJoint result, int index, int total)
        {
            float densityTotal = GetDensityTotal(index, total);
            Body body = CreatePartBodyEndIndexTotalDensity(start, end, index, total, densityTotal);
            body.LinearDamping = LinearDamping();
            Joint joint = JoinBodiesEndBodyStartEndIndexTotal(previousBody, body, start, end, index, total);
            result.Joint = joint;
            result.Body = body;
        }

        public virtual Body CreatePartBodyEndIndexTotalDensity(Vector2 start, Vector2 end, int index, int total, float density)
        {
            Body body = FarseerUtil.CreateCircle(builder.World, 0.16666667f, end, 0f, density, true);
            FarseerUtil.SetSensor(body, true);
            return body;
        }

        public virtual Joint JoinBodiesEndBodyStartEndIndexTotal(Body startBody, Body endBody, Vector2 start, Vector2 end, int index, int total)
        {
            DistanceJoint distanceJoint = JointFactory.CreateDistanceJoint(builder.World, startBody, endBody, start - startBody.Position, end - endBody.Position);
            distanceJoint.Frequency = 5f;
            distanceJoint.DampingRatio = 0.1f;
            return distanceJoint;
        }

        public const float END_BODY_DENSITY = 0.221f;

        public const float DAMPING = 0.1f;

        public const float FREQUENCY = 5f;

        public const float DENSITY = 0.13f;

        public const float JOINT_CIRCLE_RADIUS = 0.16666667f;

        public const float PART_SIZE = 1.3333334f;

        public const float LINEAR_DAMPING = 1f;

        protected float partSize = _partSize;

        public struct BodyAndJoint
        {
            public Body Body;

            public Joint Joint;
        }
    }
}
