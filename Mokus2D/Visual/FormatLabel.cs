using Microsoft.Xna.Framework.Graphics;

namespace Mokus2D.Visual
{
    public class FormatLabel(SpriteFont font, string format) : Label(font)
    {
        public void SetValues(params object[] args)
        {
            _ = Clear();
            _ = AppendFormat(format, args);
        }
    }
}
