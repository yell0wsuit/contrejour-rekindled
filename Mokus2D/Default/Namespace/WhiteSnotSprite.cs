using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class WhiteSnotSprite(ContreJourGame _game, SnotBodyClipBase _snot, float _startWidth, float _centerWidth, float _endWidth) : BlackSnotSprite(_game, _snot, _startWidth, _centerWidth, _endWidth)
    {
        public override Color initialStartColor()
        {
            return ContreJourConstants.WHITE_SNOT_START_COLOR;
        }

        public override Color initialEndColor()
        {
            return ContreJourConstants.WHITE_SNOT_END_COLOR;
        }

        public override Color EndColor()
        {
            Color color = CocosUtil.ccc4Mix(EndCircleColor(), BaseCircleColor(), activeProgress);
            color.A = 0;
            return color;
        }
    }
}
