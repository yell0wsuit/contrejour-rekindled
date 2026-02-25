using System;
using System.Collections.Generic;

using ContreJourMono.ContreJour.Menu.LevelComplete;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Util;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class FinishView : MovieStripesView, IDisposable
    {
        public FinishView(ContreJourGame _game)
            : base(_game.BlackSide, true)
        {
            game = _game;
            _ = game.Schedule(new Action(CacheTextures), 0.1f);
            Scale = 1.1f;
            Position = -ScreenConstants.W7FromIPhoneScreenCenter * 0.05f;
            CreateHero();
        }

        private void CacheTextures()
        {
            ClipFactory.Cache("McLevelComplete");
            ClipFactory.Cache("McEnergyBig");
            ClipFactory.Cache("McEnergyBigInactive");
            ClipFactory.Cache("McHeroHighliteMenu");
            ClipFactory.Cache("McTotalLine");
            if (game.BlackSide || game.WhiteSide || game.BonusChapter)
            {
                ClipFactory.Cache("McFakeHeroEyeBlinkBlack");
                ClipFactory.Cache("McFakeHeroEyeSmileBlack");
                return;
            }
            ClipFactory.Cache("McFakeHeroEyeBlink");
            ClipFactory.Cache("McFakeHeroEyeSmile");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public void ShowWithLevelStarsScoreTimeNewHighScore(LevelPosition level, int _stars, int _score, float _time, bool _newHighScore)
        {
            Show();
            newHighScore = _newHighScore;
            stars = _stars;
            score = _score;
            time = _time;
            levelPosition = level;
            DelayedAction delayedAction = new(new Action(OnShow), FINISH_DURATION);
            Run(delayedAction);
        }

        private void OnShow()
        {
            bool flag = levelPosition.Chapter == 1;
            this.color = flag ? BLUE_LIGHT_COLOR : GREY_COLOR;
            Color color = flag ? CocosUtil.ccc4Mix(BLUE_LIGHT_COLOR, ContreJourConstants.WHITE_COLOR_3, 0.9f) : GREY_COLOR;
            Color color2 = flag ? BLUE_LIGHT_COLOR : ColorUtil.Mult(GREY_COLOR, 0.7f);
            if (game.BonusChapter)
            {
                this.color = ContreJourConstants.GreenLightColor;
                color = this.color;
                color2 = ContreJourConstants.GreenLightColor * 0.5f;
            }
            Sprite sprite = ClipFactory.CreateWithAnchor("McLevelComplete");
            AddChild(sprite);
            CGSize cgsize = ScreenConstants.W7FromIPhoneSize;
            center = new Vector2(cgsize.Width / 2f, cgsize.Height / 2f);
            sprite.Position = center;
            sprite.Run(new FadeIn(0.1f));
            sprite.Color = color2;
            sprite.Scale = 1.2f;
            clickableLayer = new ClickableLayer(0)
            {
                Position = sprite.Position
            };
            AddChild(clickableLayer);
            Button button3 = Button.ButtonBigWithIcon("McRestartIcon");
            button3.ClickEvent.AddListenerSelector(new Action(restartEvent.SendEvent));
            button3.Position = CocosUtil.ccpIPad(-5f, -116f);
            Button button2 = Button.ButtonBigWithIcon("McMenuIcon");
            button2.ClickEvent.AddListenerSelector(new Action(menuEvent.SendEvent));
            button2.Position = CocosUtil.ccpIPad(85f, -116f);
            buttons = new List<Button>([button3, button2]);
            if (levelPosition.SkipAvailable)
            {
                skipButton = Button.ButtonBigWithIcon("McSkipIcon");
                skipButton.ClickEvent.AddListenerSelector(new Action(NextLevelEvent.SendEvent));
                skipButton.Position = CocosUtil.ccpIPad(185f, -110f);
                skipButton.RealScale = 1.2f;
                buttons.Add(skipButton);
            }
            for (int i = 0; i < buttons.Count; i++)
            {
                Button button = buttons[i];
                button.ClickEvent.AddListenerSelector(new Action<TouchSprite>(OnButtonClick));
                clickableLayer.AddChild(button);
                button.Color = color;
                button.Visible = false;
                _ = Schedule(delegate
                {
                    ShowItemWithButton(button);
                }, (i + 2) * 0.1f);
            }
            if (!game.BonusChapter)
            {
                skipButton.Color = ColorUtil.Mult(color, 1.2f);
            }
            energies = new List<Sprite>();
            for (int j = 0; j < 3; j++)
            {
                string text = (j < stars) ? "McEnergyBig" : "McEnergyBigInactive";
                Sprite energy = ClipFactory.CreateWithAnchor(text);
                energy.Opacity = (j < stars) ? 255 : 70;
                energy.Scale = 1.7f;
                energy.Visible = false;
                clickableLayer.AddChild(energy);
                energy.Position = CocosUtil.toIPad(STARS_OFFSET + new Vector2(100 * (j - 1), 0f));
                energies.Add(energy);
                _ = Schedule(delegate
                {
                    ShowItemWithButton(energy);
                }, j * 0.1f);
                if (j < stars)
                {
                    _ = Schedule(delegate
                    {
                        BlinkItemWithButton(energy);
                    }, 1f + j * 0.5f);
                }
            }
            string text2 = Messages.CompleteText(stars);
            Label label = ContreJourLabel.CreateLabel(CocosUtil.iPad(40, 22), text2, true);
            label.Color = this.color;
            label.Position = CocosUtil.ccpIPad(-154f, 120f);
            clickableLayer.AddChild(label);
            label.Run(new FadeIn(0.2f));
            string text3 = string.Format(Messages.LEVEL, levelPosition.Chapter + 1, levelPosition.Index + 1);
            levelField = ContreJourLabel.CreateLabel(CocosUtil.iPad(30, 18), text3, true);
            clickableLayer.AddChild(levelField);
            levelField.Color = this.color;
            levelField.Anchor = new Vector2(0f, 0.5f);
            levelField.Position = CocosUtil.ccpIPad(-30f, 46f);
            levelField.Opacity = 0;
            _ = Schedule(new Action(ShowLevel), 0.5f);
            highlite = ClipFactory.CreateWithAnchor("McHeroHighliteMenu");
            highlite.Scale = 10f;
            highlite.Opacity = 80;
            if (flag)
            {
                highlite.Color = BLUE_LIGHT_COLOR;
                highlite.Opacity = 140;
            }
            else if (game.BonusChapter)
            {
                highlite.Color = ContreJourConstants.GreenLightColor;
            }
            AddChild(highlite);
            highlite.Position = center + CocosUtil.toIPad(HERO_OFFSET);
            highlite.Visible = false;
            portal = new MenuPortal(Vector2.Zero);
            AddChild(portal);
            portal.Position = center + CocosUtil.toIPad(HERO_OFFSET) + CocosUtil.toIPad(PORTAL_OFFSET);
            portal.Visible = false;
            portal.ItemsScale = 0f;
            portal.Scale = 2f;
            portal.ScaleStep = 0.2f;
            _ = Schedule(new Action(ShowPortal), 0.5f);
            if (levelPosition.Chapter == 1)
            {
                hero.HotSpot.Color = ColorUtil.Mult(BLUE_LIGHT_COLOR, 1.5f);
            }
            hero.Position = portal.Position;
        }

        private void CreateHero()
        {
            Type type = game.ChooseSide(typeof(FakeHeroBlack), typeof(FakeHeroWhite), typeof(FakeHero), typeof(FakeHero), typeof(FakeHeroGreen));
            hero = (FakeHero)ReflectUtil.CreateInstance(type, []);
            hero.Visible = false;
            AddChild(hero, 1);
        }

        private void OnButtonClick(TouchSprite obj)
        {
            clickableLayer.Enabled = false;
        }

        private void ShowLevel()
        {
            levelField.Run(new FadeIn(0.5f));
            _ = Schedule(new Action(ShowStarsBonus), 0.4f);
        }

        private void PlayBell()
        {
            Mokus2DGame.SoundManager.PlaySound("bell", 0.6f, 0f, 0f);
        }

        private void PlayClick()
        {
            Mokus2DGame.SoundManager.PlaySound("click", 0.3f, 0f, 0f);
        }

        public void ShowPortal()
        {
            portal.Visible = true;
            portal.TargetScale = 1f;
            _ = Schedule(new Action(ShowHero), 0.3f);
        }

        private void ShowHero()
        {
            hero.Visible = true;
            portal.TargetScale = 0f;
            portal.ScaleStep = 0.05f;
            hero.Scale = 0f;
            hero.Run(new ScaleTo(0.3f, 1f));
            float opacityFloat = highlite.OpacityFloat;
            highlite.Visible = true;
            highlite.Opacity = 0;
            highlite.Run(new FadeTo(0.2f, opacityFloat));
            _ = Schedule(new Action(LookAtStar), 0.5f);
        }

        private void LookAtStar()
        {
            hero.ViewTarget = energies[1].LocalToNode(Vector2.Zero, this, true);
            _ = Schedule(new Action(LookAtScore), 0.7f);
        }

        private void LookAtScore()
        {
            hero.ViewTarget = totalField.LocalToNode(Vector2.Zero, this, true);
            if (newHighScore)
            {
                UserData.Instance.Improved = true;
                _ = Schedule(new Action(LookAtImproved), 0.7f);
                return;
            }
            _ = Schedule(new Action(LookAtPlayer), 0.5f);
        }

        private void LookAtImproved()
        {
            hero.ViewTarget = stamp.LocalToNode(Vector2.Zero, this, true);
            _ = Schedule(new Action(LookAtPlayer), 0.5f);
        }

        private void LookAtPlayer()
        {
            hero.ViewTarget = hero.Position;
            if (newHighScore || stars == 3)
            {
                _ = Schedule(new Action(Smile), 0.5f);
                return;
            }
            _ = Schedule(new Action(Blink), 0.5f);
        }

        private void Blink()
        {
            hero.Eye.Blink();
            _ = Schedule(new Action(Blink), Maths.randRange(3f, 7f));
        }

        private void Smile()
        {
            Mokus2DGame.SoundManager.PlaySound("laugh0", 0.8f, 0f, 0f);
            hero.Eye.Smile();
            _ = Schedule(new Action(Blink), Maths.randRange(3f, 7f));
        }

        private void ShowStarsBonus()
        {
            starsBonusField = ContreJourLabel.CreateProgressLabel(CocosUtil.iPad(22, 15), Messages.ENERGY_BONUS, stars * 1000, 15);
            starsBonusField.Position = CocosUtil.ccpIPad(-30f, 16f);
            clickableLayer.AddChild(starsBonusField);
            starsBonusField.Run(new FadeIn(0.2f));
            starsBonusField.Color = color;
            starsBonusField.Anchor = new Vector2(0f, 0.5f);
            _ = Schedule(new Action(ShowTimeBonus), 0.4f);
        }

        private void ShowTimeBonus()
        {
            ProgressLabel progressLabel = ContreJourLabel.CreateProgressLabel(CocosUtil.iPad(22, 15), Messages.TIME_BONUS, UserData.GetTimeBonus(time), 15);
            clickableLayer.AddChild(progressLabel);
            progressLabel.Position = CocosUtil.ccpIPad(-30f, CocosUtil.iPad(-14f, -12f));
            progressLabel.Run(new FadeIn(0.2f));
            progressLabel.Color = color;
            progressLabel.Anchor = new Vector2(0f, 0.5f);
            _ = Schedule(new Action(ShowLine), 0.4f);
            _ = Schedule(new Action(ShowTotal), 0.6f);
        }

        private void ShowLine()
        {
            MovieClip movieClip = (MovieClip)ClipFactory.CreateWithAnchor("McTotalLine");
            movieClip.Color = color;
            clickableLayer.AddChild(movieClip);
            movieClip.Speed = 2f;
            movieClip.Position = CocosUtil.ccpIPad(-30f, CocosUtil.iPad(-30, -26));
            movieClip.Repeat = false;
        }

        private void ShowTotal()
        {
            totalField = ContreJourLabel.CreateProgressLabel(CocosUtil.iPad(30, 18), Messages.TOTAL, score, 15);
            clickableLayer.AddChild(totalField);
            totalField.Position = CocosUtil.ccpIPad(-30f, CocosUtil.iPad(-59f, -50f));
            totalField.Run(new FadeIn(0.2f));
            totalField.Anchor = new Vector2(0f, 0.5f);
            totalField.Color = color;
            if (newHighScore)
            {
                _ = Schedule(new Action(ShowNewHighScore), 0.7f);
            }
        }

        private void ShowNewHighScore()
        {
            stamp = ClipFactory.CreateWithAnchor("McImprovedResult");
            MultilineLabel multilineLabel = ContreJourLabel.CreateMultilineLabel(CocosUtil.iPad(42, 20), "IMPROVED_RESULT");
            multilineLabel.AnchorY = 0.5f;
            multilineLabel.LineSpacing = 0f;
            multilineLabel.Position = new Vector2(59f, -89f);
            multilineLabel.Rotation = 10.8f;
            stamp.AddChild(multilineLabel);
            stamp.Scale = 0f;
            stamp.Position = CocosUtil.ccpIPad(-270f, 100f);
            EaseOut easeOut = new(new ScaleTo(0.25f, 0.7f), 3f);
            stamp.Rotation = 170f;
            EaseOut easeOut2 = new(new RotateTo(0.25f, 0f), 3f);
            stamp.Run(new Spawn([easeOut, easeOut2]));
            clickableLayer.AddChild(stamp);
        }

        private void BlinkItemWithButton(Sprite button)
        {
            EaseOut easeOut = new(new ScaleTo(0.15f, button.Scale * 1.2f), 3f);
            EaseElasticIn easeElasticIn = new(new ScaleTo(0.15f, button.Scale), 0.3f);
            button.Run(new Sequence([easeOut, easeElasticIn]));
            PlayBell();
        }

        private void ShowItemWithButton(Sprite button)
        {
            float scale = button.Scale;
            button.Scale = 0f;
            button.Visible = true;
            ScaleTo scaleTo = new(0.6f, scale);
            float opacityFloat = button.OpacityFloat;
            button.Opacity = 0;
            FadeTo fadeTo = new(0.3f, opacityFloat);
            button.Run(new Spawn(
            [
                fadeTo,
                new EaseElasticOut(scaleTo, 0.3f)
            ]));
        }

        private const float LABEL_MARGINS = 30f;

        private const float LABELS_Y = 46f;

        private const int FONT_SIZE_IPHONE = 15;

        private const int FONT_SIZE = 22;

        private const float FIELDS_POSITION_IPHONE = -148f;

        private const float FIELDS_POSITION = -30f;

        private const int ENERGY_STEPS = 15;

        private static readonly Vector2 STARS_OFFSET = new(100f, 120f);

        private static readonly Vector2 PORTAL_OFFSET = new(0f, 0f);

        private static readonly Vector2 HERO_OFFSET = new(-180f, -70f);

        private readonly Color BLUE_LIGHT_COLOR = ContreJourConstants.BLUE_LIGHT_COLOR;

        private readonly Color GREY_COLOR = ContreJourConstants.GREY_COLOR;

        protected List<Button> buttons = new();

        protected Vector2 center;

        protected ClickableLayer clickableLayer;

        protected Color color;

        protected List<Sprite> energies;

        protected ContreJourGame game;

        protected FakeHero hero;

        protected Sprite highlite;

        protected Label levelField;

        protected LevelPosition levelPosition;

        protected bool newHighScore;

        public readonly EventSender NextLevelEvent = new();

        protected MenuPortal portal;

        protected int score;

        protected Sprite stamp;

        protected int stars;

        protected ProgressLabel starsBonusField;

        protected float time;

        protected ProgressLabel totalField;

        private Button skipButton;
    }
}
