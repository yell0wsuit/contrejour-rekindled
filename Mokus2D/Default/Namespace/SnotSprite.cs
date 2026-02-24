using System.Collections.Generic;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Util.Data;

namespace Default.Namespace
{
    public class SnotSprite : LongNeckSprite
    {
        public SnotSprite(SnotBodyClipBase _snot, float _startWidth, float _centerWidth, float _endWidth)
        {
            snot = _snot;
            data = snot.Physics;
            startWidth = _startWidth;
            endWidth = _endWidth;
            startWidthPixels = startWidth / 0.033333335f;
            endWidthPixels = endWidth / 0.033333335f;
            centerWidth = _centerWidth;
            borderWidth = 3f;
        }

        public override void GetPairs(List<Pair<Vector2>> result)
        {
            Vector2 startPosition = snot.StartPosition;
            Pair<Vector2> pair = ContreDrawUtil.GetPointsPairStartEndWidthResult(startPosition, startPosition, data.BodyAt(0).Position, startWidth);
            result.Add(ContreDrawUtil.ccp2Pair(pair));
            Vector2 vector = startPosition;
            Body body = null;
            for (int i = 0; i < data.BodiesSize() - 1; i++)
            {
                body = data.BodyAt(i);
                Body body2 = data.BodyAt(i + 1);
                pair = ContreDrawUtil.GetPointsPairStartEndWidthResult(body.Position, vector, body2.Position, centerWidth);
                result.Add(ContreDrawUtil.ccp2Pair(pair));
                vector = body2.Position;
                if (i < data.BodiesSize() - 2)
                {
                    Vector2 vector2 = body.Position + body2.Position;
                    vector2 *= 0.5f;
                    pair = ContreDrawUtil.GetPointsPairStartEndWidthResult(vector2, body.Position, body2.Position, centerWidth);
                    result.Add(ContreDrawUtil.ccp2Pair(pair));
                }
            }
            pair = ContreDrawUtil.GetPointsPairStartEndWidthResult(snot.EndPosition(), body.Position, snot.EndPosition(), endWidth);
            result.Add(ContreDrawUtil.ccp2Pair(pair));
        }

        public const int CIRCLE_SEGMENTS = 12;

        protected SnotBodyClipBase snot;

        protected SnotData data;

        protected float startWidth;

        protected float endWidth;

        protected float startWidthPixels;

        protected float endWidthPixels;

        protected float centerWidth;

        protected List<Vector2> surface;
    }
}
