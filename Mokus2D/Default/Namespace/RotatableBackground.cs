using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class RotatableBackground : MoveBackground
    {
        public RotatableBackground(Node _node, Hashtable _config, ContreJourGame _game)
            : base(_node, _config, _game)
        {
            rotationStep = config.GetFloat("speed") / 2f;
        }

        public override void Update(float time)
        {
            node.Rotation -= rotationStep * time * 30f;
        }

        protected float rotationStep;
    }
}
