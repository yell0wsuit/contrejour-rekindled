using System;
using System.Collections.Generic;

using Default.Namespace;
using Default.Namespace.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

using Mokus2D;
using Mokus2D.Effects.Transitions;
using Mokus2D.Util;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace ContreJour
{
    public class ContreJourApplication : Mokus2DGame
    {
        public static Dictionary<int, SpriteFont> Fonts { get; } = new();

        public ContreJourApplication()
        {
            IsFixedTimeStep = false;
            graphics = new GraphicsDeviceManager(this)
            {
                SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight,
                PreferredBackBufferFormat = HardwareCapabilities.IsLowMemoryDevice ? SurfaceFormat.Dxt5 : SurfaceFormat.Color
            };
            ContentRootDirectory = "content";
            TargetElapsedTime = TimeSpan.FromTicks(166667L);
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(OnPreparingDeviceSettings);
            InactiveSleepTime = TimeSpan.FromSeconds(1.0);
            TouchPanel.EnabledGestures = GestureType.HorizontalDrag;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 480;
        }

        protected override void Draw(GameTime gameTime)
        {
            try
            {
                base.Draw(gameTime);
            }
            catch
            {
            }
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void OnPreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.One;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            ResetElapsedTime();
            Fonts[14] = Content.Load<SpriteFont>("Font14");
            Fonts[20] = Content.Load<SpriteFont>("Font20");
            Fonts[28] = Content.Load<SpriteFont>("Font28");
            Fonts[40] = Content.Load<SpriteFont>("Font40");
        }

        private void CreateDebugFields()
        {
            Node node = new();
            Root.AddChild(node);
            upsField = new IntLabel(Fonts[14]);
            memoryField = new IntLabel(Fonts[14]);
            upsField.Color = Color.Aquamarine;
            memoryField.Color = Color.Aquamarine;
            upsField.Position = new Vector2(60f, 40f);
            memoryField.Position = new Vector2(90f, 40f);
        }

        protected override void Initialize()
        {
            base.Initialize();
            GraphicsDevice.PresentationParameters.DisplayOrientation = DisplayOrientation.LandscapeRight;
            PrimitivesDrawing.RefreshGraphicsDevice(Device);
            XNAUtil.RefreshViewport(GraphicsDevice);
            gameContainer = new Node
            {
                Scale = ScreenConstants.Scales.fromIPhone2ByHeight
            };
            gameContainer.Position = (ScreenConstants.OsSizes.W7 - (ScreenConstants.W7FromIPhoneSize * gameContainer.Scale)) / 2f;
            Root.AddChild(gameContainer);
            Root.Position = new Vector2(0f, ScreenSize.Y);
            Root.ScaleY = -1f;
            SoundManager.MusicPath = "Music";
            SoundManager.PreloadSongs(["chapter1", "chapter2", "chapter3", "chapter4", "chapter5", "intro-5", "menu"]);
            ShowSplash();
        }

        protected override RootNode CreateRootNode()
        {
            return new RootNode(Device.PresentationParameters.BackBufferWidth, Device.PresentationParameters.BackBufferHeight, new Vector2(1f, -1f));
        }

        private void ShowSplash()
        {
            Splash splash = new();
            gameContainer.AddChild(splash);
            splash.EndEvent.AddListenerSelector(new Action(OnSplashExit));
            currentNode = splash;
        }

        private void OnSplashExit()
        {
            if (!UserData.Instance.IntroWatched)
            {
                UserData.Instance.IntroWatched = true;
                LoadLevel(0);
                return;
            }
            _ = ChangeScene(new Func<MainMenu>(CreateMainMenu));
        }

        private FadeTransition ChangeScene<T>(Func<T> sceneFactory) where T : Node
        {
            FadeTransition fadeTransition = new(0.5f, 1f, currentNode, () => SetCurrentNode(sceneFactory), 1);
            fadeTransition.MiddleEvent += new Action(OnChangeScene);
            gameContainer.Run(fadeTransition);
            return fadeTransition;
        }

        private void OnChangeScene()
        {
            UserData.SaveUserData();
            GC.Collect();
            SharedContent.UnloadUnused();
        }

        private Node SetCurrentNode<T>(Func<T> nodeFactory) where T : Node
        {
            if (currentNode is not null and not null)
            {
                currentNode.Dispose();
            }
            currentNode = null;
            GC.Collect();
            SharedContent.UnloadUnused();
            currentNode = nodeFactory.Invoke();
            return currentNode;
        }

        private MainMenu CreateMainMenu(int chapter)
        {
            MainMenu mainMenu = CreateMainMenu();
            mainMenu.ShowChapter(chapter);
            return mainMenu;
        }

        private MainMenu CreateMainMenu()
        {
            MainMenu mainMenu = new();
            mainMenu.LevelSelectEvent.AddListenerSelector(new Action<int>(LoadLevel));
            return mainMenu;
        }

        public void LoadLevel(int _level)
        {
            bool flag = IsFirstLevel(currentNode) || _level == 0;
            Func<ContreJourGame> func = new(ProcessLoadLevel);
            if (flag)
            {
                func = CleanLoad(func);
            }
            lastLevel = _level;
            _ = ChangeScene(func);
        }

        private Func<T> CleanLoad<T>(Func<T> action) where T : Node
        {
            ForceRemoveTextures();
            return action;
        }

        private ContreJourGame ProcessLoadLevel()
        {
            int chapter = LevelsMenu.GetLevelPosition(lastLevel).Chapter;
            ContreJourGame contreJourGame = new(chapter)
            {
                CanShowIntro = canShowIntro
            };
            contreJourGame.BackEvent.AddListenerSelector(new Action(OnLevelBack));
            contreJourGame.RestartEvent.AddListenerSelector(new Action(RestartLevel));
            contreJourGame.NextLevelEvent.AddListenerSelector(new Action(NextLevel));
            contreJourGame.LoadLevelIndex(lastLevel);
            SoundManager.PlayMusic(string.Format("chapter{0}", Maths.min(chapter + 1, 4)));
            return contreJourGame;
        }

        private void OnLevelBack()
        {
            canShowIntro = true;
            Func<MainMenu> func = new(CreateMainMenu);
            if (IsFirstLevel(currentNode))
            {
                func = CleanLoad(new Func<MainMenu>(CreateMainMenu));
            }
            _ = ChangeScene(func);
        }

        private void RestartLevel()
        {
            canShowIntro = false;
            _ = ChangeScene(new Func<ContreJourGame>(ProcessLoadLevel));
        }

        public void NextLevel()
        {
            canShowIntro = true;
            LevelPosition levelPosition = LevelsMenu.GetLevelPosition(lastLevel);
            if (levelPosition.Index < Constants.LevelsToPlay - 1)
            {
                levelPosition.Index++;
                LoadLevel(LevelsMenu.GetLevelIndex(levelPosition));
                return;
            }
            if (levelPosition.Chapter == 5)
            {
                _ = ChangeScene(() => CreateMainMenu(5));
                return;
            }
            if (CocosUtil.lite(true, levelPosition.Chapter + 1 < Constants.NormalChaptersCount))
            {
                int chapter = levelPosition.Chapter + 1;
                _ = ChangeScene(() => CreateMainMenu(chapter));
                return;
            }
            LoadLevel(169);
        }

        public void ForceRemoveTextures()
        {
        }

        public bool IsFirstLevel(Node node)
        {
            return node is ContreJourGame game && game.LevelIndex == 0;
        }

        private void OnSplashEnd()
        {
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            base.OnActivated(sender, args);
            UserData.Instance.RefreshSoundManager();
            if (currentNode is IActivatedDependent dependent)
            {
                dependent.OnGameActivated();
            }
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            UserData.SaveUserData();
        }

        protected void OnExiting(object sender, EventArgs args)
        {
            UserData.SaveUserData();
        }

        private const float FADE_OUT_DURATION = 0.5f;

        private const float FADE_IN_DURATION = 1f;

        private readonly GraphicsDeviceManager graphics;

        private Node gameContainer;

        protected Node currentNode;

        protected int lastLevel;

        private bool canShowIntro = true;

        private readonly FpsCounter upsCounter = new(10);

        private IntLabel upsField;

        private IntLabel memoryField;
    }
}
