using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class PausePanel : Node, IDisposable
    {
        public PausePanel(ContreJourGame _game)
        {
            game = _game;
            data = UserData.Instance;
            Mokus2DGame.SoundManager.MusicDisableEvent.AddListenerSelector(new Action(OnMusicDisable));
            Color color = (game.BlackSide ? ColorUtil.Mult(ContreJourConstants.BLUE_LIGHT_COLOR, 1.5f) : ContreJourConstants.GREY_COLOR);
            if (game.BonusChapter)
            {
                color = ContreJourConstants.GreenLightColor;
            }
            winSize = ScreenConstants.W7FromIPhoneSize;
            Position = new Vector2(winSize.Width, 0f);
            AddChild(backgroundLayer);
            backgroundLayer.Opacity = 0;
            Sprite sprite = ClipFactory.CreateWithAnchor("McRightPanelBackground");
            AddChild(sprite);
            sprite.Scale = (winSize.Height + 10f) / sprite.Size.Y;
            sprite.Position = new Vector2(0f, -5f);
            if (!Constants.IS_IPAD)
            {
                sprite.Scale *= 1.4f;
                sprite.Position = new Vector2(0f, -sprite.Size.Y * 0.2f);
            }
            sprite.Color = color;
            clickableLayer = new ClickableLayer(0);
            AddChild(clickableLayer);
            float num = CocosUtil.iPad(1f, 1.2f);
            Button button = Button.ButtonBigWithIcon("McPlayIcon");
            clickableLayer.AddChild(button);
            button.ClickEvent.AddListenerSelector(new Action<TouchSprite>(OnPlayClick));
            button.Position = new Vector2(CocosUtil.Wp7Retina(CocosUtil.iPad(-255f, -153f)), winSize.Height / 2f);
            button.Color = color;
            button.RealScale = num;
            restartButton = Button.ButtonBigWithIcon("McRestartIcon");
            clickableLayer.AddChild(restartButton);
            restartButton.ClickEvent.AddListenerSelector(new Action<TouchSprite>(OnRestartClick));
            float num2 = CocosUtil.Wp7Retina(CocosUtil.iPad(-130f, -78f));
            restartButton.Position = new Vector2(num2, winSize.Height / 2f + CocosUtil.Wp7Retina(CocosUtil.iPad(100, 60)));
            restartButton.Color = color;
            restartButton.RealScale = num;
            if (Constants.IS_IPAD)
            {
                restartButton.Visible = false;
            }
            Button button2 = Button.ButtonBigWithIcon("McMenuIcon");
            clickableLayer.AddChild(button2);
            button2.ClickEvent.AddListenerSelector(new Action(OnMenuClick));
            button2.Position = new Vector2(num2, winSize.Height / 2f + CocosUtil.Wp7Retina(CocosUtil.iPad(50, 0)));
            button2.Color = color;
            button2.RealScale = num;
            Button button3 = Button.ButtonBigWithIcon("McSkipIcon");
            clickableLayer.AddChild(button3);
            button3.ClickEvent.AddListenerSelector(new Action(OnSkipClick));
            button3.Position = new Vector2(num2, winSize.Height / 2f - CocosUtil.Wp7Retina(CocosUtil.iPad(50, 60)));
            button3.Color = color;
            button3.RealScale = num;
            soundButton = new ToggleButton("McSoundIcon", "McDisabledIcon");
            clickableLayer.AddChild(soundButton);
            soundButton.Position = new Vector2(num2 + CocosUtil.Wp7Retina(CocosUtil.iPad(-40, -30)), winSize.Height / 2f - CocosUtil.Wp7Retina(CocosUtil.iPad(200, -120)));
            soundButton.Color = color;
            soundButton.RealScale = CocosUtil.iPad(1f, 1.4f);
            soundButton.ToggleIcon.IgnoreParentColor = true;
            musicButton = new ToggleButton("McMusicIcon", "McDisabledIcon");
            clickableLayer.AddChild(musicButton);
            musicButton.Position = new Vector2(num2 + CocosUtil.Wp7Retina(CocosUtil.iPad(40, 30)), winSize.Height / 2f - CocosUtil.Wp7Retina(CocosUtil.iPad(200, -120)));
            musicButton.Color = color;
            musicButton.ToggleIcon.IgnoreParentColor = true;
            musicButton.RealScale = CocosUtil.iPad(1f, 1.4f);
            soundButton.Visible = false;
            musicButton.Visible = false;
            RefreshSoundButtons();
            soundButton.ClickEvent.AddListenerSelector(new Action(OnSoundClick));
            musicButton.ClickEvent.AddListenerSelector(new Action(OnMusicClick));
            buttons = new List<Button>([button, restartButton, button2, button3]);
            if (Constants.IS_IPAD)
            {
                buttons.Remove(restartButton);
            }
            scoreLabel = ContreJourLabel.CreateLabel(CocosUtil.iPad(25, 15), true);
            scoreLabel.Position = CocosUtil.ccpIPad(CocosUtil.iPad(-130, -160), CocosUtil.iPad(40, 30));
            AddChild(scoreLabel);
            scoreLabel.Color = ColorUtil.Mult(color, 0.5f);
            scoreLabel.Opacity = 0;
            scoreLabel.Visible = false;
            levelLabel = ContreJourLabel.CreateLabel(CocosUtil.iPad(25, 15), true);
            levelLabel.Position = CocosUtil.ccpIPad(CocosUtil.iPad(-130, -160), CocosUtil.iPad(80, 70));
            AddChild(levelLabel);
            levelLabel.Color = scoreLabel.Color;
            levelLabel.Opacity = 0;
            levelLabel.Visible = false;
            Visible = false;
            Position = new Vector2(winSize.Width + 300f, 0f);
            SetButtonsVisible(false);
        }

        private void OnMusicDisable()
        {
            musicButton.Toggle = true;
        }

        public new void Dispose()
        {
            Mokus2DGame.SoundManager.MusicDisableEvent.RemoveListenerSelector(new Action(OnMusicDisable));
        }

        public void SetLevelIndex(int levelIndex)
        {
            if (levelIndex == 169)
            {
                levelLabel.Visible = false;
                scoreLabel.Visible = false;
                if (buttons.Count > 0)
                {
                    buttons.RemoveAt(buttons.Count - 1);
                }
                return;
            }
            LevelData levelDataByFile = data.GetLevelDataByFile(levelIndex);
            LevelPosition levelPosition = data.GetLevelPosition(levelIndex);
            if (levelPosition.Chapter == Constants.NormalChaptersCount - 1 && levelPosition.Index == 19 && data.GetLevelDataByPosition(levelPosition) == null)
            {
                if (buttons.Count > 0)
                {
                    buttons.RemoveAt(buttons.Count - 1);
                }
                restartButton.Visible = false;
            }
            if (!levelPosition.SkipAvailable)
            {
                if (buttons.Count > 0)
                {
                    buttons.RemoveAt(buttons.Count - 1);
                }
                restartButton.Visible = false;
            }
            string text = string.Format(Messages.LEVEL, levelPosition.Chapter + 1, levelPosition.Index + 1, null);
            levelLabel.TextString = text;
            scoreLabel.AppendFormat(Messages.BEST_SCORE, [(levelDataByFile == null) ? 0 : levelDataByFile.Score]);
        }

        public void Show()
        {
            if (open)
            {
                return;
            }
            open = true;
            Visible = true;
            backgroundLayer.StopAllActions();
            backgroundLayer.OpacityFloat = 0f;
            backgroundLayer.Run(new FadeTo(0.35f, 0.35f));
            MoveTo moveTo = new(0.35f, new Vector2(winSize.Width + 10f, 0f));
            Run(new EaseOut(moveTo, 3f));
            buttonIndex = 0;
            Schedule(new Action(ProcessNextButton), 0.15f);
            scoreLabel.Visible = true;
            scoreLabel.Run(new FadeIn(0.35f));
            levelLabel.Visible = true;
            levelLabel.Run(new FadeIn(0.35f));
            soundButton.Visible = false;
            musicButton.Visible = false;
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

        private void ProcessNextButton()
        {
            ShowButton(buttons[buttonIndex]);
            buttonIndex++;
            if (buttonIndex < buttons.Count)
            {
                Schedule(new Action(ProcessNextButton), 0.1f);
                return;
            }
            Schedule(new Action(ShowMusic), 0.1f);
        }

        private void ShowMusic()
        {
            ShowButton(musicButton);
            ShowButton(soundButton);
        }

        public void SetButtonsVisible(bool value)
        {
            foreach (Button button in buttons)
            {
                button.Visible = value;
            }
        }

        public void ShowButton(Button button)
        {
            button.Scale = 0f;
            button.Visible = true;
            ScaleTo scaleTo = new(0.6f, button.RealScale);
            FadeIn fadeIn = new(0.3f);
            button.Run(new Spawn(
            [
                fadeIn,
                new EaseElasticOut(scaleTo, 0.3f)
            ]));
        }

        public void Hide()
        {
            if (!open)
            {
                return;
            }
            open = false;
            backgroundLayer.Run(new FadeTo(0.35f, 0f));
            MoveTo moveTo = new(0.5f, new Vector2(winSize.Width + 300f, 0f));
            InstantAction instantAction = new(new Action(OnHide));
            Run(new Sequence(
            [
                new EaseIn(moveTo, 3f),
                instantAction
            ]));
        }

        private void OnHide()
        {
            game.Paused = false;
            Visible = false;
            SetButtonsVisible(false);
            scoreLabel.Visible = false;
            scoreLabel.Opacity = 0;
            levelLabel.Visible = false;
            levelLabel.Opacity = 0;
        }

        private void OnPlayClick(TouchSprite sprite)
        {
            Hide();
        }

        private void OnRestartClick(TouchSprite sprite)
        {
            game.Restart();
            Hide();
        }

        private void OnMenuClick()
        {
            game.Back();
        }

        private void OnSkipClick()
        {
            game.Skip();
        }

        private void OnMusicRefresh()
        {
        }

        public void RefreshSoundButtons()
        {
            soundButton.Toggle = !Mokus2DGame.SoundManager.SoundEnabled;
            musicButton.Toggle = !Mokus2DGame.SoundManager.MusicEnabled;
        }

        private const float MOVE_OFFSET = 300f;

        private const float MOVE_DURATION = 0.5f;

        protected int buttonIndex;

        protected List<Button> buttons;

        protected ClickableLayer clickableLayer;

        protected UserData data;

        protected ContreJourGame game;

        protected Label levelLabel;

        protected ToggleButton musicButton;

        protected bool open;

        protected Button restartButton;

        protected Label scoreLabel;

        protected ToggleButton soundButton;

        protected CGSize winSize;

        private LayerColor backgroundLayer = new(Color.Black);
    }
}
