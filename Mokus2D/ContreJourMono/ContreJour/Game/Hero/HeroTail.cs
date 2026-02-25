using System;
using System.Collections.Generic;

using Default.Namespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Extensions;
using Mokus2D.Util.Data;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;
using Mokus2D.Visual.Data;
using Mokus2D.Visual.Util;

namespace ContreJourMono.ContreJour.Game.Hero
{
    public class HeroTail : PrimitivesNode
    {
        public HeroTail(Color color)
        {
            Color = color;
            while (surface.Count < 10)
            {
                surface.Add(Vector2.Zero);
            }
            points = new List<PointAndAngle>([middle, middle2, end]);
        }

        public HeroTail()
            : this(Color.Black)
        {
        }

        public float BorderWidth
        {
            get => borderWidth; set => borderWidth = value;
        }

        public void SetMovementDirection(float value)
        {
            currentAngle = value - 3.1415927f;
        }

        public Color EndColor
        {
            get
            {
                Color color = Color;
                color.A = 0;
                return color;
            }
        }

        public override void Update(float time)
        {
            float num = currentAngle;
            if (LimitAngles)
            {
                num = Maths.SimplifyAngle(currentAngle, -1.5707964f);
                num = num.Clamp(0.3926991f, 2.7488937f);
            }
            PointAndAngle pointAndAngle = points[0];
            float num2 = Maths.SimplifyAngle(num, pointAndAngle.Angle - 3.1415927f);
            float num3 = time * 30f * UpdateSpeed;
            foreach (PointAndAngle pointAndAngle2 in points)
            {
                float num4 = (float)Math.Min((double)Math.Abs(pointAndAngle2.Angle + num2) / 3.141592653589793 * 2.0, 0.5);
                float num5 = pointAndAngle2.AngleStep * Speed * num4 * num3;
                float num6 = 0.3926991f;
                pointAndAngle2.Angle = pointAndAngle2.Angle.StepTo(num2, num5).Clamp(pointAndAngle.Angle - num6, pointAndAngle.Angle + num6);
                pointAndAngle2.Update(Speed / 2f * num3, LimitAngles, num3);
            }
            Triangulate();
        }

        private void Triangulate()
        {
            Pair<Vector2> pointsPair = ContreDrawUtil.GetPointsPair(Vector2.Zero, middle.Position, Vector2.Zero, (25f - borderWidth) * 2f);
            Vector2 vector = VectorUtil.Center(middle.Position, middle2.Position);
            Pair<Vector2> pointsPair2 = ContreDrawUtil.GetPointsPair(middle.Position, middle2.Position, vector, 12.5f);
            Pair<Vector2> pointsPair3 = ContreDrawUtil.GetPointsPair(vector, middle2.Position, vector, 12.5f);
            Pair<Vector2> pointsPair4 = ContreDrawUtil.GetPointsPair(middle2.Position, middle2.Position, vector, 8.333333f);
            Vector2 vector2 = VectorUtil.Center(pointsPair.First, pointsPair.Second);
            cachedPolygon.Clear();
            cachedPolygon.Add(pointsPair.First);
            cachedPolygon.Add(pointsPair2.First);
            cachedPolygon.Add(pointsPair3.First);
            cachedPolygon.Add(pointsPair4.First);
            cachedPolygon.Add(end.Position);
            cachedPolygon.Add(pointsPair4.Second);
            cachedPolygon.Add(pointsPair3.Second);
            cachedPolygon.Add(pointsPair2.Second);
            cachedPolygon.Add(pointsPair.Second);
            cachedPolygon.Add(vector2);
            GraphUtil.CreateBezierPoints(cachedPolygon, 2, surface);
            if (vertices == null)
            {
                vertices = new VertexPositionColor[surface.Count];
                border = new VertexPositionColor[surface.Count * 6];
                GraphUtil.CreateGradientBorderColors(border, Color);
            }
            GraphUtil.CreateGradientBorder(surface, borderWidth, border);
            for (int i = 0; i < surface.Count; i++)
            {
                int num = (i % 2 == 0) ? (i / 2) : (surface.Count - 1 - i / 2);
                vertices[i] = new VertexPositionColor(surface[num].ToVector3(), Color);
            }
        }

        public override void Draw(VisualState state)
        {
            if (vertices != null)
            {
                base.Draw(state);
            }
        }

        protected override void DrawPrimitives()
        {
            GraphUtil.DrawTriangleStrip(vertices);
            GraphUtil.DrawTriangleList(border);
        }

        private const float CENTER_ANGLE = 1.5707964f;

        private const float ANGLE_LIMIT = 1.1780972f;

        private const float MIDDLE1_DISTANCE = 30f;

        private const float MIDDLE2_DISTANCE = 50f;

        private const float END_DISTANCE = 70f;

        private const int POINTS = 10;

        public bool LimitAngles;

        public float Speed;

        private float currentAngle;

        private List<PointAndAngle> points;

        private PointAndAngle middle = new(30f, 0.02f, 0f);

        private PointAndAngle middle2 = new(50f, 0.019f, 1.5707964f);

        private PointAndAngle end = new(70f, 0.018f, 3.1415927f);

        private VertexPositionColor[] vertices;

        private List<Vector2> surface = new(10);

        private List<Vector2> cachedPolygon = new(64);

        private VertexPositionColor[] border;

        private float borderWidth = 2f;

        public float UpdateSpeed = 1f;
    }
}
