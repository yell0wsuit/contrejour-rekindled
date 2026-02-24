using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class FadeEffect : TouchEffect
    {
        public FadeEffect(TouchSprite _sprite)
            : base(_sprite)
        {
        }

        public FadeEffect(Node node)
            : base(node)
        {
        }

        public override NodeAction OnAction()
        {
            return new Sequence(
            [
                new Show(),
                new FadeIn(effectTime)
            ]);
        }

        public override NodeAction OffAction()
        {
            return new Sequence(
            [
                new FadeOut(effectTime),
                new Hide()
            ]);
        }
    }
}
