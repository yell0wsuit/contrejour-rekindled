using System;

using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class AccelerometerMenu : Node, IDisposable, ITouchListener
    {
        protected virtual int Priority
        {
            get
            {
                return 1;
            }
        }

        public AccelerometerMenu()
        {
            accelerometer.Start();
            Mokus2DGame.TouchController.AddListener(this, Priority);
        }

        public bool IsFastDevice()
        {
            return true;
        }

        public virtual bool TouchBegin(Touch touch)
        {
            return false;
        }

        public virtual bool TouchMove(Touch touch)
        {
            return false;
        }

        public virtual void TouchEnd(Touch touch)
        {
        }

        public override void Update(float time)
        {
            float num = accelerometer.CurrentValue.Acceleration.X;
            if (num > 0f)
            {
                num *= -1f;
            }
            float num2 = Maths.Clamp(-accelerometer.CurrentValue.Acceleration.Y * 3f * MAX_ACC_OFFSET.X, -MAX_ACC_OFFSET.X, MAX_ACC_OFFSET.X);
            float num3 = Maths.Clamp((num + 0.7f) * MAX_ACC_OFFSET.Y * 2f, -MAX_ACC_OFFSET.Y, MAX_ACC_OFFSET.Y);
            if (!accelerometerUsed)
            {
                accelerometerOffset = new Vector2(num2, num3);
                return;
            }
            float num4 = Maths.Abs(num2 - accelerometerOffset.X);
            float num5 = Maths.Abs(num3 - accelerometerOffset.Y);
            if (num4 > 0.01f)
            {
                float num6 = Maths.max(num4 / 10f, MIN_ACC_STEP.X);
                accelerometerOffset.X = Maths.StepToTargetMaxStep(accelerometerOffset.X, num2, num6);
            }
            if (num5 > 3f)
            {
                float num7 = Maths.max(num5 / 10f, MIN_ACC_STEP.Y);
                accelerometerOffset.Y = Maths.StepToTargetMaxStep(accelerometerOffset.Y, num3, num7);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            accelerometer.Stop();
            Mokus2DGame.TouchController.RemoveListener(this);
        }

        protected Vector2 accelerometerOffset;

        protected bool accelerometerUsed;

        private readonly Vector2 MIN_ACC_STEP = new(0.001f, 1f);

        private readonly Vector2 MAX_ACC_OFFSET = new(0.14279968f, 80f);

        private readonly Accelerometer accelerometer = new();
    }
}
