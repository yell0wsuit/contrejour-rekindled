namespace Default.Namespace
{
    public class LevelPosition
    {
        public LevelPosition()
        {
            chapter = -1;
            index = -1;
        }

        public LevelPosition(int chapter, int index)
        {
            this.chapter = chapter;
            this.index = index;
        }

        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }

        public int Chapter
        {
            get
            {
                return chapter;
            }
        }

        public bool IsEndGame
        {
            get
            {
                return index == -1;
            }
        }

        public int MenuChapter
        {
            get
            {
                return !IsEndGame ? chapter : Constants.NormalChaptersCount - 1;
            }
        }

        public static LevelPosition EndGame
        {
            get
            {
                return new LevelPosition(0, -1);
            }
        }

        public bool SkipAvailable
        {
            get
            {
                return Chapter != 5 || Index < (UserData.Instance.UnlockedChapters - 1) * LevelsMenu.COLUMNS - 1;
            }
        }

        public int GlobalPosition()
        {
            return chapter * 20 + index;
        }

        private readonly int chapter;

        private int index;
    }
}
