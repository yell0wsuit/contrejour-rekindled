using Microsoft.Xna.Framework;

using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class LevelItem : Button, IBoundsNode
    {
        public LevelItem(int _level, bool unlocked, bool trialLocked)
            : base(unlocked ? "McLevelItemBackground.png" : "McLevelItemInactive.png", "McLevelItemSelected", null)
        {
            LevelPosition levelPosition = LevelsMenu.GetLevelPosition(_level);
            this.unlocked = unlocked;
            level = _level;
            index = levelPosition.Index;
            if (!trialLocked)
            {
                CreateLabel(levelPosition);
            }
            else
            {
                OpacityFloat = 0.5f;
            }
            LevelData levelData = UserData.Instance.GetLevelData(levelPosition.GlobalPosition());
            Enabled = unlocked;
            if (unlocked)
            {
                for (int i = 0; i < 3; i++)
                {
                    bool flag = levelData != null && i < levelData.StarsCount;
                    string text = (flag ? "McLevelEnergy" : "McLevelEnergyInactive");
                    Node node = ClipFactory.CreateWithAnchor(text);
                    node.IgnoreParentColor = flag;
                    node.Position = CocosUtil.ccpIPad(36f, 22f * (-1f + i));
                    AddChild(node);
                }
                return;
            }
            if (levelPosition.Chapter == 1)
            {
                base.Color = CocosUtil.ccc4Mix(ContreJourConstants.BLUE_LIGHT_COLOR, ContreJourConstants.WHITE_COLOR_3, 0.7f);
            }
        }

        private void CreateLabel(LevelPosition levelPosition)
        {
            bool flag = index < 9;
            int num = index + 1;
            Node node;
            if (flag)
            {
                node = CreateDigitChapter(num, levelPosition.Chapter);
            }
            else
            {
                node = new Node();
                Node node2 = CreateDigitChapter(num / 10, levelPosition.Chapter);
                node2.Position = CocosUtil.ccpIPad(-12f, 0f);
                node.AddChild(node2);
                node2 = CreateDigitChapter(num % 10, levelPosition.Chapter);
                node2.Position = CocosUtil.ccpIPad(12f, 0f);
                node.AddChild(node2);
            }
            AddChild(node);
            node.Position = CocosUtil.ccpIPad(-11f, 0f);
        }

        public int Level => level;

        public int Index => index;

        public Node CreateDigitChapter(int character, int chapter)
        {
            Sprite sprite = ClipFactory.CreateWithAnchor(string.Format("McLevels{0}", character));
            Color color = CocosUtil.ccc4Mix(ContreJourConstants.BLUE_LIGHT_COLOR, ContreJourConstants.WHITE_COLOR_3, 0.5f);
            sprite.Color = ((chapter == 1) ? color : ContreJourConstants.GREY_COLOR);
            if (!unlocked)
            {
                sprite.Opacity = ((chapter == 1) ? 150 : 80);
            }
            return sprite;
        }

        public override void Click(Touch touch)
        {
            if (unlocked)
            {
                base.Click(touch);
            }
        }

        public Rectangle Bounds => new Rectangle(-50, -40, 100, 80);

        public const float EFFECT_TIME = 0.1f;

        private const int STAR_X = 36;

        private const int STAR_OFFSET = 22;

        private const int MAX_STARS = 3;

        protected int index;

        protected int level;

        protected bool unlocked;
    }
}
