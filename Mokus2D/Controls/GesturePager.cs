using System;

using Mokus2D.Default.Namespace;

using Mokus2D.Input;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Mokus2D.Controls
{
    public class GesturePager : ITouchListener, IDisposable, IUpdatable
    {
        public GesturePager()
        {
            Mokus2DGame.TouchController.AddListener(this, 0);
            PageWidth = ScreenConstants.OsSizes.W7.X;
        }

        public bool Enabled
        {
            get;
            set
            {
                field = value;
                if (!value)
                {
                    currentTouch = null;
                }
            }
        } = true;

        public float CurrentPosition { get; set; }

        public float PageWidth { get; set; }

        public void Dispose()
        {
            Mokus2DGame.TouchController.RemoveListener(this);
        }

        public bool TouchBegin(Touch touch)
        {
            if (currentTouch != null || !Enabled)
            {
                return false;
            }
            currentTouch = touch;
            touchStartPosition = CurrentPosition;
            return true;
        }

        public bool TouchMove(Touch touch)
        {
            if (!Enabled)
            {
                return false;
            }
            CurrentPosition = touchStartPosition - (touch.TotalOffset.X / PageWidth);
            if (touch.LastFrameOffset.X != 0f)
            {
                direction = -touch.LastFrameOffset.X.Sign();
            }
            return true;
        }

        public void TouchEnd(Touch touch)
        {
            currentTouch = null;
            if (Math.Abs(touch.TotalOffset.X) < MinMoveOffset)
            {
                targetPosition = (int)Math.Round(CurrentPosition);
                return;
            }
            SetTargetPosition();
        }

        public void Update(float time)
        {
            if (currentTouch == null && CurrentPosition != targetPosition)
            {
                float num = Math.Max((CurrentPosition - targetPosition).Abs() / 10f, MinMoveStep) * time * 60f;
                CurrentPosition = CurrentPosition.StepTo(targetPosition, num);
            }
        }

        public void SetTargetPosition(int value)
        {
            targetPosition = value;
        }

        private void SetTargetPosition()
        {
            if (direction < 0f)
            {
                targetPosition = (int)Math.Floor(CurrentPosition);
            }
            else
            {
                targetPosition = (int)Math.Ceiling(CurrentPosition);
            }
            if (MinPosition != null)
            {
                targetPosition = Math.Max(targetPosition, MinPosition.Value);
            }
            if (MaxPosition != null)
            {
                targetPosition = Math.Min(targetPosition, MaxPosition.Value);
            }
        }

        public int? MaxPosition;

        public float MinMoveOffset = 20f;

        public float MinMoveStep = 0.025f;

        public int? MinPosition;
        private Touch currentTouch;

        private float direction;
        private int targetPosition;

        private float touchStartPosition;
    }
}
