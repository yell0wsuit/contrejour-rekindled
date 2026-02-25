using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Content;
using Mokus2D.Visual;
using Mokus2D.Visual.Data;

namespace Default.Namespace
{
    public class GameBase : Node, IUpdatable
    {
        public LevelBuilderBase Builder => builder;

        public Node GameRoot => gameRoot;

        public bool Debug => debug;

        public EventSender LevelLoadedEvent => levelLoadedEvent;

        public Updater Updater => updater;

        public virtual bool Paused
        {
            get => paused; set => paused = value;
        }

        public float TotalTime => totalTime;

        public Dictionary<string, Level> CachedLevels => _levelsCache.CachedLevels;

        public virtual Vector2 LevelSize => Builder.LevelSize;

        public virtual Vector2 PhysicsLevelSize => Builder.PhysicsLevelSize;

        public GameBase()
        {
            clipFactory = CreateClipFactory();
            AddChild(gameRoot);
            levelLoadedEvent = new EventSender();
            updater = new Updater();
            totalTime = 0f;
        }

        public virtual ClipFactory CreateClipFactory()
        {
            return new ClipFactory();
        }

        public virtual void LoadLevel(string levelName)
        {
            DoLoadLevel(levelName);
        }

        public void DoLoadLevel(string levelName)
        {
            if (_levelsCache == null)
            {
                _levelsCache = new LevelsCache();
            }
            Level level = _levelsCache.Load(levelName);
            ProcessLevel(level);
            OnLoadLevelLevel(levelName, level);
            levelLoadedEvent.SendEvent();
        }

        public virtual void OnLoadLevelLevel(string levelName, Level level)
        {
        }

        public void AddUpdatable(IRemovable updatable)
        {
            updatables.Add(updatable);
        }

        public virtual void ProcessLevel(Level level)
        {
            LevelBuilderBase levelBuilderBase = builder;
            builder = CreateLevelBuilder();
            builder.ProcessLevel(level);
            if (levelBuilderBase != null)
            {
                levelBuilderBase.Dispose();
            }
        }

        public virtual LevelBuilderBase CreateLevelBuilder()
        {
            return new LevelBuilderBase(this);
        }

        public override void Draw(VisualState state)
        {
            base.Draw(state);
        }

        public virtual void UpdateGame(float time)
        {
            Builder.Update(time);
        }

        public virtual bool HasRemove()
        {
            return false;
        }

        public override void Update(float time)
        {
            if (paused)
            {
                return;
            }
            totalTime += time;
            updater.Update(time);
            UpdateGame(time);
            ArrayList arrayList = new();
            foreach (IRemovable removable in updatables)
            {
                removable.Update(time);
                if (removable.HasRemove())
                {
                    arrayList.Add(removable);
                }
            }
            foreach (object obj in arrayList)
            {
                _ = updatables.Remove((IRemovable)obj);
            }
        }

        public void DecreaseZoomOut()
        {
        }

        protected LevelBuilderBase builder;

        protected ClipFactory clipFactory;

        protected bool debug;

        protected List<IRemovable> updatables = new();

        protected readonly Node gameRoot = new();

        protected EventSender levelLoadedEvent;

        protected Updater updater;

        protected bool paused;

        protected float totalTime;

        private LevelsCache _levelsCache;

        protected Vector2 levelSize = Mokus2DGame.ScreenSize;

        private Vector2 physicsLevelSize = Mokus2DGame.ScreenSize * Box2DConfig.DefaultConfig.SizeMultiplier;
    }
}
