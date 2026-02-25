using Mokus2D.Default.Namespace.Windows.Items;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace.Windows
{
    public class LeaderboardsWindow : XBoxWindow
    {
        public LeaderboardsWindow()
        {
            string text = string.Format("YOUR_SCORE".Localize(), UserData.Instance.TotalScore);
            CreateTitle(text);
            titleLabel.AnchorX = 1f;
            titleLabel.X = ScreenConstants.OsSizes.W7.X - 26f;
        }

        protected override void GetData()
        {
            HideLoading();
            UserData userData = UserData.Instance;
            int rank = 0;
            for (int i = 0; i < Constants.NormalChaptersCount; i++)
            {
                int score = userData.GetChapterScore(i);
                if (score <= 0)
                {
                    continue;
                }
                string name = ("CHAPTER" + (i + 1)).Localize().Replace("\n", " ");
                AddItem(new LeaderboardItem(name, score, rank++));
            }
        }
    }
}
