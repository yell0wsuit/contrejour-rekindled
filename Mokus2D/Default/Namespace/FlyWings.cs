using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class FlyWings : Node
    {
        public FlyWings()
        {
            top = ClipFactory.CreateWithAnchor("McFlyWing");
            bottom = ClipFactory.CreateWithAnchor("McFlyWing");
            topContainer = new Node();
            bottomContainer = new Node();
            topContainer.AddChild(top);
            bottomContainer.AddChild(bottom);
            top.ScaleY = 0.5f;
            bottom.ScaleY = 0.5f;
            AddChild(topContainer);
            AddChild(bottomContainer);
            topContainer.RotationRadians = -IDLE_ROTATION;
            bottomContainer.RotationRadians = IDLE_ROTATION;
            StartActionDiff(top, -ROTATION_DIFF);
            StartActionDiff(bottom, ROTATION_DIFF);
            ScaleY = 0.7f;
        }

        public void SetFlying(bool value)
        {
            if (flying != value)
            {
                flying = value;
                float num = flying ? ROTATION : IDLE_ROTATION;
                topContainer.Run(new RotateTo(0.5f, num));
                bottomContainer.Run(new RotateTo(0.5f, -num));
                float num2 = flying ? 1f : 0.5f;
                top.Run(new ScaleTo(0.5f, new Vector2(1f, num2)));
                bottom.Run(new ScaleTo(0.5f, new Vector2(1f, num2)));
                float num3 = flying ? 1f : 0.7f;
                Run(new ScaleTo(0.5f, new Vector2(ScaleX, num3)));
            }
        }

        public void StartActionDiff(Sprite wing, float diff)
        {
            EaseInOut easeInOut = new(new RotateTo(1.5f, diff), 3f);
            EaseInOut easeInOut2 = new(new RotateTo(1.5f, -diff), 3f);
            Sequence sequence = new([easeInOut, easeInOut2]);
            wing.Run(new RepeatForever(sequence));
        }

        protected Sprite bottom;

        protected Sprite top;

        protected Node topContainer;

        protected Node bottomContainer;

        protected bool flying;

        private static readonly float ROTATION_DIFF = Maths.ToRadians(5f);

        private static readonly float ROTATION = Maths.ToRadians(25f);

        private static readonly float IDLE_ROTATION = Maths.ToRadians(10f);
    }
}
