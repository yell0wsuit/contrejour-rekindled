using System;
using System.Collections.Generic;

using Mokus2D.ContreJourMono.ContreJour.Game.Eyes;
using Mokus2D.ContreJourMono.ContreJour.Game.Hero;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Util;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Mokus2D.Default.Namespace
{
    public class HeroBodyClip : ContreJourBodyClip, IVectorPositionProvider, IBonusAcceptable, ISnotLinked, IEatable, ISpikesDestroyable, ILaunchable, IRadius, IBodyClip, ITeleportable, IRestartable
    {
        public EventSender TeleportEvent => teleportEvent;

        public bool Removed
        {
            get => removed; set => removed = value;
        }

        public int SnotJoinedCount
        {
            get => snotJoinedCount;
            set
            {
                if (value != snotJoinedCount)
                {
                    snotJoinedCount = value;
                    if (snotJoinedCount >= 6)
                    {
                        XBoxUtil.AwardAchievement("spider");
                    }
                }
            }
        }

        public bool HitEnabled
        {
            get => hitEnabled; set => hitEnabled = value;
        }

        public bool SnotEnabled
        {
            get => snotEnabled; set => snotEnabled = value;
        }

        public HeroEye Eye => eye;

        public bool SpeedLocked
        {
            get => speedLocked; set => speedLocked = value;
        }

        public HeroBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            Body.IsBullet = true;
            if (Game.BlackSide || Game.WhiteSide || Game.BonusChapter)
            {
                string text = Game.ChooseSide("McHeroBlackView", "McHeroWhiteView", "McHeroBackView", "McHeroBackView", "McHeroView_6");
                bodyBackground = ClipFactory.CreateWithAnchor(text);
                _builder.ReplaceChildWith(_clip, bodyBackground);
            }
            else
            {
                bodyBackground = (Sprite)_clip;
            }
            Vector2 position = _clip.Position;
            bodyBackground.Position = Vector2.Zero;
            _clip = new Node();
            clip = _clip;
            _clip.Position = position;
            if (CocosUtil.isArmV7())
            {
                shadow = ClipFactory.CreateWithAnchor(Game.ChooseSide("McHeroShadow", "McHeroShadowWhite", "McHeroShadow", "McHeroShadow"));
                _clip.AddChild(shadow);
            }
            _builder.Add(_clip, 10);
            bodyBackground.Parent.RemoveChild(bodyBackground);
            _clip.AddChild(bodyBackground);
            worldSize = _builder.ToVec(CocosUtil.toIPhone(CocosUtil.sizeToPoint(Game.LevelSize)));
            initialPosition = Body.Position;
            Body.SleepingAllowed = false;
            FarseerUtil.SetGroupIndex(Body, -2);
            lastHitTime = 0f;
            snotEnabled = true;
            initializeBody();
            hitEnabled = true;
            if (Game.LevelIndex != 0 || !Game.CanShowIntro)
            {
                initializeBody();
                _ = Schedule(new Action(FirstRespawn), FirstRespawnTime);
                if (Game.EndLevel != null)
                {
                    _ = Schedule(new Action(Game.EndLevel.ShowPortal), 1.6f);
                }
                portal = new Portal(Game, _clip.Position);
                builder.AddChildBefore(portal, clip);
                portal.ItemsScale = 0f;
                portal.ScaleStep = 0.1f;
            }
            onGroundTime = 0f;
            airTime = 0f;
            maxAirTime = 0f;
            config["hasDust"] = true;
            string text2 = "McHotspotwhite";
            hotspot = ClipFactory.CreateWithAnchor(text2);
            _clip.AddChild(hotspot);
            eye = new HeroEye(Game);
            eyeScale = eye.Scale;
            _clip.AddChild(eye);
            if (Game.BlackSide || Game.BonusChapter)
            {
                blackTails = new List<BlackTail>();
                BlackTail blackTail = new(this);
                blackTails.Add(blackTail);
                builder.Add(blackTail, 3);
            }
            else
            {
                Color color = Game.WhiteSide ? ContreJourConstants.WHITE_TAIL_COLOR : ContreJourConstants.BLACK_COLOR;
                tail = new HeroTail(color);
                _clip.AddChild(tail, -1);
            }
            breatheScale = 0f;
            sleep = false;
            timeToSleep = 7f;
            eyeClosed = false;
            Game.RegisterHero(this);
            onGround = false;
            targetScale = new Vector2(1f, 1f);
            breatheScaleStep = 0f;
            teleportEvent = new EventSender();
            finishColor = 255f;
            Game.AddShadowSource(this);
        }

        protected virtual float FirstRespawnTime => 0.5f;

        public bool CanTeleport()
        {
            return CanDie() && !eating;
        }

        public bool CanDie()
        {
            return !restarting && snotEnabled;
        }

        public bool CanLaunch()
        {
            return true;
        }

        public float Radius()
        {
            return 0.8333333f;
        }

        public void Restart()
        {
            restarting = true;
            FinishEvent.SendEvent();
            restartOnEating = eating;
            teleportEvent.SendEvent();
            FadeOutTails();
            if (tail != null)
            {
                tail.Run(new Sequence(
                [
                    new FadeOut(0.1f),
                    new Hide()
                ]));
            }
            _ = CallAfter(new Action(HideBody), 0.1f);
        }

        private void HideBody()
        {
            clip.Run(new Sequence(
            [
                new FadeOut(0.2f),
                new InstantAction(new Action(Respawn))
            ]));
        }

        public void SetSpeedLocked(bool value)
        {
            speedLocked = value;
            eye.MoveAllowed = !value;
        }

        public void initializeBody()
        {
            Body.BodyType = BodyType.Static;
            clip.Visible = false;
            FarseerUtil.SetDensityValue(Body, 0.1f);
        }

        public bool OnGround()
        {
            for (ContactEdge contactEdge = Body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
            {
                if (contactEdge.Contact.IsTouching && contactEdge.Other.BodyType != BodyType.Dynamic && !contactEdge.Other.IsSensor())
                {
                    return true;
                }
            }
            return false;
        }

        public EventSender DestroyEvent => teleportEvent;

        public Vector2 BonusTarget()
        {
            return clip.Position;
        }

        public void SetEyeTargetAngle(float value)
        {
            eye.ViewAngle = value;
            eye.ViewDistance = 0.7f;
        }

        public virtual bool EyeAnimationsAllowed
        {
            get => eye.AnimationsAllowed; set => eye.AnimationsAllowed = value;
        }

        public bool EyeMoveAllowed
        {
            get => eye.MoveAllowed; set => eye.MoveAllowed = value;
        }

        public void SetPosition(Vector2 position)
        {
            clip.Position = position;
            Body.SetTransform(builder.ToIPhoneVec(position), Body.Rotation);
        }

        private void FirstRespawn()
        {
            if (!restarting)
            {
                Respawn();
            }
        }

        public void Respawn()
        {
            bodyBackground.OpacityFloat = 1f;
            bodyBackground.Color = ContreJourConstants.WHITE_COLOR_3;
            TeleportTail(Body.Position);
            builder.ChangeChildLayer(clip, 10);
            EyeAnimationsAllowed = true;
            eye.Visible = true;
            eye.SetDefaultView();
            if (tail != null)
            {
                tail.StopAllActions();
                tail.Scale = 1f;
                tail.Visible = true;
                tail.OpacityFloat = 1f;
            }
            FarseerUtil.SetSensor(Body, false);
            sleep = false;
            finishSet = false;
            disablePositionUpdate = false;
            finished = false;
            initializeBody();
            restarting = false;
            clip.Opacity = 255;
            Body.SetTransform(initialPosition, Body.Rotation);
            ForceClipPosition();
            portal.TargetScale = 1f;
            _ = Schedule(new Action(ShowClip), 0.7f);
            Mokus2DGame.SoundManager.PlaySound("begin5", 0.5f, 0f, 0f);
            NewBlackTail();
        }

        public new Vector2 PositionVec => Body.Position;

        private void ShowClip()
        {
            clip.StopAllActions();
            clip.OpacityFloat = 1f;
            clip.Visible = true;
            clip.Scale = 0f;
            clip.Run(new ScaleTo(0.2f, 1f));
            _ = CallAfter(new Action(StartPlay), 0.2f);
        }

        private void StartPlay()
        {
            Body.BodyType = BodyType.Dynamic;
            _ = CallAfter(new Action(RemovePortal), 0.2f);
        }

        private void RemovePortal()
        {
            portal.TargetScale = 0f;
        }

        public void ApplyBonus()
        {
            eye.ApplyBonus();
        }

        private float SpeedMultiplier()
        {
            return 0.2f;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (hotspot != null && !finishSet)
            {
                hotspot.Opacity = (int)(Game.LightPower * 150f);
                hotspot.RotationRadians = Maths.atan2Vec(Body.Position, Game.LightPoint);
            }
            if (blackTails != null)
            {
                int num = 0;
                ArrayList arrayList = new();
                foreach (BlackTail blackTail in blackTails)
                {
                    blackTail.UpdateNode(time);
                    if (num != 0 && blackTail.Length <= 1)
                    {
                        builder.RemoveChild(blackTail);
                        arrayList.Add(blackTail);
                    }
                    num++;
                }
                foreach (object obj in arrayList)
                {
                    _ = blackTails.Remove((BlackTail)obj);
                }
            }
            if (finishSet)
            {
                Vector2 vector = Maths.StepToVecTargetMaxStep(Body.Position, finishPosition, finishSpeed);
                Body.SetTransform(vector, Body.Rotation);
                ForceClipPosition();
                if (vector == finishPosition)
                {
                    finishSet = false;
                    FinishReached();
                }
            }
            onPlasticine = Body != null && FarseerUtil.IsTouchingType(Body, typeof(PlasticinePartBodyClip));
            onGround = Body != null && OnGround();
            if (onGround)
            {
                lastOnGroundTime = Game.TotalTime;
            }
            Body.AngularDamping = (Game.TotalTime - lastOnGroundTime > 0.3f) ? 0.5f : 0f;
            if (Body != null && Body.BodyType == BodyType.Static && !onGround && snotJoinedCount == 0)
            {
                airTime += time;
            }
            else
            {
                airTime = 0f;
            }
            if (onPlasticine)
            {
                onGroundTime += time;
            }
            else
            {
                onGroundTime = 0f;
            }
            TryConfuse();
            if (CocosUtil.isArmV7())
            {
                UpdateShadow(time);
            }
            float num2 = Maths.atan2Vec(Body.LinearVelocity);
            float num3 = speedLocked ? 0f : Body.LinearVelocity.Length();
            if (!speedyPosted && onGroundTime > 0.2f && num3 >= 11.666667f && hitEnabled)
            {
                XBoxUtil.AwardAchievement("speedy");
                speedyPosted = true;
            }
            if (!migthyPosted && !onGround && snotJoinedCount == 0 && num3 >= 33.333332f && hitEnabled)
            {
                XBoxUtil.AwardAchievement("mighty_bird");
                migthyPosted = true;
            }
            float num4 = (snotJoinedCount > 0) ? 33.333332f : 66.666664f;
            if (num3 > num4)
            {
                num3 = num4;
                Body.LinearVelocity = FarseerUtil.toVec(num4, num2);
            }
            eye.SetVelocity(Body.LinearVelocity);
            eye.UpdateNode(time);
            if (!finished && tail != null)
            {
                tail.LimitAngles = onGround;
                tail.SetMovementDirection(num2);
                tail.Speed = num3;
                tail.UpdateSpeed = sleep ? 0.2f : 1f;
            }
            previousSpeed = Body.LinearVelocity;
            velocity = num3;
            TryWakeUp();
            if (Body.BodyType == BodyType.Dynamic && Maths.Abs(clip.ScaleY - 1f) < 0.04f && Body.JointList == null)
            {
                TryBreathe(time);
                TrySleep();
                CheckOutOffLevel();
                CheckOutOfBorder();
            }
        }

        protected virtual void FinishReached()
        {
        }

        public void TryConfuse()
        {
            float num = FarseerUtil.b2Vec2Distance(Body.LinearVelocity, previousSpeed);
            bool flag = Game.TotalTime - lastOnGroundTime < 0.1f;
            if (num >= 4f && !finishSet && !speedLocked)
            {
                bool flag2 = false;
                if (Game.TotalTime - lastHitTime > 0.5f && hitEnabled && flag && Maths.Abs(Body.LinearVelocity.Y) < Maths.Abs(previousSpeed.Y) && Body.LinearVelocity.Length() < 5f)
                {
                    flag2 = true;
                    Mokus2DGame.SoundManager.PlaySound("landing1", Maths.min(num / 4f / 3f, 1f) / 10f, 0f, 0f);
                    lastHitTime = Game.TotalTime;
                }
                if (Maths.Abs(Body.LinearVelocity.X - previousSpeed.X) >= 4f && Body.LinearVelocity.X <= 1f)
                {
                    if (!flag2 && flag && hitEnabled && Game.TotalTime - lastHitTime > 0.5f)
                    {
                        Mokus2DGame.SoundManager.PlaySound("landing1", Maths.min(num / 4f / 3f, 1f) / 10f, 0f, 0f);
                        lastHitTime = Game.TotalTime;
                    }
                    eye.PlayAnimation(new EyeAnimation(null, "McEyeBallHit", true, true), false);
                    eye.AnimationEndEvent.AddListenerSelector(new Action(OnHitEnd));
                }
            }
        }

        public void PlayFootSound()
        {
            if (onGround)
            {
                bool flag = Game.TotalTime - lastOnGroundTime < 0.2f;
                if (!flag)
                {
                    lastFootPosition = Body.Position.X;
                }
                else if (flag && Maths.Abs(Body.Position.X - lastFootPosition) > 1f)
                {
                    lastFootPosition = Body.Position.X;
                    Mokus2DGame.SoundManager.PlayRandomSound(Sounds.FOOT_STEPS, 1f);
                }
                lastOnGroundTime = Game.TotalTime;
            }
        }

        protected virtual void UpdateShadow(float time)
        {
            ContactEdge contactEdge = Body.ContactList;
            bool flag = false;
            float num = 0f;
            int num2 = 0;
            while (contactEdge != null)
            {
                Vector2 worldPoint = FarseerUtil.GetWorldPoint(contactEdge.Contact);
                BodyClip bodyClip = (BodyClip)contactEdge.Other.UserData;
                if (bodyClip != null && contactEdge.Contact.IsTouching && !contactEdge.Contact.FixtureA.IsSensor && !contactEdge.Contact.FixtureB.IsSensor && worldPoint.Y - Body.Position.Y < -0.5833333f && (contactEdge.Other.BodyType == BodyType.Static || contactEdge.Other.BodyType == BodyType.Kinematic) && bodyClip.Config != null && !bodyClip.Config.ContainsKey("noShadow"))
                {
                    flag = true;
                    float num3 = (worldPoint.X - Body.Position.X) / builder.EngineConfig.SizeMultiplier;
                    num += num3;
                    num2++;
                }
                contactEdge = contactEdge.Next;
            }
            if (flag)
            {
                float num4 = num / num2;
                shadow.Position = Maths.StepToPointTargetMaxStep(shadow.Position, new Vector2(num4, 0f), Maths.min(1f, Maths.Abs(num4 / 10f)));
            }
            UpdateShadowOpacityTime(flag, time);
        }

        protected void UpdateShadowOpacityTime(bool hasShadow, float time)
        {
            float num = 20f * time * 30f;
            shadow.Opacity = (int)(Maths.StepToTargetMaxStep(shadow.Opacity, hasShadow ? bodyBackground.Opacity : 0, num) * Game.LightPower);
            shadow.Visible = shadow.Opacity > 0;
        }

        public void CheckOutOfBorder()
        {
            Vector2 position = Body.Position;
            if (position.X < 0.8333333f)
            {
                Body.SetTransform(new Vector2(0.8333333f, Body.Position.Y), Body.Rotation);
            }
            if (position.X > worldSize.X - 0.8333333f)
            {
                Body.SetTransform(new Vector2(worldSize.X - 0.8333333f, Body.Position.Y), Body.Rotation);
            }
            if (position.Y > worldSize.Y - 0.8333333f)
            {
                Body.SetTransform(new Vector2(Body.Position.X, worldSize.Y - 0.8333333f), Body.Rotation);
            }
        }

        public void CheckOutOffLevel()
        {
            if (Body.Position.Y < -3f)
            {
                Mokus2DGame.SoundManager.PlaySound("deathByFall2", 1f, 0f, 0f);
                FailLevelSpeedPause(Body.Position, 0f, 0f);
                UserData.Instance.OutOfScreen++;
            }
        }

        public void TryBreathe(float time)
        {
            if (velocity < 0.3f && !finishSet && Body.BodyType != BodyType.Static)
            {
                breatheScaleStep += 0.02f;
                breatheScaleStep = Maths.SimplifyAngleRadiansStartValue(breatheScaleStep, 0f);
                float num = Maths.Cos(breatheScaleStep);
                breatheScale = num * 0.04f;
                targetScale.X = 1f + breatheScale;
                targetScale.Y = 1f + (breatheScale / 2f);
                timeToSleep -= time;
                EyeAnimationsAllowed = false;
                if (sleep && eyeClosed)
                {
                    MovieClip movieClip = (MovieClip)eye.CurrentBackground;
                    int num2 = (int)(movieClip.TotalFrames * (1f + num) / 2f);
                    movieClip.GotoAndStop((uint)Maths.StepToTargetMaxStep(movieClip.CurrentFrame, num2, 1f));
                    sleepSoundTime += time;
                    if (num2 == 12 && breatheScaleStep > 3.1415927f && sleepSoundTime >= 1.5f)
                    {
                        sleepSoundTime = 0f;
                        hasToYawn = Maths.Rand() > 0.6f;
                        if (hasToYawn)
                        {
                            Mokus2DGame.SoundManager.PlaySound("breathIn4", 0.27f, 0f, 0f);
                        }
                    }
                    else if (num2 == 23 && breatheScaleStep < 3.1415927f && hasToYawn)
                    {
                        hasToYawn = false;
                        Mokus2DGame.SoundManager.PlayRandomSound(Sounds.SLEEPING, 0.5f);
                    }
                }
            }
            else
            {
                EyeAnimationsAllowed = true;
                targetScale.X = targetScale.Y = 1f;
                timeToSleep = 7f;
            }
            clip.ScaleX = Maths.StepToTargetMaxStep(clip.ScaleX, targetScale.X, 0.002f);
            clip.ScaleY = Maths.StepToTargetMaxStep(clip.ScaleY, targetScale.Y, 0.002f);
            eye.Scale = 1f / clip.ScaleX * eyeScale;
            float num3 = clip.Position.Y - (bodyBackground.Size.Y * (1f - clip.ScaleY) / 2f) - 2f;
            clip.Position = new Vector2(clip.Position.X, num3);
        }

        public void TryWakeUp()
        {
            if (sleep && velocity > 0.3f)
            {
                eyeClosed = false;
                sleep = false;
                eye.AnimationEndEvent.RemoveListenerSelector(new Action(OnEyeClose));
                RandomAnimationEye randomAnimationEye = eye;
                bool flag = true;
                randomAnimationEye.PlayAnimation(new EyeAnimation("McEyeOpen", null, flag, false), true);
            }
        }

        public void Teleport(BodyClip teleport)
        {
            TeleportTail(teleport.Body.Position);
            TeleportEvent.SendEvent();
        }

        public void TeleportTail(Vector2 position)
        {
            if (blackTails != null)
            {
                BlackTail blackTail = blackTails[0];
                blackTail.Body = null;
                blackTail.Target = position;
                builder.ChangeChildLayer(blackTail, -3);
            }
        }

        public void AfterTeleport()
        {
            if (!restarting)
            {
                NewBlackTail();
            }
        }

        public void NewBlackTail()
        {
            if (blackTails != null)
            {
                BlackTail blackTail = new(this);
                blackTails.Insert(0, blackTail);
                builder.Add(blackTail, 3);
            }
        }

        public void TrySleep()
        {
            if (!sleep && timeToSleep < 0f)
            {
                RandomAnimationEye randomAnimationEye = eye;
                bool flag = true;
                randomAnimationEye.PlayAnimation(new EyeAnimation("McEyeCloseSlow", null, flag, false), true);
                sleep = true;
                eye.AnimationEndEvent.AddListenerSelector(new Action(OnEyeClose));
            }
        }

        public void ForceClipPosition()
        {
            base.UpdatePosition();
        }

        public float DeadEyeScale()
        {
            return 1f;
        }

        public void SetScaleTime(float scale, float time)
        {
            clip.ScaleY = clip.ScaleX;
            clip.Run(new ScaleTo(time, scale));
        }

        protected void FinishLevelSpeedEyeAnimation(Vector2 targetPosition, float _finishSpeed, string eyeAnimation)
        {
            if (removed)
            {
                return;
            }
            finishSet = true;
            finishSpeed = _finishSpeed;
            finishPosition = targetPosition;
            Body.BodyType = BodyType.Static;
            Body.LinearVelocity = new Vector2(0f, 0f);
            FarseerUtil.SetSensor(Body, true);
            if (eyeAnimation != null)
            {
                eye.PlayAnimation(new EyeAnimation(eyeAnimation, null, false, false), true);
                eye.AnimationEndEvent.AddListenerSelector(new Action(OnEyeCloseFinish));
            }
            if (!restarting)
            {
                if (levelCompleted)
                {
                    Game.Finished = true;
                }
                _ = Schedule(new Action(OnFinish), 0.5f);
            }
            FinishEvent.SendEvent();
            teleportEvent.SendEvent();
            FadeOutTails();
        }

        protected virtual void FadeOutTails()
        {
            if (blackTails != null)
            {
                foreach (BlackTail blackTail in blackTails)
                {
                    blackTail.Run(new FadeOut(0.3f));
                }
            }
            if (tail != null)
            {
                tail.Run(new ScaleTo(0.3f, 0f));
            }
        }

        protected virtual void DoFinish()
        {
            if (levelCompleted)
            {
                Game.Finish(builder.ToPoint(finishPosition));
                return;
            }
            _ = Schedule(new Action(CallFail), finishPause);
        }

        protected virtual void FinishLevelSpeed(Vector2 targetPosition, float _finishSpeed)
        {
            FinishLevelSpeedEyeAnimation(targetPosition, _finishSpeed, "McEyeClose");
        }

        public void CompleteLevelSpeed(Vector2 targetPosition, float _finishSpeed)
        {
            levelCompleted = true;
            FinishLevelSpeed(targetPosition, _finishSpeed);
        }

        public void EatSpeedPauseScaleTime(Vector2 targetPosition, float _finishSpeed, float pause, float scale, float time)
        {
            eating = true;
            FailLevelSpeedPause(targetPosition, _finishSpeed, pause);
            SetScaleTime(scale, time);
        }

        public void FailLevelSpeedPause(Vector2 targetPosition, float _finishSpeed, float pause)
        {
            finishPause = pause;
            FinishLevelSpeed(targetPosition, _finishSpeed);
        }

        public void FailLevelSpeedPauseEyeAnimation(Vector2 targetPosition, float _finishSpeed, float pause, string eyeAnimation)
        {
            finishPause = pause;
            FinishLevelSpeedEyeAnimation(targetPosition, _finishSpeed, eyeAnimation);
        }

        public void Explode()
        {
            eating = true;
            UserData instance = UserData.Instance;
            instance.Acupuncture++;
            Mokus2DGame.SoundManager.PlaySound("deathBySpikes5", 0.7f, 0f, 0f);
            FailLevelSpeedPauseEyeAnimation(Body.Position, 1f, 1f, null);
            UpdatePosition();
            disablePositionUpdate = true;
            HeroExplosion heroExplosion = new();
            heroExplosion.ExplodeGame(this, Game);
        }

        public void DoExplode()
        {
            if (!restarting)
            {
                float num = 0.15f;
                bodyBackground.Run(new FadeOut(num));
                HideEye();
                hotspot.Run(new FadeOut(num));
                _ = Schedule(new Action(Hide), num);
                if (tail != null)
                {
                    tail.Visible = false;
                }
            }
        }

        private void Hide()
        {
            clip.Visible = false;
        }

        public void HideEye()
        {
        }

        private void OnFinish()
        {
            if (restarting)
            {
                return;
            }
            finished = true;
            if (removed)
            {
                return;
            }
            DoFinish();
        }

        private void CallFail()
        {
            eating = false;
            if (!restartOnEating)
            {
                Game.Fail(0f);
            }
            restartOnEating = false;
        }

        private void OnEyeCloseFinish()
        {
            eye.AnimationEndEvent.RemoveListenerSelector(new Action(OnEyeCloseFinish));
            eye.SetEyeContent(new EyeAnimation("McEyeClose", null, false, false));
            MovieClip movieClip = (MovieClip)eye.CurrentBackground;
            movieClip.GotoAndStop(movieClip.TotalFrames - 1U);
            EyeAnimationsAllowed = false;
            eye.Visible = false;
        }

        private void OnEyeClose()
        {
            if (sleep)
            {
                eyeClosed = true;
                RandomAnimationEye randomAnimationEye = eye;
                bool flag = true;
                randomAnimationEye.PlayAnimation(new EyeAnimation("McEyeSleep", null, flag, false), true);
            }
        }

        public void OnHitEnd()
        {
            eye.AnimationEndEvent.RemoveListenerSelector(new Action(OnHitEnd));
            eye.PlayAnimation(new EyeAnimation("McEyeBlink", null, false, false), false);
        }

        public override void UpdatePosition()
        {
            if (!disablePositionUpdate && Body.BodyType != BodyType.Static)
            {
                base.UpdatePosition();
            }
        }

        public override void UpdateRotation()
        {
            bodyBackground.RotationRadians = Body.Rotation;
        }

        private const int OUT_OF_SCREEN_COUNT = 50;

        private const float MAX_LINKED_SPEED = 33.333332f;

        private const float MAX_SPEED = 66.666664f;

        private const float MIGHTY_SPEED = 33.333332f;

        private const float SPEEDY_SPEED = 11.666667f;

        private const float FOOT_DISTANCE = 1f;

        private const float HIT_SOUND_PAUSE = 0.5f;

        private const float TARGET_FINISH_COLOR = 50f;

        private const float MAX_POSITION_STEP = 20f;

        private const float SHADOW_OPACITY_STEP = 20f;

        private const float SLEEP_TIMEOUT = 7f;

        private const float BREATHE_SCALE_SPEED = 0.02f;

        private const float MAX_BREATHE_SCALE = 0.04f;

        private const float BREATHE_VELOCITY = 0.3f;

        private const float HIT_SPEED = 4f;

        private const float HERO_RADIUS = 0.8333333f;

        private const float HERO_RADIUS_PIXELS = 25f;

        private const int MaxHitSpeed = 5;

        private const string DefaultShadow = "McHeroShadow";

        private static readonly Color BLACK_TAIL_COLOR = ColorUtil.CreateColor(52, 185, 242, 255);

        protected HeroEye eye;

        protected HeroTail tail;

        protected Sprite bodyBackground;

        protected Sprite hotspot;

        protected Sprite shadow;

        public readonly EventSender FinishEvent = new();

        protected EventSender teleportEvent;

        protected bool onGround;

        protected bool onPlasticine;

        protected Vector2 initialPosition;

        protected float onGroundTime;

        protected float airTime;

        protected float maxAirTime;

        protected float sleepSoundTime;

        protected float lastOnGroundTime;

        protected Vector2 previousSpeed;

        protected float velocity;

        protected Vector2 finishPosition;

        protected bool finishSet;

        protected bool restarting;

        protected bool eating;

        protected bool restartOnEating;

        protected bool finished;

        protected Vector2 targetScale;

        protected float breatheScaleStep;

        protected float finishSpeed;

        protected float breatheScale;

        protected float lastFootPosition;

        protected bool sleep;

        protected bool snotEnabled;

        protected bool hasToYawn;

        protected bool eyeClosed;

        protected bool levelCompleted;

        protected float timeToSleep;

        protected List<BlackTail> blackTails;

        protected Portal portal;

        protected int snotJoinedCount;

        protected float eyeAngle;

        protected float finishColor;

        protected bool removed;

        protected bool disablePositionUpdate;

        protected bool hitEnabled;

        protected float finishPause;

        protected Vector2 worldSize;

        protected float lastHitTime;

        protected bool migthyPosted;

        protected bool speedyPosted;

        protected bool speedLocked;

        private readonly float eyeScale;
    }
}
