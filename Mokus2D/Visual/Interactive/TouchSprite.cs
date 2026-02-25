using Mokus2D.Default.Namespace;

using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Input;
using Mokus2D.Visual.Data;

namespace Mokus2D.Visual.Interactive
{
    public class TouchSprite : Sprite, ITouchNode, ISizeNode
    {
        public TouchSprite(TextureData data)
            : base(data)
        {
        }

        public TouchSprite(string name)
            : base(name)
        {
        }

        public TouchSprite(Texture2D texture)
            : base(texture)
        {
        }

        public void TouchBegan(Touch touch)
        {
            TouchBeganEvent.SendEvent();
        }

        public void TouchEnd(Touch touch)
        {
            TouchEndEvent.SendEvent();
        }

        public void TouchOut(Touch touch)
        {
            TouchOutEvent.SendEvent();
        }

        public void Click(Touch touch)
        {
            ClickEvent.SendEvent();
        }

        public readonly EventSender TouchBeganEvent = new();

        public readonly EventSender TouchEndEvent = new();

        public readonly EventSender TouchOutEvent = new();

        public readonly EventSender ClickEvent = new();
    }
}
