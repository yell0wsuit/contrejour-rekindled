using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class ContreJourBodyClip : BodyClip
    {
        public ContreJourGame Game => game;

        public ContreJourBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            contreJourBuilder = (ContreJourLevelBuilder)_builder;
            game = contreJourBuilder.ContreJour;
        }

        public virtual float TouchDistance(Vector2 touchPosition)
        {
            return PositionVec.DistanceTo(touchPosition);
        }

        private readonly ContreJourGame game;

        protected readonly ContreJourLevelBuilder contreJourBuilder;
    }
}
