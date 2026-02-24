using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class ProgressLabel(SpriteFont font, string _format, int _value, int _steps) : Label(font)
    {
        public int Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public override void Update(float time)
        {
            currentStep++;
            Clear();
            AppendFormat(format, [CurrentValue]);
            if (currentStep == steps)
            {
                UpdateEnabled = false;
            }
        }

        public int CurrentValue
        {
            get
            {
                return (int)(value * (float)currentStep / steps);
            }
        }

        protected string format = _format;

        protected int value = _value;

        protected int steps = _steps;

        protected int currentStep = 0;
    }
}
