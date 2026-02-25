using System.Collections.Generic;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;

namespace Default.Namespace
{
    public class SurfaceCreator
    {
        public static PlasticineItem CreateParentPointsMaxWidth(ContreJourLevelBuilder builder, PlasticineBodyClip parent, List<Vector2> points, float maxWidth, out PlasticineItem leftItem)
        {
            if (builder.ContreJour.BlackSide)
            {
                maxWidth *= 1f;
            }
            List<Vector2> vertices = GetVertices(points, maxWidth);
            PlasticineItem plasticineItem = null;
            PlasticineItem plasticineItem2 = null;
            leftItem = null;
            int num = 0;
            bool flag = false;
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                if (num <= 0 && Maths.Rand() < 0.2f)
                {
                    num = Maths.randRangeInt(5, 10);
                }
                Vector2 vector = vertices[i];
                Vector2 vector2 = vertices[i + 1];
                float num2 = vector.DistanceTo(vector2);
                float num3 = FarseerUtil.Atan2Target(vector, vector2);
                Vector2 partCenterEnd = GetPartCenterEnd(vector, vector2);
                float num4 = 0.6f;
                Body body = PlasticineUtil.CreateSurfaceBodyWidthAnglePosition(builder.World, num4, num3, partCenterEnd);
                bool flag2 = !flag && (num > 0 || builder.ContreJour.WhiteSide || builder.ContreJour.RoseChapter || builder.ContreJour.BonusChapter);
                PlasticinePartBodyClip plasticinePartBodyClip = new(builder, body, parent, num2, flag2);
                num--;
                PlasticineItem plasticineItem3 = new(plasticinePartBodyClip, num2);
                if (leftItem == null || plasticineItem3.InitialPosition.X < leftItem.InitialPosition.X)
                {
                    leftItem = plasticineItem3;
                }
                if (plasticineItem2 != null)
                {
                    plasticineItem2.InsertAfter(plasticineItem3);
                }
                else
                {
                    plasticineItem = plasticineItem3;
                }
                plasticineItem2 = plasticineItem3;
            }
            plasticineItem.InsertBefore(plasticineItem2);
            return plasticineItem;
        }

        public static PlasticineItem TryAddCircleAngleFirstItemMaxItemsIndex(PlasticineItem item, float angle, PlasticineItem firstItem, int maxItems, int index)
        {
            bool flag = true;
            PlasticineItem plasticineItem = item;
            int num = 0;
            while (num < maxItems && plasticineItem != firstItem.PreviousItem)
            {
                plasticineItem = plasticineItem.NextItem;
                float num2 = Maths.SimplifyAngleRadiansStartValue(plasticineItem.Body.Rotation, -1.5707964f);
                if (Maths.Abs(num2 - angle) > 0.5235988f)
                {
                    flag = false;
                    break;
                }
                num++;
            }
            if (flag)
            {
                item.BodyClip.AddCircle(index);
                index++;
            }
            return plasticineItem;
        }

        public static Vector2 GetPartCenterEnd(Vector2 start, Vector2 end)
        {
            Vector2 vector = end - start;
            Vector2 center = FarseerUtil.GetCenter(start, end);
            Maths.RotateMinus90(ref vector);
            Maths.SetLength(ref vector, 0.41666666f);
            return center + vector;
        }

        private static void GetBezierVerticesControlEndWidth(Vector2 start, Vector2 control, Vector2 end, float width, ref List<Vector2> result)
        {
            Bezier bezier = new(start, control, end);
            RopeMetrics ropeMetricsByLengthMaxPartSizeMinParts = RopeUtil.GetRopeMetricsByLengthMaxPartSizeMinParts(bezier.Length, width, 3);
            List<float> timesSequenceWithStepStartShift = bezier.GetTimesSequenceWithStepStartShift(ropeMetricsByLengthMaxPartSizeMinParts.PartSize, ropeMetricsByLengthMaxPartSizeMinParts.PartSize);
            for (int i = 0; i < ropeMetricsByLengthMaxPartSizeMinParts.Parts; i++)
            {
                float num = timesSequenceWithStepStartShift[i];
                Vector2 pointByTime = bezier.GetPointByTime(num);
                result.Add(pointByTime);
            }
        }

        public static List<Vector2> GetVertices(List<Vector2> points, float width)
        {
            List<Vector2> list = new();
            Vector2 vector;
            for (int i = 0; i < points.Count - 1; i++)
            {
                vector = FarseerUtil.GetCenter(points[i], points[i + 1]);
                list.Add(vector);
            }
            vector = FarseerUtil.GetCenter(points[points.Count - 1], points[0]);
            list.Add(vector);
            list.Insert(0, vector);
            List<Vector2> list2 = new()
            {
                Capacity = list.Count * 5
            };
            for (int j = 0; j < list.Count - 1; j++)
            {
                GetBezierVerticesControlEndWidth(list[j], points[j], list[j + 1], width, ref list2);
            }
            list2.Add(vector);
            return list2;
        }

        private const float CIRCLE_RANDOM = 0.5f;

        private const float CIRCLE_ANGLE = 0.5235988f;

        private const int CIRCLE_DISTANCE_MAX = 6;

        private const int CIRCLE_DISTANCE_MIN = 3;

        private const float BLACK_MULTIPLIER = 1f;

        private const float GRASS_MAX_COUNT = 10f;

        private const float GRASS_MIN_COUNT = 5f;

        private const float GRASS_RANDOM = 0.2f;
    }
}
