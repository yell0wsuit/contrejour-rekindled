using Microsoft.Xna.Framework.Graphics;

namespace Mokus2D.Visual.Data
{
    public class TextureData(Texture2D texture, ClipData config)
    {
        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        public ClipData Config
        {
            get
            {
                return config;
            }
        }
    }
}
