using System;

using Default.Namespace;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Util.MathUtils;

namespace ContreJourMono.ContreJour.Game.Eyes
{
    public class RandomAnimationEye : EyeBase
    {
        protected bool HasAnimations => Animations != null && Animations.Length > 0;

        protected virtual EyeAnimation[] Animations => ANIMATIONS;

        protected bool IsWhite => Game != null && Game.WhiteSide;

        protected bool BlackEye => Game != null && (Game.WhiteSide || Game.BlackSide || Game.BonusChapter);

        public virtual bool AnimationsAllowed
        {
            get => _animationsAllowed; set => _animationsAllowed = value;
        }

        public RandomAnimationEye(ContreJourGame game, bool useMask, Vector2 maskSize)
            : base(game, useMask, maskSize)
        {
            UpdateEnabled = false;
            clipEndAction = new Action(OnClipEnd);
            ScheduleAnimation();
            CacheAnimations();
        }

        public RandomAnimationEye(ContreJourGame game)
            : this(game, false, Vector2.Zero)
        {
        }

        private void CacheAnimations()
        {
            if (HasAnimations)
            {
                foreach (EyeAnimation eyeAnimation in Animations)
                {
                    ClipFactory.Cache(ProcessName(eyeAnimation.Background));
                }
            }
        }

        protected virtual void ScheduleAnimation()
        {
            if (HasAnimations)
            {
                scheduledAnimation = Schedule(new Action(Animate), Maths.randRange(3f, 10f));
            }
        }

        public void UnscheduleAnimation()
        {
            if (scheduledAnimation != null)
            {
                StopAction(scheduledAnimation);
                scheduledAnimation = null;
            }
        }

        protected virtual void Animate()
        {
            scheduledAnimation = null;
            if (AnimationsAllowed && HasAnimations)
            {
                PlayAnimation(Animations.RandomItem(), false);
                return;
            }
            ScheduleAnimation();
        }

        protected override string ProcessName(string name)
        {
            return BlackEye ? (name + "Black") : name;
        }

        public void PlayAnimation(EyeAnimation animation, bool force)
        {
            if (isPlaying && !force)
            {
                return;
            }
            if (isPlaying)
            {
                EndAnimation();
            }
            OnAnimation(animation);
            isPlaying = true;
            SetEyeContent(animation);
            if (endDispatcher != null)
            {
                endDispatcher.EndEvent += clipEndAction;
            }
        }

        protected virtual void OnAnimation(EyeAnimation animation)
        {
        }

        private void EndAnimation()
        {
            if (endDispatcher != null)
            {
                endDispatcher.EndEvent -= clipEndAction;
            }
            isPlaying = false;
            ScheduleAnimation();
            if (ReturnToDefault)
            {
                SetDefaultView();
            }
        }

        private void OnClipEnd()
        {
            EndAnimation();
            AnimationEndEvent.SendEvent();
        }

        // Note: this type is marked as 'beforefieldinit'.
        static RandomAnimationEye()
        {
            EyeAnimation[] array = new EyeAnimation[5];
            EyeAnimation[] array2 = array;
            int num = 0;
            bool flag = true;
            array2[num] = new EyeAnimation("McEyeSmile", null, flag, false);
            array[1] = new EyeAnimation("McEyeBlink", null, false, false);
            EyeAnimation[] array3 = array;
            int num2 = 2;
            bool flag2 = true;
            array3[num2] = new EyeAnimation("McEyeBlinkOneTime", null, flag2, false);
            EyeAnimation[] array4 = array;
            int num3 = 3;
            bool flag3 = true;
            array4[num3] = new EyeAnimation("McEyeWink", null, flag3, false);
            array[4] = new EyeAnimation("McEyeAngry", null, false, false);
            ANIMATIONS = array;
        }

        private const float MIN_TIMEOUT = 3f;

        private const float MAX_TIMEOUT = 10f;

        private static readonly EyeAnimation[] ANIMATIONS;

        public readonly EventSender AnimationEndEvent = new();

        public bool ReturnToDefault = true;

        private bool _animationsAllowed = true;

        private readonly Action clipEndAction;

        private bool isPlaying;

        private DelayedAction scheduledAnimation;
    }
}
