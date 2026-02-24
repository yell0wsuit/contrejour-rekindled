using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Data;

namespace Mokus2D.Visual
{
    public abstract class SpriteBatchNode : Node
    {
        public override void Draw(VisualState state)
        {
            base.Draw(state);
            Color color = state.Color * state.Opacity;
            BeginDraw(state);
            DrawSprite(state, color);
            EndDraw();
        }

        protected abstract void DrawSprite(VisualState state, Color color);

        protected virtual void EndDraw()
        {
            batch.End();
        }

        protected virtual void BeginDraw(VisualState state)
        {
            if (batch == null)
            {
                batch = new SpriteBatch(Mokus2DGame.Device);
            }
            batch.Begin(SpriteSortMode.Deferred, Blend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, state.Matrix);
        }

        public BlendState Blend = BlendState.AlphaBlend;

        protected static SpriteBatch batch;
    }
}
