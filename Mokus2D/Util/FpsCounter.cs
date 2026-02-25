using Default.Namespace;

namespace Mokus2D.Util
{
    public class FpsCounter(int framesToCalculate) : IUpdatable
    {
        public float Fps => fps;

        public void Update(float time)
        {
            currentFrame++;
            seconds += time;
            if (currentFrame == framesToCalculate)
            {
                currentFrame = 0;
                fps = framesToCalculate / seconds;
                seconds = 0f;
            }
        }

        private int currentFrame;

        private float seconds;

        private float fps;
    }
}
