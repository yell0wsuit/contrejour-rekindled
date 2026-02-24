using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class RopeMetricsWithCoords : RopeMetrics
    {
        public Vector2 PartOffset
        {
            get
            {
                return partOffset;
            }
        }

        public RopeMetricsWithCoords(int _parts, float _partSize, Vector2 _start, Vector2 _end)
            : base(_parts, _partSize)
        {
            start = _start;
            end = _end;
            partOffset = _end - _start;
            partOffset *= 1f / _parts;
        }

        public Vector2 GetPositionByIndex(int index)
        {
            Vector2 vector = start;
            Vector2 vector2 = partOffset;
            vector2 *= index;
            return vector + vector2;
        }

        protected Vector2 start;

        protected Vector2 end;

        protected Vector2 partOffset;
    }
}
