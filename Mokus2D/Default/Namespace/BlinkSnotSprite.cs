using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class BlinkSnotSprite : SpringSnotSprite
    {
        public bool Highlite
        {
            get => highlite; set => highlite = value;
        }

        private BlinkSnotSprite(ContreJourGame _game, SnotBodyClipBase _snot, float _startWidth, float _centerWidth, float _endWidth)
            : base(_game, _snot, _startWidth, _centerWidth, _endWidth)
        {
            highliteStep = 0f;
            initialColor = CocosUtil.ccc4ToCcc3(NeckColor);
            highlite = true;
        }

        public override void Update(float time)
        {
            base.Update(time);
            float num = highliteStep;
            highliteStep = Maths.stepTo(highliteStep, highlite ? 1 : 0, 0.02f);
            if (Maths.FuzzyNotEquals(num, highliteStep, 0.0001f))
            {
                Color color = CocosUtil.ccc4Mix(HIGHLITE_COLOR, initialColor, highliteStep);
                NeckColor = CocosUtil.ccc3ToCcc4(color, NeckColor.A);
                SetBorderColors();
                SetCirclesColors();
            }
        }

        protected bool highlite;

        protected float highliteStep;

        protected Color initialColor;

        private static readonly Color HIGHLITE_COLOR = ContreJourConstants.BLUE_LIGHT_COLOR;
    }
}
