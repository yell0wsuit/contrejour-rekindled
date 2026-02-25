using Mokus2D.Effects.Actions;

namespace Mokus2D.Default.Namespace
{
    public class ButtonSprite : TouchEffect
    {
        public float TargetScale { get; set; }

        public ButtonSprite(TouchSprite _sprite)
            : base(_sprite)
        {
            initialScale = Node.Scale;
            TargetScale = initialScale * 1.1f;
        }

        public override NodeAction OnAction()
        {
            return new ScaleTo(effectTime, TargetScale);
        }

        public override NodeAction OffAction()
        {
            return new ScaleTo(effectTime, initialScale);
        }

        private readonly float initialScale;
    }
}
