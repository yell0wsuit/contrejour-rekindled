using System.Collections.Generic;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class EditorRevoluteJointProcessor(LevelBuilderBase _builder) : RevoluteJointProcessor("editorRevoluteJoint", _builder)
    {
        public override object ProcessItem(Hashtable item)
        {
            Hashtable hashtable = item.GetHashtable("config");
            Vector2 vector = item.GetVector("position");
            Mokus2D.Util.DebugLog.TODO("check coo");
            List<Body> bodiesByWorldReqResult = GetBodiesByWorldReqResult(vector, new BodyTypeReq(BodyType.Dynamic));
            bodiesByWorldReqResult.AddRange(GetBodiesByWorldReqResult(vector, new BodyTypeReq(BodyType.Static)));
            if (hashtable.Exists("rotationLocked"))
            {
                hashtable["upperAngle"] = "0";
                hashtable["lowerAngle"] = "0";
            }
            if (bodiesByWorldReqResult.Count > 0)
            {
                if (bodiesByWorldReqResult.Count > 1)
                {
                    CreateRevoluteJointPositionConfig(bodiesByWorldReqResult, vector, hashtable);
                }
                else
                {
                    List<Body> bodiesByWorldReqResult2 = GetBodiesByWorldReqResult(vector, new BodyTypeReq(BodyType.Static));
                    Body body = (bodiesByWorldReqResult2.Count > 0) ? bodiesByWorldReqResult2[0] : builder.GroundBody;
                    List<Body> list = new();
                    list.Add(body);
                    list.Add(bodiesByWorldReqResult[0]);
                    CreateRevoluteJointPositionConfig(list, vector, hashtable);
                }
            }
            return null;
        }
    }
}

