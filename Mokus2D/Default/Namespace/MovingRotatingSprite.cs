using Microsoft.Xna.Framework;

using Mokus2D.Util.MathUtils;

namespace Default.Namespace
{
    public class MovingRotatingSprite(string filename) : RotatingSprite(filename)
    {
        public override Vector2 Position
        {
            set
            {
                base.Position = value;
                initialPosition = value;
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            changer.Update(time);
            base.Position = initialPosition + new Vector2(changer.Value, 0f);
        }

        public void Initialize(float amplitude, float progress)
        {
            changer.MinValue = -amplitude;
            changer.MaxValue = amplitude;
            changer.Step = 0.1f;
            changer.Progress = progress;
        }

        protected CosChanger changer = new(0f, 0f);

        protected Vector2 initialPosition;
    }
}
