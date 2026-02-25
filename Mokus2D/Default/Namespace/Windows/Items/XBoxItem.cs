using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace.Windows.Items
{
    public abstract class XBoxItem(string backgroundName) : Sprite(backgroundName)
    {
        protected void CreatePicture(Sprite picture)
        {
            AddChild(picture);
            picture.Position = new Vector2(32f, -32f);
            picture.Scale = 0.9411765f;
        }

        public abstract Node ViewTarget { get; }

        public new abstract Vector2 Size { get; }

        protected const float BorderOffset = 10f;
    }
}
