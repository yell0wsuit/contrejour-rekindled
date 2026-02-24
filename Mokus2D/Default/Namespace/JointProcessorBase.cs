using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class JointProcessorBase(string _type, LevelBuilderBase _builder) : TypeProcessorBase(_type, _builder)
    {
        public Joint CreateJointConfig(RevoluteJoint joint, Hashtable config)
        {
            builder.World.AddJoint(joint);
            if (config.Exists("id"))
            {
                builder.CreatedObjects[config.GetString("id")] = joint;
            }
            return joint;
        }

        public Body GetBodyByWorld(Vector2 position)
        {
            return GetBodyByWorldReq(position, null);
        }

        public Body GetBodyByWorldReq(Vector2 position, IReq req)
        {
            List<Body> bodiesByWorldReqResult = GetBodiesByWorldReqResult(position, req);
            Body body = TryGetBodyByType(bodiesByWorldReqResult, BodyType.Dynamic);
            if (body != null)
            {
                return body;
            }
            Body body2 = TryGetBodyByType(bodiesByWorldReqResult, BodyType.Kinematic);
            return body2 != null ? body2 : bodiesByWorldReqResult.Count > 0 ? bodiesByWorldReqResult[0] : builder.GroundBody;
        }

        private Body TryGetBodyByType(List<Body> bodies, BodyType type)
        {
            ArrayList arrayList = IReqHelper.Filter(bodies.ToArray(), new BodyTypeReq(type));
            return arrayList.Count > 0 ? (Body)arrayList[0] : null;
        }

        public List<Body> GetBodiesByWorldReqResult(Vector2 position, IReq req)
        {
            List<Body> list = new();
            List<Body> list2 = new();
            List<Fixture> list3 = FarseerUtil.QueryPointWorldPoint(builder.World, position);
            foreach (Fixture fixture in list3)
            {
                Body body = fixture.Body;
                if (!list2.Contains(body) && (req == null || req.Meet(body)))
                {
                    list.Add(body);
                    list2.Add(body);
                }
            }
            return list;
        }
    }
}
