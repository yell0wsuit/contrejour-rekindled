using System;

namespace Mokus2D.Default.Namespace
{
    public class LevelData
    {
        public LevelData()
        {
        }

        public LevelData(int _score, int _starsCount)
        {
            score = _score;
            starsCount = _starsCount;
        }

        public int Score
        {
            get => score; set => score = value;
        }

        public int StarsCount
        {
            get => starsCount; set => starsCount = value;
        }

        public override string ToString()
        {
            return string.Format("{0}|{1}", score * 3, starsCount * 4);
        }

        public static LevelData FromString(string value)
        {
            string[] array = value.Split(['|']);
            int num = Convert.ToInt32(array[0]) / 3;
            int num2 = Convert.ToInt32(array[1]) / 4;
            return new LevelData(num, num2);
        }

        protected int score;

        protected int starsCount;
    }
}
