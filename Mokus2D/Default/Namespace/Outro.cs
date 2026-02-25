using System;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class Outro : Node, ITouchListener, IDisposable
    {
        public Outro(ContreJourGame game, bool success)
        {
            Mokus2DGame.TouchController.AddListener(this, 0);
            minY = CocosUtil.iPad(190, -100);
            margins = CocosUtil.iPad(15f, 20f);
            maxY = CocosUtil.iPad(820, 760);
            background = new LayerColor(ContreJourConstants.BLACK_COLOR);
            background.Opacity = 0;
            background.Run(new FadeTo(2f, 0.5882353f));
            AddChild(background);
            this.text = new Node();
            textPosition = ScreenConstants.W7FromIPhoneScreenCenter;
            textPosition.Y = minY;
            this.text.Position = textPosition;
            AddChild(this.text);
            string text = success ? "YOU_DID_IT" : "YOU_TRIED_HARD";
            Label label = ContreJourLabel.CreateMultilineLabel(CocosUtil.iPad(28, 22), text);
            label.Position = new Vector2(0f, CocosUtil.iPad(-120, 160));
            label.Color = ContreJourConstants.WHITE_COLOR_3;
            this.text.AddChild(label);
            MultilineLabel multilineLabel = ContreJourLabel.CreateMultilineLabel(CocosUtil.iPad(24, 16), "CREDITS");
            multilineLabel.Position = new Vector2(0f, CocosUtil.iPad(-240, -248));
            multilineLabel.Color = ContreJourConstants.WHITE_COLOR_3;
            this.text.AddChild(multilineLabel);
            creditsPosition = new Vector2(0f, CocosUtil.iPad(-270, -290));
            AddCreditsRight("GRAPHIC_ARTISTS", "MIHAI_TYMOSHENKO");
            AddCreditsRight(null, "ANDRIY_SHVUREV");
            AddCreditsRight("COMPOSER", "DAVID_LEON");
            AddCreditsRight("SFX_BY", "IHOR_PRYSHLIAK");
            AddCreditsRight("PRODUCER", "TOM_KINNINBUGRH");
            AddCreditsRight("IDEA", "ANTON_MYKHAYLETS");
            AddCreditsRight("EVERYTHING_ELSE", "MAKSYM_HRYNIV");
            AddCreditsRight("BETA_TESTERS", "BETA_TESTERS_NAMES_1");
            AddCreditsRight(null, "BETA_TESTERS_NAMES_2");
            AddCreditsRight(null, "BETA_TESTERS_NAMES_3");
            AddCreditsRight(null, "BETA_TESTERS_NAMES_4");
            Label label2 = ContreJourLabel.CreateLabel(CocosUtil.iPad(24, 16), "STARS", true);
            label2.Position = creditsPosition;
            label2.Y += 10f;
            label2.Color = ContreJourConstants.WHITE_COLOR_3;
            this.text.AddChild(label2);
            textVisible = true;
            onEndTime = 0f;
        }

        public bool TextVisible
        {
            get => textVisible;
            set
            {
                if (textVisible != value)
                {
                    textVisible = value;
                    text.StopAllActions();
                    float num = value ? 1f : 0f;
                    text.Run(new FadeTo(2f, num));
                }
            }
        }

        private bool UseAccelerometer()
        {
            return false;
        }

        private int Priority(Vector2 touchPoint)
        {
            return 1;
        }

        public void AddCreditsRight(string left, string right)
        {
            if (left != null)
            {
                Label label = ContreJourLabel.CreateLabel(CocosUtil.iPad(24, 16), left, true);
                label.Color = ContreJourConstants.WHITE_COLOR_3;
                label.Anchor = new Vector2(1f, 0.5f);
                label.Position = creditsPosition + CocosUtil.ccpIPad(-margins, 0f);
                text.AddChild(label);
            }
            if (right != null)
            {
                Label label2 = ContreJourLabel.CreateLabel(CocosUtil.iPad(24, 16), right, true);
                label2.Color = ContreJourConstants.WHITE_COLOR_3;
                label2.Anchor = new Vector2(0f, 0.5f);
                label2.Position = creditsPosition + CocosUtil.ccpIPad(margins, 0f);
                text.AddChild(label2);
            }
            NextLine();
        }

        public bool TouchBegin(Touch touch)
        {
            if (currentTouch != null)
            {
                return false;
            }
            touchSpeed = 0f;
            currentTouch = touch;
            touchPosition = CocosUtil.TouchPointInNode(touch, this);
            return true;
        }

        public bool TouchMove(Touch touch)
        {
            RefreshTouchPosition(touch);
            return true;
        }

        public void RefreshTouchPosition(Touch touch)
        {
            if (touch != currentTouch)
            {
                return;
            }
            Vector2 vector = CocosUtil.TouchPointInNode(currentTouch, this);
            touchSpeed = Maths.Clamp(vector.Y - touchPosition.Y, -100f, 100f);
            textPosition.Y = textPosition.Y + (vector.Y - touchPosition.Y);
            touchPosition = vector;
        }

        public void TouchEnd(Touch touch)
        {
            if (touch == currentTouch)
            {
                currentTouch = null;
            }
        }

        public void NextLine()
        {
            creditsPosition += CocosUtil.ccpIPad(0f, -30f);
        }

        public void BackLine()
        {
            creditsPosition += CocosUtil.ccpIPad(0f, 30f);
        }

        public override void Update(float time)
        {
            if (currentTouch == null)
            {
                if (textPosition.Y < maxY)
                {
                    textPosition += new Vector2(0f, CocosUtil.iPadValue(SPEED) * time);
                }
                textPosition.Y = textPosition.Y + touchSpeed * time * 30f;
                touchSpeed = Maths.stepTo(touchSpeed, 0f, Math.Max(1f, Math.Abs(touchSpeed / 20f)));
                float num = 0f;
                if (textPosition.Y < minY)
                {
                    num = (minY - textPosition.Y) / 5f;
                }
                if (textPosition.Y > maxY)
                {
                    num = (maxY - textPosition.Y) / 5f;
                }
                if (textPosition.Y >= maxY)
                {
                    onEndTime += time;
                }
                if (num != 0f)
                {
                    textPosition.Y = textPosition.Y + num;
                    touchSpeed = Maths.stepTo(touchSpeed, 0f, Math.Max(1f, Math.Abs(touchSpeed / 20f)));
                }
            }
            else
            {
                onEndTime = 0f;
            }
            TextVisible = onEndTime <= 5f;
            text.Position = textPosition;
        }

        public new void Dispose()
        {
            Mokus2DGame.TouchController.RemoveListener(this);
        }

        private const float TEXT_FADE_TIME = 5f;

        private const float FADE_TIME = 2f;

        private const float MAX_TOUCH_SPEED = 100f;

        private const float CREDITS_MARGINS_IPHONE = 20f;

        private const float CREDITS_MARGINS = 15f;

        private const float CREDITS_STEP = 30f;

        private static readonly float SPEED = ContreJourLabel.IsAsian ? 30 : 15;

        protected LayerColor background;

        protected Vector2 creditsPosition;

        private Touch currentTouch;

        protected float margins;

        protected float maxY;

        protected float minY;

        protected float onEndTime;

        protected Node text;

        protected Vector2 textPosition;

        protected bool textVisible;

        protected Vector2 touchPosition;

        protected float touchSpeed;
    }
}
