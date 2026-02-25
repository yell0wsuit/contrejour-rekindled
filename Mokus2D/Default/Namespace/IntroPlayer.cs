using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class IntroPlayer : Node
    {
        public IntroPlayer(ContreJourGame _game)
        {
            game = _game;
            textPosition = CocosUtil.ccpIPad(0.5f * game.LevelSize.X, 0.7f * game.LevelSize.Y);
            Schedule(new Action(PlayItem), 2f);
            messages = new List<string>(["PRODUCER", "MUSIC_BY", "GRAPHICS_BY", "DIRECTED_BY"]);
            rightMessages = new List<string>(["TOM_KINNINBUGRH", "DAVID_LEON", "MIHAI_MAKSYM", "BY_MAKSYM_HRYNIV"]);
        }

        private void PlayItem()
        {
            if (messages.Count > 0)
            {
                ShowMessageRightMessageIndex(messages[messages.Count - 1], rightMessages[rightMessages.Count - 1], 4 - messages.Count);
                messages.RemoveAt(messages.Count - 1);
                rightMessages.RemoveAt(rightMessages.Count - 1);
                Schedule(new Action(PlayItem), 3.75f);
                return;
            }
            PlayInspired();
        }

        public void PlayInspired()
        {
            Schedule(new Action(PlayLogo), 6f);
        }

        private void PlayLogo()
        {
            Sprite sprite = ClipFactory.CreateWithAnchor("McIntroLogo");
            AddChild(sprite);
            sprite.Position = textPosition;
            FadeItemShowTime(sprite, 6f);
        }

        public void ShowMessageRightMessageIndex(string message, string rightMessage, int index)
        {
            Color color = HardwareCapabilities.IsLowMemoryDevice ? Color.Black : ContreJourConstants.GREY_COLOR;
            MultilineLabel multilineLabel = ContreJourLabel.CreateMultilineLabel(CocosUtil.RetinaWp7(30f), message);
            multilineLabel.AnchorX = 1f;
            multilineLabel.Color = color;
            multilineLabel.Position = CocosUtil.ccpIPad(-10f, 0f);
            MultilineLabel multilineLabel2 = ContreJourLabel.CreateMultilineLabel(CocosUtil.RetinaWp7(30f), rightMessage);
            multilineLabel2.Position = CocosUtil.ccpIPad(10f, 0f);
            multilineLabel2.Color = color;
            multilineLabel2.AnchorX = 0f;
            Node node = new();
            AddChild(node);
            node.Position = textPosition;
            node.AddChild(multilineLabel);
            node.AddChild(multilineLabel2);
            FadeItem(multilineLabel);
            FadeItem(multilineLabel2);
        }

        public void FadeItemShowTime(Node item, float time)
        {
            item.OpacityFloat = 0f;
            item.Run(new Sequence(
            [
                new FadeIn(0.8f),
                new Delay(time),
                new FadeOut(0.8f),
                new Hide()
            ]));
        }

        public void FadeItem(Node item)
        {
            FadeItemShowTime(item, 1.5f);
        }

        private const int MESSAGES_COUNT = 4;

        private const float SHOW_TIME = 1.5f;

        private const float FADE_TIME = 0.8f;

        private static readonly Color LAST_COLOR = new(16, 16, 16);

        protected ContreJourGame game;

        protected List<string> messages;

        protected List<string> rightMessages;

        protected Vector2 textPosition;
    }
}
