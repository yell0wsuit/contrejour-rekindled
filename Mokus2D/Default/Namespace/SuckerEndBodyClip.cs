using Microsoft.Xna.Framework;

using Mokus2D.Input;

namespace Default.Namespace
{
    public class SuckerEndBodyClip(SuckerBodyClip _sucker, object _body) : ContreJourBodyClip(_sucker.Builder, _body, null, null), IClickable
    {
        public bool DisableHeroFocus
        {
            get
            {
                return true;
            }
        }

        public int Priority(Vector2 touchPoint)
        {
            return 1;
        }

        public bool AcceptFreeTouches()
        {
            return false;
        }

        public bool UseForZoom()
        {
            return false;
        }

        public bool TouchBegan(Touch _touch)
        {
            if (touch == null && !sucker.Dragging)
            {
                touch = _touch;
                sucker.StartDrag(touch);
                return true;
            }
            return false;
        }

        public void TouchEnd(Touch _touch)
        {
            sucker.FinishDrag();
            touch = null;
        }

        public bool TouchMove(Touch touch)
        {
            return true;
        }

        public void TouchOut(Touch touch)
        {
        }

        protected Touch touch;

        protected SuckerBodyClip sucker = _sucker;
    }
}
