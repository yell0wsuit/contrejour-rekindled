namespace Default.Namespace
{
    public class CrystalManager
    {
        public bool Visible
        {
            get
            {
                return false;
            }
            set
            {
                if (value != Visible)
                {
                    visible = value;
                    if (visible)
                    {
                        TryPostScores();
                    }
                }
            }
        }

        public EventSender HideEvent
        {
            get
            {
                return hideEvent;
            }
        }

        public static CrystalManager Instance()
        {
            if (instance == null)
            {
                instance = new CrystalManager();
            }
            return instance;
        }

        public static void PostAchievementWasObtainedWithDescriptionAlwaysPopup(string achievement, bool wasObtained, string description, bool alwaysPopup)
        {
        }

        private CrystalManager()
        {
            hideEvent = new EventSender();
        }

        public void TryPostScores()
        {
            UserData userData = UserData.Instance;
            for (int i = 0; i < Constants.ChaptersCount; i++)
            {
                PostLeaderboardResultForLeaderboardIdLowestValFirst(userData.GetChapterScore(i), ContreJourConstants.LEADERBOARDS[i, 0], false);
            }
            PostLeaderboardResultForLeaderboardIdLowestValFirst(userData.TotalScore, ContreJourConstants.LEADERBOARD_TOTAL[0], false);
            userData.RefreshHighscores = false;
        }

        public static void PostLeaderboardResultForLeaderboardIdLowestValFirst(int result, string leaderboard, bool lowestValFirst)
        {
        }

        public void Start()
        {
            TryPostScores();
        }

        public void ChallengeStartedWithGameConfig(string gameConfig)
        {
        }

        public void SplashScreenFinishedWithActivateCrystal(bool activateCrystal)
        {
        }

        public void CrystalUiDeactivated()
        {
            visible = false;
            hideEvent.SendEvent();
        }

        public void CrystaliPadPopoversActivated(bool activated)
        {
        }

        protected EventSender hideEvent;

        protected bool visible;

        private static CrystalManager instance;
    }
}
