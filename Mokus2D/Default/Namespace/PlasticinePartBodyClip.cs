using System;
using System.Collections.Generic;

using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Extensions;
using Mokus2D.Input;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class PlasticinePartBodyClip : ContreJourBodyClip, IClickable, IRestartable
    {
        public PlasticineItem Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }

        public bool UpdateParent
        {
            get
            {
                return updateParent;
            }
            set
            {
                updateParent = value;
            }
        }

        public PlasticineBodyClip Parent
        {
            get
            {
                return parent;
            }
        }

        public PlasticinePartHighlite Highlite
        {
            get
            {
                return highlite;
            }
            set
            {
                highlite = value;
            }
        }

        public bool Dragging
        {
            get
            {
                return dragging;
            }
            set
            {
                if (value != dragging)
                {
                    dragging = value;
                    if (!dragging)
                    {
                        groundFallTime = 0f;
                        FallGround();
                    }
                }
            }
        }

        public Vector2 Normal
        {
            get
            {
                return normal;
            }
        }

        public Vector2 Parallel
        {
            get
            {
                return parallel;
            }
        }

        public bool IsRotationDirty
        {
            get
            {
                return isRotationDirty;
            }
            set
            {
                isRotationDirty = value;
            }
        }

        public float InitialAngle
        {
            get
            {
                return initialAngle;
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public IGrassController GrassController
        {
            get
            {
                return grassController;
            }
        }

        public float Width
        {
            get
            {
                return width;
            }
        }

        public PlasticinePartBodyClip(LevelBuilderBase _builder, object _body, PlasticineBodyClip _parent, float _width, bool hasGrass)
            : base(_builder, _body, null, null)
        {
            width = _width;
            builder = _builder;
            parent = _parent;
            game = (ContreJourGame)_builder.Game;
            float num = ((Body)_body).Rotation;
            num = Maths.SimplifyAngleRadiansStartValue(num, -1.5707964f);
            initialPosition = Body.Position;
            targetPosition = initialPosition;
            targetAngle = Body.Rotation;
            initialAngle = Body.Rotation;
            float num2 = (game.WhiteSide ? 1.0471976f : 0.62831855f);
            if (Maths.Between(num, -num2, num2))
            {
                isFloor = true;
                if (hasGrass && CocosUtil.isArmV7())
                {
                    CreateGrass((ContreJourLevelBuilder)_builder, _parent, (Body)_body);
                }
            }
            globalIndex = i++;
            dirty = true;
            groundFallMaxTime = Maths.randRange(0.205f, 0.41f);
            groundFallTime = 0f;
            isTop = Maths.Between(num, 1.5707964f, 4.712389f);
            normal = GetSurfaceCenter() - Body.WorldCenter;
            parallel = Body.GetWorldPoint(new Vector2(1f, 0f)) - Body.WorldCenter;
            parent = _parent;
            Body body = (Body)_body;
            PlasticineConstants.ApplyStaticBodiesFilter(body);
            dynamic = 0;
        }

        public void MoveToInitialPosition()
        {
            SetTargetPositionAngle(initialPosition, initialAngle);
        }

        public void Restart()
        {
            if (updateParent)
            {
                parent.Restart();
            }
        }

        public void SetFillSprite(PlasticineSprite fillSprite, int fillIndex)
        {
            this.fillSprite = fillSprite;
            this.fillIndex = fillIndex;
        }

        public void SetWideBorder(PlasticineWideBorder _border, int _index)
        {
            index = _index;
            verticesOffset = index * 2 * 2 + 2;
            border = _border;
        }

        public bool UseForZoom()
        {
            return true;
        }

        public bool AcceptFreeTouches()
        {
            return true;
        }

        public void SetDirty()
        {
            parent.Changed = true;
            SetDirtyNoSibling();
            if (next != null)
            {
                next.SetDirtyNoSibling();
            }
            if (previous != null)
            {
                previous.SetDirtyNoSibling();
            }
        }

        public void SetDirtyNoSibling()
        {
            dirty = true;
            if (highlite != null)
            {
                highlite.SetDirty();
            }
        }

        public bool DisableHeroFocus
        {
            get
            {
                return true;
            }
        }

        public int Priority(Vector2 touchPoint)
        {
            return 0;
        }

        public bool TouchBegan(Touch touch)
        {
            return parent.StartDragItemTouch(item, touch);
        }

        public bool TouchMove(Touch touch)
        {
            return parent.TouchMove(touch);
        }

        public void TouchOut(Touch touch)
        {
        }

        public void TouchEnd(Touch touch)
        {
            parent.StopDragTouch(item, touch);
        }

        public void Free(Touch touch)
        {
            parent.StopDragTouch(item, touch);
            game.FreeTouch(touch);
        }

        public void SetTargetPositionAngle(Vector2 position, float angle)
        {
            if (Body.Position == position && Maths.FuzzyEquals(angle, Body.Rotation, 0.0001f))
            {
                return;
            }
            moving = true;
            lastFrameSpeed = position - Body.Position;
            lastFrameSpeed *= 1f / lastTime;
            targetAngle = Maths.SimplifyAngleRadiansStartValue(angle, Body.Rotation - 3.1415927f);
            targetPosition = position;
            if (lastFrameSpeed.Length() > 2f)
            {
                FallGround();
            }
        }

        public void FixTouchingSpeed(Vector2 currentSpeed)
        {
            for (ContactEdge contactList = Body.ContactList; contactList != null; contactList = contactList.Next)
            {
                if (contactList.Contact.IsTouching)
                {
                    Vector2 vector = currentSpeed * (50f * contactList.Other.Mass);
                    contactList.Other.ApplyForce(vector, contactList.Other.WorldCenter);
                }
            }
        }

        public void FallGround()
        {
            if (game.Debug)
            {
                return;
            }
            groundFallTime -= lastTime;
            if (isTop && groundFallTime <= 0f && lastFrameSpeed.Y > -5f && Maths.Rand() < 0.7f)
            {
                groundFallTime = groundFallMaxTime;
                Vector2 vector = Maths.randomPoint(item.GetLeftOffset(0.6666667f), item.GetRightOffset(0f));
                GravityParticle gravityParticle = (GravityParticle)game.GroundFall.AddOrGetInvisible();
                if (lastFrameSpeed.Y < 0f)
                {
                    gravityParticle.Speed = new Vector2(gravityParticle.Speed.X, gravityParticle.Speed.Y * 1.3f);
                    vector.Y += Maths.max(lastFrameSpeed.Y, CocosUtil.iPadValue(-35f));
                }
                gravityParticle.Position = CocosUtil.toIPad(vector);
            }
        }

        public override void Update(float time)
        {
            lastTime = time;
            if (previous == null)
            {
                previous = item.PreviousItem.BodyClip;
            }
            if (next == null)
            {
                next = item.NextItem.BodyClip;
                SetDirty();
            }
            MoveToTargetPosition(time);
            if (grassController != null && !game.Debug)
            {
                grassController.Update(time);
            }
            if (dust.Count > 0)
            {
                ArrayList arrayList = new();
                foreach (DustData dustData in dust)
                {
                    if (dragging)
                    {
                        dustData.Dragging = true;
                    }
                    dustData.Update(time);
                    if (dustData.HasRemove)
                    {
                        arrayList.Add(dustData);
                    }
                }
                foreach (object obj in arrayList)
                {
                    dust.Remove((DustData)obj);
                }
            }
            if (dragging && circle != null)
            {
                UpdateCircle();
            }
        }

        public void AddCircle(int _index)
        {
            int num = (_index + 1) % 2;
            circle = ClipFactory.CreateWithAnchor(string.Format("McGroundCircle{0}", num));
            circle.Opacity = 160;
            circleSize = ClipFactory.GetNodeSize(circle).Width;
            circleScale = Maths.randRange(0.8f, 1.3f);
            circle.Scale = circleScale;
            float num2 = (-circleSize * circle.Scale - Maths.randRange(-10f, 0f)) * builder.EngineConfig.SizeMultiplier;
            circlePosition = new Vector2(0f, num2);
            Vector2 worldPoint = Body.GetWorldPoint(circlePosition);
            circle.Position = builder.ToPoint(worldPoint);
            builder.Add(circle, 100);
        }

        public void UpdateWideBorderAndFill()
        {
            if (border != null && dirty)
            {
                VertexPositionColorTexture[] inBorder = border.InBorder;
                VertexPositionColorTexture[] outBorder = border.OutBorder;
                if (index == 0)
                {
                    inBorder[0].Position = CocosUtil.ccp2Point(item.GetSurfaceCenter()).ToVector3();
                    inBorder[1].Position = CocosUtil.ccp2Point(item.GetCenterOffset(-0.41666666f)).ToVector3();
                    outBorder[0].Position = CocosUtil.ccp2Point(item.GetSurfaceCenter()).ToVector3();
                    outBorder[1].Position = CocosUtil.ccp2Point(item.GetCenterOffset(0.5833333f + CocosUtil.retinaValue(0.06666667f))).ToVector3();
                }
                SetBezierPointsOffsetIndexOffset(inBorder, 0.5833333f, 0);
                SetBezierPointsOffsetIndexOffset(inBorder, -0.41666666f, 1);
                SetBezierPointsOffsetIndexOffset(outBorder, 0.5833333f, 0);
                SetBezierPointsOffsetIndexOffset(outBorder, 0.5833333f + CocosUtil.retinaValue(0.06666667f), 1);
                if (fillSprite != null)
                {
                    fillSprite.Vertices[fillIndex].Position = CocosUtil.ccp2Point(item.GetCenterOffset(-0.104166664f)).ToVector3();
                }
                dirty = false;
            }
        }

        public void SetBezierPointsOffsetIndexOffset(VertexPositionColorTexture[] vector, float offset, int indexOffset)
        {
            Vector2 centerOffset = GetCenterOffset(offset);
            Vector2 centerOffset2 = item.NextItem.BodyClip.GetCenterOffset(offset);
            Vector2 rightOffset = GetRightOffset(offset);
            Vector2 vector2 = FarseerUtil.b2Vec2Middle(centerOffset, centerOffset2);
            Vector2 vector3 = FarseerUtil.b2Vec2Middle(rightOffset, vector2);
            vector[verticesOffset + indexOffset].Position = CocosUtil.ccp2Point(builder.ToPoint(vector3)).ToVector3();
            vector[verticesOffset + indexOffset + 2].Position = CocosUtil.ccp2Point(builder.ToPoint(centerOffset2)).ToVector3();
        }

        public void MoveToTargetPosition(float time)
        {
            if (Maths.FuzzyNotEquals(targetAngle, Body.Rotation, 0.0001f))
            {
                Body.SetTransform(Body.Position, targetAngle);
            }
            if (!FarseerUtil.FuzzyEquals(targetPosition, Body.Position, 0.01f))
            {
                Vector2 vector = targetPosition - Body.Position;
                Vector2 vector2 = vector;
                vector *= 0.4f;
                vector2 *= 0.5f / time;
                float num = vector2.Length();
                if (num > 4.5f)
                {
                    vector2 *= 4.5f / num;
                }
                Vector2 vector3 = Body.Position + vector;
                Body.LinearVelocity = vector2;
                Body.SetTransform(vector3, Body.Rotation);
                SetRotationDirty();
                if (isTop)
                {
                    FallGround();
                }
            }
            else if (!FarseerUtil.b2Vec2Equal(Body.LinearVelocity, Vector2.Zero))
            {
                Body.LinearVelocity = Vector2.Zero;
                SetRotationDirty();
            }
            else if (isRotationDirty)
            {
                SetDirty();
                isRotationDirty = false;
                fixHighlite = true;
            }
            if (fixHighlite)
            {
                SetDirty();
                fixHighlite = false;
            }
        }

        public void UpdateCircle()
        {
            float num = FarseerUtil.GetProjectionTarget(Body.Position - initialPosition, normal) / builder.EngineConfig.SizeMultiplier;
            float num2 = ((num > 0f) ? (num / circleSize / 2f) : 0f);
            circle.Scale = circleScale + num2;
        }

        public void SetRotationDirty()
        {
            SetDirty();
            isRotationDirty = true;
            previous.IsRotationDirty = true;
            next.IsRotationDirty = true;
        }

        public void OnTouchWith(float offset, BodyClip objectP)
        {
            if (grassController != null)
            {
                grassController.OnTouchWith(offset, objectP);
            }
        }

        public void ScareFlyes(int offset)
        {
            if (grassController != null)
            {
                grassController.ScareFlyes(offset);
            }
        }

        public void CreateGrass(ContreJourLevelBuilder _builder, PlasticineBodyClip _parent, Body _body)
        {
            if (_builder.ContreJour.BlackSide || _builder.ContreJour.Debug)
            {
                return;
            }
            Type type = _builder.ContreJour.ChooseSide(null, typeof(WhiteGrassController), typeof(WhiteGrassController), typeof(GrassController), typeof(WhiteGrassController));
            grassController = (GrassController)ReflectUtil.CreateInstance(type, [this]);
        }

        public Vector2 GetLocalSurfaceCenter()
        {
            return new Vector2(0f, 0.5833333f);
        }

        public Vector2 GetSurfaceCenter()
        {
            return Body.GetWorldPoint(GetLocalSurfaceCenter());
        }

        public Vector2 GetLeftOffset(float offset)
        {
            return Body.GetWorldPoint(new Vector2(-width / 2f, offset));
        }

        public Vector2 GetRightOffset(float offset)
        {
            return Body.GetWorldPoint(new Vector2(width / 2f, offset));
        }

        public Vector2 GetCenterOffset(float offset)
        {
            return Body.GetWorldPoint(new Vector2(0f, offset));
        }

        public override void OnCollisionPoint(Body body2, Contact point)
        {
            BodyClip bodyClip = (BodyClip)body2.UserData;
            if (bodyClip != null)
            {
                point.GetWorldManifold(out Vector2 vector, out FixedArray2<Vector2> fixedArray);
                Vector2 localPoint = Body.GetLocalPoint(fixedArray[0]);
                float x = localPoint.X;
                item.UpdateTouchesBodyClipDistance(x, bodyClip, 1.3333334f);
                localPoint.Y += 0.2f;
                if (isFloor && !dragging && bodyClip.Config.GetBool("hasDust"))
                {
                    AddDustPointPosition(body2, point, localPoint);
                }
            }
        }

        public override void OnCollisionStartPoint(Body body2, Contact point)
        {
        }

        public void AddDustPointPosition(Body body2, Contact point, Vector2 position)
        {
            if (game.Debug)
            {
                return;
            }
            float num = body2.LinearVelocity.Length();
            if (num >= 1f && Maths.randRange01() < 0.33f)
            {
                position = Body.GetWorldPoint(position);
                DustData dustData = new(game, body2.LinearVelocity, builder.ToIPadPoint(position), num, game.WhiteSide ? 2 : 1);
                dust.Add(dustData);
            }
        }

        private const float FIX_LIMIT_OFFSET = 0.6666667f;

        private const float GROUND_FALL_OFFSET = 0.6666667f;

        private const float GROUND_FALL_TIMEOUT = 0.205f;

        private const float SPEED_MULT = 0.5f;

        private const float MAX_GROUND_VELOCITY = 4.5f;

        private const float MOVE_SPEED_MULT = 0.4f;

        private const float MAX_X_SPEED = 4f;

        private const float JUMP_SPEED_MULT = 0.33333334f;

        private const float JUMP_FORCE_MULT = 50f;

        private const float MAX_JUMP_SPEED = 5f;

        private const float MOVE_FORCE = 1.2f;

        private const float DUST_OFFSET = 0.2f;

        public const float GRASS_TRAMPLE_DISTANCE = 1.3333334f;

        private const float GRASS_ANGLE_WHITE = 1.0471976f;

        private const float GRASS_ANGLE = 0.62831855f;

        private const float GRASS_RANDOM = 0.55f;

        private const float MIN_DUST_SPEED = 1f;

        private const float DUST_RANDOM = 0.33f;

        protected int globalIndex;

        protected int index;

        protected int verticesOffset;

        protected bool dirty;

        private PlasticineWideBorder border;

        private PlasticineSprite fillSprite;

        private int fillIndex = -1;

        protected PlasticinePartHighlite highlite;

        protected Vector2 initialPosition;

        protected float initialAngle;

        protected Vector2 targetPosition;

        protected float targetAngle;

        protected ContreJourGame game;

        protected List<DustData> dust = new(64);

        protected PlasticineItem item;

        protected bool updateParent;

        protected PlasticineBodyClip parent;

        protected float width;

        protected float groundFallTime;

        protected float groundFallMaxTime;

        protected bool isFloor;

        protected bool isTop;

        protected int dynamic;

        protected bool moving;

        protected bool dragging;

        protected float lastTime;

        protected Vector2 moveForce;

        protected Vector2 lastFrameSpeed;

        protected Vector2 normal;

        protected Vector2 parallel;

        protected IGrassController grassController;

        protected PlasticinePartBodyClip previous;

        protected PlasticinePartBodyClip next;

        protected bool isRotationDirty;

        protected Vector2 fixPosition;

        protected bool fixHighlite;

        protected Sprite circle;

        protected Vector2 circlePosition;

        protected float circleSize;

        protected float circleScale;

        private static int i;
    }
}
