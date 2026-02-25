using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual.Data;
using Mokus2D.Visual.Util;

namespace Mokus2D.Visual
{
    public abstract class PrimitivesNode : Node
    {
        public override void Draw(VisualState state)
        {
            using (new PrimitivesDrawing(state, state.PrimitivesMatrix, Texture))
            {
                DrawPrimitives();
            }
        }

        public Texture2D Texture
        {
            get;
            set
            {
                DecreaseTextureReferencesCount();
                field = value;
                if (field != null)
                {
                    Mokus2DGame.SharedContent.IncreaseReferenceCount(field.Name);
                }
            }
        }

        private void DecreaseTextureReferencesCount()
        {
            if (Texture != null)
            {
                Mokus2DGame.SharedContent.DecreaseReferenceCount(Texture.Name);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            DecreaseTextureReferencesCount();
        }

        protected abstract void DrawPrimitives();
    }
}
