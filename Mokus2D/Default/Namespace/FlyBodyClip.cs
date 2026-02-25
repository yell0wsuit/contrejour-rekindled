using System;

using ContreJourMono.ContreJour.Game.Eyes;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class FlyBodyClip : ContreJourBodyClip, IClickable
    {
        public FlyBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            bodySprite = ClipFactory.CreateWithAnchor("McFlyBody");
            _clip = bodySprite;
            clip = _clip;
            _ = _builder.AddChild(_clip);
            eye = new FlyEye(Game, true, Body.Position);
            eye.Scale = 0.65f;
            clip.AddChild(eye);
            scaredTime = 0f;
            _ = Schedule(new Action(StartFly), Maths.randRange(7f, 11f));
            initialPosition = Body.Position;
            leftWings = new FlyWings();
            rightWings = new FlyWings();
            leftWings.Position = new Vector2(-WINGS_POSITION.X, WINGS_POSITION.Y);
            rightWings.Position = WINGS_POSITION;
            rightWings.ScaleX = -1f;
            clip.AddChild(leftWings);
            clip.AddChild(rightWings);
            Body.GravityScale = 0f;
        }

        public bool DisableHeroFocus => true;

        public int Priority(Vector2 touchPoint)
        {
            return 0;
        }

        public bool AcceptFreeTouches()
        {
            return false;
        }

        public bool UseForZoom()
        {
            return true;
        }

        public bool TouchBegan(Touch touch)
        {
            Mokus2DGame.SoundManager.PlaySound("fly", Maths.randRange(0.15f, 0.35f), 0f, 0f);
            eye.PlayAnimation(new EyeAnimation("McEyeBlinkMonster", null, false, false), false);
            eye.RandomPositionProvider = Game.GetTouchProvider(touch);
            Scare(builder.TouchRootVec(touch));
            return false;
        }

        public void Scare(Vector2 scarePoint)
        {
            scaredTime = Game.TotalTime + 2f;
            if (!freeFlight)
            {
                StartFly();
                return;
            }
            float num = Maths.atan2Vec(scarePoint, Body.Position);
            FlyOutAngle(1f, num);
        }

        public bool TouchMove(Touch touch)
        {
            return true;
        }

        public void TouchOut(Touch touch)
        {
        }

        public void TouchEnd(Touch touch)
        {
        }

        public void SetFlying(bool value)
        {
            float num = value ? 0f : Maths.ToRadians(30f);
            leftWings.Run(new RotateTo(0.5f, num));
            rightWings.Run(new RotateTo(0.5f, -num));
            leftWings.SetFlying(value);
            rightWings.SetFlying(value);
        }

        public void StartFly()
        {
            bool flag = freeFlight;
            freeFlight = true;
            SetFlying(true);
            stoped = false;
            if (!flag)
            {
                Mokus2DGame.SoundManager.PlaySound("fly", 0.3f, 0f, 0f);
                Body.LinearDamping = 0.7f;
                backTime = Game.TotalTime + Maths.randRange(30f, 45f);
                _ = Schedule(new Action(ToBackground), 0.3f);
            }
            Fly();
        }

        private void ToBackground()
        {
            clip.Parent.ChangeChildLayer(clip, -3);
        }

        private void ToForeground()
        {
            clip.Parent.ChangeChildLayer(clip, 0);
        }

        public void EndFly()
        {
            float num = Maths.atan2Vec(Body.Position, initialPosition);
            float num2 = 6f * FarseerUtil.b2Vec2Distance(Body.Position, initialPosition) / 6.6666665f;
            Body.LinearVelocity = Vector2.Zero;
            DoFlyImpulse(num, num2);
            freeFlight = false;
            clip.Run(new ScaleTo(1.2f, 1f));
            _ = Schedule(new Action(StartFly), Maths.randRange(10f, 20f));
            _ = Schedule(new Action(ToForeground), 0.3f);
        }

        public void ScheduleFly()
        {
            _ = Schedule(new Action(Fly), Maths.randRange(7f, 11f));
        }

        public void Fly()
        {
            if (!freeFlight)
            {
                return;
            }
            if (Game.TotalTime < backTime || Body.Position.Y < initialPosition.Y)
            {
                FlyOut();
                ScheduleFly();
                return;
            }
            EndFly();
        }

        public void FlyOut()
        {
            FlyOut(1f);
        }

        public void FlyOut(float impulseMult)
        {
            float num;
            if (FarseerUtil.b2Vec2Distance(Body.Position, initialPosition) > 6.6666665f)
            {
                num = Maths.atan2Vec(Body.Position, initialPosition) + Maths.randRange(-0.5235988f, 0.5235988f);
            }
            else
            {
                num = Maths.randRange(0.2617994f, 2.8797934f);
            }
            FlyOutAngle(impulseMult, num);
        }

        public void FlyOutAngle(float impulseMult, float angle)
        {
            float num = Maths.randRange(0.6f, 0.95f);
            clip.Run(new ScaleTo(1.2f, num));
            DoFlyImpulse(angle, Maths.randRange(3f, 6f) * impulseMult);
        }

        public void DoFlyImpulse(float angle, float impulse)
        {
            float num = impulse * Body.Mass;
            Vector2 vector = FarseerUtil.toVec(num, angle);
            Body.ApplyLinearImpulse(vector, Body.WorldCenter);
        }

        public override void Update(float time)
        {
            base.Update(time);
            eye.UpdateNode(time);
            if (!freeFlight && !stoped)
            {
                TryStop();
            }
            eye.ProviderEnabled = true;
            if (freeFlight && Game.TotalTime >= scaredTime)
            {
                float num = Body.LinearVelocity.Length();
                if (num > 1f)
                {
                    eye.ProviderEnabled = false;
                    eye.ViewAngle = Maths.atan2Vec(Body.LinearVelocity);
                    eye.ViewDistance = num / 2f;
                }
            }
            if (freeFlight)
            {
                CheckOutOfBorder();
            }
            if (FarseerUtil.b2Vec2Distance(Body.Position, Game.HeroPositionVec) < 1.6666666f && heroScaredTime < Game.TotalTime)
            {
                heroScaredTime = Game.TotalTime + 2f;
                Scare(Game.HeroPositionVec);
            }
        }

        public void CheckOutOfBorder()
        {
            if (clip.Position.X < 50f)
            {
                DoFlyImpulse(Maths.randRange(0f, 0.7853982f), Maths.randRange(3f, 6f));
                return;
            }
            if (clip.Position.X > Game.LevelSize.X - 50f)
            {
                DoFlyImpulse(Maths.randRange(2.3561945f, 3.1415927f), Maths.randRange(3f, 6f));
                return;
            }
            if (clip.Position.Y > Game.LevelSize.Y - 50f)
            {
                DoFlyImpulse(Maths.randRange(-2.3561945f, -0.7853982f), Maths.randRange(3f, 6f));
                return;
            }
            if (clip.Position.Y < 50f)
            {
                DoFlyImpulse(Maths.randRange(0.7853982f, 2.3561945f), Maths.randRange(3f, 6f));
            }
        }

        public override void UpdateRotation()
        {
            float num = -5f * Body.LinearVelocity.X;
            clip.Rotation = Maths.stepTo(clip.Rotation, num, 1f);
            bodySprite.Rotation = clip.Rotation;
        }

        public void TryStop()
        {
            if (!stoped && FarseerUtil.b2Vec2Distance(initialPosition, Body.Position) <= 0.1f)
            {
                Body.LinearDamping = 10f;
                SetFlying(false);
                stoped = true;
            }
        }

        private const float HERO_DISTANCE = 1.6666666f;

        private const float BORDER = 50f;

        private const float SCARED_TIME = 2f;

        private const float ANGLE_MULT = 5f;

        private const float STOP_DISTANCE = 0.1f;

        private const float STOP_DAMPING = 10f;

        private const float FREE_FLIGHT_DAMPING = 0.7f;

        private const float LOOK_SPEED = 1f;

        private const float SCALE_TIME = 1.2f;

        private const float MAX_SCALE = 0.95f;

        private const float MIN_SCALE = 0.6f;

        private const float MAX_STOP_SPEED = 3f;

        private const float MAX_DISTANCE = 6.6666665f;

        private const float FLY_HOME_ANGLE = 0.5235988f;

        private const float FLY_UP_ANGLE = 0.2617994f;

        private const float MAX_IMPULSE = 6f;

        private const float MIN_IMPULSE = 3f;

        private const float MAX_AIR_TIME = 45f;

        private const float MIN_AIR_TIME = 30f;

        private const float MAX_START_FLY_TIME = 20f;

        private const float MIN_START_FLY_TIME = 10f;

        private const float MAX_FLY_TIME = 11f;

        private const float MIN_FLY_TIME = 7f;

        protected FlyEye eye;

        protected Sprite bodySprite;

        protected Vector2 initialPosition;

        protected bool freeFlight;

        protected bool stoped;

        protected FlyWings leftWings;

        protected FlyWings rightWings;

        protected float backTime;

        protected float scaredTime;

        protected float heroScaredTime;

        private static readonly Vector2 WINGS_POSITION = CocosUtil.ccpIPad(8f, 2f);
    }
}
