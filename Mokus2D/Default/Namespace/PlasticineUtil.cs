using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class PlasticineUtil
    {
        public static PolygonShape CreateSurfaceBox(float width)
        {
            FarseerPhysics.Common.Vertices vertices = new(4);
            float num = width / 2f;
            float num2 = 0.41666666f;
            vertices.Add(new Vector2(-num, -num2));
            vertices.Add(new Vector2(num, -num2));
            vertices.Add(new Vector2(num, num2));
            vertices.Add(new Vector2(-num, num2));
            PolygonShape polygonShape = new(vertices, 0.3f);
            return polygonShape;
        }

        public static Body CreateSurfaceBodyWidthAnglePosition(World world, float width, float angle, Vector2 position)
        {
            Body body = FarseerUtil.CreateBox(world, position, width, 0.8333333f, angle, false, 0.3f, false);
            body.BodyType = BodyType.Kinematic;
            float num = -0.625f;
            Vector2 vector = new(-width / 2f, num);
            Vector2 vector2 = new(width / 2f, num);
            Fixture fixture = FixtureFactory.AttachEdge(vector, vector2, body, 0.3f);
            PlasticineConstants.ApplyStaticBodiesFilter(fixture);
            fixture.UserData = LIMIT;
            PlasticineConstants.ApplyStaticBodiesFilter(body);
            body.LinearDamping = 10f;
            body.AngularDamping = 10f;
            body.Inertia *= 10f;
            return body;
        }

        public static PlasticineItem GetItemCountDirection(PlasticineItem start, int count, int direction)
        {
            PlasticineItem plasticineItem = start;
            for (int i = 0; i < count; i++)
            {
                if (direction < 0)
                {
                    plasticineItem = plasticineItem.PreviousItem;
                }
                else
                {
                    plasticineItem = plasticineItem.NextItem;
                }
            }
            return plasticineItem;
        }

        private const float LINEAR_DAMPING = 10f;

        private const float ANGULAR_DAMPING = 10f;

        private const float MAX_ANGLE = 0.3926991f;

        public static object LIMIT = new();
    }
}
