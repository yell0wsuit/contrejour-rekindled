using System;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class DragableBodyClip : ContreJourBodyClip, IClickable, IRestartable, ISnotHolder
    {
        public EventSender DragStartEvent
        {
            get
            {
                return dragStartEvent;
            }
        }

        public StrongSnotBodyClip Snot
        {
            get
            {
                return snot;
            }
            set
            {
                snot = value;
            }
        }

        public DragableBodyClip(ContreJourLevelBuilder _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            if (_builder.ContreJour.WhiteSide || _builder.ContreJour.RoseChapter)
            {
                _clip = _builder.ReplaceClipWith(_clip, ReplaceClipName(_builder));
            }
            clip = _clip;
            _clip.Parent.ChangeChildLayer(_clip, 2);
            dragStartEvent = new EventSender();
            float @float = _config.GetFloat("scaleX");
            initialPosition = Body.Position;
            targetPosition = initialPosition;
            upperLimit = 3.4f * @float;
            lowerLimit = -3.4f * @float;
            float num = -config.GetFloat("rotation");
            axis = FarseerUtil.ToVecAngle(1f, MathHelper.ToRadians(num));
            CreateBoundsClip(@float);
            SetAlpha(150f);
            Body.BodyType = BodyType.Kinematic;
        }

        public void Restart()
        {
            targetPosition = initialPosition;
            limitSpeed = true;
        }

        protected virtual string ReplaceClipName(ContreJourLevelBuilder _builder)
        {
            return _builder.ContreJour.ChooseSide(null, "McDragViewWhite", "McDragView_5", null);
        }

        protected virtual void CreateBoundsClip(float scale)
        {
            limitSystem = new ParticleSystem("McDragViewLimitWhite.png");
            middle = limitSystem.AddParticleWithFrame(1);
            float num = CocosUtil.iPadValue(upperLimit / 0.033333335f);
            Vector2 vector = CocosUtil.ccpInt(Maths.ToPointAngle(num, MathHelper.ToRadians(clip.Rotation)));
            Vector2 vector2 = CocosUtil.ccpInt(clip.Position);
            left = limitSystem.AddParticleWithFrame(0);
            left.Position = vector2 - vector;
            right = limitSystem.AddParticleWithFrame(0);
            right.Position = vector2 + vector;
            builder.AddChildBefore(limitSystem, clip);
            float rotation = clip.Rotation;
            left.Rotation = rotation;
            right.Rotation = rotation;
            right.ScaleX = -1f;
            middle.Position = vector2 + vector;
            middle.ScaleX = vector.Length() / limitSystem.Size.X * 2f;
            middle.Rotation = rotation;
        }

        protected virtual Vector2 TouchOffset()
        {
            return Vector2.Zero;
        }

        public void SetAlpha(float value)
        {
            if (Maths.FuzzyNotEquals(currentAlpha, value, 0.0001f))
            {
                currentAlpha = value;
                RefreshObjectsAlpha();
            }
        }

        protected virtual void RefreshObjectsAlpha()
        {
            limitSystem.Opacity = (int)currentAlpha;
        }

        public bool UseForZoom()
        {
            return false;
        }

        public bool AcceptFreeTouches()
        {
            return true;
        }

        public bool DisableHeroFocus
        {
            get
            {
                return false;
            }
        }

        public int Priority(Vector2 touchPoint)
        {
            return 1;
        }

        public bool ProcessTouch(Touch _touch)
        {
            return FarseerUtil.b2Vec2Distance(builder.TouchRootVec(_touch), Body.Position + TouchOffset()) < CocosUtil.iPad(1.6666666f, 2.3333333f);
        }

        public bool TouchBegan(Touch _touch)
        {
            if (touch == null && ProcessTouch(_touch))
            {
                limitSpeed = false;
                touch = _touch;
                dragStartEvent.SendEvent();
                ContreJourGame contreJourGame = (ContreJourGame)builder.Game;
                contreJourGame.IncreaseZoomOut();
                Schedule(new Action(contreJourGame.FocusOnHero), 0.05f);
                draging = true;
                initialMousePosition = builder.TouchRootVec(touch);
                initialDragOffset = Body.Position - initialPosition;
                return true;
            }
            return false;
        }

        public bool TouchMove(Touch _touch)
        {
            return true;
        }

        public void TouchOut(Touch touch)
        {
        }

        public void TouchEnd(Touch _touch)
        {
            draging = false;
            builder.Game.DecreaseZoomOut();
            touch = null;
        }

        public void UpdateTouchPosition(float time)
        {
            Vector2 vector = builder.TouchRootVec(touch) - initialMousePosition + initialDragOffset;
            targetPosition = GetDragPosition(vector);
        }

        private void MoveToTarget(float time)
        {
            if (Maths.FuzzyNotEquals(time, 0f, 0.0001f) && !FarseerUtil.FuzzyEquals(targetPosition, Body.Position, 0.033333335f))
            {
                Vector2 vector = targetPosition - Body.Position;
                if (limitSpeed)
                {
                    vector = FarseerUtil.clampLength(vector, 26.666666f * time);
                }
                Vector2 vector2 = vector;
                vector2 *= 0.3f / time;
                vector *= 0.7f;
                Body.LinearVelocity = vector2;
                Body.SetTransform(vector + Body.Position, Body.Rotation);
                if (snot != null && snot.Joined && Maths.Between(vector2.Y, -1.3333334f, 0f))
                {
                    HeroBodyClip heroBodyClip = (HeroBodyClip)snot.Linked;
                    if (Maths.Abs(heroBodyClip.Body.LinearVelocity.X) < 0.16666667f && heroBodyClip.Body.Position.Y < snot.Position.Y - snot.NormalDistance * 0.8f && heroBodyClip.Body.LinearVelocity.Y > vector2.Y)
                    {
                        heroBodyClip.Body.LinearVelocity = new Vector2(heroBodyClip.Body.LinearVelocity.X, vector2.Y * 2f);
                    }
                }
            }
        }

        protected virtual Vector2 GetDragPosition(Vector2 offset)
        {
            float num = FarseerUtil.GetProjectionTarget(offset, axis);
            num = Maths.Clamp(num, lowerLimit, upperLimit);
            Vector2 vector = axis;
            vector *= num;
            return vector + initialPosition;
        }

        public override void Update(float time)
        {
            Body.LinearVelocity = Vector2.Zero;
            if (touch != null)
            {
                UpdateTouchPosition(time);
            }
            MoveToTarget(time);
            if (Maths.FuzzyNotEquals(time, 0f, 0.0001f) && Body.BodyType == BodyType.Dynamic)
            {
                Body.BodyType = BodyType.Static;
                Body.SetTransform(initialPosition, Body.Rotation);
            }
            float num = (draging ? 255f : 150f);
            SetAlpha(Maths.stepTo(currentAlpha, num, 5f));
            base.Update(time);
        }

        public virtual Vector2 SnotPosition
        {
            get
            {
                Vector2 vector = Maths.RotateAngle(builder.ToVec(new Vector2(0f, -60f)), -rotationOffsetRadians);
                return Body.GetWorldPoint(vector);
            }
        }

        public const float RESTART_SPEED = 26.666666f;

        public const float FIX_HERO_SPEED_X = 0.16666667f;

        public const float FIX_HERO_SPEED = -1.3333334f;

        public const float ALPHA_STEP = 5f;

        public const float MAX_ALPHA = 255f;

        public const float MIN_ALPHA = 150f;

        public const float MOVE_PROPORTION = 0.7f;

        public const float SPEED_PROPORTION = 0.3f;

        public const float FUZZY_PRECISSION = 0.033333335f;

        public const float TOUCH_DISTANCE_IPHONE = 2.3333333f;

        public const float TOUCH_DISTANCE = 1.6666666f;

        public const float OFFSET = 3.4f;

        protected EventSender dragStartEvent;

        protected Vector2 initialPosition;

        protected float upperLimit;

        protected float lowerLimit;

        protected Vector2 axis;

        protected bool draging;

        protected Vector2 initialMousePosition;

        protected Vector2 initialDragOffset;

        protected ParticleSystem limitSystem;

        protected Particle left;

        protected Particle middle;

        protected Particle right;

        protected Touch touch;

        protected StrongSnotBodyClip snot;

        protected float currentAlpha;

        protected Vector2 targetPosition;

        protected bool limitSpeed;
    }
}
