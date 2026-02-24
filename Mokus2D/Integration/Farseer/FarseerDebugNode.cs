using System;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Extensions;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Mokus2D.Integration.Farseer
{
    public class FarseerDebugNode(World world, float sizeMultiplier) : PrimitivesNode
    {
        protected override void DrawPrimitives()
        {
            foreach (Body body in world.BodyList)
            {
                foreach (Fixture fixture in body.FixtureList)
                {
                    DrawFixture(fixture, body);
                }
            }
            foreach (Joint joint in world.JointList)
            {
                DrawJoint(joint);
            }
        }

        private void DrawJoint(Joint j)
        {
        }

        private void DrawFixture(Fixture fixture, Body body)
        {
            if (fixture.Shape.ShapeType == ShapeType.Circle)
            {
                DrawCircle((CircleShape)fixture.Shape, body, fixture);
                return;
            }
            if (fixture.Shape.ShapeType == ShapeType.Polygon)
            {
                DrawPolygon((PolygonShape)fixture.Shape, body, fixture);
                return;
            }
            if (fixture.Shape.ShapeType == ShapeType.Edge)
            {
                DrawEdge((EdgeShape)fixture.Shape, body, fixture);
            }
        }

        private void DrawEdge(EdgeShape shape, Body body, Fixture fixture)
        {
            VertexPositionColor[] array = new VertexPositionColor[2];
            array[0].Color = GetColor(body, fixture);
            array[1].Color = GetColor(body, fixture);
            array[0].Position = body.GetWorldPoint(shape.Vertex1).ToVector3() * sizeMultiplier;
            array[1].Position = body.GetWorldPoint(shape.Vertex2).ToVector3() * sizeMultiplier;
            GraphUtil.DrawLineList(array);
        }

        private void DrawPolygon(PolygonShape shape, Body body, Fixture fixture)
        {
            VertexPositionColor[] array = new VertexPositionColor[shape.Vertices.Count];
            Color color = GetColor(body, fixture);
            for (int i = 0; i < shape.Vertices.Count; i++)
            {
                array[i].Color = color;
                Vector2 vector = shape.Vertices[i % shape.Vertices.Count];
                array[i].Position = body.GetWorldPoint(vector).ToVector3() * sizeMultiplier;
            }
            GraphUtil.DrawTriangleFan(array);
        }

        private void DrawCircle(CircleShape shape, Body body, Fixture fixture)
        {
            VertexPositionColor[] array = new VertexPositionColor[20];
            Color color = GetColor(body, fixture);
            for (int i = 0; i < 20; i++)
            {
                array[i].Color = color;
                Vector2 vector = VectorUtil.ToVector(shape.Radius, i * 6.2831855f / 20f) + shape.Position;
                array[i].Position = body.GetWorldPoint(vector).ToVector3() * sizeMultiplier;
            }
            GraphUtil.DrawTriangleFan(array);
        }

        private Color GetColor(Body body, Fixture fixture)
        {
            float num = (fixture.IsSensor ? 0.5f : 1f);
            switch (body.BodyType)
            {
                case BodyType.Static:
                    return STATIC_COLOR * num;
                case BodyType.Kinematic:
                    return KINEMATIC_COLOR * num;
                case BodyType.Dynamic:
                    return DYNAMIC_COLOR * num;
                default:
                    throw new InvalidOperationException("Unknown body type");
            }
        }

        private const int CIRCLE_SEGMENTS = 20;

        private readonly Color STATIC_COLOR = ColorUtil.CreateColor(255, 0, 255, 127);

        private readonly Color DYNAMIC_COLOR = ColorUtil.CreateColor(0, 255, 0, 127);

        private readonly Color KINEMATIC_COLOR = ColorUtil.CreateColor(0, 0, 255, 127);
    }
}
