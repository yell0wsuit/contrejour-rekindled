using System;

using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class RopeUtil
    {
        public static RopeMetrics GetRopeMetricsByLengthMaxPartSizeMinParts(float distance, float maxPartSize, int minParts)
        {
            int num = Maths.max((int)Math.Ceiling((double)(distance / maxPartSize)), minParts);
            float num2 = distance / num;
            return new RopeMetrics(num, num2);
        }

        public static RopeMetricsWithCoords GetRopeMetricsEndMaxPartSizeMinPartsLength(Vector2 start, Vector2 end, float maxPartSize, int minParts, float length)
        {
            RopeMetrics ropeMetricsByLengthMaxPartSizeMinParts = GetRopeMetricsByLengthMaxPartSizeMinParts(length, maxPartSize, minParts);
            return new RopeMetricsWithCoords(ropeMetricsByLengthMaxPartSizeMinParts.Parts, ropeMetricsByLengthMaxPartSizeMinParts.PartSize, start, end);
        }

        public static RopeMetricsWithCoords GetRopeMetricsEndMaxPartSizeMinParts(Vector2 start, Vector2 end, float maxPartSize, int minParts)
        {
            float num = (start - end).Length();
            return GetRopeMetricsEndMaxPartSizeMinPartsLength(start, end, maxPartSize, minParts, num);
        }
    }
}
