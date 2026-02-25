using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Input;

namespace Mokus2D.Default.Namespace
{
    public class SnotEye(SnotBodyClip _snot, Body _body) : ContreJourBodyClip(_snot.Builder, _body, null, null), IClickable
    {
        public SnotBodyClip Snot => snot;

        public bool AcceptFreeTouches()
        {
            return false;
        }

        public bool UseForZoom()
        {
            return false;
        }

        public bool DisableHeroFocus => true;

        public virtual int Priority(Vector2 touchPoint)
        {
            return !snot.Joined ? -10 : 1;
        }

        public virtual bool TouchBegan(Touch touch)
        {
            hasRelease = true;
            return true;
        }

        public virtual void TouchEnd(Touch touch)
        {
            CheckTouchDistance(touch);
            if (hasRelease)
            {
                snot.ReleaseSnot();
                ((ContreJourGame)builder.Game).FocusOnHero();
                hasRelease = false;
            }
        }

        public virtual bool TouchMove(Touch touch)
        {
            CheckTouchDistance(touch);
            return true;
        }

        public void TouchOut(Touch touch)
        {
        }

        private void CheckTouchDistance(Touch touch)
        {
            Vector2 vector = builder.TouchRootVec(touch);
            CheckTouchDistance(touch, FarseerUtil.b2Vec2Distance(vector, Body.Position));
        }

        protected virtual void CheckTouchDistance(Touch touch, float distance)
        {
            if (distance > CocosUtil.iPad(1.6666666f, 2.8333333f))
            {
                hasRelease = false;
                FreeTouch(touch);
            }
        }

        protected virtual void FreeTouch(Touch touch)
        {
            Game.FreeTouch(touch);
        }

        private const float MAX_RADIUS_IPHONE = 2.8333333f;

        private const float MAX_RADIUS = 1.6666666f;

        protected bool hasRelease;

        protected SnotBodyClip snot = _snot;
    }
}
