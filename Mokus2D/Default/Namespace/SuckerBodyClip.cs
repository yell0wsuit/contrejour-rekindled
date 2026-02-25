using System;

using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Extensions;
using Mokus2D.Input;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SuckerBodyClip : ContreJourBodyClip, IClickable, IVectorPositionProvider, IRestartable
    {
        public SuckerBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            _clip = new Node();
            clip = _clip;
            _builder.Add(_clip, 1);
            Body body = FarseerUtil.CreateCircle(_builder.World, 0.1f, ((Body)_body).Position, 0f, 0f, false);
            FarseerUtil.SetSensor(body, true);
            _builder.World.RemoveBody((Body)_body);
            Body = body;
            bouncer = new Bouncer(4f, 7f, 3f);
            config["noShadow"] = "true";
            float @float = config.GetFloat("Width");
            maxDistance = @float / 2f * builder.SizeMult;
            maxLength = @float / 2f + 13f;
            ghostSprite = new RenderSprite(new Vector2(@float / 2f + 14f, 28f));
            ghostSprite.Anchor = new Vector2(0f, 0.5f);
            ghostSprite.OpacityFloat = 0.5f;
            clip.AddChild(ghostSprite);
            ghostNeck = CreateNeck();
            ghostNeck.NeckColor = new Color(50, 50, 50, 255);
            ghostSprite.AddChild(ghostNeck);
            ghostPimpa = CreatePimpa();
            ghostSprite.AddChild(ghostPimpa);
            limit = ClipFactory.CreateWithAnchor("McRoundDragFrameView");
            builder.Add(limit, -1);
            limit.Position = builder.ToPoint(Body.Position);
            limit.Scale = @float / 200f;
            neck = CreateNeck();
            clip.AddChild(neck);
            Node node = ClipFactory.CreateWithAnchor("McSuckerHighlite");
            clip.AddChild(node);
            Sprite sprite = ClipFactory.CreateWithAnchor("McSnotStart");
            clip.AddChild(sprite);
            eye = new MonsterEye(Game, false, Body.Position);
            clip.AddChild(eye);
            eye.Visible = false;
            pimpa = new Node();
            clip.AddChild(pimpa);
            pimpaHighlite = ClipFactory.CreateWithAnchor("McSuckerHighlite");
            pimpa.AddChild(pimpaHighlite);
            Node node2 = CreatePimpa();
            pimpa.AddChild(node2);
        }

        protected virtual float BounceVolume => 0.3f;

        protected virtual string BounceSound => "landing1";

        public bool Dragging => touch != null;

        public Vector2 TargetPosition
        {
            get
            {
                Vector2 vector = builder.TouchRootVec(touch);
                return FarseerUtil.LimitDistance(vector, Body.Position, maxDistance);
            }
        }

        public override float TouchDistance(Vector2 touchPosition)
        {
            return endBody != null
                ? Math.Min(touchPosition.DistanceTo(Body.Position), touchPosition.DistanceTo(endBody.Position))
                : touchPosition.DistanceTo(Body.Position);
        }

        public bool DisableHeroFocus => true;

        public int Priority(Vector2 touchPoint)
        {
            return 0;
        }

        public bool AcceptFreeTouches()
        {
            return false;
        }

        public bool UseForZoom()
        {
            return false;
        }

        public bool TouchBegan(Touch _touch)
        {
            if (creating)
            {
                return false;
            }
            if (touch == null)
            {
                StartDrag(_touch);
                return true;
            }
            return false;
        }

        public void TouchEnd(Touch _touch)
        {
            FinishDrag();
        }

        public bool TouchMove(Touch _touch)
        {
            return true;
        }

        public void TouchOut(Touch touch)
        {
        }

        public void Restart()
        {
            if (end != null)
            {
                eye.Visible = false;
                DestroyBodies();
                ghostSprite.Visible = false;
            }
        }

        public virtual Node CreatePimpa()
        {
            return ClipFactory.CreateWithAnchor("McSuckerBodyStrong");
        }

        protected virtual SuckerNeckSprite CreateNeck()
        {
            return new TexturedSuckerNeck("McSuckerStrongSnotTexture.png");
        }

        private void RedrawGhost()
        {
            ghostSprite.RedrawTexture();
        }

        public override void Update(float time)
        {
            base.Update(time);
            bouncer.Update(time);
            bouncePosition = pimpaPosition + Maths.toPoint(bouncer.Value, bounceAngle);
            if (!Maths.ccpEqual(pimpa.Position, bouncePosition))
            {
                pimpa.Position = Maths.stepToPoint(pimpa.Position, bouncePosition, 1000f * time);
                neck.Length = pimpa.Position.Length();
                neck.Rotation = MathHelper.ToDegrees(Maths.atan2(pimpa.Position.Y, pimpa.Position.X));
            }
            limit.Opacity = (int)Maths.stepTo(limit.Opacity, (touch != null) ? 200 : 80, time * 200f);
            pimpaHighlite.Opacity = (int)Maths.stepTo(pimpaHighlite.Opacity, (end != null) ? 255 : 0, time * 600f);
            if (touch != null)
            {
                RefreshGhostPosition(time, TargetPosition);
            }
            eye.Update(time);
            neck.Update(time);
        }

        public void RefreshGhostPosition(float time, Vector2 position)
        {
            Vector2 vector = builder.ToPoint(position - Body.Position);
            ghostPimpa.Position = new Vector2(vector.Length(), 0f);
            ghostNeck.Length = ghostPimpa.Position.X;
            ghostNeck.UpdateNode(time);
            RedrawGhost();
            ghostSprite.Rotation = MathHelper.ToDegrees(Maths.atan2(vector.Y, vector.X));
        }

        public override void OnCollisionStartPoint(Body body2, Contact point)
        {
            BodyClip bodyClip = body2.UserData as BodyClip;
            if (bodyClip is not null and HeroBodyClip)
            {
                Vector2 worldPoint = FarseerUtil.GetWorldPoint(point);
                if (FarseerUtil.b2Vec2Distance(worldPoint, Body.Position) > 1.1666666f)
                {
                    neck.Bounce();
                    PlayBounceSound();
                }
            }
        }

        protected void PlayBounceSound()
        {
            Mokus2DGame.SoundManager.PlaySound(BounceSound, BounceVolume, 0f, 0f);
        }

        public void StartDrag(Touch _touch)
        {
            if (touch != null)
            {
                throw new InvalidOperationException("already dragging");
            }
            Mokus2DGame.SoundManager.PlaySound("leapOn1", 1f, 0f, 0f);
            ghostSprite.StopAllActions();
            ghostSprite.Visible = true;
            touch = _touch;
            startDragPosition = (end != null) ? endBody.Position : builder.TouchRootVec(touch);
        }

        public void DestroyBodies()
        {
            pulled = false;
            end = null;
            builder.World.RemoveBody(endBody);
            Body.DestroyFixture(middleFixture);
            FarseerUtil.SetSensor(Body, true);
            pimpaPosition = Vector2.Zero;
            bouncer.Start();
        }

        public virtual void CreateBodies()
        {
            creating = false;
            pulled = true;
            endBody = FarseerUtil.CreateCircle(builder.World, 0.1f, createPosition, 0f, 0f, false);
            float num = FarseerUtil.b2Vec2Distance(createPosition, Body.Position);
            float num2 = Maths.atan2Vec(Body.Position, createPosition);
            float num3 = num / 2f;
            float num4 = 0.1f;
            Vector2 vector = FarseerUtil.rotate(new Vector2(num3, 0f), num2);
            FarseerPhysics.Common.Vertices vertices = new(4);
            vertices.Add(FarseerUtil.rotate(new Vector2(-num3, -num4), num2) + vector);
            vertices.Add(FarseerUtil.rotate(new Vector2(num3, -num4), num2) + vector);
            vertices.Add(FarseerUtil.rotate(new Vector2(num3, num4), num2) + vector);
            vertices.Add(FarseerUtil.rotate(new Vector2(-num3, num4), num2) + vector);
            PolygonShape polygonShape = new(vertices, builder.EngineConfig.Density);
            bounceAngle = num2;
            middleFixture = FarseerUtil.AddShapeToDensity(polygonShape, Body, builder.EngineConfig.Density);
            FarseerUtil.SetSensor(Body, false);
            end = new SuckerEndBodyClip(this, endBody);
            pimpaPosition = builder.ToPoint(endBody.Position - Body.Position);
            bouncer.Start();
            neck.LightBounce();
            eye.Visible = true;
            eye.PositionProvider = this;
            _ = Schedule(new Action(RefreshPositionProvider), Maths.randRange(1.5f, 2.5f));
            FinishDragEvent.SendEvent();
        }

        private void RefreshPositionProvider()
        {
            eye.PositionProvider = null;
        }

        public void CancelDrag()
        {
            touch = null;
        }

        public void FinishDrag()
        {
            if (creating)
            {
                return;
            }
            bool flag = pulled;
            if (pulled)
            {
                DestroyBodies();
            }
            if (FarseerUtil.b2Vec2Distance(TargetPosition, Body.Position) > 1.5f)
            {
                createPosition = TargetPosition;
                RefreshGhostPosition(0f, createPosition);
                if (flag)
                {
                    creating = true;
                    _ = Schedule(new Action(CreateBodies), 0.2f);
                }
                else
                {
                    CreateBodies();
                }
            }
            else
            {
                eye.Visible = false;
                ghostSprite.Visible = false;
                if (flag)
                {
                    RemoveEvent.SendEvent();
                }
            }
            touch = null;
        }

        private const float PIMPA_SPEED = 1000f;

        private const float MIN_BOUNCE_DISTANCE = 1.1666666f;

        private const float MIN_CENTER_DISTANCE = 1.5f;

        private const float MIN_DISTANCE = 0.6666667f;

        private const float END_RADIUS = 0.1f;

        public readonly EventSender FinishDragEvent = new();

        public readonly EventSender RemoveEvent = new();

        private readonly RenderSprite ghostSprite;

        protected float bounceAngle;

        protected Vector2 bouncePosition;

        protected Bouncer bouncer;

        protected Vector2 createPosition;

        protected bool creating;

        protected SuckerEndBodyClip end;

        protected Body endBody;

        protected MonsterEye eye;

        protected SuckerNeckSprite ghostNeck;

        protected Node ghostPimpa;

        protected Sprite limit;

        protected float maxDistance;

        protected float maxLength;

        protected Fixture middleFixture;

        protected SuckerNeckSprite neck;

        protected Node pimpa;

        protected Sprite pimpaHighlite;

        protected Vector2 pimpaPosition;

        protected bool pulled;

        protected Vector2 startDragPosition;

        protected Touch touch;
    }
}
