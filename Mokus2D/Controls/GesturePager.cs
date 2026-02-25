using System;

using Default.Namespace;

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
            pageWidth = ScreenConstants.OsSizes.W7.X;
        }

        public bool Enabled
        {
            get => enabled;
            set
            {
                enabled = value;
                if (!value)
                {
                    currentTouch = null;
                }
            }
        }

        public float CurrentPosition
        {
            get => currentPosition; set => currentPosition = value;
        }

        public float PageWidth
        {
            get => pageWidth; set => pageWidth = value;
        }

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
            touchStartPosition = currentPosition;
            return true;
        }

        public bool TouchMove(Touch touch)
        {
            if (!Enabled)
            {
                return false;
            }
            currentPosition = touchStartPosition - touch.TotalOffset.X / pageWidth;
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
                targetPosition = (int)Math.Round(currentPosition);
                return;
            }
            SetTargetPosition();
        }

        public void Update(float time)
        {
            if (currentTouch == null && currentPosition != targetPosition)
            {
                float num = Math.Max((currentPosition - targetPosition).Abs() / 10f, MinMoveStep) * time * 60f;
                currentPosition = currentPosition.StepTo(targetPosition, num);
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
                targetPosition = (int)Math.Floor(currentPosition);
            }
            else
            {
                targetPosition = (int)Math.Ceiling(currentPosition);
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

        private float currentPosition;

        private Touch currentTouch;

        private float direction;

        private bool enabled = true;

        private float pageWidth;

        private int targetPosition;

        private float touchStartPosition;
    }
}
