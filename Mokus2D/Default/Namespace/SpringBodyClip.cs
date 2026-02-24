using System;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Input;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SpringBodyClip : ContreJourBodyClip, IClickable, IRestartable
    {
        public SpringBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            if (!Game.BlackSide)
            {
                _clip = _builder.ReplaceClipWith(_clip, GetClipName());
                _clip.Parent.ChangeChildLayer(_clip, 2);
                clip = _clip;
            }
            Body.IsBullet = true;
            config["noShadow"] = "true";
            config["noSound"] = "true";
            movie = (MovieClip)clip;
            movie.Stoped = true;
            movie.Repeat = false;
            movie.MinFrame = 7f;
            if (!Game.BonusChapter)
            {
                CreateShadow();
            }
            startScale = clip.ScaleX;
            suckPoint = builder.ToVec(SUCK_POINT * clip.ScaleX);
            suckDistance = 150f * clip.ScaleX * builder.SizeMult;
            bodyCenterVec = FarseerUtil.rotate(builder.ToVec(BODY_CENTER * clip.ScaleX), InitialBodyAngle);
            breatheChanger = new CosChanger(0.06f, 0.07f);
            breatheChanger.MinValue = 0.95f;
            breatheChanger.MaxValue = 1.04f;
            foreach (Fixture fixture in Body.FixtureList)
            {
                fixture.Friction = 1f;
                if (IsSticky(fixture))
                {
                    fixture.IsSensor = true;
                }
            }
            launchTime = -0.2f;
            smoke = new WhiteSmoke(Game.BlackSide ? "McWhiteSmokeBlack.png" : "McWhiteSmoke.png");
            if (Game.BonusChapter)
            {
                smoke.Color = ContreJourConstants.GreenLightColor;
            }
            smoke.ScaleDownOnDestroy = false;
            RefreshSmokeAngle();
            builder.Add(smoke, 11);
            CreateSmoke();
            timeToSmoke = Maths.RandRangeMinMax(1.5f, 2.3f);
            clip.AddedToStage += new Action(RefreshPoints);
            SetThinSmokeRange();
            data = UserData.Instance;
        }

        protected virtual Vector2 SmokePoint
        {
            get
            {
                return SMOKE_POINT;
            }
        }

        public Vector2 WorldSuckPoint
        {
            get
            {
                return FarseerUtil.rotate(suckPoint, BodyAngle) + Body.Position;
            }
        }

        public bool UseForZoom()
        {
            return false;
        }

        public bool DisableHeroFocus
        {
            get
            {
                return true;
            }
        }

        public virtual int Priority(Vector2 touchPoint)
        {
            return sticked == null || !IsTouchDistance(touchPoint) ? -10 : 2;
        }

        public bool AcceptFreeTouches()
        {
            return false;
        }

        public virtual bool TouchBegan(Touch touch)
        {
            if (!IsTouchDistance(builder.TouchRootVec(touch)))
            {
                return false;
            }
            if (sticked != null && sticked.Body.BodyType == BodyType.Dynamic)
            {
                return false;
            }
            LaunchTouching();
            if (sticked != null)
            {
                Launch();
            }
            else if (Maths.FuzzyEquals(movie.CurrentFrame, 7f, 0.0001f) && movie.Stoped)
            {
                Mokus2DGame.SoundManager.PlaySound("perdelkaOutEmpty1", 0.7f, 0f, 0f);
                Spit();
            }
            return true;
        }

        public virtual bool TouchMove(Touch touch)
        {
            return true;
        }

        public virtual void TouchOut(Touch touch)
        {
        }

        public virtual void TouchEnd(Touch touch)
        {
        }

        public virtual void Restart()
        {
            launchTime = -0.2f;
        }

        protected virtual string GetClipName()
        {
            ContreJourGame game = Game;
            string text = "McSpringViewWhite";
            string text2 = "McSpringView_6";
            return game.Choose("McSpringView_5", null, text, null, text2);
        }

        private EventSender GetDestroyEvent(Body teleportBody)
        {
            ISnotLinked snotLinked = (ISnotLinked)teleportBody.UserData;
            return snotLinked.DestroyEvent;
        }

        protected virtual void SetSticked(ILaunchable value)
        {
            if (sticked != null)
            {
                sticked.DestroyEvent.RemoveListenerSelector(new Action(OnTeleport));
            }
            sticked = value;
            if (sticked != null)
            {
                sticked.DestroyEvent.AddListenerSelector(new Action(OnTeleport));
            }
        }

        public void RefreshSmokeAngle()
        {
            smoke.Angle = new Range(clip.Rotation + 90f, 20f);
        }

        protected virtual void CreateShadow()
        {
            Node node = ClipFactory.CreateWithAnchor(Game.WhiteSide ? "McSpringShadowWhite" : "McSpringShadow");
            builder.AddChildBefore(node, clip);
            node.Position = clip.Position;
            node.Rotation = clip.Rotation;
            node.ScaleX = clip.ScaleX;
            node.ScaleY = clip.ScaleY;
        }

        protected virtual void RefreshPoints()
        {
            smoke.SmokePosition = clip.LocalToNode(CocosUtil.toIPad(SmokePoint), builder.Root, true);
        }

        private float OpacityStep()
        {
            return 200f;
        }

        public void SetThinSmokeRange()
        {
            SetSmokeRange(0f);
            smoke.MaxOpacity = 75f;
            smoke.OpacityStep = OpacityStep() / 6f;
            smoke.ScaleStep = 1.1666666f;
        }

        public void SetWhideSmokeRange()
        {
            SetSmokeRange(20f);
            smoke.MaxOpacity = 120f;
            smoke.OpacityStep = OpacityStep();
            smoke.ScaleStep = 7f;
        }

        public void SetSmokeRange(float range)
        {
            smoke.HorizontalPosition = new Range(smoke.SmokePosition.X, range);
            smoke.VerticalPosition = new Range(smoke.SmokePosition.Y, range);
        }

        public void CreateSmoke()
        {
            for (int i = 0; i < 10; i++)
            {
                Particle particle = smoke.AddParticle(clip.Position);
                particle.Visible = false;
            }
        }

        private void ShowSmoke()
        {
            smoke.ShowAllParticles();
        }

        public bool CanLaunch(object bodyClip)
        {
            return bodyClip is ILaunchable && ((ILaunchable)bodyClip).CanLaunch();
        }

        private bool IsTouchDistance(Vector2 touchPosition)
        {
            return GetTouchDistance(touchPosition) < 1.1666666f;
        }

        private float GetTouchDistance(Vector2 touchPosition)
        {
            return FarseerUtil.b2Vec2Distance(Body.GetWorldPoint(bodyCenterVec), touchPosition);
        }

        public void LaunchTouching()
        {
            for (ContactEdge contactEdge = Body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
            {
                if ((!contactEdge.Contact.FixtureA.IsSensor || !contactEdge.Contact.FixtureB.IsSensor) && (IsSticky(contactEdge.Contact.FixtureA) || IsSticky(contactEdge.Contact.FixtureB)) && contactEdge.Contact.IsTouching)
                {
                    object userData = contactEdge.Other.UserData;
                    if (userData != sticked && CanLaunch(userData))
                    {
                        ApplyImpulseTo((ILaunchable)userData);
                    }
                }
            }
        }

        public void Launch()
        {
            sticked.SetSpeedLocked(false);
            sticked.HitEnabled = true;
            Game.FocusOnHero();
            sticked.Body.BodyType = BodyType.Dynamic;
            ApplyImpulseTo(sticked);
            launchTime = Game.TotalTime;
            SetSticked(null);
            Mokus2DGame.SoundManager.PlaySound("perdelkaOut0", 0.7f, 0f, 0f);
            Spit();
            data.SpringShot++;
        }

        public void ApplyImpulseTo(ILaunchable bodyClip)
        {
            float num = 1f;
            if (bodyClip.Body.BodyType == BodyType.Dynamic)
            {
                float num2 = FarseerUtil.b2Vec2Distance(WorldSuckPoint, bodyClip.Body.Position);
                num2 -= bodyClip.Radius();
                if (num2 > 0f)
                {
                    num = Maths.max(0f, (6.6666665f - num2) / 6.6666665f);
                }
            }
            ApplyImpulseTo(25f * num, bodyClip.Body);
        }

        public void ApplyImpulseTo(float impulse, Body _body)
        {
            impulse = impulse * _body.Mass * startScale;
            _body.LinearVelocity = Vector2.Zero;
            Vector2 vector = FarseerUtil.ToVecAngle(impulse, MathHelper.ToRadians(clip.Rotation + 90f));
            _body.ApplyLinearImpulse(vector, _body.WorldCenter);
        }

        public void Spit()
        {
            SetWhideSmokeRange();
            ShowSmoke();
            movie.Rewind = true;
            movie.MinFrame = 0f;
            movie.MaxFrame = movie.TotalFrames;
            movie.Stoped = false;
        }

        private void EnableJoin()
        {
        }

        public float JointStep()
        {
            return 0.6666667f;
        }

        protected virtual void UpdateSticked()
        {
            if (sticked.Body.BodyType != BodyType.Dynamic)
            {
                return;
            }
            Vector2 worldSuckPoint = WorldSuckPoint;
            Vector2 vector = worldSuckPoint - sticked.Body.Position;
            float num = vector.Length();
            vector.Normalize();
            vector *= sticked.Body.Mass;
            vector *= Math.Min((suckDistance - num) * 100f, 200f);
            sticked.Body.ApplyForce(vector, sticked.Body.WorldCenter);
            Vector2 vector2 = FarseerUtil.rotate(new Vector2(1f, 0f), BodyAngle);
            float projectionTarget = FarseerUtil.GetProjectionTarget(sticked.Body.LinearVelocity, vector2);
            Vector2 vector3 = worldSuckPoint - sticked.Body.Position;
            float projectionTarget2 = FarseerUtil.GetProjectionTarget(vector3, vector2);
            if (projectionTarget * projectionTarget2 < 0f || (Maths.Abs(projectionTarget) < 10f && Maths.Abs(projectionTarget2) > 1f))
            {
                Vector2 vector4 = vector2;
                vector4 *= projectionTarget2;
                vector4 *= sticked.Body.Mass;
                vector4 *= 200f;
                sticked.Body.ApplyForce(vector4, sticked.Body.WorldCenter);
            }
            relativeStickedPosition = sticked.Body.Position - Body.Position;
            relativeStickedPosition = FarseerUtil.rotate(relativeStickedPosition, -BodyAngle);
            FixClosePosition(relativeStickedPosition);
            CheckFixed(projectionTarget2);
        }

        private void ApplyRelativePosition(Vector2 relativePosition)
        {
            Vector2 vector = FarseerUtil.rotate(relativePosition, BodyAngle);
            vector += Body.Position;
            sticked.Body.SetTransform(vector, sticked.Body.Rotation);
        }

        public void FixClosePosition(Vector2 relativePosition)
        {
            float num = relativePosition.Y - 2.2f;
            if (num > 0f && Maths.Abs(relativePosition.X) * 2f > num)
            {
                relativePosition.X = Maths.stepTo(relativePosition.X, Math.Sign(relativePosition.X) * num / 2f, 0.3f);
                ApplyRelativePosition(relativePosition);
                return;
            }
            if (num < 0f)
            {
                relativePosition.X = Maths.stepTo(relativePosition.X, 0f, 0.3f);
                ApplyRelativePosition(relativePosition);
            }
        }

        public void CheckFixed(float positionProjection)
        {
            if (Maths.Abs(positionProjection) < 0.1f && CheckStickedBodyContacts("base"))
            {
                sticked.Body.BodyType = BodyType.Kinematic;
                sticked.Body.LinearVelocity = Vector2.Zero;
                sticked.Body.AngularVelocity = 0f;
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (sticked != null)
            {
                if (sticked.Body.BodyType == BodyType.Kinematic || CheckBodyContactsKeyCount(sticked.Body, "sticky", 1))
                {
                    UpdateSticked();
                    UpdateSuckingClip();
                }
                else
                {
                    FixAnimation();
                    SetSticked(null);
                }
            }
            else if (movie.Stoped && Maths.FuzzyEquals(movie.CurrentFrame, 7f, 0.0001f))
            {
                breatheChanger.Update(time);
                clip.ScaleY = breatheChanger.Value * startScale;
                clip.ScaleX = (2f - breatheChanger.Value) * startScale;
                if (timeToSmoke < 0f)
                {
                    timeToSmoke = Maths.RandRangeMinMax(1.5f, 2.3f);
                    int num = 0;
                    while (smoke.Particles.Count < 10 && num < 3)
                    {
                        SetThinSmokeRange();
                        GravityParticle gravityParticle = (GravityParticle)smoke.AddOrGetInvisible();
                        gravityParticle.Speed *= 0.16666667f;
                        num++;
                    }
                }
            }
            if (Maths.FuzzyEquals(movie.CurrentFrame, 0f, 0.0001f))
            {
                movie.CurrentFrame = 7f;
                movie.Stoped = true;
            }
            timeToSmoke -= time;
        }

        private void ReverseSmoke()
        {
            smoke.ScaleStep = -10.5f;
            foreach (Particle particle in smoke.Particles)
            {
                GravityParticle gravityParticle = (GravityParticle)particle;
                Vector2 vector = gravityParticle.Speed * -6f;
                float num = vector.Length();
                if (num > CocosUtil.iPadValue(300f))
                {
                    vector *= CocosUtil.iPadValue(300f) / num;
                }
                gravityParticle.Speed = vector;
            }
        }

        public void UpdateSuckingClip()
        {
            float num = ((sticked.Body.BodyType == BodyType.Dynamic) ? Maths.min(1f, relativeStickedPosition.Y / suckDistance) : 0f);
            movie.CurrentFrame = 7f + Maths.max(5f * (1f - num) - 1f, 0f);
            if (Maths.FuzzyEquals(num, 0f, 0.0001f))
            {
                movie.Stoped = true;
            }
        }

        private void ProcessCollisionPoint(Body body2, Contact point)
        {
            if (sticked != null || Game.TotalTime - launchTime < 0.2f)
            {
                return;
            }
            object userData = body2.UserData;
            if (point.IsTouching && (!point.FixtureA.IsSensor || !point.FixtureB.IsSensor) && CanLaunch(userData) && (IsSticky(point.FixtureA) || IsSticky(point.FixtureB) || CheckBodyContactsKeyCount(body2, "base", 2)))
            {
                ReverseSmoke();
                ILaunchable launchable = (ILaunchable)userData;
                SetSticked(launchable);
                launchable.HitEnabled = false;
                Mokus2DGame.SoundManager.PlaySound("perdelkaOut0", 0.7f, 0f, 0f);
            }
        }

        public override void OnCollisionStartPoint(Body body2, Contact point)
        {
            ProcessCollisionPoint(body2, point);
        }

        public override void OnCollisionPoint(Body body2, Contact point)
        {
            ProcessCollisionPoint(body2, point);
        }

        private void OnTeleport()
        {
            if (sticked != null)
            {
                if (sticked.Body.BodyType == BodyType.Kinematic)
                {
                    sticked.Body.BodyType = BodyType.Dynamic;
                }
                Spit();
                SetSticked(null);
            }
        }

        public override void OnCollisionEndPoint(Body body2, Contact point)
        {
            if (sticked != null && body2 == sticked.Body && !FarseerUtil.IsTouchingBody(Body, sticked.Body))
            {
                FixAnimation();
                SetSticked(null);
            }
        }

        public void FixAnimation()
        {
            if (Maths.FuzzyNotEquals(movie.CurrentFrame, 7f, 0.0001f))
            {
                movie.MinFrame = 7f;
                movie.Rewind = true;
                movie.Stoped = false;
            }
        }

        public bool CheckBodyContactsKeyCount(Body target, string fixtureKey, int count)
        {
            ContactEdge contactEdge = target.ContactList;
            int num = 0;
            while (contactEdge != null)
            {
                if (contactEdge.Contact.IsTouching && (CheckFixtureKey(contactEdge.Contact.FixtureA, fixtureKey) || CheckFixtureKey(contactEdge.Contact.FixtureB, fixtureKey)))
                {
                    num++;
                }
                if (num == count)
                {
                    return true;
                }
                contactEdge = contactEdge.Next;
            }
            return false;
        }

        public bool CheckStickedBodyContacts(string fixtureKey)
        {
            return CheckBodyContactsKeyCount(sticked.Body, fixtureKey, 2);
        }

        public bool CheckFixtureKey(Fixture fixture, string key)
        {
            Hashtable hashtable = fixture.UserData as Hashtable;
            return hashtable != null && hashtable.ContainsKey(key);
        }

        public bool IsSticky(Fixture fixture)
        {
            return CheckFixtureKey(fixture, "sticky");
        }

        private const float SLOW_SMOKE_MULT = 6f;

        private const float MAX_JOINED_DISTANCE = 1.3333334f;

        private const float TO_CENTER_FORCE = 7f;

        private const float SCALE_STEP = 7f;

        private const float OPACITY_STEP = 200f;

        private const float SMOKE_RANGE = 20f;

        private const float MAX_TIME_TO_SMOKE = 2.3f;

        private const float MIN_TIME_TO_SMOKE = 1.5f;

        private const float LAUNCH_PAUSE = 0.2f;

        private const int SPIT_FRAMES = 5;

        private const int BASE_FRAME = 7;

        private const int SMOKE_PARTS = 10;

        private const float BUBBLE_DISTANCE = 2.6666667f;

        private const float JOINT_STEP = 0.6666667f;

        private const float MAX_DISTANCE = 1.1666666f;

        private const float IMPULSE = 25f;

        private const float IMPULSE_DISTANCE = 6.6666665f;

        private const float SUCK_DISTANCE = 150f;

        private const int SUCK_FORCE = 100;

        private static readonly Vector2 BODY_CENTER = new(0f, 20f);

        private static readonly Vector2 SMOKE_POINT = new(0f, 60f);

        private static readonly Vector2 SUCK_POINT = new(0f, 40f);

        private static readonly Vector2 STICKY_POINT = new(0f, 40f);

        protected Vector2 bodyCenterVec;

        protected CosChanger breatheChanger;

        protected UserData data;

        protected float launchTime;

        protected MovieClip movie;

        protected Node redDot;

        protected Vector2 relativeStickedPosition;

        protected CGSize size;

        protected WhiteSmoke smoke;

        protected float startScale;

        protected ILaunchable sticked;

        protected float suckDistance;

        protected Vector2 suckPoint;

        protected float timeToSmoke;
    }
}
