using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class GreenSnotSprite(ContreJourGame _game, SnotBodyClipBase _snot, float _startWidth, float _centerWidth, float _endWidth) : BlackSnotSprite(_game, _snot, _startWidth, _centerWidth, _endWidth)
    {
        public override Color initialStartColor()
        {
            return ContreJourConstants.GreenSnotStart;
        }

        public override Color initialEndColor()
        {
            return ContreJourConstants.GreenSnotEnd;
        }

        public override Color EndColor()
        {
            Color color = CocosUtil.ccc4Mix(EndCircleColor(), BaseCircleColor(), activeProgress);
            color.A = 0;
            return color;
        }
    }
}
