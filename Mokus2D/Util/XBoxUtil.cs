using System.Collections.Generic;

using Default.Namespace;

namespace Mokus2D.Util
{
    public static class XBoxUtil
    {
        public static void AwardAchievement(string achievement)
        {
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

        private static readonly List<string> awardedAchievements = new(64);
    }
}
