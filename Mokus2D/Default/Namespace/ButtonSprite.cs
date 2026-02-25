using Mokus2D.Effects.Actions;

namespace Default.Namespace
{
    public class ButtonSprite : TouchEffect
    {
        public float TargetScale
        {
            get => targetScale; set => targetScale = value;
        }

        public ButtonSprite(TouchSprite _sprite)
            : base(_sprite)
        {
            initialScale = Node.Scale;
            targetScale = initialScale * 1.1f;
        }

        public override NodeAction OnAction()
        {
            return new ScaleTo(effectTime, targetScale);
        }

        public override NodeAction OffAction()
        {
            return new ScaleTo(effectTime, initialScale);
        }

        private float targetScale;

        private readonly float initialScale;
    }
}
