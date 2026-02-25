using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class LevelsMenu : ClickableLayer
    {
        public LevelsMenu(int chapter, Vector2 initialPosition)
            : base(0)
        {
            this.initialPosition = initialPosition;
            Position = initialPosition;
            List<int> list = LevelsList[chapter];
            CGSize cgsize = ScreenConstants.W7FromIPhoneSize;
            Vector2 vector = CocosUtil.iPad(BORDER_OFFSET, BORDER_OFFSET_IPHONE);
            Vector2 vector2 = new Vector2(vector.X, cgsize.Height - vector.Y) - ScreenConstants.W7FromIPhoneScreenCenter;
            Vector2 vector3 = new((cgsize.Width - vector.X * 2f) / (COLUMNS - 1), (cgsize.Height - vector.Y * 2f) / (ROWS - 1));
            UserData instance = UserData.Instance;
            if (!Constants.IsTrial && instance.LastLevelOpen && chapter == 4)
            {
                vector2 = CreateRoseButton(vector2, cgsize, vector);
            }
            bool flag = chapter == 5;
            for (int i = 0; i < 20; i++)
            {
                int num = list[i];
                LevelPosition levelPosition = GetLevelPosition(num);
                bool flag2 = instance.GetUnlockedLevels(levelPosition.Chapter) >= levelPosition.Index;
                bool flag3 = Constants.IsTrial && i >= 10;
                if (flag)
                {
                    flag3 = i >= (ROWS - LockedRows) * COLUMNS;
                }
                LevelItem levelItem = new(num, flag2 && !flag3, flag3)
                {
                    RealScale = 1.15f
                };
                AddChild(levelItem);
                levelItem.Position = vector2 + new Vector2(vector3.X * (i % COLUMNS), -vector3.Y * (i / COLUMNS));
                levelItem.ClickEvent.AddListenerSelector(new Action<TouchSprite>(OnLevelClick));
                if (flag)
                {
                    levelItem.Color = ContreJourConstants.GreenLightColor.LerpToWhite(0.5f);
                }
            }
            if (Constants.IsTrial)
            {
                adsButton = CreateGetMoreButton(chapter);
                return;
            }
            if (flag && LockedRows > 0)
            {
                adsButton = CreateUnlockButton();
            }
        }

        public override Vector2 Position
        {
            set
            {
                base.Position = value;
                if (adsButton != null)
                {
                    adsButton.Position = adsButtonPosition + (Position - initialPosition) * 0.7f;
                }
            }
        }

        private Sprite CreateUnlockButton()
        {
            adsButtonPosition = new Vector2(0f, -RowOffset / 2f * (4 - LockedRows));
            Sprite sprite = new("McGetMoreLevelsButton");
            AddChild(sprite);
            sprite.Position = adsButtonPosition;
            sprite.Scale = 1.45f;
            sprite.Color = ContreJourConstants.GreenLightColor;
            Label label = ContreJourLabel.CreateLabel(20f, "COMPLETE_MORE_CHAPTERS", true);
            label.Scale *= 0.75f;
            sprite.AddChild(label);
            label.Y = 6f;
            return sprite;
        }

        private static int LockedRows => Constants.NormalChaptersCount - UserData.Instance.UnlockedChapters;

        private Sprite CreateGetMoreButton(int chapter)
        {
            TouchSprite touchSprite = new("McGetMoreLevelsButton");
            Node node;
            if (ContreJourLabel.IsEnglish)
            {
                node = new Sprite("McGetMoreLevelsText");
            }
            else
            {
                node = ContreJourLabel.CreateLabel(20f, "MORE_LEVELS_IN_FULL_VERSION", true);
                node.Scale = 0.8f;
            }
            touchSprite.AddChild(node);
            touchSprite.ClickEvent.AddListenerSelector(new Action(GetMoreEvent.SendEvent));
            node.Y = 10f;
            touchSprite.Scale = 1.45f;
            ButtonSprite buttonSprite = new(touchSprite)
            {
                TargetScale = touchSprite.Scale * 1.03f
            };
            AddChild(touchSprite);
            adsButtonPosition = GetMorePosition;
            touchSprite.Position = GetMorePosition;
            if (chapter == 1)
            {
                touchSprite.Color = CocosUtil.ccc4Mix(ContreJourConstants.BLUE_LIGHT_COLOR, ContreJourConstants.WHITE_COLOR_3, 0.7f);
            }
            return touchSprite;
        }

        private Vector2 CreateRoseButton(Vector2 position, CGSize winSize, Vector2 borderOffset)
        {
            Button button = new("McRoseButton.png", null, null);
            AddChild(button);
            button.Position = new Vector2(winSize.Width / 2f, borderOffset.Y - button.Size.Y / 2f - 15f) - ScreenConstants.W7FromIPhoneScreenCenter;
            button.ClickEvent.AddListenerSelector(new Action(OnRoseClick));
            button.RealScale = 1.15f;
            position.Y += button.Size.Y / 2f - 15f;
            Scale = 0.85f;
            initialScale = Scale;
            Node node = ClipFactory.CreateWithAnchor("McVenzel");
            node.Position = button.Position + CocosUtil.ccpIPad(56f, 0f);
            node.Scale = 1.15f;
            AddChild(node);
            node = ClipFactory.CreateWithAnchor("McVenzel");
            node.ScaleX = -1f;
            node.ScaleVec *= 1.15f;
            node.Position = button.Position - CocosUtil.ccpIPad(56f, 0f);
            AddChild(node);
            return position;
        }

        public static List<List<int>> LevelsList
        {
            get
            {
                if (LEVELS_LIST.Count == 0)
                {
                    for (int i = 0; i < Constants.ChaptersCount; i++)
                    {
                        LEVELS_LIST.Add(new List<int>());
                        for (int j = 0; j < 20; j++)
                        {
                            LEVELS_LIST[i].Add(LEVELS[i, j]);
                        }
                    }
                }
                return LEVELS_LIST;
            }
        }

        public static int GetLevelIndex(LevelPosition position)
        {
            List<int> list = LevelsList[position.Chapter];
            return list[position.Index];
        }

        public static LevelPosition GetLevelPosition(int level)
        {
            if (level == 169)
            {
                return LevelPosition.EndGame;
            }
            for (int i = 0; i < Constants.ChaptersCount; i++)
            {
                List<int> list = LevelsList[i];
                int num = list.IndexOf(level);
                if (num != -1)
                {
                    return new LevelPosition(i, num);
                }
            }
            throw new Exception("InvalidIndex: invalid level index");
        }

        public event Action<int> SelectLevelEvent;

        private void OnRoseClick()
        {
            if (Scale == initialScale)
            {
                SelectLevelEvent.Dispatch(169);
            }
        }

        private void OnLevelClick(TouchSprite item)
        {
            if (Scale == initialScale)
            {
                SelectLevelEvent.Dispatch(((LevelItem)item).Level);
            }
        }

        public static readonly int ROWS = 4;

        public static readonly int COLUMNS = 20 / ROWS;

        public static Vector2 BORDER_OFFSET_IPHONE = new Vector2(120f, 80f) * 2f;

        public static Vector2 BORDER_OFFSET = new(200f, 220f);

        private static readonly Vector2 GetMorePosition = new(0f, -120f);

        public static List<List<int>> LEVELS_LIST = new();

        private float RowOffset = 120f;

        public static int[,] LEVELS = new int[,]
        {
            {
                0, 45, 1, 4, 35, 3, 19, 26, 9, 47,
                27, 14, 61, 64, 118, 72, 7, 23, 6, 119
            },
            {
                33, 13, 121, 16, 90, 82, 86, 32, 124, 28,
                77, 57, 83, 31, 94, 67, 46, 84, 75, 78
            },
            {
                18, 51, 49, 120, 36, 56, 52, 95, 37, 81,
                79, 91, 85, 92, 93, 88, 74, 89, 80, 55
            },
            {
                98, 105, 97, 99, 108, 96, 107, 109, 111, 103,
                104, 100, 114, 102, 106, 112, 110, 101, 113, 116
            },
            {
                149, 147, 153, 148, 152, 157, 141, 132, 133, 139,
                151, 146, 128, 150, 154, 123, 156, 168, 155, 134
            },
            {
                188, 171, 170, 172, 190, 196, 191, 176, 186, 174,
                194, 178, 195, 182, 189, 197, 198, 183, 179, 199
            }
        };

        public readonly EventSender GetMoreEvent = new();

        private float initialScale = 1f;

        private Vector2 initialPosition;

        private Sprite adsButton;

        private Vector2 adsButtonPosition;
    }
}
