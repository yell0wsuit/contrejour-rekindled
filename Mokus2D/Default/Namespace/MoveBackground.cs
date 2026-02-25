using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;
using Mokus2D.Visual.Util;

namespace Default.Namespace
{
    public class MoveBackground : BackgroundBase
    {
        public MoveBackground(Node _node, Hashtable _config, ContreJourGame _game)
            : base(_node, _config, _game)
        {
            if (_config.Exists("moveOffset"))
            {
                moveOffset = _config.Exists("moveOffset") ? GraphUtil.StringToVector(_config.GetString("moveOffset")) : Vector2.Zero;
                moveOffset = CocosUtil.toIPad(moveOffset);
                MoveTo moveTo = new(60f, _node.Position + moveOffset);
                _node.Run(moveTo);
            }
        }

        protected Vector2 moveOffset;
    }
}
