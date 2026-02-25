using System;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class ContreJourLevelBuilder : LevelBuilderBase
    {
        public ContreJourLevelBuilder(GameBase _game)
            : base(_game)
        {
            ClipFactory.debug_builder = this;
        }

        public override void AddProcessors()
        {
            base.AddProcessors();
            processors.Add(new PlasticineProcessor(this));
            processors.Add(new SnotProcessor(this));
            processors.Add(new BackSnotProcessor(this));
            processors.Add(new StrongSnotProcessor(this));
            processors.Add(new BridgeSnotProcessor(this));
            processors.Add(new VariableSnotProcessor(this));
            processors.Add(new TrampolineSnotProcessor(this));
            processors.Add(new LightPointProcessor(this));
            processors.Add(new ForegroundProcessor(this));
            processors.Add(new LianaProcessor(this));
            processors.Add(new FakeProcessor("hint", this));
        }

        public ContreJourGame ContreJour => (ContreJourGame)game;

        public void AddAlphaBackground(Node child)
        {
            if (!game.Debug)
            {
                ContreJour.AlphaBackground.AddChild(child);
            }
        }

        public void AddAlphaBackgroundZ(Node child, int z)
        {
            if (!game.Debug)
            {
                ContreJour.AlphaBackground.AddChild(child, z);
            }
        }

        public override LevelBuilderBase GetBuilder()
        {
            return this;
        }

        public override string GetViewType(Hashtable config)
        {
            return ContreJour.BlackSide && config.Exists("blackViewType") ? config.GetString("blackViewType") : base.GetViewType(config);
        }

        public override void Update(float time)
        {
            float num = Math.Min(time, maxWorldUpdateTime);
            float num2 = num * physicsSpeed / 2f;
            world.Step(num2);
            world.Step(num2);
            updater.Update(time);
        }
    }
}
