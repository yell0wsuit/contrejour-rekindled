using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

using Microsoft.Xna.Framework;

using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class StrongSnotBodyClip : SnotBodyClip
    {
        public float NormalDistance => normalDistance;

        public StrongSnotBodyClip(LevelBuilderBase _builder, SnotData _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            normalDistance = CurrentDistance();
            maxSnotDistance = Maths.max(normalDistance * 1.25f, normalDistance + 30f * builder.EngineConfig.SizeMultiplier);
            extremeSnotDistance = normalDistance * 2f;
            targetColor = 255f;
            DragableBodyClip dragableBodyClip = Physics.JoinedBody.UserData as DragableBodyClip;
            if (dragableBodyClip != null)
            {
                dragableBodyClip.Snot = this;
            }
        }

        public new Vector2 Position => Physics.FirstBody.Position;

        public override string[] OnSound()
        {
            return Sounds.ROPE_ON;
        }

        public override float JoinDistance()
        {
            return 1.3333334f;
        }

        public override void CreateTail()
        {
        }

        public override void CreateHighlite(ContreJourGame _game)
        {
        }

        public override void InitSizes()
        {
            base.InitSizes();
            centerWidth = 10f * builder.EngineConfig.SizeMultiplier;
        }

        public void EnsureSpeedY(float value)
        {
            for (int i = 0; i < Physics.BodiesSize(); i++)
            {
                Body body = Physics.BodyAt(i);
                if (body.LinearVelocity.Y > value)
                {
                    body.LinearVelocity = new Vector2(body.LinearVelocity.X, value);
                }
            }
        }

        public override string BaseEndClipName()
        {
            return game.ChooseSide("McStrongSnotEndBlack", "McStrongSnotEndWhite", "McStrongSnotEnd", "McStrongSnotEnd", "McSnotEnd_6");
        }

        public override SnotSprite CreateClip()
        {
            return new TextureSnotSprite((ContreJourGame)builder.Game, this, startWidth, centerWidth, endWidth);
        }

        public float CurrentDistance()
        {
            return (Physics.FirstBody.Position - Physics.EndBody.Position).Length();
        }

        public override float JoinedDamping()
        {
            return 0f;
        }

        protected override void UpdateDragBodyPosition(BodyAndPoint target, float time)
        {
            base.UpdateDragBodyPosition(target, time);
            Body body = Physics.EndBody;
            for (int i = Physics.BodiesSize() - 2; i >= 0; i--)
            {
                Body body2 = Physics.BodyAt(i);
                if ((body.Position - body2.Position).Length() <= Physics.Metrics.PartSize * 1.2f)
                {
                    break;
                }
                body2.Position = FarseerUtil.LimitDistance(body2.Position, body.Position, Physics.Metrics.PartSize * 1.2f);
                body = body2;
            }
        }

        public override bool TouchBegan(Touch _touch)
        {
            if (base.TouchBegan(_touch))
            {
                dragJoint = new FixedMouseJoint(Physics.EndBody, Physics.EndBody.Position)
                {
                    MaxForce = 100f,
                    Frequency = 100f,
                    WorldAnchorB = GetDragTarget().Point
                };
                return true;
            }
            return false;
        }

        public override void EndDrag()
        {
            base.EndDrag();
            if (dragJoint != null)
            {
                builder.World.RemoveJoint(dragJoint);
                dragJoint = null;
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            float num = targetColor;
            float num2 = CurrentDistance();
            bool flag = CanRelease();
            if (stickyJoint != null && num2 > maxSnotDistance && flag)
            {
                targetColor = Maths.StepToTargetMaxStep(targetColor, 0f, 20f);
                if (timeToRelease > 0.6f || num2 > extremeSnotDistance)
                {
                    ReleaseSnot();
                }
                else
                {
                    timeToRelease += time;
                }
            }
            else
            {
                float num3 = 0f;
                if (flag && num2 > normalDistance)
                {
                    num3 = (num2 - normalDistance) / normalDistance * 200f;
                }
                targetColor = Maths.StepToTargetMaxStep(targetColor, 255f - num3, 20f);
                timeToRelease = 0f;
            }
            if (Maths.FuzzyNotEquals(targetColor, num, 0.0001f))
            {
                ((TextureSnotSprite)clipContent).TextureColor = new Color(255, (int)targetColor, (int)targetColor);
            }
        }

        public bool CanRelease()
        {
            return stickyJoint != null && (!(linked is HeroBodyClip) || ((HeroBodyClip)linked).OnGround() || linked.SnotJoinedCount > 1);
        }

        public override void SetDamping(float value)
        {
        }

        public override float DragDistanceMultiplier()
        {
            return 1f;
        }

        public override void ApplyDisconnectForce()
        {
        }

        private const float JOIN_DISTANCE = 1.3333334f;

        private const float COLOR_STEP = 20f;

        private const float RELEASE_TIME = 0.6f;

        private const float MAX_STRETCHING = 30f;

        private const float MAX_DISTANCE_MULT = 1.25f;

        private const float MOUSE_FORCE = 100f;

        private FixedMouseJoint dragJoint;

        protected float maxSnotDistance;

        protected float normalDistance;

        protected float extremeSnotDistance;

        protected float timeToRelease;

        protected float targetColor;
    }
}
