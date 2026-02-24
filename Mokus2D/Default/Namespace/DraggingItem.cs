using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Mokus2D.Input;
using Mokus2D.Util.Data;

namespace Default.Namespace
{
    public class DraggingItem
    {
        public DraggingItem(LevelBuilderBase _builder, PlasticineItem _item, Touch _touch)
        {
            currentTouch = _touch;
            dragItem = _item;
            builder = _builder;
            game = (ContreJourGame)_builder.Game;
            initialTouchPosition = builder.TouchRootVec(_touch);
            lastTouchPosition = builder.TouchRootPoint(_touch);
            initialPosition = dragItem.Body.Position;
            left = PlasticineUtil.GetItemCountDirection(dragItem, 7, -1);
            right = PlasticineUtil.GetItemCountDirection(dragItem, 7, 1);
            SetDragging(true);
        }

        public void SetDragging(bool value)
        {
            PlasticineItem plasticineItem = dragItem.PreviousItem;
            PlasticineItem plasticineItem2 = dragItem.NextItem;
            int i = 0;
            dragItem.BodyClip.Dragging = value;
            while (i < 7)
            {
                plasticineItem.BodyClip.Dragging = value;
                plasticineItem2.BodyClip.Dragging = value;
                plasticineItem = plasticineItem.PreviousItem;
                plasticineItem2 = plasticineItem2.NextItem;
                i++;
            }
        }

