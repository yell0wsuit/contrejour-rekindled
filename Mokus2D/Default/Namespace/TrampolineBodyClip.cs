using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Extensions;
using Mokus2D.Input;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class TrampolineBodyClip : SnotBodyClipBase
    {
        public TrampolineBodyClip(LevelBuilderBase _builder, SnotData _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            for (int i = 2; i < Physics.BodiesSize() - 2; i++)
            {
                TrampolinePartBodyClip trampolinePartBodyClip = Physics.BodyAt(i).UserData as TrampolinePartBodyClip;
                if (trampolinePartBodyClip != null)
                {
                    trampolinePartBodyClip.Parent = this;
                }
            }
            part.Parent = this;
            center = Physics.GetWorldStartPoint() + Physics.EndBody.Position;
            startTrampolineWidth = (Physics.GetWorldStartPoint() - Physics.EndBody.Position).Length();
            center *= 0.5f;
            normal = Physics.EndBody.Position - Physics.GetWorldStartPoint();
            normal = normal.Rotate90();
            normal *= 1f / normal.Length();
            impulseMultiplier = startTrampolineWidth / 6.533333f;
            startDistance = impulseMultiplier * 1.621671f;
            maxDistance = impulseMultiplier * 5f;
            centerDistanceDiff = maxDistance - startDistance;
            trajectory = new Trajectory(game)
            {
                Impulse = impulseMultiplier
            };
            builder.Add(trajectory, 11);
            trajectory.Position = builder.ToIPadPoint(center);
            trajectory.Angle = _config.GetFloat("rotation").ToRadians() + 1.5707964f;
            timeFromLaunch = 0.3f;
            SetJointsDamping(1f);
        }

        public bool Dragging
        {
            get => dragging; set => dragging = value;
        }

        public override Body Body
        {
            protected set
            {
                if (value != null && value.UserData != null)
                {
                    part = (TrampolinePartBodyClip)value.UserData;
                    value.UserData = null;
                }
                base.Body = value;
            }
        }

        protected override MonsterEye CreateEye()
        {
            return null;
        }

        public Vector2 CenterBodyPosition()
        {
            return CenterBody().WorldCenter;
        }

        public Body CenterBody()
        {
            return Physics.BodyAt(Physics.BodiesSize() / 2);
        }

        public void StartDrag(Touch _touch)
        {
            touch = _touch;
            DragEvent.SendEvent();
            dragging = true;
            Body body = CenterBody();
            dragJoint = JointFactory.CreateFixedMouseJoint(builder.World, body, body.WorldCenter);
            dragJoint.MaxForce = 500f;
            dragJoint.Frequency = 100f;
            trajectory.Enabled = true;
            if (!Constants.IS_RETINA)
            {
                SetJointsDamping(20f);
            }
            game.IncreaseZoomOut();
        }

        public void EndDrag()
        {
            if (touch != null)
            {
                Launch();
                StopDrag();
            }
        }

        public void StopDrag()
        {
            if (dragging)
            {
                game.DecreaseZoomOut();
                dragging = false;
                builder.World.RemoveJoint(dragJoint);
                dragJoint = null;
                if (!Constants.IS_RETINA)
                {
                    SetJointsDamping(1f);
                }
            }
            trajectory.Enabled = false;
        }

        public override void AddClipsToStage()
        {
            container.AddChild(clipContent);
            container.AddChild(baseEndClip);
            container.AddChild(baseClip);
            builder.Add(container, Layer());
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (part != null)
            {
                part.Update(time);
            }
            timeFromLaunch += time;
            if (dragging)
            {
                Vector2 vector = builder.TouchRootVec(touch);
                vector -= center;
                if (FarseerUtil.GetProjectionTarget(vector, normal) > -1.621671f)
                {
                    game.FreeTouch(touch);
                    StopDrag();
                    return;
                }
                UpdateDragPositionTime(vector, time);
            }
        }

        public void UpdateDragPositionTime(Vector2 dragTarget, float time)
        {
            Vector2 worldCenter = CenterBody().WorldCenter;
            float num = dragTarget.Length();
            if (num > maxDistance)
            {
                dragTarget *= maxDistance / dragTarget.Length();
                num = maxDistance;
            }
            trajectory.Angle = Maths.atan2Vec(dragTarget) + 3.1415927f;
            trajectory.Impulse = Maths.max(num - 1.621671f, 1f) * impulseMultiplier;
            dragTarget += center;
            dragTarget = Maths.StepToVecTargetMaxStep(worldCenter, dragTarget, 1.6666666f);
            dragJoint.WorldAnchorB = dragTarget;
            for (int i = 0; i < Physics.BodiesSize(); i++)
            {
                Body body = Physics.BodyAt(i);
                for (ContactEdge contactEdge = body.ContactList; contactEdge != null; contactEdge = contactEdge.Next)
                {
                    if (!contactEdge.Other.FixtureList[0].IsSensor)
                    {
                        Vector2 vector = CenterBodyPosition() - contactEdge.Other.WorldCenter;
                        vector *= 1f / vector.Length();
                        vector *= 3f * contactEdge.Other.Mass * time * 30f;
                        contactEdge.Other.ApplyForce(vector, contactEdge.Other.WorldCenter);
                    }
                }
            }
        }

        public void Launch()
        {
            Mokus2DGame.SoundManager.PlaySound("landing3", 0.4f, 0f, 0f);
            Vector2 vector = center - CenterBodyPosition();
            float num = vector.Length();
            if (num <= 1.621671f)
            {
                return;
            }
            List<Body> list = new();
            launchBodies.Clear();
            for (int i = 0; i < Physics.BodiesSize(); i++)
            {
                Body body = Physics.BodyAt(i);
                ContactEdge contactEdge = body.ContactList;
                float num2 = (body.WorldCenter - center).Length();
                list.Add(body);
                while (contactEdge != null)
                {
                    if (contactEdge.Contact.IsTouching)
                    {
                        float num3 = (contactEdge.Other.WorldCenter - center).Length();
                        if (num3 < num2)
                        {
                            launchBodies.Add(contactEdge.Other);
                        }
                    }
                    contactEdge = contactEdge.Next;
                }
            }
            vector *= 1f / num;
            float num4 = (num - startDistance) / centerDistanceDiff * impulseMultiplier;
            LaunchBodiesImpulseDirection(list, 6f * num4, vector);
            LaunchBodiesImpulseDirection(launchBodies, 22f * num4, vector);
            impulseVec = vector;
            impulseVec *= 22f * num4;
            timeFromLaunch = 0f;
        }

        public void OnCollisionStart(Body launchBody)
        {
            if (HeroTouchEvent.Enabled && launchBody.UserData is HeroBodyClip)
            {
                HeroTouchEvent.SendEvent();
            }
            if (timeFromLaunch < 0.3f && !launchBodies.Contains(launchBody))
            {
                launchBodies.Add(launchBody);
                Vector2 vector = impulseVec;
                vector *= launchBody.Mass;
                launchBody.LinearVelocity = new Vector2(0f, 0f);
                launchBody.ApplyLinearImpulse(vector, launchBody.WorldCenter);
                UpdateAchievement(launchBody);
            }
        }

        public void UpdateAchievement(Body launchBody)
        {
            object userData = launchBody.UserData;
        }

        public void LaunchBodiesImpulseDirection(List<Body> bodies, float impulse, Vector2 direction)
        {
            foreach (Body body in bodies)
            {
                Vector2 vector = direction * (impulse * body.Mass);
                body.LinearVelocity = new Vector2(0f, 0f);
                body.ApplyLinearImpulse(vector, body.WorldCenter);
                UpdateAchievement(body);
            }
        }

        public void SetJointsDamping(float value)
        {
            for (int i = 1; i < Physics.JoitsSize; i++)
            {
                DistanceJoint distanceJoint = (DistanceJoint)Physics.JointAt(i);
                distanceJoint.DampingRatio = value;
            }
        }

        public override void InitSizes()
        {
            base.InitSizes();
            startWidthPixels = 14f;
            startWidth = startWidthPixels * builder.EngineConfig.SizeMultiplier;
            endWidthPixels = startWidth;
            endWidth = startWidth;
        }

        public override SnotSprite CreateClip()
        {
            return game.WhiteSide
                ? new WhiteTrampolineSprite(game, this, startWidth, centerWidth, endWidth)
                : game.BlackSide
                ? new BlackTrampolineSprite(game, this, startWidth, centerWidth, endWidth)
                : new SnotSprite(this, startWidth, centerWidth, endWidth);
        }

        public override string BaseEndClipName()
        {
            return game.ChooseSide("McTrampolineEndBlack", "McTrampolineEndWhite", "McTrampolineEnd");
        }

        public override string BaseClipName()
        {
            return BaseEndClipName();
        }

        private const float POST_LAUNCH_TIME = 0.3f;

        private const float DEFAULT_WIDTH = 6.533333f;

        private const float TO_CENTER_FORCE = 3f;

        private const float MAX_STEP = 1.6666666f;

        private const float MAX_CENTER_DISTANCE = 5f;

        private const float START_CENTER_DISTANCE = 1.621671f;

        private const float BODY_LAUNCH_IMPULSE = 22f;

        private const float MAX_LAUNCH_IMPULSE = 6f;

        private const float DRAG_FORCE = 500f;

        public readonly EventSender DragEvent = new();

        public readonly EventSender HeroTouchEvent = new();

        protected Vector2 center;

        protected float centerDistanceDiff;

        protected FixedMouseJoint dragJoint;

        protected Vector2 dragOffset;

        protected bool dragging;

        protected float impulseMultiplier;

        protected Vector2 impulseVec;

        protected Vector2 initialPosition;

        protected List<Body> launchBodies = new();

        protected float maxDistance;

        protected Vector2 normal;

        protected TrampolinePartBodyClip part;

        private readonly Trajectory trajectory;

        protected float startDistance;

        protected float startTrampolineWidth;

        protected float timeFromLaunch;

        protected Touch touch;
    }
}
