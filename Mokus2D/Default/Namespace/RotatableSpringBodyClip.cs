using System;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Extensions;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class RotatableSpringBodyClip : RotatableSpringBase, IRestartable
    {
        public RotatableSpringBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            game = (ContreJourGame)builder.Game;
            Body.BodyType = BodyType.Kinematic;
            bodyCenterVec = Vector2.Zero;
            touchPoint = new Sprite("McRotatorPoint");
            circle = ClipFactory.CreateWithAnchor("McRotatorCircle");
            circle.Scale = 1.6f;
            if (Game.BonusChapter)
            {
                circle.Color = ContreJourConstants.GreenLightColor;
                touchPoint.Color = ContreJourConstants.GreenLightColor;
                minOpacity = 0.9f;
            }
            builder.AddChildAfter(circle, clip);
            circle.AddChild(touchPoint);
            touchPoint.IgnoreParentOpacity = true;
            circle.Position = clip.Position;
            touchPoint.Position = new Vector2(50f, 0f);
            touchPointSpeed = Maths.randRange(0.02f, 0.03f) / 1.5f;
            touchPointNeededSpeed = touchPointSpeed;
            startSpringWidth = _config.GetFloat("Width");
            trajectory = new Trajectory(game)
            {
                Impulse = startSpringWidth / 32f
            };
            builder.AddChildAfter(trajectory, circle);
            foreach (Fixture fixture in Body.FixtureList)
            {
                if (fixture.UserData is Hashtable && ((Hashtable)fixture.UserData).GetString("id", null) == "touch")
                {
                    fixture.IsSensor = true;
                    break;
                }
            }
            StopActions();
        }

        protected override string GetClipName()
        {
            return !Game.BonusChapter ? "McRotatableSpring" : "McRotatableSpring_6";
        }

        public override void Restart()
        {
            base.Restart();
            targetRotation = Maths.SimplifyAngleRadiansStartValue(0f, (float)((double)Body.Rotation - 3.141592653589793));
        }

        public override int Priority(Vector2 touchPoint)
        {
            return touchPoint.DistanceTo(Body.Position) < TOUCH_DISTANCE ? base.Priority(touchPoint) : IsRotatorTouched(touchPoint) ? 0 : -100;
        }

        public override float TouchDistance(Vector2 touchPosition)
        {
            float num = touchPosition.DistanceTo(Body.Position);
            return Math.Min(num, Math.Abs(num - TOUCH_RADIUS));
        }

        protected override Vector2 SmokePoint => new Vector2(0f, 40f);

        protected override bool IsMoving => rotateTouch != null;

        private void UpdateTouchPoint()
        {
            if (rotateTouch != null)
            {
                float num = Maths.SimplifyAngleRadiansStartValue(touchAngle, touchPointAngle - 3.1415927f);
                touchPointSpeed = Maths.stepTo(touchPointSpeed, Math.Abs(num - touchPointAngle) / 10f, 0.01f);
                touchPointAngle = Maths.stepTo(touchPointAngle, num, touchPointSpeed);
            }
            else
            {
                touchPointSpeed = Maths.stepTo(touchPointSpeed, touchPointNeededSpeed, 0.002f);
                touchPointAngle += touchPointSpeed * Math.Sign(lastDirection);
            }
            touchPoint.Position = Maths.toPoint(50f, touchPointAngle);
            touchPoint.RotationRadians = touchPointAngle;
        }

        public override void Update(float time)
        {
            if (rotateTouch != null)
            {
                float num = touchAngle;
                touchAngle = GetTouchAngle();
                if (num != touchAngle)
                {
                    lastDirection = Math.Sign(touchAngle - num);
                }
                targetRotation = startAngle + touchAngle - startTouchAngle;
                targetRotation = Maths.round(targetRotation, 0.3926991f);
                targetRotation = Maths.SimplifyAngleRadiansStartValue(targetRotation, (float)((double)Body.Rotation - 3.141592653589793));
            }
            if (Maths.FuzzyNotEquals(Body.Rotation, targetRotation, 0.0001f))
            {
                float num2 = ((double)Math.Abs(targetRotation - Body.Rotation) > 0.7853981633974483) ? 5 : 10;
                Body.AngularVelocity = (targetRotation - Body.Rotation) / time / num2;
            }
            else
            {
                Body.AngularVelocity = 0f;
                Body.Rotation = targetRotation;
                RefreshSmokeAngle();
            }
            trajectory.Angle = Body.Rotation + 1.5707964f;
            trajectory.Position = Position + FarseerUtil.toVec(80f, trajectory.Angle);
            UpdateTouchPoint();
            base.Update(time);
        }

        public override bool TouchBegan(Touch touch)
        {
            if (!base.TouchBegan(touch) && IsRotatorTouched(builder.TouchRootVec(touch)))
            {
                RunActions();
                rotateTouch = touch;
                trajectory.Enabled = true;
                startAngle = Body.Rotation;
                startTouchAngle = GetTouchAngle();
                return true;
            }
            return true;
        }

        private bool IsRotatorTouched(Vector2 touchPosition)
        {
            float num = Body.Position.DistanceTo(touchPosition);
            return rotateTouch == null && Math.Abs(num - TOUCH_RADIUS) < TOUCH_DISTANCE;
        }

        private float GetTouchAngle()
        {
            Vector2 vector = builder.TouchRootVec(rotateTouch);
            return (vector - Body.Position).Atan2();
        }

        public override void TouchEnd(Touch touch)
        {
            if (touch == rotateTouch)
            {
                trajectory.Enabled = false;
                rotateTouch = null;
                StopActions();
                return;
            }
            base.TouchEnd(touch);
        }

        private void StopActions()
        {
            circle.StopAllActions();
            circle.Run(new FadeTo(0.5f, minOpacity));
            touchPoint.StopAllActions();
            touchPoint.Run(new FadeTo(0.3f, minOpacity));
        }

        private void RunFadeInOut(Node node, float _in, float _out)
        {
            node.StopAllActions();
            FadeTo fadeTo = new(2.5f, _out);
            FadeTo fadeTo2 = new(2.5f, _in);
            RepeatForever repeatForever = new(new Sequence([fadeTo, fadeTo2]));
            node.Run(repeatForever);
        }

        private void RunActions()
        {
            RunFadeInOut(circle, 1f, 0.7f);
            touchPoint.StopAllActions();
            touchPoint.Run(new FadeIn(0.3f));
        }

        private const float ACTION_TIME = 2.5f;

        private const float ANGLE_REMAINDER = 0.3926991f;

        private const float TOUCH_RADIUS_PIXELS = 80f;

        private const int CIRCLE_RADIUS = 50;

        private const float DEFAULT_WIDTH = 32f;

        private static readonly float TOUCH_RADIUS = 80f * Box2DConfig.DefaultConfig.SizeMultiplier;

        private static readonly float TOUCH_DISTANCE = 40f * Box2DConfig.DefaultConfig.SizeMultiplier;

        private Touch rotateTouch;

        private float touchPointAngle;

        private float touchPointSpeed;

        private float touchPointNeededSpeed;

        private float lastDirection = 1f;

        private float startAngle;

        private float startTouchAngle;

        protected Sprite circle;

        protected Sprite touchPoint;

        private float targetRotation;

        private float touchAngle;

        private float minOpacity = 0.5f;

        private float startSpringWidth;

        private Trajectory trajectory;

        private ContreJourGame game;
    }
}
