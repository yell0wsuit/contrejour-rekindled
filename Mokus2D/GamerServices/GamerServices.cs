using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Microsoft.Xna.Framework;

namespace Mokus2D.GamerServices
{
    public sealed class GamerServicesComponent(Game game) : GameComponent(game)
    {
    }

    public sealed class GameUpdateRequiredException : Exception
    {
        public GameUpdateRequiredException()
        {
        }

        public GameUpdateRequiredException(string message)
            : base(message)
        {
        }
    }

    public enum MessageBoxIcon
    {
        None = 0
    }

    public enum GamerPrivilegeSetting
    {
        Allowed = 0,
        Blocked = 1
    }

    public enum LeaderboardKey
    {
        BestScoreLifeTime = 0
    }

    public static class Guide
    {
        public static bool IsVisible { get; private set; }

        public static bool IsTrialMode { get; set; }

        public static IAsyncResult BeginShowMessageBox(
            string title,
            string text,
            IEnumerable<string> buttons,
            int focusButton,
            MessageBoxIcon icon,
            AsyncCallback callback,
            object state)
        {
            IsVisible = true;
            CompletedAsyncResult<int> result = new(focusButton, state);
            callback?.Invoke(result);
            return result;
        }

        public static int EndShowMessageBox(IAsyncResult result)
        {
            IsVisible = false;
            return (result as CompletedAsyncResult<int>)?.Result ?? 0;
        }

        public static void ShowMarketplace(PlayerIndex playerIndex)
        {
        }
    }

    public class Gamer
    {
        public static SignedInGamerCollection SignedInGamers { get; } = new();

        public virtual string DisplayName { get; protected set; } = "Player";

        public virtual LeaderboardWriter LeaderboardWriter => null;

        public virtual IAsyncResult BeginGetProfile(AsyncCallback callback, object state)
        {
            CompletedAsyncResult<GamerProfile> result = new(new GamerProfile(), state);
            callback?.Invoke(result);
            return result;
        }

        public virtual GamerProfile EndGetProfile(IAsyncResult result)
        {
            return (result as CompletedAsyncResult<GamerProfile>)?.Result ?? new GamerProfile();
        }
    }

    public sealed class SignedInGamerCollection : List<SignedInGamer>
    {
        public SignedInGamer this[PlayerIndex index]
        {
            get
            {
                int i = (int)index;
                return i >= 0 && i < Count ? this[i] : null;
            }
        }
    }

    public class SignedInGamer : Gamer
    {
        public GamerPrivileges Privileges { get; } = new GamerPrivileges();

        public override LeaderboardWriter LeaderboardWriter { get; } = new LeaderboardWriter();

        public IAsyncResult BeginAwardAchievement(string achievementKey, AsyncCallback callback, object state)
        {
            CompletedAsyncResult<bool> result = new(true, state);
            callback?.Invoke(result);
            return result;
        }

        public IAsyncResult BeginGetAchievements(AsyncCallback callback, object state)
        {
            CompletedAsyncResult<AchievementCollection> result = new(new AchievementCollection(), state);
            callback?.Invoke(result);
            return result;
        }

        public AchievementCollection EndGetAchievements(IAsyncResult result)
        {
            return (result as CompletedAsyncResult<AchievementCollection>)?.Result ?? new AchievementCollection();
        }
    }

    public sealed class GamerPrivileges
    {
        public GamerPrivilegeSetting AllowProfileViewing { get; set; } = GamerPrivilegeSetting.Blocked;
    }

    public sealed class GamerProfile
    {
        private static readonly byte[] EmptyPng = Convert.FromBase64String(
            "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAusB9Ywdr2gAAAAASUVORK5CYII=");

        public Stream GetGamerPicture()
        {
            return new MemoryStream(EmptyPng, writable: false);
        }
    }

    public sealed class Achievement
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int GamerScore { get; set; }

        public bool IsEarned { get; set; }

        public Stream GetPicture()
        {
            return new GamerProfile().GetGamerPicture();
        }
    }

    public sealed class AchievementCollection : List<Achievement>
    {
    }

    public sealed class LeaderboardIdentity
    {
        private LeaderboardIdentity(LeaderboardKey key)
        {
            Key = key;
        }

        public LeaderboardKey Key { get; }

        public static LeaderboardIdentity Create(LeaderboardKey key)
        {
            return new LeaderboardIdentity(key);
        }
    }

    public sealed class LeaderboardEntry
    {
        public SignedInGamer Gamer { get; set; } = new SignedInGamer();

        public long Rating { get; set; }
    }

    public sealed class LeaderboardWriter
    {
        private readonly LeaderboardEntry entry = new();

        public LeaderboardEntry GetLeaderboard(LeaderboardIdentity identity)
        {
            return entry;
        }
    }

    public sealed class LeaderboardReader
    {
        public IList<LeaderboardEntry> Entries { get; } = new List<LeaderboardEntry>();

        public static IAsyncResult BeginRead(
            LeaderboardIdentity identity,
            SignedInGamer gamer,
            int maxResults,
            AsyncCallback callback,
            object state)
        {
            LeaderboardReader reader = new();
            CompletedAsyncResult<LeaderboardReader> result = new(reader, state);
            callback?.Invoke(result);
            return result;
        }

        public static LeaderboardReader EndRead(IAsyncResult result)
        {
            return (result as CompletedAsyncResult<LeaderboardReader>)?.Result ?? new LeaderboardReader();
        }
    }

    internal sealed class CompletedAsyncResult<T>(T value, object state) : IAsyncResult
    {
        public T Result { get; } = value;

        public object AsyncState { get; } = state;

        public WaitHandle AsyncWaitHandle { get; } = CompletedEvent;

        public bool CompletedSynchronously => true;

        public bool IsCompleted => true;

        private static readonly ManualResetEvent CompletedEvent = new(true);
    }
}
