using Microsoft.Xna.Framework.Graphics;

namespace Mokus2D.Visual.Data
{
    public class TextureData(Texture2D texture, ClipData config)
    {
        public Texture2D Texture => texture;

        public ClipData Config => config;
    }
}
