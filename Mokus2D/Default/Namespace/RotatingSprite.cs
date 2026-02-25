using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Visual;
using Mokus2D.Visual.Data;

namespace Mokus2D.Default.Namespace
{
    public class RotatingSprite : Sprite
    {
        public float Speed { get; set; }

        public RotatingSprite(string name)
            : base(name)
        {
        }

        public RotatingSprite(Texture2D texture)
            : base(texture)
        {
        }

        public RotatingSprite(TextureData data)
            : base(data)
        {
        }

        public override void Update(float time)
        {
            Rotation += Speed * time;
        }
    }
}
