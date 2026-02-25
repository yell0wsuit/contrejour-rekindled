using System.Globalization;

using Mokus2D.Localization;

namespace Default.Namespace
{
    public static class Messages
    {
        public static string CompleteText(int stars)
        {
            return string.Format("COMPLETE_TEXT_{0}", stars).Localize();
        }

        public static string Localize(this string id)
        {
            return LocalizedMessages.GetString(id, CultureInfo.CurrentUICulture) ?? id;
        }

        public static readonly string LEVEL = "LEVEL".Localize();

        public static readonly string BEST_SCORE = "BEST_SCORE".Localize();

        public static readonly string ENERGY_BONUS = "ENERGY_BONUS".Localize();

        public static readonly string TIME_BONUS = "TIME_BONUS".Localize();

        public static readonly string TOTAL = "TOTAL".Localize();

        public static readonly string STARS_AND_SCORE = "STARS_AND_SCORE".Localize();
    }
}
