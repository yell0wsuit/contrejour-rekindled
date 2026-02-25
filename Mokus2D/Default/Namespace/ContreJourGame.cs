using System;
using System.Collections.Generic;

using Default.Namespace.Interfaces;

using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Extensions;
using Mokus2D.Input;
using Mokus2D.Util;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class ContreJourGame : GameBase, IDisposable, ITouchListener, IActivatedDependent
    {
        public override bool Paused
        {
            set
            {
                if (Paused != value)
                {
                    clickableLayer.Enabled = !value;
                    GameRoot.UpdateEnabled = !value;
                    paused = value;
                    updater.Paused = value;
                    if (particles != null)
                    {
                        particles.Paused = value;
                    }
                }
            }
        }

        public int Frame
        {
            get => frame; set => frame = value;
        }

        public WindManager WindManager => windManager;

        public Node AlphaBackground => alphaBackground;

        public bool BlackSide => blackSide;

        public bool WhiteSide => whiteSide;

        public int Chapter => chapter;

        public EventSender BackEvent => backEvent;

        public EventSender RestartEvent => restartEvent;

        public EventSender NextLevelEvent => nextLevelEvent;

        public HeroBodyClip Hero => hero;

        public Vector2 HeroPositionVec => hero.Body.Position;

        public float FlyOpacity
        {
            get => flyOpacity;
            set
            {
                if (Maths.FuzzyNotEquals(flyOpacity, value, 0.0001f))
                {
                    flyOpacity = value;
                }
            }
        }

        public List<PlasticineBodyClip> Plasticine => plasticine;

        public Vector2 LightPoint
        {
            get => lightPoint; set => lightPoint = value;
        }

        public bool LightPowerChanged => lightPowerChanged;

        public LightColor LightColor => lightColor;

        public ParticleSystem Flyes => flyes;

        public ParticleSystem Dust => dust;

        public ParticleSystem Grass => grass;

        public ParticleSystem Energy => energy;

        public GroundFall GroundFall => groundFall;

        public EndLevelBodyClip EndLevel
        {
            get => endLevel; set => endLevel = value;
        }

        public bool TouchEnabled
        {
            get => touchEnabled; set => touchEnabled = value;
        }

        public int LevelIndex => levelIndex;

        public int LevelPosition => levelPosition;

        public IBonusAcceptable BonusTarget
        {
            get => bonusTarget ?? hero; set => bonusTarget = value;
        }

        public bool CanShowIntro { get; set; }

        public int StarsCollected => starsCollected;

        public bool SnotSend
        {
            get => snotSend; set => snotSend = value;
        }

        public ClickableLayer ClickableLayer => clickableLayer;

        public Color ButtonsColor => buttonsColor;

        public bool RestartEnabled
        {
            get => restartEnabled; set => restartEnabled = value;
        }

        public bool Finished
        {
            get => finished; set => finished = value;
        }

        public ContreJourGame(int _chapter)
        {
            freeDisabledTouches = new List<Touch>();
            data = UserData.Instance;
            chapter = _chapter;
            blackSide = chapter == 1;
            whiteSide = chapter == 3;
            touchEnabled = true;
            CGSize cgsize = ScreenConstants.W7FromIPhoneSize;
            Vector2 vector = CocosUtil.toIPhone(new Vector2(cgsize.Width, cgsize.Height));
            Vector2 vector2 = blackSide ? new Vector2(cgsize.Width / 2f, cgsize.Height * 2f) : vector;
            lightPoint = Box2DConfig.DefaultConfig.ToVec(vector2);
            lightPower = 1f;
            lightColor = ChooseSide(PlasticineConstants.BLUE, PlasticineConstants.BLACK_LIGHT, PlasticineConstants.LAST_LIGHT, PlasticineConstants.WHITE, PlasticineConstants.Green);
            startLightColor = lightColor;
            flyOpacity = 255f;
            Mokus2DGame.TouchController.AddListener(this, 0);
            draggingItems = new Dictionary<Touch, IClickable>();
            providersValue = 0f;
            positionProviders = new ArrayList();
            positionDependent = new ArrayList();
            windManager = new WindManager(whiteSide ? 0.02f : 0.03f);
            ClipFactory.Cache("McBackSnotEyeBlinkOneTime");
            ClipFactory.Cache("McBackSnotEyeBlink");
            alphaBackground = new Node();
            gameRoot.AddChild(alphaBackground, -10);
            freeTouches = new List<Touch>();
            backEvent = new EventSender();
            restartEvent = new EventSender();
            nextLevelEvent = new EventSender();
            clickableLayer = new ClickableLayer(0);
            AddChild(clickableLayer, 15);
            restartLayer = new LayerColor(ContreJourConstants.BLACK_COLOR);
            AddChild(restartLayer, 100);
            restartLayer.Visible = false;
            Color color = ColorUtil.Mult(ContreJourConstants.BLUE_LIGHT_COLOR, 2f);
            buttonsColor = blackSide ? color : ContreJourConstants.GREY_COLOR;
            int cornerOffset = CocosUtil.CornerOffset;
            pausePanel = new PausePanel(this);
            AddChild(pausePanel, 15);
            texturesToUnload = new List<string>();
            finishView = new FinishView(this);
            starsCollected = 0;
            touchFixPoint = CocosUtil.ScreenPosition(new Vector2(0f, 1f));
            teleports = new Hashtable();
            _ = updater.CallAfterSelectorDelay(new Action(EnableRestart), 1.5f);
            Mokus2DGame.KeysController.AddBackKeyListener(new Action(OnBackPress), 0);
        }

        public new ContreJourLevelBuilder Builder => (ContreJourLevelBuilder)base.Builder;

        public override ClipFactory CreateClipFactory()
        {
            return new ContreJourClipFactory();
        }

        public override LevelBuilderBase CreateLevelBuilder()
        {
            return new ContreJourLevelBuilder(this)
            {
                NamespacePrefix = "Default.Namespace."
            };
        }

        public void LoadLevelIndex(int index)
        {
            Maths.Randomize(levelIndex);
            levelIndex = index;
            pausePanel.SetLevelIndex(index);
            string text = string.Format(CocosUtil.iPad("level{0}", "level{0}iPhone"), index);
            LoadLevel(text);
            if (!Constants.IS_IPAD && Array.IndexOf(MIN_ZOOM_LEVELS, index) >= 0)
            {
                screenControl.ForceZoomOut();
            }
        }

        public override void LoadLevel(string levelName)
        {
            base.LoadLevel(levelName);
            Builder.PhysicsSpeed = 1.2f;
        }

        public override void ProcessLevel(Level level)
        {
            if (!blackSide && CocosUtil.isArmV7())
            {
                CreateFlyes();
                CreateGrass();
            }
            CreateEnergy();
            base.ProcessLevel(level);
            if (!Constants.IS_IPAD)
            {
                Hashtable levelProperties = level.levelProperties;
                levelSize = new Vector2(levelProperties.GetFloat("Width"), levelProperties.GetFloat("Height"));
                alphaBackground.Scale = levelSize.X / ScreenConstants.OsSizes.IPhoneRetina.X;
                alphaBackground.Y = -(ScreenConstants.OsSizes.IPhoneRetina.Y * alphaBackground.Scale - levelSize.Y) / 2f;
                screenControl = new ScreenControl(this);
                AddUpdatable(screenControl);
                RefreshScreenControlEnabled();
            }
            Builder.Add(energy, 9);
            CreateBackgrounds(level);
            if (CocosUtil.isArmV7())
            {
                CreateParticles();
                CreateDust();
                CreateGroundFall();
            }
            if (!blackSide && CocosUtil.isArmV7())
            {
                Builder.Add(flyes, 6);
                Builder.Add(grass, -1);
            }
        }

        private void CreateBackgrounds(Level level)
        {
            if (!HardwareCapabilities.IsLowMemoryDevice)
            {
                ArrayList arrayList = level.LevelProperties.GetArrayList("backgrounds");
                using (List<object>.Enumerator enumerator = arrayList.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        object obj = enumerator.Current;
                        Hashtable hashtable = (Hashtable)obj;
                        ProcessBackgroundItem(hashtable);
                    }
                    return;
                }
            }
            string texturePath = ClipFactory.GetTexturePath(string.Format("{0}Chapter16Bit", chapter + 1));
            Sprite sprite = new(Mokus2DGame.SharedContent.Load<Texture2D>(texturePath));
            sprite.Anchor = new Vector2(0f, 1f);
            Builder.AddAlphaBackground(sprite);
        }

        public void DisableScreenDrag(Touch touch)
        {
            if (screenControl.Touch == touch)
            {
                screenControl.TouchEnd(touch);
            }
        }

        public override void OnLoadLevelLevel(string levelName, Level level)
        {
            FarseerUtil.CreateLevelBordersSizeBorders(Builder.GroundBody, PhysicsLevelSize, new FarseerUtil.Borders(true, true, true, false));
            PlasticineConstants.ApplyStaticBodiesFilter(Builder.GroundBody);
            bool is_IPAD = Constants.IS_IPAD;
        }

        public bool RoseChapter => chapter == 4;

        public bool BonusChapter => chapter == 5;

        public int HeroIndex => Builder.Root.Children.IndexOf(hero.Clip);

        public Vector2 HeroPositionPixels => hero.Clip.Position;

        public float LightPower
        {
            get => lightPower;
            set
            {
                if (Maths.FuzzyNotEquals(lightPower, value, 0.0001f))
                {
                    lightPower = value;
                    lightPowerChanged = true;
                    RefreshLightColor();
                }
            }
        }

        public void OnGameActivated()
        {
            pausePanel.RefreshSoundButtons();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Mokus2DGame.TouchController.RemoveListener(this);
                Mokus2DGame.KeysController.RemoveBackKeyListener(new Action(OnBackPress));
                pausePanel.Dispose();
                finishView.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<string> TexturesToUnload()
        {
            return texturesToUnload;
        }

        public bool TouchBegin(Touch touch)
        {
            if (!touchEnabled || paused)
            {
                return false;
            }
            TouchPositionProvider touchPositionProvider = new(touch, Builder);
            PositionProviderValue positionProviderValue = new(touchPositionProvider, 5f);
            AddPositionProvider(positionProviderValue);
            if (screenControl != null)
            {
                screenControl.Touched = true;
            }
            if (!ProcessTouchIsFree(touch, false))
            {
                if (!Constants.IS_IPAD && screenControl != null && !screenControl.Dragging)
                {
                    screenControl.TouchBegan(touch);
                }
                freeTouches.Add(touch);
            }
            return true;
        }

        public bool TouchMove(Touch touch)
        {
            if (freeTouches.Contains(touch))
            {
                UpdateFreeTouch(touch);
            }
            if (screenControl.Touch == touch)
            {
                _ = screenControl.TouchMove(touch);
            }
            if (draggingItems.ContainsKey(touch))
            {
                IClickable clickable = draggingItems[touch];
                _ = clickable.TouchMove(touch);
            }
            return true;
        }

        public void TouchEnd(Touch touch)
        {
            if (freeTouches.Contains(touch))
            {
                _ = freeTouches.Remove(touch);
            }
            if (screenControl.Touch == touch)
            {
                screenControl.TouchEnd(touch);
            }
            _ = freeDisabledTouches.Remove(touch);
            screenControl.EndZoomTouch(touch);
            if (draggingItems.ContainsKey(touch))
            {
                IClickable clickable = draggingItems[touch];
                _ = draggingItems.Remove(touch);
                clickable.TouchEnd(touch);
            }
            foreach (object obj in positionProviders)
            {
                PositionProviderValue positionProviderValue = (PositionProviderValue)obj;
                if (positionProviderValue.Provider is TouchPositionProvider && (positionProviderValue.Provider as TouchPositionProvider).Touch == touch)
                {
                    RemovePositionProvider(positionProviderValue);
                    using (List<object>.Enumerator enumerator2 = positionDependent.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            object obj2 = enumerator2.Current;
                            IPositionDepedent positionDepedent = (IPositionDepedent)obj2;
                            positionDepedent.ProviderRemove(positionProviderValue.Provider);
                        }
                        break;
                    }
                }
            }
        }

        private void SetFinished(bool value)
        {
            finished = value;
            RestartEnabled = !finished;
        }

        public T Choose<T>(T normal = default(T), T blue = default(T), T white = default(T), T last = default(T), T green = default(T)) where T : class
        {
            if (BonusChapter)
            {
                T t = green;
                if (green == null)
                {
                    t = normal;
                }
                return t;
            }
            if (RoseChapter)
            {
                T t2 = last;
                if (last == null)
                {
                    t2 = normal;
                }
                return t2;
            }
            if (WhiteSide)
            {
                T t3 = white;
                if (white == null)
                {
                    t3 = normal;
                }
                return t3;
            }
            if (BlackSide)
            {
                T t4 = blue;
                if (blue == null)
                {
                    t4 = normal;
                }
                return t4;
            }
            return normal;
        }

        public T ChooseSide<T>(T black, T white, T last, T normal, T green)
        {
            return BonusChapter ? green : ChooseSide(black, white, last, normal);
        }

        public T ChooseSide<T>(T black, T white, T last, T normal)
        {
            return RoseChapter ? last : ChooseSide(black, white, normal);
        }

        public T ChooseSide<T>(T black, T white, T normal)
        {
            return blackSide ? black : whiteSide ? white : normal;
        }

        public void AddShadowSource(BodyClip source)
        {
        }

        public void AddColorOverlay()
        {
        }

        public int GetRandomColor()
        {
            return Maths.Rand() >= 0.3f ? 0 : 255;
        }

        public void CollectStar()
        {
            starsCollected++;
        }

        public void HardRestart()
        {
            if (hero != null)
            {
                hero.Removed = true;
            }
            restartEvent.SendEvent();
            DisableEvents();
        }

        public void SoftRestart()
        {
            totalTime = 0f;
            starsCollected = 0;
            foreach (Body body in Builder.World.BodyList)
            {
                IRestartable restartable = body.UserData as IRestartable;
                if (restartable != null)
                {
                    restartable.Restart();
                }
            }
            foreach (object obj in updatables)
            {
                if (obj is IRestartable)
                {
                    (obj as IRestartable).Restart();
                }
            }
        }

        public void SetRestartEnabled(bool value)
        {
            if (value != restartEnabled)
            {
                restartEnabled = value;
            }
        }

        public void Restart()
        {
            if (levelIndex is 0 or 66)
            {
                HardRestart();
                return;
            }
            if (restartEnabled)
            {
                RestartEnabled = false;
                restartLayer.Visible = true;
                restartLayer.Opacity = 0;
                NodeAction nodeAction = new EaseOut(new FadeTo(0.3f, 0.5882353f), 3f);
                NodeAction nodeAction2 = new EaseIn(new FadeTo(0.3f, 0f), 3f);
                Sequence sequence = new(
                [
                    nodeAction,
                    new Delay(0.1f),
                    nodeAction2,
                    new Hide()
                ]);
                restartLayer.Run(sequence);
                SoftRestart();
                _ = updater.CallAfterSelectorDelay(new Action(EnableRestart), 1.5f);
            }
        }

        private void EnableRestart()
        {
            if (!finished)
            {
                RestartEnabled = true;
            }
        }

        public void Back()
        {
            if (hero != null)
            {
                hero.Removed = true;
            }
            backEvent.SendEvent();
            DisableEvents();
        }

        public void Skip()
        {
            if (hero != null)
            {
                hero.Removed = true;
            }
            LevelPosition levelPosition = LevelsMenu.GetLevelPosition(levelIndex);
            data.SkipLevel(levelPosition);
            nextLevelEvent.SendEvent();
            DisableEvents();
        }

        public void DisableEvents()
        {
            restartEvent.Enabled = false;
            backEvent.Enabled = false;
            nextLevelEvent.Enabled = false;
        }

        public void AddTextureToUnload(string name)
        {
            if (!texturesToUnload.Contains(name))
            {
                texturesToUnload.Add(name);
            }
        }

        public void AddForeground(ForegroundBase foreground)
        {
            foregrounds.Add(foreground);
        }

        private void RefreshLightColor()
        {
            lightColor = startLightColor.Clone();
            lightColor.LightOutColor.R = (byte)(lightColor.LightOutColor.R * lightPower);
            lightColor.LightOutColor.G = (byte)(lightColor.LightOutColor.G * lightPower);
            lightColor.LightOutColor.B = (byte)(lightColor.LightOutColor.B * lightPower);
        }

        public void RegisterPlasticine(PlasticineBodyClip item)
        {
            plasticine.Add(item);
        }

        private void OnRestartClick(Button item)
        {
            Restart();
        }

        public TeleportBodyClip GetTeleport(string color)
        {
            return teleports.ContainsKey(color) ? (TeleportBodyClip)teleports.GetObject(color) : null;
        }

        public void RegisterTeleportColor(TeleportBodyClip teleport, string color)
        {
            teleports[color] = teleport;
        }

        public void OnBackPress()
        {
            if (finished || pausePanel.Visible)
            {
                Back();
                Mokus2DGame.KeysController.RemoveBackKeyListener(new Action(OnBackPress));
                return;
            }
            pausePanel.Show();
            Paused = true;
        }

        private void CreateFlyes()
        {
            if (CocosUtil.isArmV7())
            {
                flyes = new ParticleSystem("McFly.png");
                if (BonusChapter)
                {
                    flyes.Color = ContreJourConstants.GreenLightColor;
                }
            }
        }

        private void CreateGroundFall()
        {
            if (CocosUtil.isArmV7())
            {
                groundFall = new GroundFall(this);
                Builder.Add(groundFall, -1);
            }
        }

        public void ProcessBackgroundItem(Hashtable background)
        {
            string @string = background.GetString("type");
            texturesToUnload.Add(@string);
            Sprite sprite = ClipFactory.CreateWithAnchor(@string);
            Hashtable hashtable = background.GetHashtable("config");
            Vector2 vector = background.GetVector("position");
            Vector2 vector2 = background.GetVector("scale");
            sprite.Position = CocosUtil.toIPad(vector);
            float @float = background.GetFloat("initialScale", 1f);
            sprite.ScaleX = vector2.X / @float;
            sprite.ScaleY = vector2.Y / @float;
            sprite.Rotation = -background.GetFloat("rotation");
            if (chapter == 5)
            {
                sprite.Rotation = 0f;
                sprite.Scale = ScreenConstants.W7FromIPhoneSize.X / sprite.Size.X;
            }
            AddBackgroundColorConfig(sprite, hashtable);
            if (hashtable.Exists("foreground"))
            {
                Builder.AddForeground(sprite);
            }
            else if (blackSide)
            {
                int @int = hashtable.GetInt("z", -10);
                Builder.AddAlphaBackgroundZ(sprite, @int);
            }
            else
            {
                Builder.AddAlphaBackground(sprite);
            }
            if (hashtable.Exists("type"))
            {
                string text = "Default.Namespace." + hashtable.GetString("type");
                BackgroundBase backgroundBase = (BackgroundBase)ReflectUtil.CreateInstance(text, [sprite, hashtable, this]);
                backgrounds.Add(backgroundBase);
            }
        }

        public void AddBackgroundColorConfig(Node background, Hashtable config)
        {
            if (config.ContainsKey("color"))
            {
                string @string = config.GetString("color");
                background.Color = @string.ToColor();
            }
        }

        public void CreateGrass()
        {
            string text = ChooseSide(null, "McWhiteGrass.png", "McGrass_5.png", "McTotalGrass.png", "McGrass_6.png");
            grass = new ParticleSystem(text);
        }

        public void CreateDust()
        {
            dust = new ParticleSystem(whiteSide ? "McDustWhite.png" : "McDust.png");
            Builder.Add(dust, 1);
        }

        public void CreateEnergy()
        {
            energy = new ParticleSystem("McEnergyBall.png");
        }

        public void IncreaseZoomOut()
        {
            if (zoomOutCount == 0)
            {
                zoomOutTime = 0f;
            }
            zoomOutCount++;
        }

        public new void DecreaseZoomOut()
        {
            zoomOutCount--;
            if (zoomOutCount == 0)
            {
                zoomOutTime = 0f;
            }
        }

        public void IncreaseScreenDragDisable()
        {
            screenDragDisableCount++;
            RefreshScreenControlEnabled();
        }

        private void RefreshScreenControlEnabled()
        {
            if (screenControl != null)
            {
                screenControl.Enabled = screenDragDisableCount == 0;
            }
        }

        public void DecreaseScreenDragDisable()
        {
            screenDragDisableCount--;
            RefreshScreenControlEnabled();
        }

        public void CreateParticles()
        {
            if (BonusChapter)
            {
                return;
            }
            if (whiteSide)
            {
                particles = new WhiteSnow();
                Builder.Add(particles, -1);
                particles.CreateBetweenBounds(40);
                return;
            }
            if (chapter == 2)
            {
                particles = new SnowFall();
                Builder.Add(particles, 11);
                particles.CreateBetweenBounds(40);
                return;
            }
            if (blackSide)
            {
                particles = new BlueLights();
                Builder.AddAlphaBackgroundZ(particles, -9);
                particles.CreateBetweenBounds(20);
                return;
            }
            if (RoseChapter)
            {
                particles = new LastParticles();
                Builder.Add(particles, 11);
                particles.CreateBetweenBounds(40);
                return;
            }
            particles = new BlackFall();
            Builder.Add(particles, -2);
            particles.CreateBetweenBounds(40);
        }

        public void HideBonuses()
        {
            endLevel.SetVisible(false);
            energy.Visible = false;
            energy.Opacity = 0;
        }

        public void FocusOnHero()
        {
            screenControl.FocusOnHero();
        }

        public void ShowBonuses()
        {
            endLevel.SetVisible(true);
            endLevel.ShowPortal();
            energy.Visible = true;
            energy.Run(new FadeIn(2f));
        }

        public void Fail(float restartTime)
        {
            if (Maths.FuzzyEquals(restartTime, 0f, 0.0001f))
            {
                Restart();
                return;
            }
            _ = Updater.CallAfterSelectorDelay(new Action(Restart), restartTime);
        }

        public void Finish(Vector2 zoomPoint)
        {
            RestartEnabled = false;
            LevelPosition levelPosition = LevelsMenu.GetLevelPosition(levelIndex);
            LevelData levelDataByPosition = data.GetLevelDataByPosition(levelPosition);
            int num = data.CompleteLevel(levelPosition, starsCollected, totalTime);
            bool flag = levelDataByPosition != null && (levelDataByPosition.Score < num || levelDataByPosition.StarsCount < starsCollected);
            finishView.ShowWithLevelStarsScoreTimeNewHighScore(levelPosition, starsCollected, num, totalTime, flag);
            finishView.NextLevelEvent.AddListenerSelector(new Action(nextLevelEvent.SendEvent));
            FinishWithViewPosition(finishView, zoomPoint);
        }

        public void FinishWithViewPosition(MovieStripesView view, Vector2 zoomPoint)
        {
            finished = true;
            if (view != null)
            {
                AddChild(view, 16);
                view.RestartEvent.AddListenerSelector(new Action(HardRestart));
                view.MenuEvent.AddListenerSelector(new Action(backEvent.SendEvent));
            }
            CGSize cgsize = ScreenConstants.W7FromIPhoneSize;
            Vector2 vector = new(0f, 30f);
            Vector2 vector2 = new(cgsize.Width * -0.20000005f, cgsize.Height * -0.20000005f - 30f);
            ZoomToScaleRightTopLeftBottomTime(zoomPoint * gameRoot.Scale, 1.2f, vector, vector2, 2.4f);
        }

        public void ZoomOut(float time)
        {
            ZoomToScaleTime(new Vector2(0f, 0f), 1f, time);
        }

        public void ZoomToScaleTime(Vector2 zoomPoint, float scale, float time)
        {
            CGSize cgsize = ScreenConstants.W7FromIPhoneSize;
            ZoomToScaleRightTopLeftBottomTime(zoomPoint, scale, new Vector2(0f, 0f), new Vector2(cgsize.Width * (1f - scale), cgsize.Height * (1f - scale)), time);
        }

        public void ZoomToScaleRightTopLeftBottomTime(Vector2 zoomPoint, float scale, Vector2 rightTop, Vector2 leftBottom, float time)
        {
            CGSize cgsize = ScreenConstants.W7FromIPhoneSize;
            Vector2 vector = new(cgsize.Width / 2f - zoomPoint.X * scale, cgsize.Height / 2f - zoomPoint.Y * scale);
            Vector2 vector2 = vector;
            vector2.X = vector2.X.Clamp(leftBottom.X, rightTop.X);
            vector2.Y = vector2.Y.Clamp(leftBottom.Y, rightTop.Y);
            MoveTo moveTo = new(time, vector2);
            ScaleTo scaleTo = new(time, scale * gameRoot.Scale);
            gameRoot.Run(new Spawn(
            [
                new EaseInOut(moveTo, 3f),
                new EaseInOut(scaleTo, 3f)
            ]));
        }

        public void RegisterHero(HeroBodyClip _hero)
        {
            hero = _hero;
            AddPositionProvider(new PositionProviderValue(_hero, 5f));
        }

        private void RemovePositionProvider(PositionProviderValue provider)
        {
            _ = positionProviders.Remove(provider);
            providersValue -= provider.Value;
        }

        public void AddPositionDependent(IPositionDepedent dependent)
        {
            positionDependent.Add(dependent);
        }

        public TouchPositionProvider GetTouchProvider(Touch touch)
        {
            foreach (object obj in positionProviders)
            {
                PositionProviderValue positionProviderValue = (PositionProviderValue)obj;
                TouchPositionProvider touchPositionProvider = positionProviderValue.Provider as TouchPositionProvider;
                if (touchPositionProvider != null && touchPositionProvider.Touch == touch)
                {
                    return touchPositionProvider;
                }
            }
            return null;
        }

        public void AddPositionProvider(PositionProviderValue provider)
        {
            providersValue += provider.Value;
            positionProviders.Add(provider);
            foreach (object obj in positionDependent)
            {
                IPositionDepedent positionDepedent = (IPositionDepedent)obj;
                positionDepedent.ProviderAdded(provider.Provider);
            }
        }

        public IVectorPositionProvider GetRandomPositionProvider()
        {
            if (positionProviders.Count == 0)
            {
                return null;
            }
            float num = Maths.RandRangeMinMax(0f, providersValue);
            float num2 = 0f;
            int num3 = 0;
            while (num > num2)
            {
                PositionProviderValue positionProviderValue = (PositionProviderValue)positionProviders[num3];
                num2 += positionProviderValue.Value;
                num3++;
            }
            return ((PositionProviderValue)positionProviders[num3 - 1]).Provider;
        }

        private void UpdateZoomOut(float time)
        {
            if (finished || screenControl == null)
            {
                return;
            }
            zoomOutTime += time;
            if (zoomOutTime > 0.3f)
            {
                if (!screenControl.ZoomOut && zoomOutCount > 0)
                {
                    screenControl.ZoomOut = true;
                    return;
                }
                if (screenControl.ZoomOut && zoomOutCount <= 0)
                {
                    screenControl.ZoomOut = false;
                }
            }
        }

        public override void UpdateGame(float time)
        {
            if (!Constants.IS_IPAD)
            {
                UpdateZoomOut(time);
            }
            frame++;
            base.UpdateGame(time);
            foreach (PlasticineBodyClip plasticineBodyClip in plasticine)
            {
                plasticineBodyClip.UpdateGraphics(time);
            }
            if (!debug && CocosUtil.isArmV7() && grass != null)
            {
                grass.Update(time);
            }
            lightPowerChanged = false;
            ArrayList arrayList = new();
            foreach (Touch touch in freeTouches)
            {
                if (ProcessTouchIsFree(touch, true))
                {
                    arrayList.Add(touch);
                }
            }
            foreach (object obj in arrayList)
            {
                _ = freeTouches.Remove((Touch)obj);
            }
            foreach (IUpdatable updatable in backgrounds)
            {
                updatable.Update(time);
            }
            for (int i = 0; i < foregrounds.Count; i++)
            {
                foregrounds[i].Update(time);
            }
            windManager.Update(time);
        }

        public void RenewGround()
        {
            foreach (PlasticineBodyClip plasticineBodyClip in plasticine)
            {
                plasticineBodyClip.Restart();
            }
        }

        public void UpdateFreeTouch(Touch touch)
        {
            if (ProcessTouchIsFree(touch, true))
            {
                _ = freeTouches.Remove(touch);
            }
        }

        public void AddView(MovieStripesView view)
        {
            AddChild(view, 16);
        }

        public bool ProcessTouchIsFree(Touch touch, bool isFree)
        {
            Vector2 vector = Builder.TouchRootVec(touch);
            float num = CocosUtil.iPad(1.1666666f, 1.5f);
            AABB aabb = FarseerUtil.CreateAABBCenterWidthHeight(vector, num * 2f, num * 2f);
            AABBQuery aabbquery = new();
            Builder.World.QueryAABB(new Func<Fixture, bool>(aabbquery.CallbackReportFixture), ref aabb);
            List<BodyClip> list = new();
            for (int i = 0; i < aabbquery.Fixtures.Count; i++)
            {
                BodyClip bodyClip = aabbquery.Fixtures[i].Body.UserData as BodyClip;
                if (bodyClip != null)
                {
                    IClickable clickable = bodyClip as IClickable;
                    if (clickable != null && (!isFree || clickable.AcceptFreeTouches()))
                    {
                        list.Add(bodyClip);
                    }
                }
            }
            IClickable clickable2 = null;
            bool flag = false;
            if (list.Count != 0)
            {
                ClickableComparer clickableComparer = new(vector);
                list.Sort(clickableComparer);
                foreach (BodyClip bodyClip2 in list)
                {
                    IClickable clickable3 = (IClickable)bodyClip2;
                    if (clickable3.TouchBegan(touch))
                    {
                        draggingItems[touch] = clickable3;
                        flag = true;
                        clickable2 = clickable3;
                        break;
                    }
                }
            }
            if (flag && !clickable2.UseForZoom())
            {
                screenControl.EndZoomTouch(touch);
            }
            else
            {
                screenControl.StartZoomTouch(touch);
            }
            if (!isFree && (clickable2 == null || !clickable2.DisableHeroFocus))
            {
                screenControl.Touched = true;
            }
            return flag;
        }

        public bool IsFreeEnabled(Touch touch)
        {
            return !freeDisabledTouches.Contains(touch);
        }

        public void DisableFreeing(Touch touch)
        {
            freeDisabledTouches.Add(touch);
        }

        public void FreeTouch(Touch touch)
        {
            _ = draggingItems.Remove(touch);
            if (!freeTouches.Contains(touch))
            {
                freeTouches.Add(touch);
            }
        }

        public const float RESTART_TIME = 1.5f;

        public const float WIND_STEP_WHITE = 0.02f;

        public const float WIND_STEP = 0.03f;

        private const float STRIPES_HEIGHT = 30f;

        public const float ZOOM_SCALE = 1.2f;

        public const float CLICK_RADIUS_IPHONE = 1.5f;

        public const float CLICK_RADIUS = 1.1666666f;

        public readonly List<SnotPoint> SnotPoints = new(64);

        private readonly PausePanel pausePanel;

        protected Node alphaBackground;

        protected EventSender backEvent;

        protected List<BackgroundBase> backgrounds = new();

        protected bool blackSide;

        protected IBonusAcceptable bonusTarget;

        protected Color buttonsColor;

        protected int chapter;

        protected ClickableLayer clickableLayer;

        protected UserData data;

        protected Dictionary<Touch, IClickable> draggingItems;

        protected ParticleSystem dust;

        protected EndLevelBodyClip endLevel;

        protected ParticleSystem energy;

        protected FinishView finishView;

        protected bool finished;

        protected float flyOpacity;

        protected ParticleSystem flyes;

        protected List<ForegroundBase> foregrounds = new();

        protected int frame;

        protected List<Touch> freeDisabledTouches;

        protected List<Touch> freeTouches;

        protected ParticleSystem grass;

        protected GroundFall groundFall;

        private HeroBodyClip hero;

        protected int levelIndex;

        protected int levelPosition;

        protected LightColor lightColor;

        protected Vector2 lightPoint;

        protected float lightPower;

        protected bool lightPowerChanged;

        protected EventSender nextLevelEvent;

        protected GravityParticleSystem particles;

        protected List<PlasticineBodyClip> plasticine = new(8);

        protected ArrayList positionDependent;

        protected ArrayList positionProviders;

        protected float providersValue;

        protected bool restartEnabled;

        protected EventSender restartEvent;

        protected LayerColor restartLayer;

        protected ScreenControl screenControl;

        protected bool snotSend;

        protected int starsCollected;

        protected LightColor startLightColor;

        protected Hashtable teleports;

        protected List<string> texturesToUnload;

        protected bool touchEnabled;

        protected Vector2 touchFixPoint;

        protected bool whiteSide;

        protected WindManager windManager;

        protected int zoomOutCount;

        protected float zoomOutTime;

        public static readonly int[] MIN_ZOOM_LEVELS =
        [
            51, 53, 52, 54, 49, 37, 74, 79, 80, 76,
            55, 77, 86, 87, 83, 94, 84, 91, 95, 85,
            92, 93, 89
        ];

        public static readonly int[] LOW_FPS_LEVELS = [4, 6, 44, 53, 54, 12];

        private int screenDragDisableCount;

        public class ClickableComparer(Vector2 sourcePoint) : IComparer<BodyClip>
        {
            public int Compare(BodyClip clip1, BodyClip clip2)
            {
                IClickable clickable = (IClickable)clip1;
                IClickable clickable2 = (IClickable)clip2;
                if (clickable2.Priority(sourcePoint) > clickable.Priority(sourcePoint))
                {
                    return 1;
                }
                if (clickable2.Priority(sourcePoint) < clickable.Priority(sourcePoint))
                {
                    return -1;
                }
                float num = clickable.TouchDistance(sourcePoint);
                float num2 = clickable2.TouchDistance(sourcePoint);
                return Maths.FuzzyEquals(num, num2, 0.0001f) ? 0 : num >= num2 ? 1 : -1;
            }
        }
    }
}
