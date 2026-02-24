using System;

using Default.Namespace.Windows.Items;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

using Mokus2D.Visual;

namespace Default.Namespace.Windows
{
    public class AchievementsWindow : XBoxWindow
    {
        public AchievementsWindow()
        {
            CreateTitle("ACHIEVEMENTS");
        }

        protected override void ProcessGamerProfile()
        {
        }

        protected override void GetData()
        {
            base.GetData();
            gamer.BeginGetAchievements(new AsyncCallback(OnGetAchievements), gamer);
        }

        private void OnGetAchievements(IAsyncResult result)
        {
            HideLoading();
            if (result.IsCompleted)
            {
                try
                {
                    ProcessAchievements(result);
                    return;
                }
                catch (GameUpdateRequiredException)
                {
                    throw;
                }
                catch
                {
                    OnInternetError();
                    return;
                }
            }
            OnInternetError();
        }

        private void ProcessAchievements(IAsyncResult result)
        {
            AchievementCollection achievementCollection = gamer.EndGetAchievements(result);
            int num = 0;
            int num2 = 0;
            foreach (Achievement achievement in achievementCollection)
            {
                AddItem(new AchievementItem(achievement));
                if (achievement.IsEarned)
                {
                    num += achievement.GamerScore;
                    num2++;
                }
            }
            Label label = ContreJourLabel.CreateLabel(14f, string.Format("{0}/{1}  {2}/200", num2, achievementCollection.Count, num), true);
            label.Position = new Vector2(715f, titleLabel.Y);
            label.AnchorX = 1f;
            container.AddChild(label);
            ShowItem(label);
            AddChild(new Sprite("McXBoxGoldIcon")
            {
                Position = new Vector2(744f, 450f)
            });
        }
    }
}
