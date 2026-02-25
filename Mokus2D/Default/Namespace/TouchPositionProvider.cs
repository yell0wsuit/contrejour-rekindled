using Microsoft.Xna.Framework;

using Mokus2D.Input;

namespace Mokus2D.Default.Namespace
{
    public class TouchPositionProvider(Touch _touch, LevelBuilderBase _builder) : IVectorPositionProvider
    {
        public Touch Touch => touch;

        public Vector2 PositionVec => builder.TouchRootVec(touch);

        protected LevelBuilderBase builder = _builder;

        protected Touch touch = _touch;
    }
}
