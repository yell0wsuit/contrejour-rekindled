using System;

using ContreJourMono.ContreJour.Menu.LevelComplete;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class Splash : Node, ITouchListener, IDisposable
    {
        public Splash()
        {
            Mokus2DGame.KeysController.AddBackKeyListener(new Action(OnBackClick), 0);
            background = new LayerColor(Color.White);
            AddChild(background);
            center = ScreenConstants.W7FromIPhoneScreenCenter;
            if (Constants.IS_IPAD)
            {
                originalLogo = new Sprite("Default-LandscapeRight.png");
            }
            else if (Constants.IS_RETINA)
            {
                originalLogo = new Sprite(ClipFactory.CreateWithoutConfig("Default@2x"));
                originalLogo.Rotation = -90f;
            }
            else
            {
                originalLogo = new Sprite("Default.png");
                originalLogo.Rotation = -90f;
            }
            originalLogo.Position = center;
            AddChild(originalLogo);
            title = ClipFactory.CreateWithAnchor("McChillingo");
            title.Visible = false;
            AddChild(title);
            if (!Constants.IS_IPAD)
            {
                title.Scale = 0.9375f;
                title.Position = new Vector2(W7IPhoneWidthDiff / 2f, -40f);
            }
            Mokus2DGame.TouchController.AddListener(this, 0);
            Schedule(new Action(StartLogo), 1f);
            Schedule(new Action(PlaySplashSound), 0.6f);
        }

        private void PlaySplashSound()
        {
            UserData instance = UserData.Instance;
            instance.RefreshSoundManager();
            if (Mokus2DGame.SoundManager.SoundEnabled && Mokus2DGame.SoundManager.HasControl)
            {
                Mokus2DGame.SoundManager.MusicEnabled = true;
                Mokus2DGame.SoundManager.PlayMusic("intro-5");
            }
        }

        private void OnBackClick()
        {
            StopSplashSound();
            Mokus2DGame.Instance.Exit();
        }

        private void StartLogo()
        {
            blackHeroPosition = (Constants.IS_IPAD ? BLACK_HERO_POSITION : BLACK_HERO_POSITION_IPHONE);
            logo = (MovieClip)ClipFactory.CreateWithAnchor("McChillingoLogo");
            AddChild(logo);
            logo.Visible = false;
            logo.Position = LOGO_POSITION;
            if (!Constants.IS_IPAD)
            {
                logo.Scale = 0.9375f;
                logo.Position = LOGO_POSITION_IPHONE;
            }
            Schedule(new Action(Play), 0.5f);
            hero = new FakeHero();
            if (!Constants.IS_IPAD)
            {
                hero.Scale = 0.9375f;
            }
            hero.Opacity = 0;
            AddChild(hero);
            if (Constants.IS_IPAD)
            {
                hero.Position = HERO_POSITION;
            }
            else
            {
                hero.Position = HERO_POSITION_IPHONE;
            }
            hero.Visible = false;
        }

        private void RemoveListeners()
        {
            if (logo != null)
            {
                logo.EndEvent -= new Action(ShowHero);
            }
            Mokus2DGame.KeysController.RemoveBackKeyListener(new Action(OnBackClick));
            Mokus2DGame.TouchController.RemoveListener(this);
        }

        public bool TouchBegin(Touch touch)
        {
            SendEnd();
            return false;
        }

        public bool TouchMove(Touch touch)
        {
            return false;
        }

        public void TouchEnd(Touch touch)
        {
        }

        private void Play()
        {
            title.Visible = true;
            logo.Visible = true;
            logo.Repeat = false;
            RemoveChild(originalLogo);
            logo.EndEvent += new Action(ShowHero);
        }

        private void ShowHero()
        {
            hero.Visible = true;
            hero.Run(new FadeIn(0.5f));
            Schedule(new Action(EndJump), 0.5f);
            hero.Eye.Open();
        }

        private void EndJump()
        {
            logo.Visible = false;
            hero.SetViewAngle(0.7853982f, 1f);
            Schedule(new Action(LookRight), 0.3f);
        }

        private void LookRight()
        {
            hero.SetViewAngle(2.3561945f, 1f);
            Schedule(new Action(StartMove), 0.3f);
        }

        private void StartMove()
        {
            Vector2 vector = new(ScreenConstants.W7FromIPhoneSize.X, 0f);
            EaseIn easeIn = new(new MoveTo(2f, hero.Position + vector), 3f);
            EaseIn easeIn2 = new(new MoveTo(2f, title.Position + vector), 3f);
            hero.Run(easeIn);
            hero.SetViewAngle(0f, 0f);
            hero.SetMoveAngle(0f, 1f);
            title.Run(easeIn2);
            EaseIn easeIn3 = new(new RotateToDegrees(2f, -1080f), 3f);
            hero.Background.Run(easeIn3);
            Schedule(new Action(RefreshSpeed), 0.6f);
            Schedule(new Action(SlowLookRight), 0.1f);
            Schedule(new Action(ShowMokus), 2f);
        }

        private void SlowLookRight()
        {
            hero.Eye.EyeStep = 0.4f;
            hero.SetViewAngle(0f, 1f);
        }

        private void RefreshSpeed()
        {
            hero.Speed = 6f;
        }

        private void ShowMokus()
        {
            RemoveChild(logo);
            RemoveChild(title);
            hero.Visible = false;
            mokusLogo = ClipFactory.CreateWithAnchor("McMokusLogo");
            AddChild(mokusLogo);
            mokusLogo.Position = center;
            background.Run(new FadeColor(1f, new Color(0, 0, 0)));
            Schedule(new Action(MoveMokus), 1f);
        }

        private void MoveMokus()
        {
            blackHero = new FakeHeroBlack();
            AddChild(blackHero);
            blackHero.Position = blackHeroPosition + new Vector2(ScreenConstants.W7FromIPhoneSize.X / 2.5f, 0f);
            blackHero.Scale = 0.7f;
            EaseOut easeOut = new(new MoveTo(1f, blackHeroPosition), 3f);
            EaseOut easeOut2 = new(new RotateToDegrees(1f, 540f), 3f);
            blackHero.Run(easeOut);
            blackHero.SetMoveAngle(-3.1415927f, CocosUtil.r(3f));
            blackHero.SetViewAngle(-3.1415927f, 1f);
            blackHero.Background.Run(easeOut2);
            Schedule(new Action(MoveMokusOut), 1f);
        }

        private void MoveMokusOut()
        {
            Vector2 vector = new(ScreenConstants.W7FromIPhoneSize.X * 0.8f, 0f);
            NodeAction nodeAction = new EaseIn(new MoveTo(1f, blackHeroPosition + vector), 3f);
            NodeAction nodeAction2 = new EaseIn(new RotateToDegrees(1f, -540f), 3f);
            blackHero.Run(nodeAction);
            blackHero.SetMoveAngle(0f, CocosUtil.r(3f));
            blackHero.SetViewAngle(0f, 1f);
            blackHero.Eye.EyeStep = 0.2f;
            blackHero.Background.Run(nodeAction2);
            NodeAction nodeAction3 = new EaseIn(new MoveTo(1f, center + vector), 3f);
            mokusLogo.Run(nodeAction3);
            Schedule(new Action(SendEnd), 1.5f);
        }

        private static void StopSplashSound()
        {
            Mokus2DGame.SoundManager.StopMusic();
            UserData.Instance.RefreshSoundManager();
        }

        public void SendEnd()
        {
            if (!ended)
            {
                StopSplashSound();
                RemoveListeners();
                StopAllActions();
                HideAll();
                ended = true;
            }
        }

        private void HideAll()
        {
            LayerColor layerColor = new(Color.Black);
            AddChild(layerColor, 10);
            layerColor.Opacity = 0;
            layerColor.Run(new Sequence(
            [
                new FadeIn(0.3f),
                new InstantAction(new Action(ShowHeadphones))
            ]));
        }

        private void ShowHeadphones()
        {
            RenderSprite renderSprite = new(ScreenConstants.W7FromIPhoneSize + new Vector2(10f));
            LayerColor layerColor = new(Color.Black);
            renderSprite.AddChild(layerColor);
            Sprite sprite = ClipFactory.CreateWithAnchor("McHeadphones");
            sprite.Position = ScreenConstants.W7FromIPhoneScreenCenter;
            sprite.IgnoreParentColor = true;
            layerColor.AddChild(sprite);
            MultilineLabel multilineLabel = ContreJourLabel.CreateMultilineLabel(CocosUtil.iPad(34, 22), "USE_HEADPHONES");
            multilineLabel.Color = ContreJourConstants.GREY_COLOR;
            sprite.AddChild(multilineLabel);
            multilineLabel.Position = CocosUtil.ccpIPad(-100f, -160f);
            renderSprite.Opacity = 0;
            AddChild(renderSprite, 10);
            renderSprite.Anchor = new Vector2(0f, 1f);
            renderSprite.RedrawTexture();
            renderSprite.Run(new Sequence(
            [
                new FadeIn(0.5f),
                new Delay(2f),
                new InstantAction(delegate
                {
                    EndEvent.SendEvent();
                })
            ]));
        }

        public new void Dispose()
        {
        }

        private const float JUMP_DURATION = 0.5f;

        public readonly EventSender EndEvent = new();

        protected Vector2 blackHeroPosition;

        protected LayerColor background;

        protected Sprite title;

        protected MovieClip logo;

        protected Sprite originalLogo;

        protected FakeHero hero;

        protected Vector2 center;

        protected Sprite mokusLogo;

        protected FakeHeroBlack blackHero;

        private bool ended;

        private static readonly float W7IPhoneWidthDiff = ScreenConstants.W7FromIPhoneSize.X - ScreenConstants.OsSizes.IPhoneRetina.X;

        private static readonly Vector2 JUMP_OFFSET = new(0f, 100f);

        private static readonly Vector2 BLACK_HERO_POSITION_IPHONE = CocosUtil.Vector2Retina(360f + W7IPhoneWidthDiff / 4f, 153f);

        private static readonly Vector2 BLACK_HERO_POSITION = new(730f, 370f);

        private static readonly Vector2 HERO_POSITION_IPHONE = CocosUtil.Vector2Retina(113f + W7IPhoneWidthDiff / 4f, 159f);

        private static readonly Vector2 HERO_POSITION = new(241.3f, 382.9f);

        private static readonly Vector2 LOGO_POSITION_IPHONE = CocosUtil.Vector2Retina(112.6f + W7IPhoneWidthDiff / 4f, 154.1f);

        private static readonly Vector2 LOGO_POSITION = new(241.55f, 372.25f);
    }
}
