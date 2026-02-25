using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Util.Data;

namespace Default.Namespace
{
    public class LianaSprite : LongNeckSprite
    {
        public LianaSprite(ILianaDrawData _data, Color neckColor, float _width, float _borderWidth)
        {
            data = _data;
            width = _width * 0.033333335f;
            NeckColor = neckColor;
            borderWidth = _borderWidth;
            CreateParts();
        }

        public LianaSprite(ILianaDrawData _data, Color _color)
            : this(_data, _color, 0f, CocosUtil.r(Maths.randRange(8f, 10f)))
        {
        }

        public void CreateParts()
        {
            partsLength.Add(2);
            for (int i = 0; i < data.PointsCount() - 2; i++)
            {
                Vector2 vector = data.PositionAt(i);
                Vector2 vector2 = data.PositionAt(i + 1);
                Vector2 vector3 = data.PositionAt(i + 2);
                float num = FarseerUtil.b2Vec2Distance(vector, vector2) / 2f + FarseerUtil.b2Vec2Distance(vector2, vector3) / 2f;
                int num2 = (int)Math.Ceiling((double)(num / MAX_PART_LENGHT));
                num2 += num2 % 2;
                partsLength.Add(num2);
            }
            partsLength.Add(2);
        }

        public override void GetPairs(List<Pair<Vector2>> result)
        {
            Vector2 vector = data.PositionAt(0);
            Vector2 vector2 = data.PositionAt(1);
            Pair<Vector2> pair = ContreDrawUtil.GetPointsPairStartEndWidthResult(vector, vector, vector2, width);
            result.Add(pair);
            result.Add(pair);
            for (int i = 1; i < data.PointsCount(); i++)
            {
                Vector2 vector3 = data.PositionAt(i - 1);
                vector2 = data.PositionAt(i);
                vector = FarseerUtil.b2Vec2Middle(vector3, vector2);
                pair = ContreDrawUtil.GetPointsPairStartEndWidthResult(vector, vector, vector2, width);
                result.Add(pair);
                Vector2 vector4 = data.PositionAt(Maths.min(i + 1, data.PointsCount() - 1));
                pair = ContreDrawUtil.GetPointsPairStartEndWidthResult(vector2, vector3, vector4, width);
                result.Add(pair);
            }
            Vector2 vector5 = data.PositionAt(data.PointsCount() - 1);
            pair = ContreDrawUtil.GetPointsPairStartEndWidthResult(vector5, vector, vector5, width);
            result.Add(pair);
            result.Add(pair);
        }

        public override void AddBezierPointsBezier(List<Vector2> source, ref List<Vector2> bezier)
        {
            Maths.AddBezierPointsPointsSegmentsVector(bezier, source, partsLength);
        }

        protected float width;

        protected ILianaDrawData data;

        protected List<int> partsLength = new();

        private readonly float MAX_PART_LENGHT = 0.26666668f;
    }
}
