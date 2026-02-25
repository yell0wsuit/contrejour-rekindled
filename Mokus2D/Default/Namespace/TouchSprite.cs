using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Input;
using Mokus2D.Visual;
using Mokus2D.Visual.Data;

namespace Mokus2D.Default.Namespace
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

        public virtual void TouchBegan(Touch touch)
        {
            TouchBeganEvent.SendEvent();
        }

        public virtual void TouchEnd(Touch touch)
        {
            TouchEndEvent.SendEvent();
        }

        public virtual void TouchOut(Touch touch)
        {
            TouchOutEvent.SendEvent();
        }

        public virtual void Click(Touch touch)
        {
            ClickEvent.SendEvent(this);
        }

        public readonly EventSender TouchBeganEvent = new();

        public readonly EventSender TouchEndEvent = new();

        public readonly EventSender TouchOutEvent = new();

        public readonly EventSender<TouchSprite> ClickEvent = new();
    }
}
