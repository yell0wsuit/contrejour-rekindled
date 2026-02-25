using System;

using Default.Namespace;

using Microsoft.Xna.Framework;

using Mokus2D.Input;
using Mokus2D.Util.MathUtils;

namespace Mokus2D.Controls
{
    public class ScrollLayer : ClickableLayer
    {
        public float ReturnCoeff
        {
            get => returnCoeff;
            set
            {
                if (returnCoeff != value)
                {
                    returnCoeff = value;
                }
            }
        }

        public bool Scrolling => scrollTouch != null;

        public void RefreshBorders()
        {
            MinPosition = MaxPosition = Position;
        }

        protected override bool OnTouchMove(Touch touch)
        {
            if (scrollTouch == touch)
            {
                Vector2 touchPosition = GetTouchPosition();
                Vector2 vector = inertia;
                inertia = touchPosition - previousTouchPosition;
                inertia.X = FixInertiaValue(vector.X, inertia.X);
                inertia.Y = FixInertiaValue(vector.Y, inertia.Y);
                previousTouchPosition = touchPosition;
                Position = initialPosition + touchPosition - initialTouchPosition;
                EnsureBounds();
            }
            return true;
        }

        private float FixInertiaValue(float old, float current)
        {
            float num = current;
            if (current.Sign() == old.Sign() && old.Abs() > current.Abs())
            {
                num = old.StepTo(current, 20f);
            }
            return num.Clamp(-100f, 100f);
        }

        protected override bool OnTouchBegin(Touch touch)
        {
            if (scrollTouch == null)
            {
                scrollTouch = touch;
                initialPosition = Position;
                initialTouchPosition = GetTouchPosition();
                previousTouchPosition = initialTouchPosition;
            }
            return true;
        }

        private Vector2 GetTouchPosition()
        {
            return Parent.GlobalToLocal(scrollTouch.Position, true);
        }

        protected override void OnTouchEnd(Touch touch)
        {
            if (scrollTouch == touch)
            {
                scrollTouch = null;
            }
        }

        private float GetTimeValue(float value, float time)
        {
            return value * 60f * time;
        }

        public override void Update(float time)
        {
            if (scrollTouch == null)
            {
                if (InertiaHorizontal)
                {
                    X += inertia.X;
                    inertia.X = inertia.X.StepTo(0f, GetTimeValue(2f, time));
                    if (!X.Between(MinPosition.X, MaxPosition.X))
                    {
                        X = GetReturnValue(X, MinPosition.X, MaxPosition.X, time);
                        inertia.X = inertia.X.StepTo(0f, GetTimeValue(3f, time));
                    }
                }
                if (InertiaVertical)
                {
                    Y += inertia.Y;
                    inertia.Y = inertia.Y.StepTo(0f, GetTimeValue(2f, time));
                    if (!Y.Between(MinPosition.Y, MaxPosition.Y))
                    {
                        Y = GetReturnValue(Y, MinPosition.Y, MaxPosition.Y, time);
                        inertia.Y = inertia.Y.StepTo(0f, GetTimeValue(3f, time));
                    }
                }
            }
        }

        private float GetReturnValue(float current, float min, float max, float time)
        {
            return current < min ? StepToBorder(current, min, time) : current > max ? StepToBorder(current, max, time) : current;
        }

        private float StepToBorder(float current, float max, float time)
        {
            float num = Math.Max(Math.Abs(current - max) * returnCoeff, 1f);
            return current.StepTo(max, GetTimeValue(num, time));
        }

        private void EnsureBounds()
        {
            if (!InertiaHorizontal)
            {
                X = X.Clamp(MinPosition.X, MaxPosition.X);
            }
            if (!InertiaVertical)
            {
                Y = Y.Clamp(MinPosition.Y, MaxPosition.Y);
            }
        }

        public ScrollLayer()
            : base(0)
        {
        }

        public const float MinReturnStep = 1f;

        public const float InertiaChange = 2f;

        public const float InertiaChangeOffLimit = 3f;

        public const float MaxInertia = 100f;

        public const float MaxInertiaStep = 20f;

        public Vector2 MinPosition = Vector2.Zero;

        public Vector2 MaxPosition = Vector2.Zero;

        public bool InertiaHorizontal;

        public bool InertiaVertical = true;

        private Touch scrollTouch;

        private Vector2 initialPosition;

        private Vector2 initialTouchPosition;

        private Vector2 previousTouchPosition;

        private Vector2 inertia = Vector2.Zero;

        private float returnCoeff = 0.2f;
    }
}
