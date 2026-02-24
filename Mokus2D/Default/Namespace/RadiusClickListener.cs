using Microsoft.Xna.Framework;

using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class RadiusClickListener(Node _target, float _clickRadius, int priority = 0) : ClickListener(priority)
    {
        public bool DisableDrag
        {
            get
            {
                return disableDrag;
            }
            set
            {
                disableDrag = value;
            }
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled && target.Visible;
            }
        }

        private bool SpriteContainsPoint(Touch touch)
        {
            return CocosUtil.TouchPointInNode(touch, target).Length() < clickRadius;
        }

        public override bool TouchBegin(Touch touch)
        {
            if (SpriteContainsPoint(touch))
            {
                base.TouchBegin(touch);
                return true;
            }
            return false;
        }

        protected override bool IsOutStartPosition(Touch touch, Vector2 _startPosition)
        {
            bool flag = !SpriteContainsPoint(touch);
            if (disableDrag)
            {
                flag |= base.IsOutStartPosition(touch, _startPosition);
            }
            return flag;
        }

        protected Node target = _target;

        protected float clickRadius = _clickRadius;

        protected bool disableDrag;
    }
}
