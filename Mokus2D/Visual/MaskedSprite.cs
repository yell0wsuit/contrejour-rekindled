using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mokus2D.Visual
{
    public class MaskedSprite : RenderSprite
    {
        public MaskedSprite(AnchorNode mask)
            : this(mask, mask.Size)
        {
            Anchor = mask.Anchor;
        }

        public MaskedSprite(SpriteBatchNode mask, Vector2 size)
            : this(size)
        {
            Mask = mask;
        }

        public bool UpdateMask
        {
            get => maskRoot.UpdateChildren; set => maskRoot.UpdateChildren = value;
        }

        public SpriteBatchNode Mask
        {
            get => mask;
            set
            {
                if (mask != null)
                {
                    maskRoot.RemoveChild(mask);
                }
                mask = value;
                if (mask != null)
                {
                    maskRoot.AddChild(mask);
                }
            }
        }

        public MaskedSprite(Vector2 size)
            : base(size)
        {
            blendState = new BlendState();
            blendState.ColorDestinationBlend = Microsoft.Xna.Framework.Graphics.Blend.SourceAlpha;
            blendState.AlphaDestinationBlend = Microsoft.Xna.Framework.Graphics.Blend.SourceAlpha;
            blendState.ColorSourceBlend = Microsoft.Xna.Framework.Graphics.Blend.Zero;
            blendState.AlphaSourceBlend = Microsoft.Xna.Framework.Graphics.Blend.Zero;
            maskRoot = new MaskRoot((int)size.X, (int)size.Y);
        }

        protected override void DrawContent()
        {
            if (mask == null)
            {
                return;
            }
            base.DrawContent();
            maskRoot.ScaleX = Math.Sign(Root.ScaleX);
            maskRoot.ScaleY = Math.Sign(Root.ScaleY);
            maskRoot.SpritesScaleFactor = Root.SpritesScaleFactor;
            BlendState blend = mask.Blend;
            mask.Blend = blendState;
            maskRoot.Position = AnchorInPixels;
            maskRoot.DrawAll();
            mask.Blend = blend;
        }

        protected readonly MaskRoot maskRoot;

        private readonly BlendState blendState;

        private SpriteBatchNode mask;
    }
}
