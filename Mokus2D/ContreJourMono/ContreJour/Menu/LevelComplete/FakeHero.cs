using System;

using ContreJourMono.ContreJour.Game.Hero;

using Default.Namespace;

using Microsoft.Xna.Framework;

using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace ContreJourMono.ContreJour.Menu.LevelComplete
{
    public class FakeHero : Node
    {
        public FakeHero()
        {
            background = ClipFactory.CreateWithAnchor(ProcessName("Menu/FakeHero/McFakeHeroBackground"));
            shadow = ClipFactory.CreateWithAnchor(ProcessName("Menu/FakeHero/McFakeHeroShadow"));
            hotSpot = ClipFactory.CreateWithAnchor(ProcessName("Menu/FakeHero/McFakeHeroHotspot"));
            tail = new HeroTail(TailColor);
            AddChild(tail);
            tail.LimitAngles = true;
            tail.Scale = 2f;
            tail.Speed = 0f;
            AddChild(shadow);
            AddChild(background);
            AddChild(hotSpot);
            eye = CreateEye();
            AddChild(eye);
        }

        public HeroTail Tail => tail;

        protected virtual Color TailColor => Color.Black;

        public Sprite Background => background;

        public FakeHeroEye Eye => eye;

        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                tail.Speed = value;
            }
        }

        public Sprite HotSpot => hotSpot;

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
                eye.ViewAngle = (float)Math.Atan2(vector.Y, vector.X);
                eye.ViewDistance = vector.Length() / 200f;
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
            tail.SetMovementDirection(angle);
            Speed = speed;
        }

        public void SetViewAngle(float angle, float ratio)
        {
            eye.ViewAngle = angle;
            eye.ViewDistance = ratio;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (breathing)
            {
                breatheChanger.Update(time);
                ScaleVec = new Vector2(1f + breatheChanger.Value, 1f + breatheChanger.Value / 2f);
                base.Position = realPosition + new Vector2(0f, breatheChanger.Value / 4f * background.Size.X * background.ScaleX);
            }
        }

        private const float ViewDistance = 200f;

        private readonly Sprite background;

        private readonly CosChanger breatheChanger = new(0f, 0.07f, 0.04f);

        private readonly FakeHeroEye eye;

        private readonly Sprite hotSpot;

        private readonly Sprite shadow;

        private readonly HeroTail tail;

        private readonly bool breathing;

        private Vector2 realPosition;

        private float speed;

        private Vector2 viewTarget;
    }
}
