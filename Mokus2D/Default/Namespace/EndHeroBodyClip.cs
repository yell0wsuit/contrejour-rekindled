using System;
using System.Collections.Generic;

using ContreJourMono.ContreJour.Game.Eyes;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class EndHeroBodyClip : HeroBodyClip
    {
        public EndHeroBodyClip(LevelBuilderBase _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            Game.Energy.Blend = BlendState.Additive;
            data = UserData.Instance;
            EyeAnimationsAllowed = false;
            animationsAllowed = true;
            Game.IncreaseScreenDragDisable();
            Game.BackEvent.AddListenerSelector(new Action(OnBack));
        }

        protected override float FirstRespawnTime
        {
            get
            {
                return 3f;
            }
        }

        private new bool EyeAnimationsAllowed
        {
            set
            {
                base.EyeAnimationsAllowed = value && animationsAllowed;
            }
        }

        public void AddStripesView()
        {
            stripesView = new MovieStripesView(false, false);
            Game.AddView(stripesView);
            stripesView.Show();
        }

        private void OnBack()
        {
            if (outro != null)
            {
                outro.Dispose();
            }
        }

        protected override void DoFinish()
        {
            if (!levelCompleted)
            {
                base.DoFinish();
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (finished)
            {
                tail.Update(time);
            }
            if (hasToStop && !stoped)
            {
                Vector2 linearVelocity = Body.LinearVelocity;
                linearVelocity.X = Math.Min(linearVelocity.X, -0.05f);
                Body.LinearVelocity = linearVelocity;
            }
            if (hasToStop && !stoped && Math.Abs(shakePosition - clip.Position.X - STOP_OFFSET) < 10f && (double)Math.Abs(Body.LinearVelocity.X) < 0.1)
            {
                Body.BodyType = BodyType.Static;
                Body.LinearVelocity = Vector2.Zero;
                Body.AngularVelocity = 0f;
                stoped = true;
                sleep = true;
                EyeMoveAllowed = false;
                Mokus2DGame.SoundManager.PlaySound("petitkoIsHoping", 0.3f, 0f, 0f);
                Schedule(new Action(LookAtRose), 0.2f);
                if (data.RoseSaved)
                {
                    Schedule(new Action(SmileAfterLook), 2f);
                    return;
                }
                Schedule(new Action(LookAtTear), 1.5f);
            }
        }

        private void LookAtTear()
        {
            EndRoseBodyClip endRoseBodyClip = (EndRoseBodyClip)builder.GetObject("rose");
            endRoseBodyClip.DropTear();
            Schedule(new Action(LookAfterTear), 1f);
        }

        private void LookAfterTear()
        {
            SetEyeTargetAngle(3.7699113f);
            Schedule(new Action(LookAtRose2), 1.5f);
        }

        private void LookAtRose2()
        {
            LookAtRose();
            Schedule(new Action(BecomeSad), 2f);
        }

        private void BecomeSad()
        {
            SetEyeTargetAngle(4.712389f);
            Mokus2DGame.SoundManager.PlaySound("saddness", 0.8f, 0f, 0f);
            eye.PlayAnimation(new EyeAnimation("McEyeCloseSlow", null, false, false), true);
            eye.ReturnToDefault = false;
            ((MovieClip)eye.CurrentBackground).MaxFrame = 18f;
            ((MovieClip)eye.CurrentBackground).Repeat = false;
            NodeAction nodeAction = new EaseInOut(new RotateBy(2f, -70.ToRadians()), 2f);
            tail.Run(nodeAction);
            Schedule(new Action(ShowOutro), 1f);
        }

        public void LookAtRose()
        {
            SetEyeTargetAngle(2.3561945f);
        }

        private void SmileAfterLook()
        {
            eye.ViewDistance = 0f;
            eye.Smile();
            Schedule(new Action(ShowOutro), 1f);
        }

        private void ShowOutro()
        {
            outro = new Outro(Game, data.RoseSaved);
            Game.AddChild(outro, 15);
        }

        protected override void FinishLevelSpeed(Vector2 targetPosition, float _finishSpeed)
        {
            if (levelCompleted)
            {
                Game.RestartEnabled = false;
                eye.UnscheduleAnimation();
                FinishLevelSpeedEyeAnimation(targetPosition, _finishSpeed, null);
                return;
            }
            base.FinishLevelSpeed(targetPosition, _finishSpeed);
        }

        protected override void FinishReached()
        {
            if (!levelCompleted)
            {
                return;
            }
            NodeAction nodeAction = Actions.ShakeWithDurationPositionOffsetCountScaleDiff(8f, clip.Position, 4f, 50, 0.1f);
            clip.Run(nodeAction);
            Schedule(new Action(AfterShake), 8f);
            CreateLights();
            Game.ZoomToScaleTime(clip.Position, CocosUtil.iPad(1.4f, 1.6f), 10f);
            Game.TouchEnabled = false;
            animationsAllowed = false;
            EyeAnimationsAllowed = false;
            AddStripesView();
            Game.RenewGround();
            Schedule(new Action(PlayShakeSound), 7f);
        }

        private void PlayShakeSound()
        {
            Mokus2DGame.SoundManager.PlaySound("petitkoIsTrying", 0.3f, 0f, 0f);
        }

        public void CreateLights()
        {
            float num = data.TotalStars * 0.33333334f;
            int num2 = 1;
            while (num2 < num)
            {
                float num3 = (float)Math.Sin((double)(num2 / num * 1.5707964f));
                float num4 = 8f * num3;
                Schedule(new Action(CreateLight), num4);
                num2++;
            }
            energySpeed = 200f;
        }

        private void CreateLight()
        {
            EnergyPart energyPart = new(Game, this, Maths.randRange(0f, 6.2831855f), clip.Position);
            energyPart.Collect();
            energyPart.SpeedValue = CocosUtil.iPadValue(Math.Min(energySpeed, 600f));
            energySpeed += CocosUtil.iPadValue(10f);
            energy.Add(energyPart);
            eye.ApplyBonus();
        }

        protected override void UpdateShadow(float time)
        {
            if (!stoped)
            {
                base.UpdateShadow(time);
                return;
            }
            UpdateShadowOpacityTime(true, time);
        }

        private void AfterShake()
        {
            clip.StopAllActions();
            finished = false;
            Body.BodyType = BodyType.Dynamic;
            FarseerUtil.SetSensor(Body, false);
            hasToStop = true;
            animationsAllowed = false;
            shakePosition = clip.Position.X;
            EyeAnimationsAllowed = false;
            Mokus2DGame.SoundManager.PlaySound("newClip1", 0.5f, 0f, 0f);
        }

        protected override void FadeOutTails()
        {
        }

        private const float SOUND_TIME = 7f;

        private const float LIGHTS_TIME = 8f;

        private const float END_SHAKE_TIME = 8f;

        private static readonly float STOP_OFFSET = CocosUtil.iPadValue(124f);

        protected bool animationsAllowed;

        protected UserData data;

        protected List<EnergyPart> energy = new();

        protected float energySpeed;

        protected bool hasToStop;

        protected Outro outro;

        protected float shakePosition;

        protected bool stoped;

        protected MovieStripesView stripesView;
    }
}
