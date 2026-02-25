using System;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Mokus2D.GamerServices;

using Mokus2D;
using Mokus2D.Util;

namespace Default.Namespace
{
    public class UserData
    {
        public static UserData Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = ReadUserData();
                }
                return instance;
            }
        }

        public UserData()
        {
            Mokus2DGame.SoundManager.MusicDisableEvent.AddListenerSelector(new Action(OnMusicDisable));
        }

        private void OnMusicDisable()
        {
            MusicDisabled = true;
        }

        public LevelData[] LevelData
        {
            get;
            set
            {
                int num = 0;
                while (num < value.Length && num < field.Length)
                {
                    field[num] = value[num];
                    num++;
                }
            }
        } = new LevelData[Constants.ChaptersCount * 20];

        public int[] UnlockedLevels
        {
            get;
            set
            {
                int num = 0;
                while (num < value.Length && num < field.Length)
                {
                    field[num] = value[num];
                    num++;
                }
            }
        } = new int[Constants.ChaptersCount];

        public bool MusicDisabled { get; set; }

        public bool SoundDisabled { get; set; }

        public DateTime EnjoyDate { get; set; }

        public bool IntroWatched { get; set; }

        public bool InstallSent { get; set; }

        public bool Improved { get; set; }

        public bool EnjoyShown { get; set; }

        public bool HasToShowEnjoy
        {
            get
            {
                float num = (DateTime.Now - EnjoyDate).Days;
                if (EnjoyShown || !Improved || TotalStars < 30)
                {
                    return false;
                }
                if (num < 1f)
                {
                    DateTime enjoyDate = EnjoyDate;
                    return false;
                }
                return true;
            }
        }

        public bool LastLevelOpen => GetLevelDataByPosition(new LevelPosition(4, 19)) != null;

        public int OutOfScreen { get; set; }

        public int SpringShot { get; set; }

        public int TrampolineShot { get; set; }

        public int Acupuncture { get; set; }

        public int FeedMonster { get; set; }

        public int SnotEyeHit { get; set; }

        public int BlocksDestroyed { get; set; }

        public bool RefreshHighscores { get; set; }

        public int UnlockedChapters
        {
            get => Math.Max(unlockedChapters, 2); set => unlockedChapters = Math.Max(2, value);
        }

        public int TotalStars => GetStarsEnd(0, ContreJourConstants.LEVEL_COUNT);

        public int TotalScore => GetScoreEnd(0, ContreJourConstants.LEVEL_COUNT);

        public bool RoseSaved => TotalStars >= 240;

        public static void SaveUserData()
        {
            if (instance == null)
            {
                return;
            }
            try
            {
                _ = Directory.CreateDirectory(GetSaveDirectoryPath());
                using StreamWriter writer = new(GetSaveFilePath());
                serializer.Serialize(writer, instance);
            }
            catch (Exception)
            {
            }
        }

        public void RefreshSoundManager()
        {
            Mokus2DGame.SoundManager.MusicEnabled = !MusicDisabled && Mokus2DGame.SoundManager.HasControl;
            Mokus2DGame.SoundManager.SoundEnabled = !SoundDisabled;
        }

        private static UserData ReadUserData()
        {
            string path = GetSaveFilePath();
            if (!File.Exists(path))
            {
                return new UserData();
            }
            try
            {
                using StreamReader reader = new(path);
                return (UserData)serializer.Deserialize(reader) ?? new UserData();
            }
            catch (Exception)
            {
                return new UserData();
            }
        }

        private static string GetSaveDirectoryPath()
        {
            if (saveDirectoryPath != null)
            {
                return saveDirectoryPath;
            }
            string exeSaveDir = Path.Combine(AppContext.BaseDirectory, SaveDirectoryName);
            if (TryCreateDirectory(exeSaveDir))
            {
                saveDirectoryPath = exeSaveDir;
                return saveDirectoryPath;
            }
            string documentsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), SaveDirectoryName);
            if (TryCreateDirectory(documentsDir))
            {
                saveDirectoryPath = documentsDir;
                return saveDirectoryPath;
            }
            string localAppDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), SaveDirectoryName);
            if (TryCreateDirectory(localAppDataDir))
            {
                saveDirectoryPath = localAppDataDir;
                return saveDirectoryPath;
            }
            saveDirectoryPath = ".";
            return saveDirectoryPath;
        }

        private static string GetSaveFilePath() => Path.Combine(GetSaveDirectoryPath(), FILE_NAME);

        private static bool TryCreateDirectory(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    _ = Directory.CreateDirectory(path);
                }
                string testFile = Path.Combine(path, ".write_test");
                File.WriteAllText(testFile, "test");
                File.Delete(testFile);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static int StarsToUnlock(int chapter)
        {
            return STARS_TO_UNLOCK[chapter];
        }

        public static int GetTimeBonus(float time)
        {
            return (int)Maths.max(2000f * (180f - time) / 180f, 0f);
        }

        public void SetUnlockedLevelsChapter(int value, int chapter)
        {
            UnlockedLevels[chapter] = value;
        }

        public int GetUnlockedLevels(int chapter)
        {
            return UnlockedLevels[chapter];
        }

        public void UnlockChapter(int chapter)
        {
            UnlockedChapters = Math.Max(chapter + 1, unlockedChapters);
        }

        public string TotalStarsString()
        {
            return TotalStars.ToString();
        }

        public int GetChapterStars(int chapter)
        {
            return GetStarsEnd(chapter * 20, (chapter + 1) * 20);
        }

        public int GetChapterScore(int chapter)
        {
            return GetScoreEnd(chapter * 20, (chapter + 1) * 20);
        }

        public bool GetCompleted(int chapter)
        {
            for (int i = chapter * 20; i < (chapter + 1) * 20; i++)
            {
                if (GetLevelData(i) == null)
                {
                    return false;
                }
            }
            return true;
        }

        public bool GetPerfect(int chapter)
        {
            for (int i = chapter * 20; i < (chapter + 1) * 20; i++)
            {
                LevelData levelData = GetLevelData(i);
                if (levelData == null || levelData.StarsCount < 3)
                {
                    return false;
                }
            }
            return true;
        }

        public int GetStarsEnd(int start, int end)
        {
            int num = 0;
            for (int i = start; i < Maths.min(LevelData.Length, end); i++)
            {
                LevelData levelData = GetLevelData(i);
                if (levelData != null)
                {
                    num += levelData.StarsCount;
                }
            }
            return num;
        }

        public int GetScoreEnd(int start, int end)
        {
            int num = 0;
            for (int i = start; i < Maths.min(LevelData.Length, end); i++)
            {
                LevelData levelData = GetLevelData(i);
                if (levelData != null)
                {
                    num += levelData.Score;
                }
            }
            return num;
        }

        public LevelPosition GetLevelPosition(int index)
        {
            return LevelsMenu.GetLevelPosition(index);
        }

        public LevelData GetLevelDataByFile(int index)
        {
            LevelPosition levelPosition = LevelsMenu.GetLevelPosition(index);
            return GetLevelData(levelPosition.GlobalPosition());
        }

        public LevelData GetLevelDataByPosition(LevelPosition position)
        {
            return GetLevelData(position.GlobalPosition());
        }

        public void SetLevelData(LevelData data, int index)
        {
            LevelData[index] = data;
        }

        public LevelData GetLevelData(int index)
        {
            return LevelData[index];
        }

        public void CompleteAll()
        {
            for (int i = 0; i < (Constants.ChaptersCount * 20) - 1; i++)
            {
                _ = CompleteLevel(new LevelPosition(i / 20, i % 20), 3, 100f);
            }
        }

        public void SkipLevel(LevelPosition position)
        {
            if (GetUnlockedLevels(position.Chapter) < position.Index + 1)
            {
                SetUnlockedLevelsChapter(position.Index + 1, position.Chapter);
            }
        }

        public int CompleteLevel(LevelPosition position, int stars, float time)
        {
            SkipLevel(position);
            LevelData levelData = GetLevelDataByPosition(position) ?? new LevelData();
            int num = GetTimeBonus(time) + (stars * 1000);
            RefreshHighscores = true;
            bool flag = num > levelData.Score;
            if (flag || stars > levelData.StarsCount)
            {
                levelData = new LevelData(Maths.max(num, levelData.Score), Maths.max(stars, levelData.StarsCount));
                SetLevelData(levelData, position.GlobalPosition());
                if (flag)
                {
                    SaveHighscore();
                }
            }
            levelPostponed = true;
            postponedLevel = position;
            PostLevelAchievements();
            return num;
        }

        private void SaveHighscore()
        {
            if (Gamer.SignedInGamers.Count <= 0)
            {
                return;
            }
            Gamer gamer = Gamer.SignedInGamers[PlayerIndex.One];
            if (gamer != null && !Constants.IsTrial)
            {
                LeaderboardWriter leaderboardWriter = gamer.LeaderboardWriter;
                LeaderboardIdentity leaderboardIdentity = LeaderboardIdentity.Create(LeaderboardKey.BestScoreLifeTime);
                LeaderboardEntry leaderboard = leaderboardWriter.GetLeaderboard(leaderboardIdentity);
                leaderboard.Rating = TotalScore;
            }
        }

        public void PostLevelAchievements()
        {
            int totalStars = TotalStars;
            if (totalStars is >= 90 and >= 180)
            {
                XBoxUtil.AwardAchievement("blue_lantern");
                if (totalStars >= 300)
                {
                    XBoxUtil.AwardAchievement("sunrise");
                }
            }
            if (GetCompleted(postponedLevel.Chapter) && GetPerfect(postponedLevel.Chapter))
            {
                XBoxUtil.AwardAchievement(Achievements.GetChapterPerfect(postponedLevel.Chapter));
            }
        }

        public void RefreshPostponedData()
        {
            if (levelPostponed)
            {
                levelPostponed = false;
                CrystalManager.PostLeaderboardResultForLeaderboardIdLowestValFirst(GetChapterScore(postponedLevel.Chapter), ContreJourConstants.LEADERBOARDS[postponedLevel.Chapter, 0], false);
                CrystalManager.PostLeaderboardResultForLeaderboardIdLowestValFirst(TotalScore, ContreJourConstants.LEADERBOARD_TOTAL[0], false);
            }
        }

        private const string FILE_NAME = "contreJourData.xml";

        private const string SaveDirectoryName = "ContreJour_savedata";

        private const int ENJOY_STARS = 30;

        private const int ROSE_NEEDED_LIGHTS = 240;

        private const int MinUnlockedChapters = 2;

        private static readonly int[] STARS_TO_UNLOCK = [0, 0, 90, 140, 200];

        private static UserData instance;

        private static string saveDirectoryPath;

        private static readonly XmlSerializer serializer = new(typeof(UserData));

        private static bool levelPostponed;

        private static LevelPosition postponedLevel;
        private int unlockedChapters;
    }
}
