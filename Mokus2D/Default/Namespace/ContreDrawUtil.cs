using System;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Util.Data;

namespace Default.Namespace
{
    public static class ContreDrawUtil
    {
        public static Pair<Vector2> ccp2Pair(Pair<Vector2> pair)
        {
            return new Pair<Vector2>(CocosUtil.ccp2Point(pair.First), CocosUtil.ccp2Point(pair.Second));
        }

        public static Pair<Vector2> GetPointsPair(Vector2 center, Vector2 start, Vector2 end, float width)
        {
            Vector2 vector = end - start;
            Vector2 vector2 = vector.Rotate90().Normalize(width / 2f);
            return new Pair<Vector2>(center + vector2, center - vector2);
        }

        public static Pair<Vector2> GetPointsPairStartEndWidthResult(Vector2 center, Vector2 start, Vector2 end, float width)
        {
            Vector2 vector = Box2DConfig.DefaultConfig.ToPoint(center);
            Vector2 vector2 = Box2DConfig.DefaultConfig.ToPoint(start);
            Vector2 vector3 = Box2DConfig.DefaultConfig.ToPoint(end);
            width /= 0.033333335f;
            return GetPointsPair(vector, vector2, vector3, width);
        }
    }
}
