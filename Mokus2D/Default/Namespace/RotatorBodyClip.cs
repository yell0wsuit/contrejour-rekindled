using System;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class RotatorBodyClip : FurBodyClip, IClickable, IRestartable, ISnotHolder
    {
        public RotatorBodyClip(ContreJourLevelBuilder _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            lastDirection = 1f;
            Body.BodyType = BodyType.Kinematic;
            circle = ClipFactory.CreateWithAnchor("McRotatorCircle");
            clip.AddChild(circle);
            touchPoint = ClipFactory.CreateWithAnchor("McRotatorPoint");
            clip.AddChild(touchPoint);
            touchPointSpeed = Maths.randRange(0.02f, 0.03f);
            touchPointNeededSpeed = touchPointSpeed;
            RunActions();
        }

        public void Restart()
        {
            targetAngle = Maths.round(targetAngle, 6.2831855f);
        }

        public bool Rotating => Body.AngularVelocity != 0f;

        public bool DisableHeroFocus => false;

        public int Priority(Vector2 touchPoint)
        {
            return -1;
        }

        private void StopActions()
        {
            circle.StopAllActions();
            circle.Run(new FadeTo(0.5f, 0.78431374f));
            touchPoint.StopAllActions();
            touchPoint.Run(new FadeTo(0.5f, 0.78431374f));
        }

        private void RunFadeIn_Out_(Node node, float _in, float _out)
        {
            FadeTo fadeTo = new(ACTION_TIME, _out);
            FadeTo fadeTo2 = new(ACTION_TIME, _in);
            RepeatForever repeatForever = new(new Sequence([fadeTo, fadeTo2]));
            node.Run(repeatForever);
        }

        public void RunActions()
        {
            RunFadeIn_Out_(circle, 0.78431374f, 0.3137255f);
            RunFadeIn_Out_(touchPoint, 0.5882353f, 0.11764706f);
        }

        public override string GrassTexture()
        {
            return "McRotatorGrassAll.png";
        }

        public override int GrassCount()
        {
            return GRASS_COUNT;
        }

        public override int GrassRadius()
        {
            return GRASS_RADIUS;
        }

        public override float Width()
        {
            return WIDTH;
        }

        public bool AcceptFreeTouches()
        {
            return false;
        }

        public bool UseForZoom()
        {
            return false;
        }

        public override void Update(float time)
        {
            if (touch != null)
            {
                touchPointAngle = Maths.SimplifyAngleRadiansStartValue(touchPointAngle, startTouchAngle + targetAngle - 3.1415927f);
                touchPointSpeed = Maths.stepTo(touchPointSpeed, 0.2f, 0.01f);
                float num = touchPointAngle;
                touchPointAngle = Maths.stepTo(touchPointAngle, targetAngle + startTouchAngle, touchPointSpeed);
                lastPointSpeed = Maths.Abs(touchPointAngle - num);
            }
            else
            {
                touchPointSpeed = Maths.stepTo(touchPointSpeed, touchPointNeededSpeed, 0.003f);
                touchPointAngle += touchPointSpeed * Math.Sign(lastDirection);
            }
            touchPoint.Position = Maths.toPoint(POINT_OFFSET, touchPointAngle - Body.Rotation);
            float num2 = targetAngle - Body.Rotation;
            if (Math.Abs(num2) > 0.0062831854f)
            {
                float num3 = (touch != null) ? 20 : 5;
                Body.AngularVelocity = num2 * num3;
            }
            else
            {
                Body.SetTransform(Body.Position, targetAngle);
                Body.AngularVelocity = 0f;
            }
            base.Update(time);
        }

        public void SetTouching(bool value)
        {
            if (touching != value)
            {
                touching = value;
            }
        }

        public bool TouchBegan(Touch _touch)
        {
            float num = FarseerUtil.b2Vec2Distance(Body.Position, builder.TouchRootVec(_touch));
            if (Maths.Between(num / clip.Scale, MIN_TOUCH_RADIUS, MAX_TOUCH_RADIUS))
            {
                touch = _touch;
                startTouchAngle = Maths.atan2Vec(Body.Position, builder.TouchRootVec(touch)) - Body.Rotation;
                SetTouching(true);
                StopActions();
                return true;
            }
            return false;
        }

        public bool TouchMove(Touch touch)
        {
            float num = targetAngle;
            RefreshAngle();
            lastDirection = targetAngle - num;
            return true;
        }

        public void TouchOut(Touch touch)
        {
        }

        public void RefreshAngle()
        {
            targetAngle = Maths.atan2Vec(Body.Position, builder.TouchRootVec(touch)) - startTouchAngle;
            targetAngle = Maths.SimplifyAngleRadiansStartValue(targetAngle, Body.Rotation - 3.1415927f);
        }

        public void TouchEnd(Touch _touch)
        {
            touchPointSpeed = lastPointSpeed;
            SetTouching(false);
            float num = Maths.round(targetAngle, ANGLE_REMAINDER);
            if (Maths.Abs(num - targetAngle) < ANGLE_REMAINDER / 5f)
            {
                targetAngle = num;
            }
            else if (lastDirection < 0f)
            {
                targetAngle = Maths.floor(targetAngle, ANGLE_REMAINDER);
            }
            else
            {
                targetAngle = Maths.ceil(targetAngle, ANGLE_REMAINDER);
            }
            RunActions();
            touch = null;
        }

        public Vector2 SnotPosition => Body.Position;

        protected float targetAngle;

        protected Touch touch;

        protected float startTouchAngle;

        protected Sprite circle;

        protected Sprite touchPoint;

        protected bool touching;

        protected float lastDirection;

        protected float touchPointAngle;

        protected float touchPointSpeed;

        protected float touchPointNeededSpeed;

        protected float lastPointSpeed;

        private static string[] GRASS_SPRITES = ["McRotatorGrass0", "McRotatorGrass1", "McRotatorGrass2"];

        private float ACTION_TIME = 2.5f;

        private int GRASS_COUNT = 26;

        private float MAX_TOUCH_RADIUS = 3f;

        private float MIN_TOUCH_RADIUS = 0.33333334f;

        private float ANGLE_REMAINDER = 0.3926991f;

        private float WIDTH = 120f;

        private float POINT_OFFSET = CocosUtil.iPadValue(48f);

        private int GRASS_RADIUS = (int)CocosUtil.iPadValue(60f);

        private float MAX_RADIUS = CocosUtil.iPadValue(50f) * 0.033333335f;
    }
}