        private bool FixBorderDrag(Vector2 currentTouchPosition)
        {
            Vector2 vector = builder.ToVec(currentTouchPosition);
            float num = FarseerUtil.b2Vec2Distance(vector, dragItem.Body.Position);
            PlasticineItem plasticineItem = dragItem;
            bool flag;
            do
            {
                plasticineItem = plasticineItem.PreviousItem;
                float num2 = FarseerUtil.b2Vec2Distance(plasticineItem.Body.Position, vector);
                flag = num2 < num;
                if (flag)
                {
                    num = num2;
                }
            }
            while (flag);
            plasticineItem = plasticineItem.NextItem;
            if (plasticineItem == dragItem)
            {
                do
                {
                    plasticineItem = plasticineItem.NextItem;
                    float num3 = FarseerUtil.b2Vec2Distance(plasticineItem.Body.Position, vector);
                    flag = num3 < num;
                    if (flag)
                    {
                        num = num3;
                    }
                }
                while (flag);
                plasticineItem = plasticineItem.PreviousItem;
            }
            if (plasticineItem != dragItem)
            {
                float num4 = Maths.Abs(plasticineItem.BodyClip.InitialAngle - dragItem.BodyClip.InitialAngle);
                if (num4 >= 0.3926991f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateWithTouch(Touch touch)
        {
            currentTouch = touch;
            return Update();
        }

        public bool Update()
        {
            bool flag = false;
            Vector2 vector = CocosUtil.toIPhone(builder.TouchRootPoint(currentTouch));
            Vector2 vector2 = builder.ToVec(vector);
            if (!Maths.ccpEqual(vector, lastTouchPosition))
            {
                CalculatePositions();
                lastTouchPosition = vector;
                flag = true;
            }
            float projectionTarget = FarseerUtil.GetProjectionTarget(vector2 - dragItem.Body.Position, dragItem.BodyClip.Normal);
            if (Maths.Abs(projectionTarget) > 3.3333333f)
            {
                dragItem.BodyClip.Free(currentTouch);
            }
            return flag;
        }

        public void CalculateBasePoints()
        {
            basePoints.First = builder.ToPoint(left.GetRight());
            basePoints.Second = builder.ToPoint(right.GetLeft());
            baseCenter = Maths.GetCenterPointWith(basePoints.First, basePoints.Second);
            baseAnchors.First = builder.ToPoint(left.GetNextAnchorPosition());
            baseAnchors.Second = builder.ToPoint(right.GetPreviuosAnchorPosition());
            baseAngle = Maths.GetAngleTarget(baseAnchors.First, baseAnchors.Second);
        }

        public Pair<Vector2> GetTopPointsResultWidth(Vector2 top, float width)
        {
            Pair<Vector2> pair = default(Pair<Vector2>);
            Vector2 vector = Maths.ToPointAngle(width, baseAngle);
            pair.Second = top + vector;
            vector = Maths.ToPointAngle(width, baseAngle + 3.1415927f);
            pair.First = top + vector;
            return pair;
        }

        public Vector2 GetCurrentDragPoint()
        {
            return builder.ToPoint(GetCurrentDragPosition());
        }

        public Vector2 GetCurrentDragPosition()
        {
            Vector2 vector = builder.TouchRootVec(currentTouch);
            Vector2 vector2 = initialPosition;
            vector2 += vector;
            vector2 -= initialTouchPosition;
            return dragItem.GetTargetPoint(vector2);
        }

        public void CalculatePositions()
        {
            CalculateBasePoints();
            Vector2 currentDragPoint = GetCurrentDragPoint();
            Pair<Vector2> topPointsResultWidth = GetTopPointsResultWidth(currentDragPoint, 1.2f / builder.EngineConfig.SizeMultiplier);
            Pair<Vector2> topPointsResultWidth2 = GetTopPointsResultWidth(currentDragPoint, 0.3f / builder.EngineConfig.SizeMultiplier);
            Vector2 centerPointWith = Maths.GetCenterPointWith(baseAnchors.First, topPointsResultWidth.First);
            Vector2 centerPointWith2 = Maths.GetCenterPointWith(baseAnchors.Second, topPointsResultWidth.Second);
            List<Vector2> list = new();
            List<Vector2> list2 = new();
            list2.Add(basePoints.First);
            list2.Add(baseAnchors.First);
            list2.Add(centerPointWith);
            list2.Add(topPointsResultWidth.First);
            list2.Add(topPointsResultWidth2.First);
            Maths.AddBezierPointsPointsSegments(list, list2, 3);
            list2 = new List<Vector2>();
            list2.Add(topPointsResultWidth2.Second);
            list2.Add(topPointsResultWidth.Second);
            list2.Add(centerPointWith2);
            list2.Add(baseAnchors.Second);
            list2.Add(basePoints.Second);
            Maths.AddBezierPointsPointsSegments(list, list2, 3);
            for (int i = 0; i < list.Count; i++)
            {
                dragItem.BodyClip.Parent.SetDotPositionPosition(i, list[i]);
            }
            currentDragPoint.Y += 30f;
            dragItem.BodyClip.Parent.SetDotPositionPosition(0, currentDragPoint);
            UpdatePositions(list);
        }

        public void UpdatePositions(List<Vector2> positions)
        {
            int i = 0;
            PlasticineItem plasticineItem = left.NextItem;
            while (i < positions.Count - 1)
            {
                Vector2 vector = positions[i];
                Vector2 vector2 = positions[i + 1];
                Vector2 centerPointWith = Maths.GetCenterPointWith(vector, vector2);
                float angleTarget = Maths.GetAngleTarget(vector, vector2);
                plasticineItem.SetTargetPointAngle(builder.ToVec(centerPointWith), angleTarget);
                plasticineItem = plasticineItem.NextItem;
                i++;
            }
            plasticineItem = left.NextItem;
            for (i = 0; i < positions.Count; i++)
            {
                plasticineItem.BodyClip.SetDirty();
                plasticineItem = plasticineItem.NextItem;
            }
        }

        public void Finish()
        {
            SetDragging(false);
        }

        private const float FREE_ANGLE_DIFF = 0.3926991f;

        private const float ORTO_FREE_DISTANCE = 1f;

        private const float FREE_DISTANCE = 3.3333333f;

        public const int DRAG_COUNT = 7;

        protected Pair<Vector2> baseAnchors;

        protected float baseAngle;

        protected Vector2 baseCenter;

        protected Pair<Vector2> basePoints;

        protected LevelBuilderBase builder;

        protected Touch currentTouch;

        protected PlasticineItem dragItem;

        protected ContreJourGame game;

        protected Vector2 initialPosition;

        protected Vector2 initialTouchPosition;

        protected Vector2 lastTouchPosition;

        protected PlasticineItem left;

        protected Pair<Vector2> movingAnchors;

        protected PlasticineItem right;
    }
}
