using System;
using System.Collections.Generic;

using Mokus2D.Default.Namespace.Interfaces;
using Mokus2D.Default.Namespace.Windows;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mokus2D.Effects.Actions;
using Mokus2D.Extensions;
using Mokus2D.Util;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Mokus2D.Default.Namespace
{
    public class MainMenu : AccelerometerMenu, IActivatedDependent
    {
        public bool PlanetsLoaded => planetsLoaded;

        public MainMenu()
        {
            data = UserData.Instance;
            Mokus2DGame.SoundManager.MusicDisableEvent.AddListenerSelector(new Action(OnMusicDisable));
            foreground = new Node();
            AddChild(foreground, 3);
            winSize = ScreenConstants.W7FromIPhoneSize;
            centerPosition = ScreenConstants.W7FromIPhoneScreenCenter;
            CreateBackgrounds();
            CreatePlanets();
            CreateLabels();
            CreateScore();
            clickableLayer = new ClickableLayer(-1);
            AddChild(clickableLayer, 5);
            if (loadedLevel != -1 && !spinner.Exploding)
            {
                LevelPosition levelPosition = LevelsMenu.GetLevelPosition(loadedLevel);
                spinner.Visible = false;
                currentChapter = levelPosition.MenuChapter;
                spinner.CurrentIndex = currentChapter;
                names.Visible = false;
                names.Opacity = 0;
                CreateLevelsMenu();
                RefreshScore();
                inChapter = true;
                spinner.Enabled = false;
                FixCurrentPosition();
            }
            else
            {
                planetsLoaded = true;
                Mokus2DGame.SoundManager.PlayMusic("menu");
            }
            CreateButtons();
            CreateLiteButtons();
            Mokus2DGame.KeysController.AddBackKeyListener(new Action(OnBackClick), 0);
            achievementsWindow = new AchievementsWindow
            {
                Visible = false
            };
            achievementsWindow.OpenChangeEvent.AddListenerSelector(new Action(OnWindowOpenChange));
            AddChild(achievementsWindow, 100);
            leaderboardsWindow = new LeaderboardsWindow
            {
                Visible = false
            };
            leaderboardsWindow.OpenChangeEvent.AddListenerSelector(new Action(OnWindowOpenChange));
            AddChild(leaderboardsWindow, 100);
        }

        private void OnMusicDisable()
        {
            musicButton.Toggle = true;
        }

        public void AddForeground(Node node)
        {
            foreground.AddChild(node);
        }

        public void CreateLiteButtons()
        {
        }

        public void CreateButtons()
        {
            musicButton = new ToggleButton("McButtonMenuBackground.png", "McMusicIcon", "McDisabledIcon");
            ApplyButtonProperties(musicButton);
            musicButton.Position = new Vector2(winSize.Width - 76f, 10f);
            musicButton.ClickEvent.AddListenerSelector(new Action(OnMusicClick));
            musicButton.ToggleIcon.IgnoreParentColor = true;
            soundButton = new ToggleButton("McButtonMenuBackground.png", "McSoundIcon", "McDisabledIcon");
            ApplyButtonProperties(soundButton);
            soundButton.ToggleIcon.IgnoreParentColor = true;
            soundButton.Position = musicButton.Position - new Vector2(soundButton.Size.X * soundButton.RealScale, 0f);
            soundButton.ClickEvent.AddListenerSelector(new Action(OnSoundClick));
            RefreshSoundButtons();
            achievementsButton = new Button("McXBoxButton", null, null);
            ProcessXBoxButton(achievementsButton, "ACHIEVEMENTS", 12f, 18f, 1f);
            achievementsButton.Position = new Vector2(soundButton.Position.X - ((soundButton.Size.X + 68f) * soundButton.RealScale), 0f);
            achievementsButton.ClickEvent.AddListenerSelector(new Action(OnAchievementsClick));
            leaderboardsButton = new Button("McXBoxButton", null, null);
            ProcessXBoxButton(leaderboardsButton, "LEADERBOARDS", 12f, 18f, 1f);
            leaderboardsButton.Position = new Vector2(achievementsButton.Position.X - ((achievementsButton.Size.X + 10f) * achievementsButton.RealScale), 0f);
            leaderboardsButton.ClickEvent.AddListenerSelector(new Action(OnLeaderboardsClick));
        }

        private void ProcessXBoxButton(Button button, string text, float textSize = 12f, float labelOffset = 18f, float scaleMult = 1f)
        {
            ApplyButtonProperties(button);
            button.RealScale = 1f / ScreenConstants.Scales.fromIPhone2ByHeight * scaleMult;
            button.Scale = button.RealScale;
            Label label = ContreJourLabel.CreateLabel(textSize, text, true);
            label.Color = ContreJourConstants.GREY_COLOR;
            label.Y = labelOffset;
            button.AddChild(label);
        }

        private void OnLeaderboardsClick()
        {
            SetCurrentWindow(leaderboardsWindow);
        }

        private void OnAchievementsClick()
        {
            SetCurrentWindow(achievementsWindow);
        }

        private void ApplyButtonProperties(Button button)
        {
            button.StopEventPropagation = true;
            button.AnchorY = 1f;
            button.RealScale = 0.9f;
            foreach (Node child in button.Children)
            {
                child.Y = button.Size.Y / 2f;
                child.Scale = 2f;
            }
            clickableLayer.AddChild(button);
        }

        private void RefreshSoundButtons()
        {
            soundButton.Toggle = !Mokus2DGame.SoundManager.SoundEnabled;
            musicButton.Toggle = !Mokus2DGame.SoundManager.MusicEnabled;
        }

        public void CacheImages()
        {
            ClipFactory.Cache("McLevelItemBackground");
            ClipFactory.Cache("McLevelItemSelected");
            ClipFactory.Cache("McLevels1");
            ClipFactory.Cache("McLevelEnergy");
            ClipFactory.Cache("McLevels2");
            ClipFactory.Cache("McLevels3");
            ClipFactory.Cache("McLevels4");
            ClipFactory.Cache("McLevels5");
            ClipFactory.Cache("McLevels6");
            ClipFactory.Cache("McLevels7");
            ClipFactory.Cache("McLevels8");
            ClipFactory.Cache("McLevels9");
            ClipFactory.Cache("McLevels0");
        }

        private void OnSoundClick()
        {
            data.SoundDisabled = soundButton.Toggle;
            Mokus2DGame.SoundManager.SoundEnabled = !soundButton.Toggle;
        }

        private void OnMusicClick()
        {
            data.MusicDisabled = musicButton.Toggle;
            Mokus2DGame.SoundManager.MusicEnabled = !musicButton.Toggle;
        }

        public void CreatePlanets()
        {
            spinner = new PlanetsSpinner(this);
            AddChild(spinner);
            spinner.Position = centerPosition;
            spinner.SelectEvent.AddListenerSelector(new Action<int>(OnChapterSelect));
        }

        public void CreateLabels()
        {
            names = new NamesChanger
            {
                Position = CocosUtil.iPad(NAMES_POSITION, NAMES_POSITION_IPHONE)
            };
            AddChild(names, 4);
            logo = ClipFactory.CreateWithAnchor("McMainMenuLogo");
            AddChild(logo, 4);
            logo.Position = new Vector2(winSize.Width - CocosUtil.iPad(100, 80) - logo.Size.X, winSize.Height);
            if (!Constants.IS_IPAD)
            {
                logo.Scale = 1.3f;
            }
        }

        public void CreateBackgrounds()
        {
            List<string> list = Backgrounds();
            backgroundImages = new List<Sprite>();
            background = new Node();
            AddChild(background, -2);
            background.Scale = 1.3f;
            int num = 1;
            foreach (string text in list)
            {
                Sprite sprite;
                if (HardwareCapabilities.IsLowMemoryDevice)
                {
                    string texturePath = ClipFactory.GetTexturePath(string.Format("{0}Chapter16Bit", num));
                    sprite = new Sprite(Mokus2DGame.SharedContent.Load<Texture2D>(texturePath))
                    {
                        Position = ScreenConstants.IPhoneScreenCenter
                    };
                }
                else
                {
                    sprite = ClipFactory.CreateWithAnchor(text);
                    sprite.Scale = 2f;
                    sprite.Position = CocosUtil.iPad(new Vector2(0f, winSize.Height), new Vector2(0f, winSize.Height + 20f));
                }
                if (!Constants.IS_IPAD)
                {
                    sprite.Scale *= 0.9375f;
                }
                sprite.OpacityFloat = 0f;
                sprite.Visible = false;
                background.AddChild(sprite);
                backgroundImages.Add(sprite);
                num++;
            }
            if (!HardwareCapabilities.IsLowMemoryDevice)
            {
                backgroundImages[0].Position = new Vector2(0f, winSize.Height + 50f);
            }
            blackLayer = new LayerColor(new Color(0, 0, 0, 255));
            AddChild(blackLayer, 2);
            blackLayer.Opacity = 0;
            blackLayer.Visible = false;
            ground = ClipFactory.CreateWithAnchor(CocosUtil.iPad("McMenuGround", "McMenuGroundPhone"));
            ground.ScaleY = 1.05f;
            ground.Position = new Vector2(0f, winSize.Height * (1f + ground.ScaleY) / 2f);
            ground.ScaleX = ScreenConstants.W7FromIPhoneSize.X / ScreenConstants.OsSizes.IPhoneRetina.X;
            AddChild(ground, 3);
            Sprite sprite2 = backgroundImages[1];
            sprite2.Color = BLUE_COLOR;
            if (!HardwareCapabilities.IsLowMemoryDevice)
            {
                backgroundImages[5].Position = new Vector2(0f, winSize.Height - 100f);
            }
            backgroundChanger = new BackgroundChanger(backgroundImages);
        }

        public void CreateScore()
        {
            starsIcon = ClipFactory.CreateWithAnchor("McEnergyIcon");
            AddChild(starsIcon, 4);
            starsIcon.Position = CocosUtil.iPad(new Vector2(30f, 24f), new Vector2(20f, 16f));
            starsIcon.Scale = CocosUtil.iPad(1f, 1.3f);
            starsField = ContreJourLabel.CreateLabel(CocosUtil.iPad(22, 16), true);
            starsField.Anchor = new Vector2(0f, 0.5f);
            AddChild(starsField, 4);
            starsField.Position = CocosUtil.iPad(new Vector2(52f, 20f), new Vector2(48f, 14f));
            starsField.Visible = true;
            starsIcon.Visible = true;
            RefreshScore();
        }

        public List<string> Backgrounds()
        {
            return ["McBackground4Content", "McBackgroundContent1_5", "McMenuBackground3", "McChapter4MenuBackground", "McChapter5MenuBackground", "McBackgroundContent6_1"];
        }

        private void OnChapterSelect(int chapter)
        {
            if (!inChapter && spinner.Scale == 1f)
            {
                Mokus2DGame.SoundManager.PlaySound("click", 0.8f, 0f, 0f);
                currentChapter = chapter;
                HidePlanets();
            }
        }

        public void HideScore()
        {
            starsField.Run(new FadeOut(0.3f));
            starsIcon.Run(new FadeOut(0.3f));
        }

        public void ShowScore()
        {
            RefreshScore();
            starsField.Run(new FadeIn(0.3f));
            starsIcon.Run(new FadeIn(0.3f));
        }

        public void RefreshScore()
        {
            int num = inChapter ? data.GetChapterStars(currentChapter) : data.TotalStars;
            int num2 = inChapter ? data.GetChapterScore(currentChapter) : data.TotalScore;
            int num3 = inChapter ? 60 : (ContreJourConstants.LEVEL_COUNT * 3);
            string text = string.Format(Messages.STARS_AND_SCORE, num, num3, num2);
            starsField.TextString = text;
        }

        public void HidePlanets()
        {
            HideScore();
            blackLayer.StopAllActions();
            inChapter = true;
            spinner.Enabled = false;
            ScaleTo scaleTo = new(0.3f, 5f);
            InstantAction instantAction = new(new Action(ShowLevels));
            spinner.Run(new Sequence([scaleTo, instantAction]));
            names.Run(new Sequence(
            [
                new FadeTo(0.1f, 0f),
                new Hide()
            ]));
            blackLayer.Visible = true;
            blackLayer.Run(new FadeTo(0.3f, 1f));
        }

        public void ShowLevels()
        {
            ShowScore();
            spinner.Visible = false;
            blackLayer.Run(new Sequence(
            [
                new FadeTo(2f, 0f),
                new Hide()
            ]));
            CreateLevelsMenu();
            float scale = levelsMenu.Scale;
            levelsMenu.Scale = scale * 0.5f;
            levelsMenu.Run(new Sequence(
            [
                new ScaleTo(0.3f, scale)
            ]));
            levelsMenu.Position = ScreenConstants.W7FromIPhoneScreenCenter;
        }

        public void CreateLevelsMenu()
        {
            levelsMenu = new LevelsMenu(currentChapter, ScreenConstants.W7FromIPhoneScreenCenter);
            levelsMenu.SelectLevelEvent += new Action<int>(OnLevelSelect);
            AddChild(levelsMenu);
        }

        private void OnLevelSelect(int level)
        {
            if (LevelSelectEvent.Enabled)
            {
                inLevel = true;
                loadedLevel = level;
                LevelSelectEvent.SendEvent(level);
                LevelSelectEvent.Enabled = false;
                CrystalManager.Instance().Visible = false;
            }
        }

        private void OnBackClick()
        {
            if (currentWindow != null && currentWindow.Open)
            {
                currentWindow.Open = false;
                return;
            }
            if (inChapter)
            {
                HideLevels();
                inChapter = false;
                return;
            }
            Mokus2DGame.Instance.Exit();
        }

        public void HideLevels()
        {
            HideScore();
            blackLayer.StopAllActions();
            InstantAction instantAction = new(new Action(ShowPlanets));
            blackLayer.Run(new Sequence(
            [
                new FadeTo(0.3f, 1f),
                instantAction
            ]));
            levelsMenu.Run(new ScaleTo(0.3f, 0.5f));
            blackLayer.Visible = true;
        }

        private void ShowPlanets()
        {
            planetsLoaded = true;
            Mokus2DGame.SoundManager.PlayMusic("menu");
            names.StopAllActions();
            names.Visible = true;
            names.Run(new FadeTo(0.3f, 1f));
            accelerometerUsed = false;
            inChapter = false;
            ShowScore();
            RemoveChild(levelsMenu);
            levelsMenu = null;
            blackLayer.Run(new Sequence(
            [
                new FadeTo(1f, 0f),
                new Hide()
            ]));
            spinner.Visible = true;
            spinner.Run(new ScaleTo(0.3f, 1f));
            spinner.Enabled = true;
        }

        public void ShowChapter(int chapter)
        {
            spinner.Scale = 1f;
            if (inChapter)
            {
                levelsMenu.Scale = 0f;
                levelsMenu.Visible = false;
                HideLevels();
            }
            spinner.SetTargetChapter(chapter);
        }

        private void FixCurrentPosition()
        {
        }

        public override void Update(float time)
        {
            base.Update(time);
            RefreshPosition();
            RefreshColors();
        }

        private void RefreshColors()
        {
            Color color = CocosUtil.ccc4Mix(BackColors[backgroundChanger.FirstIndex], BackColors[backgroundChanger.NextIndex], 1f - backgroundChanger.Offset);
            Color color2 = CocosUtil.ccc4Mix(color, Color.White, 0.7f);
            ground.Color = color;
            logo.Color = color2;
            Color color3 = CocosUtil.ccc4Mix(FONT_COLORS[backgroundChanger.FirstIndex], FONT_COLORS[backgroundChanger.NextIndex], 1f - backgroundChanger.Offset);
            starsField.Color = color3;
            soundButton.Color = color2;
            musicButton.Color = color2;
            achievementsButton.Color = color2;
            leaderboardsButton.Color = color2;
        }

        private void OnWindowOpenChange()
        {
            if (!currentWindow.Open)
            {
                clickableLayer.Enabled = true;
                if (levelsMenu != null)
                {
                    levelsMenu.Enabled = true;
                }
                spinner.Enabled = !inChapter;
                currentWindow = null;
            }
        }

        private void SetCurrentWindow(PopUpWindow window)
        {
            currentWindow = window;
            window.Open = true;
            clickableLayer.Enabled = false;
            spinner.Enabled = false;
            if (levelsMenu != null)
            {
                levelsMenu.Enabled = false;
            }
        }

        public void RefreshPosition()
        {
            backgroundChanger.CurrentIndex = spinner.CurrentIndex;
            names.CurrentIndex = spinner.CurrentIndex;
            if (!inChapter)
            {
                spinner.AccelerometerOffset = accelerometerOffset;
            }
            else if (!inLevel && levelsMenu != null)
            {
                levelsMenu.Position = new Vector2(accelerometerOffset.X * 1.05f * RADIUS / 2f, accelerometerOffset.Y * CocosUtil.iPadValue(0.25f)) + ScreenConstants.W7FromIPhoneScreenCenter;
            }
            accelerometerUsed = true;
            Vector2 vector = new(-accelerometerOffset.X * 1.05f * RADIUS, -accelerometerOffset.Y * 0.1f);
            background.Position = vector + CocosUtil.iPad(new Vector2(-100f, -50f), new Vector2(-60f, -10f));
            Vector2 vector2 = new(accelerometerOffset.X * 0.3f * RADIUS, accelerometerOffset.Y * CocosUtil.iPad(0.3f, 0.07f));
            names.Position = CocosUtil.iPad(NAMES_POSITION, NAMES_POSITION_IPHONE) + vector2;
            foreground.Position = vector2 * 0.2f;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Mokus2DGame.KeysController.RemoveBackKeyListener(new Action(OnBackClick));
            spinner.Dispose();
            Mokus2DGame.SoundManager.MusicDisableEvent.RemoveListenerSelector(new Action(OnMusicDisable));
        }

        public void OnGameActivated()
        {
            RefreshSoundButtons();
        }

        ~MainMenu()
        {
            DebugLog.info("~MainMenu", null);
        }

        private const float FADE_TIME = 2f;

        private const float EFFECTS_TIME = 0.3f;

        private const float BACKGROUND_ACC_MULT = 1.05f;

        private const float NAMES_ACC_MULT_IPHONE = 0.07f;

        private const float NAMES_ACC_MULT = 0.3f;

        private const float FOREGROUND_ACC_MULT = 1.1f;

        private const float MIN_SPEED = 0.12f;

        private const float MIN_SPEED_BACK = 0.02f;

        private const float MIN_ROTATION_DIFF = 0.2f;

        private const float ROTATION_MULTIPLIER = 1.8f;

        private const float MAX_SCALE = 1.5f;

        protected UserData data;

        protected LevelsMenu levelsMenu;

        public readonly EventSender<int> LevelSelectEvent = new();

        protected Sprite ground;

        protected Sprite logo;

        protected List<Sprite> backgroundImages;

        protected LayerColor blackLayer;

        protected Label starsField;

        protected Node starsIcon;

        protected Vector2 centerPosition;

        protected Node background;

        protected ToggleButton soundButton;

        protected ToggleButton musicButton;

        private Button achievementsButton;

        private Button leaderboardsButton;

        protected ClickableLayer clickableLayer;

        protected NamesChanger names;

        protected Node foreground;

        private PlanetsSpinner spinner;

        private BackgroundChanger backgroundChanger;

        private readonly LeaderboardsWindow leaderboardsWindow;

        private readonly AchievementsWindow achievementsWindow;

        private PopUpWindow currentWindow;

        protected bool inChapter;

        protected bool inLevel;

        protected int currentChapter;

        protected CGSize winSize;

        protected bool planetsLoaded;

        private static int loadedLevel = -1;

        private static readonly Color BLUE_COLOR = ContreJourConstants.BLUE_LIGHT_COLOR;

        private static readonly Color GREY_COLOR = ContreJourConstants.GREY_COLOR;

        private static readonly Color GreenColor = 12573952.ToRGBColor();

        private static readonly Color[] FONT_COLORS =
        [
            GREY_COLOR,
            CocosUtil.ccc4Mix(BLUE_COLOR, ContreJourConstants.WHITE_COLOR_3, 0.8f),
            CocosUtil.lite(CocosUtil.ccc4Mix(BLUE_COLOR, ContreJourConstants.WHITE_COLOR_3, 0.8f), GREY_COLOR),
            GREY_COLOR,
            GREY_COLOR,
            GreenColor
        ];

        private static readonly Color[] BackColors =
        [
            GREY_COLOR,
            BLUE_COLOR,
            CocosUtil.lite(BLUE_COLOR, GREY_COLOR),
            ColorUtil.Mult(GREY_COLOR, 0.5f),
            GREY_COLOR,
            GreenColor
        ];

        private readonly Vector2 NAMES_POSITION_IPHONE = CocosUtil.Vector2Retina(80f, 236f);

        private readonly Vector2 NAMES_POSITION = new(200f, 590f);

        private readonly float FRONT_SCALE = CocosUtil.lite(1f, 0.7f);

        private readonly float MIN_SCALE = CocosUtil.lite(0.5f, 0.4f);

        private readonly float RADIUS = CocosUtil.iPadValue(CocosUtil.lite(340, 360));
    }
}
