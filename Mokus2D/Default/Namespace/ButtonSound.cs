using System;

using Mokus2D;

namespace Mokus2D.Default.Namespace
{
    public class ButtonSound
    {
        public ButtonSound(TouchSprite sprite)
        {
            sprite.ClickEvent.AddListenerSelector(new Action(OnClick));
        }

        private void OnClick()
        {
            Mokus2DGame.SoundManager.PlaySound("click", 0.8f, 0f, 0f);
        }
    }
}
