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
            get;
            set
            {
                if (field != null)
                {
                    maskRoot.RemoveChild(field);
                }
                field = value;
                if (field != null)
                {
                    maskRoot.AddChild(field);
                }
            }
        }

        public MaskedSprite(Vector2 size)
            : base(size)
        {
            blendState = new BlendState
            {
                ColorDestinationBlend = Microsoft.Xna.Framework.Graphics.Blend.SourceAlpha,
                AlphaDestinationBlend = Microsoft.Xna.Framework.Graphics.Blend.SourceAlpha,
                ColorSourceBlend = Microsoft.Xna.Framework.Graphics.Blend.Zero,
                AlphaSourceBlend = Microsoft.Xna.Framework.Graphics.Blend.Zero
            };
            maskRoot = new MaskRoot((int)size.X, (int)size.Y);
        }

        protected override void DrawContent()
        {
            if (Mask == null)
            {
                return;
            }
            base.DrawContent();
            maskRoot.ScaleX = Math.Sign(Root.ScaleX);
            maskRoot.ScaleY = Math.Sign(Root.ScaleY);
            maskRoot.SpritesScaleFactor = Root.SpritesScaleFactor;
            BlendState blend = Mask.Blend;
            Mask.Blend = blendState;
            maskRoot.Position = AnchorInPixels;
            maskRoot.DrawAll();
            Mask.Blend = blend;
        }

        protected readonly MaskRoot maskRoot;

        private readonly BlendState blendState;
    }
}
