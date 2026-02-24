using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class RadiusRotatableBackground : RotatableBackground
    {
        public RadiusRotatableBackground(Node _node, Hashtable _config, ContreJourGame _game)
            : base(_node, _config, _game)
        {
            radius = CocosUtil.iPadValue(40f);
            centerPosition = node.Position;
            centerPosition.X = centerPosition.X + radius;
            rotationStep = 0.2f;
            rotation = 0f;
        }

        public override void Update(float time)
        {
            base.Update(time);
            Vector2 vector = Maths.ToPointAngle(radius, MathHelper.ToRadians(node.Rotation * 2f));
            node.Position = vector + centerPosition;
        }

        protected float radius;

        protected Vector2 centerPosition;

        protected float rotation;
    }
}
