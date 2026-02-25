using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Extensions;
using Mokus2D.Input;
using Mokus2D.Util;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SnotBodyClip : SnotBodyClipBase, IClickable, IVectorPositionProvider, IRestartable
    {
        public SnotBodyClip(LevelBuilderBase _builder, SnotData _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            ContreJourGame contreJourGame = (ContreJourGame)_builder.Game;
            CreateHighlite(contreJourGame);
            length = Physics.InitialLength;
            linkEvent = new EventSender();
            releaseEvent = new EventSender();
            SetDamping(FreeDamping());
            movable = config.GetBool("movable");
            if (movable)
            {
                SnotPoint snotPoint = new(builder, _body.EyeBody.Position, null, null)
                {
                    Used = true
                };
                snotEye = new MovableSnotEye(this, Physics.EyeBody, snotPoint);
            }
            else
            {
                snotEye = new SnotEye(this, Physics.EyeBody);
            }
            eyeJointDef = new RevoluteJointDef(Physics.EyeJoint);
            game.AddPositionProvider(new PositionProviderValue(this, 1f));
            stickyJoint = null;
            if (game.BlackSide || game.BonusChapter)
            {
                CreateTail();
            }
            touchEndTime = -1f;
            dynamicDrag = config.GetBool("dynamicDrag");
            if (game.LevelIndex == 169)
            {
                _ = game.Updater.CallAfterSelectorDelay(new Action(Blink), Maths.randRange(5f, 8f));
                return;
            }
            _ = game.Updater.CallAfterSelectorDelay(new Action(Blink), Maths.randRange(1f, 2f));
        }

        public bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    if (enabled)
                    {
                        StopParts();
                        return;
                    }
                    ReleaseSnot();
                }
            }
        }

        public override Vector2 StartPosition => snotEye.Body.Position;

        public EventSender LinkEvent => linkEvent;

        public EventSender ReleaseEvent => releaseEvent;

        public ISnotLinked Linked => linked;

        public bool Dragging => touch != null;

        public bool Joined => stickyJoint != null;

        protected virtual float MoveTeleportCoeff => 0.5f;

        public bool UseForZoom()
        {
            return false;
        }

        public bool AcceptFreeTouches()
        {
            return true;
        }

        public bool DisableHeroFocus => false;

        public int Priority(Vector2 touchPoint)
        {
            return 1;
        }

        public virtual bool TouchBegan(Touch _touch)
        {
            if (touch != null || stickyJoint != null || !Enabled)
            {
                return false;
            }
            Vector2 vector = builder.TouchRootVec(_touch);
            if (movable && vector.DistanceTo(Body.Position) > vector.DistanceTo(Physics.EyeBody.Position))
            {
                return false;
            }
            SetZ(Layer() + 1);
            touch = _touch;
            Physics.EndBody.BodyType = BodyType.Static;
            Physics.EndBody.LinearVelocity = default(Vector2);
            SetDamping(3f);
            TryRemoveJoint();
            Physics.EndBody.SetTransform(GetDragTarget().Point, Physics.EndBody.Rotation);
            Mokus2DGame.SoundManager.PlaySound("leapOn1", 0.5f, 0f, 0f);
            return true;
        }

        public void TouchEnd(Touch _touch)
        {
            if (_touch == touch)
            {
                EndDrag();
            }
        }

        public bool TouchMove(Touch _touch)
        {
            return true;
        }

        public void TouchOut(Touch _touch)
        {
        }

        public void Restart()
        {
            touchEndTime = -1f;
        }

        public override Vector2 PositionVec => Physics.EndBody.Position;

        private static float closestReq(object item, object param)
        {
            BodyClip bodyClip = (BodyClip)item;
            Vector2 vector = (Vector2)param;
            return -FarseerUtil.b2Vec2Distance(bodyClip.Body.Position, vector);
        }

        private static bool linkableReq(BodyClip clip, object param)
        {
            LinkableReqParams linkableReqParams = (LinkableReqParams)param;
            return clip.Body.BodyType != BodyType.Static && FarseerUtil.b2Vec2Distance(linkableReqParams.Position, clip.Body.Position) < linkableReqParams.Distance && clip is ISnotLinked && (clip as ISnotLinked).SnotEnabled;
        }

        public void SetLengthDiff(float value)
        {
            length = Physics.InitialLength + value;
            float num = length / Physics.JoitsSize;
            for (int i = 0; i < Physics.JoitsSize; i++)
            {
                DistanceJoint distanceJoint = (DistanceJoint)Physics.JointAt(i);
                distanceJoint.Length = num;
            }
        }

        public float ExtremeDamping()
        {
            return 6f;
        }

        public float FreeDamping()
        {
            return 0.5f;
        }

        public virtual float JoinedDamping()
        {
            return 0f;
        }

        public virtual void CreateHighlite(ContreJourGame _game)
        {
            if (!_game.BlackSide && !_game.Debug)
            {
                highlite = ClipFactory.CreateWithAnchor("McSnotEndHighlite");
                highliteChanger = new CosChanger(0.05f, 0.1f);
            }
        }

        public void Blink()
        {
            _ = game.Updater.CallAfterSelectorDelay(new Action(Blink), Maths.randRange(10f, 25f));
            if (stickyJoint == null)
            {
                blinking = true;
                eye.Open = true;
                _ = game.Updater.CallAfterSelectorDelay(new Action(EndBlink), Maths.randRange(2f, 5f));
            }
        }

        public void EndBlink()
        {
            blinking = false;
            if (stickyJoint == null)
            {
                eye.Open = false;
            }
        }

        public virtual void CreateTail()
        {
            string text = game.BlackSide ? "snotTailTextureBlack.png" : "McTailTextureGreen.png";
            blackTail = new BlackTail(Physics.EndBody, builder, text);
            builder.Add(blackTail, 3);
            blackTail.Width = CocosUtil.r(20f);
        }

        public override void AddClipsToStage()
        {
            if (highlite != null)
            {
                container.AddChild(highlite);
            }
            container.AddChild(clipContent);
            container.AddChild(baseEndClip);
            container.AddChild(baseClip);
            if (eye != null)
            {
                container.AddChild(eye);
            }
            builder.Add(container, Layer());
        }

        public override SnotSprite CreateClip()
        {
            ContreJourGame contreJourGame = (ContreJourGame)builder.Game;
            Type type = contreJourGame.ChooseSide(typeof(BlackSnotSprite), typeof(WhiteSnotSprite), typeof(SpringSnotSprite), typeof(SpringSnotSprite), typeof(GreenSnotSprite));
            return (SnotSprite)ReflectUtil.CreateInstance(type, [contreJourGame, this, startWidth, centerWidth, endWidth]);
        }

        public override void Update(float time)
        {
            if (hasRelease)
            {
                ReleaseSnot();
                hasRelease = false;
            }
            if (Dragging)
            {
                for (int i = 0; i < Physics.BodiesSize(); i++)
                {
                    Physics.BodyAt(i).Awake = true;
                }
            }
            if (stickyJoint == null && ((touchEndTime > 0f && game.TotalTime - touchEndTime < 0.3f) || Dragging))
            {
                if (Dragging)
                {
                    BodyAndPoint dragTarget = GetDragTarget();
                    UpdateDragBodyPosition(dragTarget, time / 3f);
                }
                else
                {
                    BodyAndPoint dragTarget2 = GetDragTarget(Physics.EndBody.Position);
                    if (dragTarget2.Body != null)
                    {
                        UpdateDragBodyPosition(dragTarget2, time / 3f);
                    }
                }
                if (stickyJoint != null)
                {
                    touchEndTime = 0f;
                }
            }
            ((SpringSnotSprite)clipContent).Active = Dragging || Joined || blinking;
            base.Update(time);
            if (highlite != null)
            {
                highlite.Position = baseEndClip.Position;
                highlite.Opacity = (int)(100f + highliteChanger.Value * 155f);
                highliteChanger.Update(time);
            }
            if (movable)
            {
                baseClip.Position = builder.ToPoint(snotEye.Body.Position);
            }
            eye.Position = baseClip.Position;
            if (blackTail != null)
            {
                blackTail.Update(time);
                blackTail.Moving = stickyJoint != null;
            }
            clipContent.OpacityFloat = clipContent.OpacityFloat.StepTo(enabled ? 1f : 0.5f, 0.05f);
            baseEndClip.Color = Color.White * clipContent.OpacityFloat;
        }

        public override Vector2 EndPosition()
        {
            return stickyJoint == null ? base.EndPosition() : stickyJoint.WorldAnchorB;
        }

        protected virtual void UpdateDragBodyPosition(BodyAndPoint target, float time)
        {
            FarseerUtil.MoveBody(Physics.EndBody, target.Point, Physics.EndBody.Rotation, MoveTeleportCoeff, time);
            if (target.Body != null)
            {
                JoinToPosition(target.Body, target.Point);
                EndDrag();
            }
        }

        public override string BaseEndClipName()
        {
            return ((ContreJourGame)builder.Game).ChooseSide("McSnotEndBlack", "McSnotEndWhite", "McSnotEnd", "McSnotEnd", "McSnotEnd_6");
        }

        public override string BaseClipName()
        {
            return ((ContreJourGame)builder.Game).ChooseSide("McSnotStartBlack", "McSnotStartWhite", "McSnotStart", "McSnotStart", "McSnotStart_6");
        }

        public virtual string[] OnSound()
        {
            return Sounds.LEAP_ON;
        }

        public virtual float DragDistanceMultiplier()
        {
            return 2.6f;
        }

        public Vector2 OtherLocalAnchor(JointEdge edge)
        {
            RevoluteJoint revoluteJoint = (RevoluteJoint)edge.Joint;
            return revoluteJoint.BodyA == edge.Other
                ? revoluteJoint.BodyA.GetLocalPoint(revoluteJoint.LocalAnchorA)
                : revoluteJoint.BodyB.GetLocalPoint(revoluteJoint.LocalAnchorB);
        }

        public Vector2 ThisLocalAnchor(JointEdge edge)
        {
            RevoluteJoint revoluteJoint = (RevoluteJoint)edge.Joint;
            return revoluteJoint.BodyA != edge.Other
                ? revoluteJoint.BodyA.GetLocalPoint(revoluteJoint.LocalAnchorA)
                : revoluteJoint.BodyB.GetLocalPoint(revoluteJoint.LocalAnchorB);
        }

        public float MaxLength()
        {
            return length * DragDistanceMultiplier();
        }

        public BodyAndPoint GetDragTarget()
        {
            Vector2 vector = builder.TouchRootVec(touch);
            Vector2 vector2 = FarseerUtil.LimitDistance(vector, Physics.EyeBody.Position, MaxLength());
            return GetDragTarget(vector2);
        }

        public BodyAndPoint GetDragTarget(Vector2 _position)
        {
            LinkableReqParams linkableReqParams = new(JoinDistance(), _position);
            List<BodyClip> list = FarseerUtil.QueryBodyClipsCenterRadiusReqParam(builder.World, _position, JoinDistance(), new FarseerUtil.ClipSatisfyDelegate(linkableReq), linkableReqParams);
            if (list.Count == 0)
            {
                return new BodyAndPoint(null, _position);
            }
            BodyClip bodyClip = (BodyClip)Arrays.MaxItem(list, new MaxItemDelegate(closestReq), _position);
            return new BodyAndPoint(bodyClip.Body, bodyClip.Body.Position);
        }

        public void SetZ(int value)
        {
            builder.ChangeChildLayer(container, value);
        }

        public override int Layer()
        {
            return 4;
        }

        public void OnLinkedDestroy()
        {
            hasRelease = true;
        }

        public void ReleaseSnot()
        {
            if (stickyJoint != null)
            {
                Mokus2DGame.SoundManager.PlayRandomSound(Sounds.LEAP_OUT, 0.5f);
                SetDamping(FreeDamping());
                if (linked != null)
                {
                    linked.SnotJoinedCount--;
                    linked.DestroyEvent.RemoveListenerSelector(new Action(OnLinkedDestroy));
                    linked = null;
                }
                if (stickyJoint.BodyA != null && stickyJoint.BodyB != null)
                {
                    builder.World.RemoveJoint(stickyJoint);
                }
                stickyJoint = null;
                eye.Open = false;
                ApplyDisconnectForce();
                SetZ(Layer());
                releaseEvent.SendEvent();
            }
            if (touch != null)
            {
                EndDrag();
            }
        }

        public virtual void ApplyDisconnectForce()
        {
            Vector2 vector = Physics.GetWorldStartPoint();
            vector -= Physics.EndBody.Position;
            vector *= 10f / vector.Length();
            Physics.EndBody.ApplyForce(vector, Physics.EndBody.WorldCenter);
        }

        public virtual float JoinDistance()
        {
            return 2f;
        }

        private void JoinToPosition(Body joinBody, Vector2 joinPoint)
        {
            BodyClip bodyClip = (BodyClip)joinBody.UserData;
            if (bodyClip is ISnotLinked)
            {
                linked = (ISnotLinked)bodyClip;
                linked.SnotJoinedCount++;
                linked.DestroyEvent.AddListenerSelector(new Action(OnLinkedDestroy));
            }
            Mokus2DGame.SoundManager.PlayRandomSound(OnSound(), 0.5f);
            SetDamping(JoinedDamping());
            Physics.EndBody.SetTransform(joinPoint, Physics.EndBody.Rotation);
            Physics.EndBody.LinearVelocity = Vector2.Zero;
            stickyJoint = FarseerUtil.CreateRevoluteJoint(builder.World, Physics.EndBody, joinBody, joinPoint, false);
            eye.Open = true;
            SetZ(Layer() + 1);
            game.FocusOnHero();
            linkEvent.SendEvent();
        }

        public bool BodyConnectedToStaticProcessed(Body _body, ref List<Body> processed)
        {
            if (_body.BodyType != BodyType.Dynamic)
            {
                return true;
            }
            for (JointEdge jointEdge = _body.JointList; jointEdge != null; jointEdge = jointEdge.Next)
            {
                if (!processed.Contains(jointEdge.Other))
                {
                    processed.Add(jointEdge.Other);
                    BodyClip bodyClip = jointEdge.Other.UserData as BodyClip;
                    if (bodyClip is not null and SnotBodyClip)
                    {
                        SnotBodyClip snotBodyClip = (SnotBodyClip)bodyClip;
                        if (snotBodyClip.ConnectedToStatic(ref processed))
                        {
                            return true;
                        }
                    }
                    else if (BodyConnectedToStaticProcessed(jointEdge.Other, ref processed))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsConnectedToStatic()
        {
            if (dynamicDrag)
            {
                return false;
            }
            List<Body> list = new();
            list.Add(Physics.EyeBody);
            return BodyConnectedToStaticProcessed(Physics.EyeBody, ref list);
        }

        public bool ConnectedToStatic(ref List<Body> processed)
        {
            bool flag = BodyConnectedToStaticProcessed(Physics.EyeBody, ref processed);
            return !flag && !Dragging ? BodyConnectedToStaticProcessed(Physics.EndBody, ref processed) : flag;
        }

        public void StopParts()
        {
            for (int i = 0; i < Physics.BodiesSize(); i++)
            {
                Physics.BodyAt(i).LinearVelocity *= 0.2f;
            }
        }

        public virtual void SetDamping(float value)
        {
            for (int i = 0; i < Physics.BodiesSize(); i++)
            {
                Physics.BodyAt(i).Awake = true;
                Physics.BodyAt(i).LinearDamping = value;
            }
        }

        public void TryRemoveJoint()
        {
            if (!IsConnectedToStatic() && !jointRemoved)
            {
                builder.World.RemoveJoint(Physics.EyeJoint);
                Physics.EyeJoint = null;
                jointRemoved = true;
            }
        }

        public virtual void EndDrag()
        {
            if (!Dragging)
            {
                return;
            }
            touchEndTime = game.TotalTime;
            SetDamping(FreeDamping());
            Physics.EndBody.BodyType = BodyType.Dynamic;
            SetZ(Layer());
            touch = null;
            if (jointRemoved)
            {
                jointRemoved = false;
                Physics.EyeBody.LinearVelocity = new Vector2(0f, 0f);
                Physics.EyeJoint = eyeJointDef.Create(builder.World);
            }
        }

        private const bool AUTOJOIN = false;

        private const float DisabledOpacity = 0.5f;

        private const float BLINK_TIME_MAX = 25f;

        private const float BLINK_TIME_MIN = 10f;

        private const float DRAG_DAMPING = 3f;

        private const float FREE_DAMPING = 0.5f;

        private const float HERO_STICK_DISTANCE = 2f;

        private const float MAX_DRAG_FORCE = 100000f;

        private const float DRAG_DISTANCE_MULTIPLIER = 2.6f;

        private const float DISCONNECT_FORCE = 10f;

        private const string Movable = "movable";

        private readonly RevoluteJointDef eyeJointDef;

        protected BlackTail blackTail;

        protected bool blinking;

        protected bool dynamicDrag;

        protected bool hasRelease;

        protected Sprite highlite;

        protected CosChanger highliteChanger;

        protected bool jointRemoved;

        protected float length;

        protected EventSender linkEvent;

        protected ISnotLinked linked;

        protected EventSender releaseEvent;

        protected SnotEye snotEye;

        protected RevoluteJoint stickyJoint;

        protected Touch touch;

        protected float touchEndTime;

        private readonly bool movable;

        private bool enabled = true;

        public class BodyAndPoint(Body body, Vector2 point)
        {
            public Body Body = body;

            public Vector2 Point = point;
        }

        public class LinkableReqParams(float distance, Vector2 position)
        {
            public float Distance = distance;

            public Vector2 Position = position;
        }
    }
}
