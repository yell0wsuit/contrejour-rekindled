using System;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Input;
using Mokus2D.Util.MathUtils;

namespace Default.Namespace
{
    public class MovableSnotEye(SnotBodyClip _snot, Body _body, SnotPoint targetPoint) : SnotEye(_snot, _body), IRestartable
    {
        public override int Priority(Vector2 touchPoint)
        {
            return !snot.Joined ? 0 : 1;
        }

        protected override void CheckTouchDistance(Touch touch, float distance)
        {
            if (distance > 55f * builder.SizeMult && snot.Linked == null)
            {
                hasRelease = false;
                moving = true;
                movingTouch = touch;
                snot.Enabled = false;
                DestroyEyeJoint();
                snot.Physics.EyeJoint = null;
                return;
            }
            base.CheckTouchDistance(touch, distance);
        }

        private void DestroyEyeJoint()
        {
            if (snot.Physics.EyeJoint != null)
            {
                restoreJoint = true;
                builder.World.RemoveJoint(snot.Physics.EyeJoint);
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (moving)
            {
                DragSnot(time);
            }
            else if (!moving && targetPoint != null && !FarseerUtil.FuzzyEquals(Body.Position, targetPoint.Body.Position, 0.01f))
            {
                Vector2 vector = Body.Position.StepTo(targetPoint.Body.Position, targetSpeed * time);
                MoveTo(vector, time);
            }
            else if (!snot.Enabled)
            {
                EndSnotDrag();
            }
            if (targetPoint != null)
            {
                targetPoint.Enabled = snot.Linked == null;
            }
        }

        private void EndSnotDrag()
        {
            Body.BodyType = BodyType.Dynamic;
            snot.Enabled = true;
            if (restoreJoint)
            {
                snot.Physics.EyeJoint = FarseerUtil.CreateRevoluteJoint(builder.World, snot.Physics.JoinedBody, snot.Physics.EyeBody, targetPoint.Body.Position, false);
                restoreJoint = false;
            }
        }

        private void DragSnot(float time)
        {
            Body.BodyType = BodyType.Kinematic;
            Body.LinearVelocity = Vector2.Zero;
            Vector2 vector = builder.TouchRootVec(movingTouch);
            Vector2 vector2 = targetPoint.Body.Position;
            if (vector2.DistanceTo(vector) > 55f * builder.SizeMult)
            {
                if (Body.Position.FuzzyEquals(targetPoint.Body.Position, 0.1f))
                {
                    snot.Physics.EndBody.ApplyForce((Body.Position - vector) * 3f);
                }
                vector2 = vector;
                foreach (SnotPoint snotPoint in Game.SnotPoints)
                {
                    if (!snotPoint.Used && vector2.DistanceTo(snotPoint.Body.Position) < 50f * builder.SizeMult)
                    {
                        targetPoint.Used = false;
                        vector2 = snotPoint.Body.Position;
                        targetPoint = snotPoint;
                        targetPoint.Used = true;
                    }
                }
            }
            float num = Math.Max(targetPosition.DistanceTo(vector2) / 5f, 0.5f);
            targetPosition = targetPosition.StepTo(vector2, num);
            MoveTo(targetPosition, time);
        }

        public void MoveTo(Vector2 position, float time)
        {
            Vector2 vector = position - Body.Position;
            Body.Position = Body.Position + vector;
            for (int i = 0; i < snot.Physics.BodiesSize(); i++)
            {
                snot.Physics.BodyAt(i).Position += vector;
            }
        }

        public override bool TouchBegan(Touch touch)
        {
            _ = base.TouchBegan(touch);
            targetPosition = targetPoint.Body.Position;
            snot.StopParts();
            return true;
        }

        public override void TouchEnd(Touch touch)
        {
            base.TouchEnd(touch);
            EndMove();
        }

        private void EndMove()
        {
            targetSpeed = MathHelper.Clamp(targetPoint.Body.Position.DistanceTo(Body.Position) * 5f, 400f * builder.SizeMult, 1500f * builder.SizeMult);
            moving = false;
        }

        protected override void FreeTouch(Touch touch)
        {
        }

        public void Restart()
        {
            if (moving)
            {
                base.FreeTouch(movingTouch);
            }
            Body.BodyType = BodyType.Kinematic;
            Body.LinearVelocity = Vector2.Zero;
            DestroyEyeJoint();
            snot.Enabled = false;
            targetPoint.Enabled = true;
            targetPoint.Used = false;
            targetPoint = initialPoint;
            targetPoint.Used = true;
            EndMove();
        }

        private const float Radius = 55f;

        private const float MaxSpeed = 1500f;

        private const float MinSpeed = 400f;

        private const float StickRadius = 50f;

        private bool moving;

        private Touch movingTouch;
        private bool restoreJoint;

        private float targetSpeed;

        private readonly SnotPoint initialPoint = targetPoint;

        private Vector2 targetPosition;
    }
}
