using System.Collections.Generic;

using Mokus2D.ContreJourMono.ContreJour.Game.Eyes;

using Microsoft.Xna.Framework;

using Mokus2D;

namespace Mokus2D.Default.Namespace
{
    public class HeroEye : RandomAnimationEye
    {
        public bool MoveAllowed
        {
            get => moveAllowed;
            set
            {
                moveAllowed = value;
                if (!value)
                {
                    ViewDistance = 0f;
                }
            }
        }

        public HeroEye(ContreJourGame game)
            : base(game, true, new Vector2(50f, 50f))
        {
            moveAllowed = true;
            colorTime = 0f;
            colorProgress = 0f;
            sounds = new Dictionary<string, List<string>>();
            sounds["McEyeSmile"] = new List<string>(["laugh0", "laugh1", "laughl3"]);
            sounds["McEyeWink"] = new List<string>(["suspicious0", "suspicious1", "suspicious3"]);
            sounds["McEyeAngry"] = new List<string>(["angry2"]);
            sounds["McEyeBlinkOneTime"] = new List<string>(["clip0"]);
            sounds["McEyeBlink"] = new List<string>(["clip1"]);
            if (!BlackEye)
            {
                Scale = 1.07f;
            }
        }

        public void SetVelocity(Vector2 velocity)
        {
            if (moveAllowed)
            {
                ViewAngle = Maths.atan2Vec(velocity);
                ViewDistance = velocity.Length() / 3f;
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            currentBackground.Position = currentEyeBall.Position * 0.5f;
            if (colorTime > 0f)
            {
                colorProgress = Maths.StepToTargetMaxStep(colorProgress, 1f, 0.1f);
                isDefaultColor = false;
                colorTime -= time;
            }
            else if (!isDefaultColor)
            {
                colorProgress = Maths.StepToTargetMaxStep(colorProgress, 0f, 0.1f);
                if (Maths.FuzzyEquals(colorProgress, 0f, 0.0001f))
                {
                    isDefaultColor = true;
                    RefreshColor(START_COLOR);
                }
            }
            if (!isDefaultColor)
            {
                Color color = Color.Lerp(START_COLOR, BONUS_COLOR, colorProgress);
                RefreshColor(color);
            }
        }

        protected override void OnAnimation(EyeAnimation animation)
        {
            if (!TryPlaySound(animation.Background))
            {
                _ = TryPlaySound(animation.EyeBall);
            }
        }

        public bool TryPlaySound(string key)
        {
            if (key != null && sounds.ContainsKey(key))
            {
                List<string> list = sounds[key];
                float num = key.StartsWith("McEyeBlink") ? 0.3f : 0.75f;
                Mokus2DGame.SoundManager.PlayRandomSound(list, num);
                return true;
            }
            return false;
        }

        protected override float ViewRadius => base.ViewRadius * 2f;

        public void ApplyBonus()
        {
            colorTime = 0.5f;
        }

        public void RefreshColor(Color color)
        {
            if (currentBackground.Parent != null)
            {
                currentBackground.Color = color;
            }
        }

        public void Smile()
        {
            bool flag = true;
            PlayAnimation(new EyeAnimation("McEyeSmile", null, flag, false), true);
        }

        protected override void RefreshLayout()
        {
            base.RefreshLayout();
            isDefaultColor = false;
        }

        protected override void CreateDefaultView()
        {
            if (!BlackEye)
            {
                base.CreateDefaultView();
                return;
            }
            background = ClipFactory.CreateWithAnchor("McEyeBlack");
            eyeBall = ClipFactory.CreateWithAnchor(Game.ChooseSide("McEyeBallBlack", "McEyeBallWhite", null, null, "McEyeBall_6"));
        }

        protected override string ProcessName(string _name)
        {
            string text = base.ProcessName(_name);
            if (text == "McEyeBallHitBlack")
            {
                if (IsWhite)
                {
                    return "McEyeBallHitWhite";
                }
                if (Game.BonusChapter)
                {
                    return "McEyeBallHit_6";
                }
            }
            return text;
        }

        private const float HERO_EYE_SCALE = 1.07f;

        private const float EYE_CORNER_SPEED = 3f;

        private const float COLOR_TIME = 0.5f;

        private const string EyeBallBlack = "McEyeBallBlack";

        private const string EyeBallWhite = "McEyeBallWhite";

        private const string EyeBallGreen = "McEyeBall_6";

        private static readonly Color START_COLOR = new(255, 255, 255);

        private static readonly Color BONUS_COLOR = new(143, 238, 255);

        protected bool isDefaultColor;

        protected float colorTime;

        protected float colorProgress;

        protected Dictionary<string, List<string>> sounds;

        protected bool moveAllowed;
    }
}
