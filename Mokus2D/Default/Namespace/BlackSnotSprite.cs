using Microsoft.Xna.Framework;

using Mokus2D.Extensions;

namespace Mokus2D.Default.Namespace
{
    public class BlackSnotSprite : SpringSnotSprite
    {
        public BlackSnotSprite(ContreJourGame _game, SnotBodyClipBase _snot, float _startWidth, float _centerWidth, float _endWidth)
            : base(_game, _snot, _startWidth, _centerWidth, _endWidth)
        {
            borderWidth = 3f;
        }

        public int PointsInNeckPart()
        {
            return 4;
        }

        public virtual Color initialStartColor()
        {
            return START_COLOR;
        }

        public virtual Color initialEndColor()
        {
            return END_COLOR;
        }

        public override Color BaseCircleColor()
        {
            return initialStartColor();
        }

        public override Color EndCircleColor()
        {
            return initialEndColor();
        }

        public override Color EndColor()
        {
            Color color = EndCircleColor().Mult(activeProgress);
            color.A = 0;
            return color;
        }

        public virtual Color GetIntermidiateColorLineSize(int index, int lineSize)
        {
            return CocosUtil.ccc4Mix(initialEndColor(), initialStartColor(), index / (float)lineSize);
        }

        public override void CreateVectors(int _allPointsSize)
        {
            base.CreateVectors(_allPointsSize);
            int num = _allPointsSize / 2;
            Color color = initialStartColor();
            Color color2 = GetIntermidiateColorLineSize(1, num);
            for (int i = 0; i < num - 1; i++)
            {
                int num2 = i * 6;
                vertices[num2].Color = color;
                vertices[num2 + 1].Color = color2;
                vertices[num2 + 2].Color = color;
                vertices[num2 + 3].Color = color;
                vertices[num2 + 4].Color = color2;
                vertices[num2 + 5].Color = color2;
                color = color2;
                color2 = GetIntermidiateColorLineSize(i + 2, num);
            }
        }

        public override void SetBorderColors()
        {
            BlackDrawUtil.SetBorderColors(allPointsSize, initialStartColor(), initialEndColor(), EndColor(), border);
        }

        private readonly Color END_COLOR = new(0, 254, 254, 255);

        private readonly Color START_COLOR = new(0, 94, 118, 255);
    }
}
