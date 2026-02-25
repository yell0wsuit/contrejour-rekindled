using Mokus2D.Default.Namespace.Windows.Items;

using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace.Windows
{
    public class AchievementsWindow : XBoxWindow
    {
        public AchievementsWindow()
        {
            CreateTitle("ACHIEVEMENTS");
        }

        protected override void GetData()
        {
            HideLoading();
            ProcessAchievements(BuildLocalAchievements());
        }

        private static AchievementCollection BuildLocalAchievements()
        {
            AchievementCollection collection = [];
            UserData userData = UserData.Instance;
            foreach ((string key, int score) in AllAchievements)
            {
                collection.Add(new Achievement
                {
                    Name = ("ACH_" + key + "_NAME").Localize(),
                    Description = ("ACH_" + key + "_DESC").Localize(),
                    GamerScore = score,
                    IsEarned = IsAchievementEarned(key, userData)
                });
            }
            return collection;
        }

        private static bool IsAchievementEarned(string key, UserData userData)
        {
            if (key == Achievements.BlueLantern)
            {
                return userData.TotalStars >= 180;
            }

            if (key == Achievements.Sunrise)
            {
                return userData.TotalStars >= 300;
            }

            for (int i = 0; i < Constants.NormalChaptersCount; i++)
            {
                if (key == Achievements.GetChapterPerfect(i))
                {
                    return userData.GetPerfect(i);
                }
            }
            return userData.EarnedAchievements.Contains(key);
        }

        private void ProcessAchievements(AchievementCollection achievementCollection)
        {
            int earned = 0;
            int earnedCount = 0;
            foreach (Achievement achievement in achievementCollection)
            {
                AddItem(new AchievementItem(achievement));
                if (achievement.IsEarned)
                {
                    earned += achievement.GamerScore;
                    earnedCount++;
                }
            }
            Label label = ContreJourLabel.CreateLabel(14f, string.Format("{0}/{1}  {2}/200", earnedCount, achievementCollection.Count, earned), true);
            label.Position = new Vector2(715f, titleLabel.Y);
            label.AnchorX = 1f;
            container.AddChild(label);
            ShowItem(label);
            AddChild(new Sprite("McXBoxGoldIcon")
            {
                Position = new Vector2(744f, 450f)
            });
        }

        private static readonly (string key, int score)[] AllAchievements =
        [
            (Achievements.Speedy, 8),
            (Achievements.MightyBird, 8),
            (Achievements.RushHour, 8),
            (Achievements.FastPerfect, 8),
            (Achievements.Spider, 8),
            (Achievements.BlueLantern, 20),
            (Achievements.Sunrise, 20),
            (Achievements.LittlePrince, 20),
            (Achievements.GetChapterPerfect(0), 20),
            (Achievements.GetChapterPerfect(1), 20),
            (Achievements.GetChapterPerfect(2), 20),
            (Achievements.GetChapterPerfect(3), 20),
            (Achievements.GetChapterPerfect(4), 20),
        ];
    }
}
