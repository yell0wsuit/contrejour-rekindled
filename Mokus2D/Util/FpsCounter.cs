using Mokus2D.Default.Namespace;

namespace Mokus2D.Util
{
    public class FpsCounter(int framesToCalculate) : IUpdatable
    {
        public float Fps { get; private set; }

        public void Update(float time)
        {
            currentFrame++;
            seconds += time;
            if (currentFrame == framesToCalculate)
            {
                currentFrame = 0;
                Fps = framesToCalculate / seconds;
                seconds = 0f;
            }
        }

        private int currentFrame;

        private float seconds;
    }
}
