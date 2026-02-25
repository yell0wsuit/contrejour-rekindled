using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SnotPoint : ContreJourBodyClip
    {
        public bool Used
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    if (!field)
                    {
                        UnuseEvent.SendEvent();
                    }
                }
            }
        }

        public SnotPoint(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            Body body = Body;
            builder.World.RemoveBody(Body);
            Create(body.Position);
        }

        public SnotPoint(LevelBuilderBase _builder, Vector2 position, Node _clip, Hashtable _config)
            : base(_builder, null, _clip, _config)
        {
            Create(position);
        }

        public override void Update(float time)
        {
            base.Update(time);
            clip.OpacityFloat = clip.OpacityFloat.StepTo(Enabled ? 1f : 0.2f, 0.05f);
        }

        private void Create(Vector2 position)
        {
            Body = FarseerUtil.CreateCircle(builder.World, Radius * builder.SizeMult, position, 0f, 0f, false);
            FarseerUtil.SetSensor(Body, true);
            Game.SnotPoints.Add(this);
            clip = new Sprite("McSnotPoint");
            _ = builder.AddChild(clip, 3);
        }

        private static readonly float Radius = 20f;
        public bool Enabled = true;

        public readonly EventSender UnuseEvent = new();
    }
}
