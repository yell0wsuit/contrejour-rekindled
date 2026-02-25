using System;

using Default.Namespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Content;
using Mokus2D.Input;
using Mokus2D.Util;
using Mokus2D.Visual;
using Mokus2D.Visual.GameDebug;
using Mokus2D.Visual.Util;

namespace Mokus2D
{
    public class Mokus2DGame : Game
    {
        public Mokus2DGame()
        {
            Instance = this;
        }

        public static GraphicsDevice Device => Instance.GraphicsDevice;

        public static TouchController TouchController => Instance.touchController;

        public static KeysController KeysController => Instance.keysController;

        public static SoundManager SoundManager => Instance.soundManager;

        public static Scheduler Scheduler => Instance.scheduler;

        public static Vector2 ScreenSize => new Vector2(Device.PresentationParameters.BackBufferWidth, Device.PresentationParameters.BackBufferHeight);

        public static ContentManager ContentManager => Instance.Content;

        public static ReferenceCountingContentManager SharedContent
        {
            get
            {
                if (field == null)
                {
                    field = new ReferenceCountingContentManager(Instance.Services);
                }
                return field;
            }
        }

        public static Mokus2DGame Instance { get; private set; }

        public RootNode Root
        {
            get
            {
                if (field == null)
                {
                    field = CreateRootNode();
                }
                return field;
            }
        }

        public string ContentRootDirectory
        {
            set
            {
                Content.RootDirectory = value;
                SharedContent.RootDirectory = value;
            }
        }

        protected override void OnActivated(object sender, EventArgs args)
        {
            SoundManager.OnGameActivated();
        }

        protected virtual RootNode CreateRootNode()
        {
            return new RootNode(Device.PresentationParameters.BackBufferWidth, Device.PresentationParameters.BackBufferHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            gameUpdateGarbageTracer.Start();
            base.Update(gameTime);
            float num = Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, MaxUpdateTime);
            soundManager.Update(num);
            keysController.Update(num);
            touchGarbageTracer.Start();
            touchController.Update(num);
            touchGarbageTracer.End();
            schedulerGarbageTracer.Start();
            scheduler.Update(num);
            schedulerGarbageTracer.End();
            rootGarbageTracer.Start();
            Root.UpdateNode(num);
            rootGarbageTracer.End();
            gameUpdateGarbageTracer.End();
        }

        protected override void Initialize()
        {
            base.Initialize();
            XNAUtil.RefreshViewport(GraphicsDevice);
            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.None
            };
        }

        protected override void Draw(GameTime gameTime)
        {
            gameDrawGarbageTracer.Start();
            GraphicsDevice.Clear(BackgroundColor);
            base.Draw(gameTime);
            Root.DrawAll();
            gameDrawGarbageTracer.End();
        }

        private readonly KeysController keysController = new();

        private readonly Scheduler scheduler = new();

        private readonly SoundManager soundManager = new();

        private readonly TouchController touchController = new();

        public Color BackgroundColor = Color.Black;

        public float MaxUpdateTime = 0.04f;
        private GarbageTracer gameDrawGarbageTracer = new("Game.Draw", false);

        private GarbageTracer gameUpdateGarbageTracer = new("Game.Update", false);

        private GarbageTracer rootGarbageTracer = new("Root.Update", false);

        private GarbageTracer schedulerGarbageTracer = new("Scheduler", false);

        private GarbageTracer touchGarbageTracer = new("TouchController", false);
    }
}
