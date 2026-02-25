using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Extensions;
using Mokus2D.Util.Data;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class LongNeckSprite : PrimitivesNode
    {
        public Color NeckColor
        {
            get => neckColor;
            set
            {
                neckColor = value;
                if (border != null)
                {
                    SetBorderColors();
                }
                if (vertices != null)
                {
                    SetNeckColors();
                }
            }
        }

        public LongNeckSprite()
        {
            neckColor = new Color(0, 0, 0, 255);
            borderWidth = 2f;
        }

        public virtual void GetPairs(List<Pair<Vector2>> target)
        {
        }

        public override void Update(float time)
        {
            cachedPairs.Clear();
            GetPairs(cachedPairs);
            if (cachedPairs.Count <= 2)
            {
                return;
            }
            first.Clear();
            second.Clear();
            foreach (Pair<Vector2> pair in cachedPairs)
            {
                first.Add(pair.First);
                second.Add(pair.Second);
            }
            firstBezier.Clear();
            secondBezier.Clear();
            AddBezierPointsBezier(first, ref firstBezier);
            AddBezierPointsBezier(second, ref secondBezier);
            CreatePolygonsFirstBezierSecondBezier(cachedPairs, firstBezier, secondBezier);
        }

        public virtual void AddBezierPointsBezier(List<Vector2> source, ref List<Vector2> bezier)
        {
            Maths.AddBezierPointsPointsSegments(bezier, source, 6);
        }

        public void CreatePolygonsFirstBezierSecondBezier(List<Pair<Vector2>> pairs, List<Vector2> firstBezier, List<Vector2> secondBezier)
        {
            ProcessBezierSecond(firstBezier, secondBezier);
            for (int i = 0; i < firstBezier.Count - 1; i++)
            {
                int num = i * 6;
                vertices[num].Position = firstBezier[i].ToVector3();
                vertices[num + 1].Position = firstBezier[i + 1].ToVector3();
                vertices[num + 2].Position = secondBezier[i].ToVector3();
                vertices[num + 3].Position = secondBezier[i].ToVector3();
                vertices[num + 4].Position = secondBezier[i + 1].ToVector3();
                vertices[num + 5].Position = firstBezier[i + 1].ToVector3();
            }
        }

        public void ProcessBezierSecond(List<Vector2> firstBezier, List<Vector2> secondBezier)
        {
            allPoints.Clear();
            allPoints.Capacity = firstBezier.Count + secondBezier.Count;
            allPoints.AddRange(secondBezier);
            for (int i = firstBezier.Count - 1; i >= 0; i--)
            {
                allPoints.Add(firstBezier[i]);
            }
            TryCreateVectors(allPoints);
            GraphUtil.CreateGradientBorderWidthVertices(allPoints, borderWidth, border);
        }

        public void TryCreateVectors(List<Vector2> allPoints)
        {
            if (!created)
            {
                CreateVectors(allPoints.Count);
                created = true;
            }
        }

        public virtual void CreateVectors(int _allPointsSize)
        {
            vertices = new VertexPositionColorTexture[(_allPointsSize - 2) * 3];
            allPointsSize = _allPointsSize;
            int num = allPointsSize * 6;
            border = new VertexPositionColorTexture[num];
            SetBorderColors();
            SetNeckColors();
        }

        protected virtual void SetNeckColors()
        {
            GraphUtil.SetColor(vertices, neckColor);
        }

        public virtual void SetBorderColors()
        {
            GraphUtil.CreateGradientColorsList(allPointsSize, neckColor, EndColor(), border);
        }

        public virtual Color EndColor()
        {
            return NeckColor.ChangeAlpha(0);
        }

        protected override void DrawPrimitives()
        {
            DrawPolygons();
            DrawBorder();
        }

        public virtual void DrawBorder()
        {
            if (border != null)
            {
                GraphUtil.FillTrianglesList(border, null);
            }
        }

        public virtual void DrawPolygons()
        {
            if (vertices != null)
            {
                GraphUtil.FillTrianglesList(vertices, null);
            }
        }

        private const int BEZIER_PARTS = 6;

        protected bool created;

        protected VertexPositionColorTexture[] vertices;

        protected VertexPositionColorTexture[] border;

        private Color neckColor;

        protected float borderWidth;

        protected int allPointsSize;

        private readonly List<Vector2> first = new(64);

        private readonly List<Vector2> second = new(64);

        private List<Vector2> firstBezier = new(64);

        private List<Vector2> secondBezier = new(64);

        private readonly List<Vector2> allPoints = new();

        private readonly List<Pair<Vector2>> cachedPairs = new(64);
    }
}
