using System;
using System.Collections.Generic;

using Default.Namespace;

using Mokus2D.GamerServices;

namespace Mokus2D.Util
{
    public static class XBoxUtil
    {
        public static void AwardAchievement(string achievement)
        {
            if (Default.Namespace.Constants.IsTrial)
            {
                return;
            }
            if (!awardedAchievements.Contains(achievement))
            {
                awardedAchievements.Add(achievement);
                List<string> earned = UserData.Instance.EarnedAchievements;
                if (!earned.Contains(achievement))
                {
                    earned.Add(achievement);
                    UserData.SaveUserData();
                }
            }
        }

        public static void GetProfile(this Gamer gamer, Action<GamerProfile> callback, Action errorCallback)
        {
            try
            {
                _ = gamer.BeginGetProfile(delegate (IAsyncResult result)
                {
                    if (result.IsCompleted)
                    {
                        callback.Invoke(gamer.EndGetProfile(result));
                        return;
                    }
                    errorCallback.Invoke();
                }, null);
            }
            catch (GameUpdateRequiredException)
            {
                throw;
            }
            catch
            {
                errorCallback.Invoke();
            }
        }

        public static void ShowUpdateRequired()
        {
            if (!Guide.IsVisible)
            {
                _ = Guide.BeginShowMessageBox("TITLE_UPDATE_AVAILABLE".Localize(), "UPDATE_IS_AVAILABLE".Localize(),
                [
                    "YES".Localize(),
                    "NO".Localize()
                ], 1, MessageBoxIcon.None, new AsyncCallback(OnUpdateRequiredResult), null);
            }
        }

        private static void OnUpdateRequiredResult(IAsyncResult result)
        {
            if (Guide.EndShowMessageBox(result) == 0)
            {
                MarketUtils.NavigateToMarket();
            }
        }

        private static readonly List<string> awardedAchievements = new(64);

        private static readonly bool achievementShown;
    }
}
