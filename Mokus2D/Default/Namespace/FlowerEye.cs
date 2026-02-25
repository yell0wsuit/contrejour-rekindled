using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class FlowerEye : MonsterEye
    {
        public FlowerEye(ContreJourGame _game, bool _visible, Vector2 position)
            : base(_game, _visible, position)
        {
            string text = "McFlowerHeadWhite";
            string text2 = "McFlowerHead_6";
            string text3 = _game.Choose("McFlowerHead", null, text, null, text2);
            baseNode = new Sprite(text3);
            if (!_game.WhiteSide)
            {
                Scale = 0.85f;
            }
            AddChild(baseNode, -1);
        }

        public override Vector2 Position
        {
            set
            {
                base.Position = value;
                initialPosition = value;
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            base.Position = initialPosition + (currentEyeBall.Position * 2f);
        }

        private const string DefaultBaseClip = "McFlowerHead";

        protected Node baseNode;

        protected Vector2 initialPosition;
    }
}
