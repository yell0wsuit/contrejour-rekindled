using Mokus2D.Input;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public interface ITouchNode : ISizeNode
    {
        void TouchBegan(Touch touch);

        void TouchEnd(Touch touch);

        void TouchOut(Touch touch);

        void Click(Touch touch);
    }
}
