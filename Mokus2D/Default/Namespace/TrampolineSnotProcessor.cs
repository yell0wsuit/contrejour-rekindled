using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class TrampolineSnotProcessor(LevelBuilderBase _builder) : BridgeSnotProcessor(_builder, "trampoline", 0.6666667f)
    {
        public override float GetDensityTotal(int index, int total)
        {
            return 2f;
        }

        public override void GetLocalPartPositionsTotalStartEnd(int index, int total, ref Vector2 start, ref Vector2 end)
        {
            start -= end;
            end = start;
            if (index != 0)
            {
                start *= 1.25f;
            }
            if (index != total - 1)
            {
                end *= -0.25f;
                return;
            }
            end = new Vector2(0f, 0f);
        }

        public override RopeMetricsWithCoords GetRopeMetricsEndItem(Vector2 start, Vector2 end, Hashtable item)
        {
            RopeMetricsWithCoords ropeMetricsWithCoords = base.GetRopeMetricsEndItem(start, end, item);
            if (ropeMetricsWithCoords.Parts % 2 == 0)
            {
                ropeMetricsWithCoords = RopeUtil.GetRopeMetricsEndMaxPartSizeMinParts(start, end, partSize, ropeMetricsWithCoords.Parts + 1);
            }
            return ropeMetricsWithCoords;
        }

        public override Joint JoinBodiesEndBodyStartEndIndexTotal(Body startBody, Body endBody, Vector2 start, Vector2 end, int index, int total)
        {
            Vector2 vector = start - end;
            Vector2 vector2 = vector;
            vector += end;
            vector2 += end;
            DistanceJoint distanceJoint = JointFactory.CreateDistanceJoint(builder.World, startBody, endBody, vector2 - startBody.Position, vector - endBody.Position);
            distanceJoint.Length = 0f;
            distanceJoint.Frequency = 4.5f;
            distanceJoint.DampingRatio = 1f;
            return distanceJoint;
        }

        public override Body CreatePartBodyEndIndexTotalDensity(Vector2 start, Vector2 end, int index, int total, float density)
        {
            Body body = base.CreatePartBodyEndIndexTotalDensity(start, end, index, total, density);
            PlasticineConstants.ApplyActiveBodiesFilter(body);
            TrampolinePartBodyClip trampolinePartBodyClip = new(builder, body);
            parts.Add(trampolinePartBodyClip);
            body.AngularDamping = 20f;
            Body body2 = null;
            if (index == 0)
            {
                body2 = FarseerUtil.CreateCircle(builder.World, START_RADIUS, start + new Vector2(0f, START_RADIUS), 0f, 0f, false);
            }
            else if (index == total - 1)
            {
                body2 = FarseerUtil.CreateCircle(builder.World, START_RADIUS, end + new Vector2(0f, START_RADIUS), 0f, 0f, false);
            }
            if (body2 != null)
            {
                PlasticineConstants.ApplyActiveBodiesFilter(body2);
            }
            return body;
        }

        public override object ProcessItem(Hashtable item)
        {
            parts.Clear();
            SnotData snotData = (SnotData)base.ProcessItem(item);
            foreach (TrampolinePartBodyClip trampolinePartBodyClip in parts)
            {
                trampolinePartBodyClip.Data = snotData;
            }
            Vector2 vector = item.GetVector("end");
            Vector2 vector2 = item.GetVector("start");
            _ = FarseerUtil.CreateRevoluteJoint(builder.World, builder.GroundBody, snotData.EndBody, vector, false);
            _ = FarseerUtil.CreateRevoluteJoint(builder.World, builder.GroundBody, snotData.FirstBody, vector2, false);
            builder.World.RemoveBody(snotData.EyeBody);
            snotData.EyeBody = null;
            return snotData;
        }

        private const float SPRING_K = 800f;

        protected List<TrampolinePartBodyClip> parts = new();

        private static readonly float START_RADIUS = 5f * Box2DConfig.DefaultConfig.SizeMultiplier;

        public readonly float PART_ANGLE = MathHelper.ToRadians(15f);
    }
}
