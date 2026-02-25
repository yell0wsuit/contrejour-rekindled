using System;

using Default.Namespace.Windows.Items;

using Microsoft.Xna.Framework.GamerServices;

using Mokus2D.Visual;

namespace Default.Namespace.Windows
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
            base.GetData();
            if (gamer != null)
            {
                LeaderboardIdentity leaderboardIdentity = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime);
                _ = LeaderboardReader.BeginRead(leaderboardIdentity, gamer, 50, new AsyncCallback(OnLeaderboardRead), null);
            }
        }

        private void OnLeaderboardRead(IAsyncResult result)
        {
            HideLoading();
            if (result.IsCompleted)
            {
                try
                {
                    LeaderboardReader leaderboardReader = LeaderboardReader.EndRead(result);
                    int num = 0;
                    foreach (LeaderboardEntry leaderboardEntry in leaderboardReader.Entries)
                    {
                        AddItem(new LeaderboardItem(gamer, leaderboardEntry, num++));
                    }
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
    }
}
