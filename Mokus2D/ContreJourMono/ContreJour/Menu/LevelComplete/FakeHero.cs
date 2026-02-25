using System;

using Mokus2D.Default.Namespace;

using Microsoft.Xna.Framework;

using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;
using Mokus2D.ContreJourMono.ContreJour.Game.Hero;

namespace Mokus2D.ContreJourMono.ContreJour.Menu.LevelComplete
{
    public class FakeHero : Node
    {
        public FakeHero()
        {
            Background = ClipFactory.CreateWithAnchor(ProcessName("Menu/FakeHero/McFakeHeroBackground"));
            shadow = ClipFactory.CreateWithAnchor(ProcessName("Menu/FakeHero/McFakeHeroShadow"));
            HotSpot = ClipFactory.CreateWithAnchor(ProcessName("Menu/FakeHero/McFakeHeroHotspot"));
            Tail = new HeroTail(TailColor);
            AddChild(Tail);
            Tail.LimitAngles = true;
            Tail.Scale = 2f;
            Tail.Speed = 0f;
            AddChild(shadow);
            AddChild(Background);
            AddChild(HotSpot);
            Eye = CreateEye();
            AddChild(Eye);
        }

        public HeroTail Tail { get; }

        protected virtual Color TailColor => Color.Black;

        public Sprite Background { get; }

        public FakeHeroEye Eye { get; }

        public float Speed
        {
            get; set
            {
                field = value;
                Tail.Speed = value;
            }
        }

        public Sprite HotSpot { get; }

        public new Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                realPosition = value;
            }
        }

        public Vector2 ViewTarget
        {
            set
            {
                viewTarget = value;
                Vector2 vector = Parent.LocalToNode(value, this, true);
                Eye.ViewAngle = (float)Math.Atan2(vector.Y, vector.X);
                Eye.ViewDistance = vector.Length() / 200f;
            }
        }

        public void LookAt(Node node)
        {
            Vector2 vector = node.LocalToNode(Vector2.Zero, Parent, true);
            ViewTarget = vector;
        }

        protected virtual string ProcessName(string name)
        {
            return name;
        }

        protected virtual FakeHeroEye CreateEye()
        {
            return new FakeHeroEye();
        }

        public void SetMoveAngle(float angle, float speed)
        {
            Tail.SetMovementDirection(angle);
            Speed = speed;
        }

        public void SetViewAngle(float angle, float ratio)
        {
            Eye.ViewAngle = angle;
            Eye.ViewDistance = ratio;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (breathing)
            {
                breatheChanger.Update(time);
                ScaleVec = new Vector2(1f + breatheChanger.Value, 1f + (breatheChanger.Value / 2f));
                base.Position = realPosition + new Vector2(0f, breatheChanger.Value / 4f * Background.Size.X * Background.ScaleX);
            }
        }

        private const float ViewDistance = 200f;
        private readonly CosChanger breatheChanger = new(0f, 0.07f, 0.04f);
        private readonly Sprite shadow;
        private readonly bool breathing;

        private Vector2 realPosition;
        private Vector2 viewTarget;
    }
}
