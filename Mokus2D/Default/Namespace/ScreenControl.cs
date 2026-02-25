using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;
using Mokus2D.Input;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class ScreenControl : IRemovable, IUpdatable
    {
        public bool Dragging => touch != null;

        public Touch Touch => touch;

        public bool ZoomOut
        {
            get => zoomOut;
            set
            {
                if (zoomOut != value)
                {
                    zoomOut = value;
                    if (!value)
                    {
                        moved = false;
                        noMoveTime = 0f;
                    }
                }
            }
        }

        public bool TouchEnabled
        {
            get => touchEnabled; set => touchEnabled = value;
        }

        public bool Touched
        {
            get => touched;
            set
            {
                touched = value;
                noMoveTime = 0f;
                heroStill = false;
            }
        }

        public bool Enabled { get; set; }

        public ScreenControl(ContreJourGame _game)
        {
            game = _game;
            levelSize = _game.LevelSize;
            levelCenter = CocosUtil.toIPad(CocosUtil.sizeToPoint(levelSize)) * 0.5f;
            screenCenter = CocosUtil.ScreenCenter();
            maxValue = Vector2.Zero;
            minScale = Maths.min(MAX_SCREEN_SIZE.X / levelSize.Width, MAX_SCREEN_SIZE.Y / levelSize.Height);
            maxScale = 2f;
            currentCenter = levelCenter;
            SetGameScale(minScale);
            targetScale = minScale;
            noMoveTime = 3f;
            touchEnabled = true;
            heroStill = true;
            Enabled = true;
        }

        public void FocusOnHero()
        {
            if (!Dragging && !zooming)
            {
                noMoveTime = 0f;
            }
        }

        public void StartZoomTouch(Touch _touch)
        {
            if (!zoomTouches.Contains(_touch))
            {
                zoomTouches.Add(_touch);
                if (!zooming && zoomTouches.Count >= 2)
                {
                    zoomTouch1 = zoomTouches[0];
                    zoomTouch2 = zoomTouches[1];
                    zooming = true;
                    zoomStartDistance = ZoomPointsDistance();
                    zoomStartScale = game.GameRoot.Scale;
                    zoomCenter = game.GameRoot.GlobalToLocal(CocosUtil.ScreenCenter(), true);
                    if (Dragging)
                    {
                        TouchEnd(touch);
                    }
                }
            }
        }

        public void EndZoomTouch(Touch _touch)
        {
            if (_touch == touch)
            {
                TouchEnd(_touch);
            }
            if (zoomTouches.Contains(_touch))
            {
                zoomTouches.Remove(_touch);
                if (zooming && (_touch == zoomTouch1 || _touch == zoomTouch2))
                {
                    zooming = false;
                    zoomTouch1 = (zoomTouch2 = null);
                }
            }
        }

        public void SetGameScale(float value)
        {
            gameScale = value;
            game.GameRoot.Scale = value;
            minValue = ScreenConstants.W7FromIPhoneSize - CocosUtil.toIPad(levelSize) * gameScale;
        }

        public void SetGameScaleTargetCenter(float value, Vector2 targetCenter)
        {
            currentCenter = targetCenter;
            if (value < minScale)
            {
                targetCenter = levelCenter;
            }
            SetGameScale(value);
            game.GameRoot.Position = ClampPosition(GetRootPosition(targetCenter));
        }

        public void ForceZoomOut()
        {
            ZoomOut = true;
            gameScale = minScale;
            maxScale = minScale;
            targetScale = minScale;
        }

        public float ZoomPointsDistance()
        {
            Vector2 vector = CocosUtil.TouchPointInNode(zoomTouch1, game);
            Vector2 vector2 = CocosUtil.TouchPointInNode(zoomTouch2, game);
            return vector.DistanceTo(vector2);
        }

        public bool TouchMove(Touch _touch)
        {
            return true;
        }

        public void TouchBegan(Touch _touch)
        {
            if (zooming || targetScale == minScale)
            {
                return;
            }
            touch = _touch;
            startTouchPosition = CocosUtil.TouchPointInNode(touch, game);
            startRootPosition = game.GameRoot.Position;
        }

        public void TouchEnd(Touch _touch)
        {
            touch = null;
        }

        public bool HasRemove()
        {
            return false;
        }

        private void FixZoom()
        {
            if (!zooming && (gameScale < minScale || gameScale > maxScale))
            {
                float num = Maths.stepTo(gameScale, Maths.Clamp(gameScale, minScale, maxScale), 0.02f);
                SetGameScaleTargetCenter(num, currentCenter);
            }
        }

        private void FixBounds()
        {
            if (!zooming && !Dragging)
            {
                Vector2 vector = ClampPosition(game.GameRoot.Position);
                if (!Maths.ccpEqual(vector, game.GameRoot.Position))
                {
                    game.GameRoot.Position = Maths.stepToPoint(game.GameRoot.Position, vector, 5f);
                }
            }
        }

        public void Update(float time)
        {
        }

        public void PostponeMove()
        {
            moved = true;
            noMoveTime = 2f;
            lastHeroPosition = game.Hero.Position;
            heroStill = true;
        }

        public void UpdateTargetPositionAndScale(float time)
        {
            float num;
            if (!touched)
            {
                num = minScale;
            }
            else
            {
                num = (zoomOut ? minScale : targetScale);
            }
            if (noMoveTime > 0f || heroStill)
            {
                noMoveTime -= time;
                if (game.Hero.Position.DistanceTo(lastHeroPosition) > 40f)
                {
                    heroStill = false;
                    noMoveTime = 0f;
                }
                return;
            }
            if (gameScale != num)
            {
                float num2 = Maths.stepTo(gameScale, num, Math.Max(Math.Abs(num - gameScale) / 30f, 0.002f));
                Vector2 vector = game.Hero.Position;
                vector = game.GameRoot.GlobalToLocal(screenCenter, true);
                float num3 = vector.DistanceTo(game.Hero.Position);
                vector = Maths.stepToPoint(vector, game.Hero.Position, Maths.Clamp(num3 / 20f, 2f, 5f));
                SetGameScaleTargetCenter(num2, vector);
                return;
            }
            Vector2 vector2 = ClampPosition(GetRootPosition(game.Hero.Position));
            float num4 = game.GameRoot.Position.DistanceTo(vector2);
            game.GameRoot.Position = Maths.stepToPoint(game.GameRoot.Position, vector2, Math.Max(num4 / 10f, 2f));
        }

        public void UpdateZoom()
        {
            float num = ZoomPointsDistance();
            float num2 = zoomStartScale * num / zoomStartDistance;
            if (num2 < minScale)
            {
                moved = false;
                float num3 = minScale - num2;
                num3 = (float)Math.Pow((double)(num3 * 100f), 0.5) / 100f;
                num2 = minScale - num3;
            }
            else if (num2 > maxScale)
            {
                float num4 = num2 - maxScale;
                num4 = (float)Math.Pow((double)(num4 * 100f), 0.5) / 50f;
                num2 = maxScale + num4;
            }
            SetGameScaleTargetCenter(num2, zoomCenter);
            targetScale = Maths.Clamp(gameScale, minScale, maxScale);
        }

        public void UpdateDragging()
        {
            Vector2 vector = game.GlobalToLocal(touch.Position, true);
            Vector2 vector2 = startRootPosition + vector - startTouchPosition;
            Vector2 vector3 = ClampPosition(vector2);
            if (vector2 != vector3)
            {
                Vector2 vector4 = vector2 - vector3;
                float num = vector4.Length();
                vector4.Normalize();
                vector4 *= (float)Math.Pow((double)num, 0.6);
                vector2 = vector3 + vector4;
            }
            game.GameRoot.Position = Maths.stepToPoint(game.GameRoot.Position, vector2, gameScale * 10f);
            currentCenter = game.GameRoot.GlobalToLocal(CocosUtil.ScreenCenter(), true);
        }

        public Vector2 GetRootPosition(Vector2 center)
        {
            return GetRootPositionTargetScale(center, gameScale);
        }

        public Vector2 GetRootPositionTargetScale(Vector2 center, float scale)
        {
            center *= scale;
            return screenCenter - center;
        }

        private void SetCenterPosition(Vector2 center)
        {
            game.GameRoot.Position = GetRootPosition(center);
        }

        public Vector2 ClampPosition(Vector2 position)
        {
            return position.Clamp(minValue, maxValue);
        }

        private const float MAX_SCALE = 2f;

        private static readonly Vector2 MAX_SCREEN_SIZE = ScreenConstants.WP7_LEVEL_SIZE;

        protected ContreJourGame game;

        protected CGSize levelSize;

        protected Vector2 screenCenter;

        protected Vector2 levelCenter;

        protected Vector2 minValue;

        protected Vector2 maxValue;

        protected Touch touch;

        protected bool dragging;

        protected float minScale;

        protected float maxScale;

        protected Touch zoomTouch1;

        protected Touch zoomTouch2;

        protected float noMoveTime;

        protected float zoomStartDistance;

        protected float zoomStartScale;

        protected Vector2 zoomCenter;

        protected Vector2 zoomCenterOffset;

        protected Vector2 startTouchPosition;

        protected Vector2 startRootPosition;

        protected Vector2 lastHeroPosition;

        protected float gameScale = 1f;

        protected float targetScale;

        protected bool zoomOut;

        protected bool touchEnabled;

        protected List<Touch> zoomTouches = new();

        protected bool zooming;

        protected Vector2 currentCenter;

        protected bool moved;

        protected bool heroStill;

        protected bool touched;
    }
}
