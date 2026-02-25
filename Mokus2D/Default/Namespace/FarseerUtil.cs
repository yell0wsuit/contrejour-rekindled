using System;
using System.Collections.Generic;

using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public static class FarseerUtil
    {
        public static Vector2 clampLength(Vector2 vec, float maxLength)
        {
            float num = vec.Length();
            if (num > maxLength)
            {
                vec *= maxLength / num;
            }
            return vec;
        }

        public static Vector2 toVec(float module, float angle)
        {
            return new Vector2(module * Maths.Cos(angle), module * Maths.Sin(angle));
        }

        public static Vector2 stepToVec(Vector2 source, Vector2 target, float maxStep)
        {
            Vector2 vector = target - source;
            if (vector.Length() < maxStep)
            {
                return target;
            }
            float num = Maths.atan2Vec(vector);
            source += toVec(maxStep, num);
            return source;
        }

        public static Vector2 b2Vec2Middle(Vector2 vec, Vector2 vec2)
        {
            return new Vector2((vec.X + vec2.X) / 2f, (vec.Y + vec2.Y) / 2f);
        }

        public static float b2Vec2Distance(Vector2 vec, Vector2 vec2)
        {
            return (vec - vec2).Length();
        }

        public static float b2Vec2ScalarMult(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static bool FuzzyEquals(Vector2 vec, Vector2 vec2, float delta = 0.01f)
        {
            return Maths.FuzzyEquals(vec.X, vec2.X, delta) && Maths.FuzzyEquals(vec.Y, vec2.Y, delta);
        }

        public static bool b2Vec2Equal(Vector2 vec, Vector2 vec2)
        {
            return vec.X == vec2.X && vec.Y == vec2.Y;
        }

        public static Vector2 rotate(Vector2 vec, float angle)
        {
            float num = Maths.Cos(angle);
            float num2 = Maths.Sin(angle);
            return new Vector2(vec.X * num - vec.Y * num2, vec.Y * num + vec.X * num2);
        }

        public static Vector2 rotateAround(Vector2 source, Vector2 rotationCenter, float angle)
        {
            source -= rotationCenter;
            source = rotate(source, angle);
            return source + rotationCenter;
        }

        public static Vector2 rotate90(ref Vector2 vec)
        {
            return new Vector2(vec.Y, -vec.X);
        }

        public static float classifyVec(ref Vector2 vec, ref Vector2 start, ref Vector2 end)
        {
            Vector2 vector = vec;
            Vector2 vector2 = end - start;
            Vector2 vector3 = vector - start;
            return vector2.X * vector3.Y - vector3.X * vector2.Y;
        }

        private static bool protocolReq(object clip, object interfaceType)
        {
            return ((Type)interfaceType).IsAssignableFrom(clip.GetType());
        }

        private static bool typeReq(BodyClip clip, object type)
        {
            return ((Type)type).IsInstanceOfType(clip);
        }

        public static Fixture GetFixtureProperty(Body body, string property)
        {
            return GetFixturePropertyValue(body, property, "true");
        }

        public static Fixture GetFixturePropertyValue(Body body, string property, string value)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                Hashtable hashtable = fixture.UserData as Hashtable;
                if (hashtable != null && hashtable.Exists(property) && value == hashtable.GetString(property))
                {
                    return fixture;
                }
            }
            return null;
        }

        public static void SetMassMass(Body body, float mass)
        {
            body.Mass = mass;
        }

        public static Vector2 LimitDistance(Vector2 vec, Vector2 center, float maxDistance)
        {
            Vector2 vector = vec - center;
            float num = vector.Length();
            if (num < maxDistance)
            {
                return vec;
            }
            vector *= maxDistance / num;
            return vector + center;
        }

        public static void LimitSpeedSpeed(Body body, float maxSpeed)
        {
            Vector2 vector = body.LinearVelocity;
            float num = vector.Length();
            if (num > maxSpeed)
            {
                vector *= maxSpeed / num;
                body.LinearVelocity = vector;
            }
        }

        public static Fixture FixtureByIdBody(string _id, Body body)
        {
            return GetFixturePropertyValue(body, "id", _id);
        }

        public static void StepToTargetStep(Body body, Vector2 target, float step)
        {
            Vector2 vector = stepToVec(body.Position, target, step);
            body.SetTransform(vector, body.Rotation);
        }

        public static void SetDensityValue(Body body, float value)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                fixture.Shape.Density = value;
            }
            body.ResetMassData();
        }

        public static void WakeUpJoined(Body body)
        {
            for (JointEdge jointEdge = body.JointList; jointEdge != null; jointEdge = jointEdge.Next)
            {
                jointEdge.Joint.BodyA.Awake = true;
                jointEdge.Joint.BodyB.Awake = true;
            }
        }

        public static void SetGroupIndex(Body body, short index)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                fixture.CollisionGroup = index;
            }
        }

        public static RevoluteJoint CreateRevoluteJointBody2PositionCollideConnectedLimitAngles(World world, Body body1, Body body2, Vector2 position, bool collideConnected, bool limitAngles)
        {
            RevoluteJoint revoluteJoint = JointFactory.CreateRevoluteJoint(world, body1, body2, position - body2.Position);
            revoluteJoint.CollideConnected = collideConnected;
            if (limitAngles)
            {
                revoluteJoint.LowerLimit = 0f;
                revoluteJoint.UpperLimit = 0f;
                revoluteJoint.LimitEnabled = true;
            }
            return revoluteJoint;
        }

        public static RevoluteJoint CreateRevoluteJoint(World world, Body body1, Body body2, Vector2 position, bool collideConnected)
        {
            return CreateRevoluteJointBody2PositionCollideConnectedLimitAngles(world, body1, body2, position, collideConnected, false);
        }

        public static DistanceJoint CreateDistanceJointBody2CollideConnectedFreqDamping(World world, Body body1, Body body2, bool collideConnected, float freq, float damping)
        {
            DistanceJoint distanceJoint = JointFactory.CreateDistanceJoint(world, body1, body2, Vector2.Zero, Vector2.Zero);
            distanceJoint.CollideConnected = collideConnected;
            distanceJoint.Frequency = freq;
            distanceJoint.DampingRatio = damping;
            return distanceJoint;
        }

        public static void SetSensor(Body body, bool value)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                fixture.IsSensor = value;
            }
        }

        public static float ScalarMultiplyTo(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Vector2 GetVectorProjectionTarget(Vector2 source, Vector2 target)
        {
            float num = ScalarMultiplyTo(source, target);
            float num2 = target.LengthSquared();
            if (Maths.FuzzyEquals(num2, 0f, 0.0001f))
            {
                return new Vector2(0f, 0f);
            }
            num /= num2;
            return new Vector2(num * target.X, num * target.Y);
        }

        public static float GetProjectionTarget(Vector2 source, Vector2 target)
        {
            return ScalarMultiplyTo(source, target) / target.Length();
        }

        public static Vector2 GetWorldPoint(Contact contact)
        {
            contact.GetWorldManifold(out Vector2 vector, out FixedArray2<Vector2> fixedArray);
            return fixedArray[0];
        }

        public static bool IsTouchingType(Body body, Type type)
        {
            for (ContactEdge contactEdge = body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
            {
                if (contactEdge.Contact.IsTouching && !contactEdge.Contact.FixtureA.IsSensor && !contactEdge.Contact.FixtureB.IsSensor && (type == typeof(Nullable) || (contactEdge.Other.UserData != null && type.IsInstanceOfType(contactEdge.Other.UserData))))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsTouchingBody(Body bodyA, Body bodyB)
        {
            for (ContactEdge contactEdge = bodyA.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
            {
                if (contactEdge.Contact.IsTouching && contactEdge.Other == bodyB)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsTouching(Body body)
        {
            return IsTouchingType(body, typeof(Nullable));
        }

        public static bool IsSensor(Contact contact)
        {
            return contact.FixtureA.IsSensor || contact.FixtureB.IsSensor;
        }

        public static bool IsSensor(this Body body)
        {
            foreach (Fixture fixture in body.FixtureList)
            {
                if (!fixture.IsSensor)
                {
                    return false;
                }
            }
            return true;
        }

        public static Body CreateCircle(World world, float radius, Vector2 position, float rotation = 0f, float density = 0f, bool dynamic = false)
        {
            CircleShape circleShape = new(radius, density);
            return BodyFromShape(world, circleShape, position, rotation, false, density, dynamic);
        }

        public static Body CreateBoxByVerticesPositionVerticesSensorDensityDynamic(World world, Vector2 position, List<Vector2> vertices, bool sensor, float density, bool dynamic)
        {
            Vertices vertices2 = new(vertices);
            PolygonShape polygonShape = new(vertices2, density);
            return BodyFromShape(world, polygonShape, position, 0f, sensor, density, dynamic);
        }

        public static Body CreateBox(World world, Vector2 position, float width, float height, float rotation, bool sensor, float density, bool dynamic)
        {
            Vertices vertices = new(4);
            float num = width / 2f;
            float num2 = height / 2f;
            vertices.Add(new Vector2(-num, -num2));
            vertices.Add(new Vector2(num, -num2));
            vertices.Add(new Vector2(num, num2));
            vertices.Add(new Vector2(-num, num2));
            PolygonShape polygonShape = new(vertices, density);
            return BodyFromShape(world, polygonShape, position, rotation, sensor, density, dynamic);
        }

        public static Fixture AddShapeToDensity(Shape shape, Body body, float density)
        {
            Fixture fixture = body.CreateFixture(shape, density);
            fixture.Friction = Box2DConfig.DefaultConfig.Friction;
            return fixture;
        }

        public static Body BodyFromShape(World world, Shape shape, Vector2 position, float rotation, bool sensor, float density, bool dynamic)
        {
            Body body = BodyFactory.CreateBody(world);
            body.BodyType = dynamic ? BodyType.Dynamic : BodyType.Static;
            body.Position = position;
            body.Rotation = rotation;
            Fixture fixture = AddShapeToDensity(shape, body, density);
            fixture.Friction = Box2DConfig.DefaultConfig.Friction;
            fixture.IsSensor = sensor;
            return body;
        }

        public static List<Fixture> RaycastWorldStartPointEndPoint(World world, Vector2 startPoint, Vector2 endPoint)
        {
            RaycastQuery raycastQuery = new();
            world.RayCast(new Func<Fixture, Vector2, Vector2, float, float>(raycastQuery.ReportFixture), startPoint, endPoint);
            return raycastQuery.Fixtures;
        }

        public static List<Fixture> QueryPointWorldPoint(World world, Vector2 point)
        {
            List<Fixture> list = new();
            AABB aabb = AabbFromPointPoint(point);
            AABBQuery aabbquery = new();
            world.QueryAABB(new Func<Fixture, bool>(aabbquery.CallbackReportFixture), ref aabb);
            for (int i = 0; i < aabbquery.Fixtures.Count; i++)
            {
                Fixture fixture = aabbquery.Fixtures[i];
                if (fixture.TestPoint(ref point))
                {
                    list.Add(fixture);
                }
            }
            return list;
        }

        public static void MoveBody(Body body, Vector2 position, float teleportCoeff, float time)
        {
            MoveBody(body, position, body.Rotation, teleportCoeff, time);
        }

        public static void MoveBody(Body body, Vector2 position, float angle, float teleportCoeff, float time)
        {
            float num = 1f - teleportCoeff;
            Vector2 vector = position - body.Position;
            float num2 = angle - body.Rotation;
            Vector2 vector2 = vector;
            vector2 *= num / time;
            vector *= teleportCoeff;
            body.LinearVelocity = vector2;
            body.AngularVelocity = num2 * num / time;
            body.SetTransform(body.Position + vector, body.Rotation + num2 * teleportCoeff);
        }

        public static BodyClip Query(World world, Vector2 center, float radius, Type type)
        {
            List<Fixture> list = QueryAABBWorldCenterRadius(world, center, radius);
            foreach (Fixture fixture in list)
            {
                if (fixture.Body.UserData != null && type.IsInstanceOfType(fixture.Body.UserData))
                {
                    return (BodyClip)fixture.Body.UserData;
                }
            }
            return null;
        }

        public static List<BodyClip> QueryAll(World world, Vector2 center, float radius, Type protocol)
        {
            return QueryBodyClipsCenterRadiusReqParam(world, center, radius, new ClipSatisfyDelegate(protocolReq), protocol);
        }

        public static List<BodyClip> QueryBodyClipsCenterRadiusType(World world, Vector2 center, float radius, Type type)
        {
            return QueryBodyClipsCenterRadiusReqParam(world, center, radius, new ClipSatisfyDelegate(typeReq), type);
        }

        public static List<BodyClip> QueryBodyClipsCenterRadiusReqParam(World world, Vector2 center, float radius, ClipSatisfyDelegate clipSatisfyDelegate, object param)
        {
            List<Fixture> list = QueryAABBWorldCenterRadius(world, center, radius);
            List<BodyClip> list2 = new();
            foreach (Fixture fixture in list)
            {
                BodyClip bodyClip = fixture.Body.UserData as BodyClip;
                if (bodyClip != null && clipSatisfyDelegate(bodyClip, param))
                {
                    list2.Add(bodyClip);
                }
            }
            return list2;
        }

        private static List<Fixture> QueryAABBWorldCenterRadius(World world, Vector2 center, float radius)
        {
            AABB aabb = CreateAABBCenterWidthHeight(center, radius * 2f, radius * 2f);
            return QueryAABBWorldAabb(world, aabb);
        }

        public static Fixture GetClosestFixture(World world, Vector2 center, float width, float height)
        {
            List<Fixture> list = Query(world, center, width, height);
            if (list.Count == 0)
            {
                return null;
            }
            FixtureDistanceComparer fixtureDistanceComparer = new(center);
            Fixture fixture = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                if (fixtureDistanceComparer.Compare(fixture, list[i]) < 0)
                {
                    fixture = list[i];
                }
            }
            return fixture;
        }

        public static List<Fixture> Query(World world, Vector2 center, float width, float height)
        {
            AABB aabb = CreateAABBCenterWidthHeight(center, width, height);
            return QueryAABBWorldAabb(world, aabb);
        }

        public static List<Fixture> QueryAABBWorldAabb(World world, AABB aabb)
        {
            AABBQuery aabbquery = new();
            world.QueryAABB(new Func<Fixture, bool>(aabbquery.CallbackReportFixture), ref aabb);
            return aabbquery.Fixtures;
        }

        public static void FixturesUnderPointWorldPoint(List<Fixture> fixtures, World world, Vector2 point)
        {
            foreach (Body body in world.BodyList)
            {
                foreach (Fixture fixture in body.FixtureList)
                {
                    if (fixture.TestPoint(ref point))
                    {
                        fixtures.Add(fixture);
                    }
                }
            }
        }

        public static AABB AabbFromPointPoint(Vector2 center)
        {
            return CreateAABBCenterWidthHeight(center, 0.033333335f, 0.033333335f);
        }

        public static AABB CreateAABBCenterWidthHeight(Vector2 center, float width, float height)
        {
            AABB aabb = default(AABB);
            aabb.LowerBound = center;
            aabb.UpperBound = center;
            aabb.LowerBound -= new Vector2(width / 2f, height / 2f);
            aabb.UpperBound += new Vector2(width / 2f, height / 2f);
            return aabb;
        }

        public static void CreateLevelBordersSizeBorders(Body groundBody, Vector2 size, Borders borders)
        {
            if (borders.Bottom)
            {
                CreateBorderStartEndDirection(groundBody, new Vector2(-size.X / 2f, 0f), new Vector2(size.X * 1.5f, 0f), new Vector2(0f, -0.16666667f));
            }
            if (borders.Top)
            {
                CreateBorderStartEndDirection(groundBody, new Vector2(-size.X / 2f, size.Y), new Vector2(size.X * 1.5f, size.Y), new Vector2(0f, 0.16666667f));
            }
            if (borders.Left)
            {
                CreateBorderStartEndDirection(groundBody, new Vector2(0f, size.Y * 1.5f), new Vector2(0f, -size.Y / 2f), new Vector2(-0.16666667f, 0f));
            }
            if (borders.Right)
            {
                CreateBorderStartEndDirection(groundBody, new Vector2(size.X, size.Y * 1.5f), new Vector2(size.X, -size.Y / 2f), new Vector2(0.16666667f, 0f));
            }
        }

        public static void CreateBorderStartEndDirection(Body groundBody, Vector2 start, Vector2 end, Vector2 direction)
        {
            for (int i = 0; i < 2; i++)
            {
                _ = FixtureFactory.AttachEdge(start, end, groundBody);
                start += direction;
                end += direction;
            }
        }

        private static void CreateLevelBordersSize(Body groundBody, Vector2 size)
        {
            CreateLevelBordersSizeBorders(groundBody, size, new Borders(true, true, true, true));
        }

        public static float Atan2Target(Vector2 source, Vector2 target)
        {
            return Maths.atan2(target.Y - source.Y, target.X - source.X);
        }

        public static Vector2 ToVecAngle(float module, float angle)
        {
            return new Vector2(module * Maths.Cos(angle), module * Maths.Sin(angle));
        }

        public static Vector2 GetCrossPointStart1End1Start2End2(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2)
        {
            float num = (end1.Y - start1.Y) * (start2.X - end2.X) - (start2.Y - end2.Y) * (end1.X - start1.X);
            float num2 = (end1.Y - start1.Y) * (start2.X - start1.X) - (start2.Y - start1.Y) * (end1.X - start1.X);
            float num3 = (start2.Y - start1.Y) * (start2.X - end2.X) - (start2.Y - end2.Y) * (start2.X - start1.X);
            if (Maths.FuzzyEquals(num, 0f, 0.0001f) && Maths.FuzzyEquals(num2, 0f, 0.0001f) && Maths.FuzzyEquals(num3, 0f, 0.0001f))
            {
                return start1;
            }
            if (Maths.FuzzyEquals(num, 0f, 0.0001f))
            {
                return new Vector2(100100100f, 100100100f);
            }
            float num4 = num3 / num;
            Vector2 vector = new(start1.X + (end1.X - start1.X) * num4, start1.Y + (end1.Y - start1.Y) * num4);
            return vector;
        }

        public static Vector2 GetCenter(Vector2 first, Vector2 second)
        {
            return (first + second) / 2f;
        }

        public static Vector2 GetCenter(ArrayList points)
        {
            Vector2 vector = default(Vector2);
            foreach (object obj in points)
            {
                Vector2 vector2 = (Vector2)obj;
                vector.X += vector2.X;
                vector.Y += vector2.Y;
            }
            vector *= 1f / points.Count;
            return vector;
        }

        private const float BORDER_STEP = 0.16666667f;

        public delegate bool ClipSatisfyDelegate(BodyClip clip, object param);

        public class Borders(bool left, bool top, bool right, bool bottom)
        {
            public bool Left { get; set; } = left;

            public bool Top { get; set; } = top;

            public bool Right { get; set; } = right;

            public bool Bottom { get; set; } = bottom;
        }

        public class b2Vec2Segment
        {
            public b2Vec2Segment()
            {
                A = Vector2.Zero;
                B = Vector2.Zero;
            }

            public b2Vec2Segment(Vector2 A, Vector2 B)
            {
                this.A = A;
                this.B = B;
            }

            public Vector2 A = default(Vector2);

            public Vector2 B = default(Vector2);
        }
    }
}
