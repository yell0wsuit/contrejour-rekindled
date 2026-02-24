using Microsoft.Xna.Framework;

using Mokus2D.Input;

namespace Default.Namespace
{
    public class TouchPositionProvider(Touch _touch, LevelBuilderBase _builder) : IVectorPositionProvider
    {
        public Touch Touch
        {
            get
            {
                return touch;
            }
        }

        public Vector2 PositionVec
        {
            get
            {
                return builder.TouchRootVec(touch);
            }
        }

        protected LevelBuilderBase builder = _builder;

        protected Touch touch = _touch;
    }
}
