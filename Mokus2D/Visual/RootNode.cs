using Microsoft.Xna.Framework;

using Mokus2D.Visual.Data;

namespace Mokus2D.Visual
{
    public class RootNode : Node
    {
        public RootNode(int width, int height, Vector2 spritesScaleFactor)
        {
            Root = this;
            rootState = new VisualState(width, height, spritesScaleFactor);
            compositeState = new VisualState(rootState);
        }

        public RootNode(int width, int height)
            : this(width, height, Vector2.One)
        {
        }

        public Vector2 SpritesScaleFactor
        {
            get => rootState.SpritesScaleFactor; set => rootState.SpritesScaleFactor = value;
        }

        public void DrawAll()
        {
            DrawNode(rootState);
        }

        private readonly VisualState rootState;
    }
}
