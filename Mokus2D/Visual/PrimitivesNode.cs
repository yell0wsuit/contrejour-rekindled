using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Data;
using Mokus2D.Visual.Util;

namespace Mokus2D.Visual
{
    public abstract class PrimitivesNode : Node
    {
        public override void Draw(VisualState state)
        {
            using (new PrimitivesDrawing(state, state.PrimitivesMatrix, texture))
            {
                DrawPrimitives();
            }
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
            set
            {
                DecreaseTextureReferencesCount();
                texture = value;
                if (texture != null)
                {
                    Mokus2DGame.SharedContent.IncreaseReferenceCount(texture.Name);
                }
            }
        }

        private void DecreaseTextureReferencesCount()
        {
            if (texture != null)
            {
                Mokus2DGame.SharedContent.DecreaseReferenceCount(texture.Name);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DecreaseTextureReferencesCount();
        }

        protected abstract void DrawPrimitives();

        private Texture2D texture;
    }
}
