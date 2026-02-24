using Microsoft.Xna.Framework;

using Mokus2D.Visual.Data;

namespace Mokus2D.Visual
{
    public class AnimatedMaskedSprite : MaskedSprite
    {
        public AnimatedMaskedSprite(AnchorNode mask)
            : base(mask)
        {
        }

        public AnimatedMaskedSprite(SpriteBatchNode mask, Vector2 size)
            : base(mask, size)
        {
        }

        public AnimatedMaskedSprite(Vector2 size)
            : base(size)
        {
        }

        public override void Update(float time)
        {
            maskRoot.UpdateNode(time);
            renderRoot.UpdateNode(time);
            RedrawTexture();
        }

        public override void Draw(VisualState state)
        {
            base.Draw(state);
        }
    }
}
