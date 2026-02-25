using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Util.Data;

namespace Mokus2D.Default.Namespace
{
    public class SpikesFlowerSprite : LongNeckSprite
    {
        public SpikesFlowerSprite(SpikesFlowerBodyClip bodyClip, float _scale)
        {
            if (bodyClip.Game.WhiteSide)
            {
                NeckColor = 10066329.ToRGBColor();
            }
            else if (bodyClip.Game.BonusChapter)
            {
                NeckColor = ContreJourConstants.GreenSpikesFlower;
            }
            spikes = bodyClip;
            basePoints = new Pair<Vector2>(CocosUtil.ccp2(6f, -32f) * _scale, CocosUtil.ccp2(-6f, -32f) * _scale);
            centerPoints = new Pair<Vector2>(CocosUtil.ccp2(-3f, 0f) * _scale, CocosUtil.ccp2(3f, 0f) * _scale);
            childScale = _scale;
        }

        public override void GetPairs(List<Pair<Vector2>> result)
        {
            Vector2 vector = CocosUtil.toRetina(spikes.Eye.Position);
            Pair<Vector2> pair = new(vector + CocosUtil.ccp2(5f * childScale, 0f), vector - CocosUtil.ccp2(5f * childScale, 0f));
            result.Add(pair);
            result.Add(centerPoints);
            result.Add(basePoints);
        }

        protected SpikesFlowerBodyClip spikes;

        protected Pair<Vector2> basePoints;

        protected Pair<Vector2> centerPoints;

        protected float childScale;
    }
}
