using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;

namespace Default.Namespace
{
    public class PlasticineItem : LinkedListItem
    {
        public float InitialAngle => initialAngle;

        public Vector2 InitialPosition => initialPosition;

        public PlasticineItem(PlasticinePartBodyClip _bodyClip, float _width)
            : base(_bodyClip)
        {
            _bodyClip.Item = this;
            _bodyClip.SetDirty();
            width = _width;
            world = BodyClip.Builder.World;
            initialPosition = Body.Position;
            initialAngle = Body.Rotation;
            innerPosition = GetBorderVec(-0.33333334f);
            outerPosition = GetBorderVec(1.3333334f);
            normalVec = outerPosition - innerPosition;
            ortoAngle = -Maths.atan2(normalVec.Y, normalVec.X) + 1.5707964f;
            ortogonalVec = normalVec.Rotate90();
        }

        private float Width => width;

        public void UpdateTouchesBodyClipDistance(float offset, BodyClip objectP, float maxDistance)
        {
            BodyClip.OnTouchWith(offset, objectP);
            PlasticineItem plasticineItem = PreviousItem;
            float num;
            for (num = width / 2f + offset + plasticineItem.Width / 2f; num < maxDistance; num += plasticineItem.Width / 2f)
            {
                plasticineItem.BodyClip.OnTouchWith(num, objectP);
                num += plasticineItem.Width / 2f;
                plasticineItem = plasticineItem.PreviousItem;
            }
            plasticineItem = NextItem;
            num = offset - width / 2f - plasticineItem.Width / 2f;
            while (Maths.Abs(num) < maxDistance)
            {
                plasticineItem.BodyClip.OnTouchWith(num, objectP);
                num -= plasticineItem.Width / 2f;
                plasticineItem = plasticineItem.NextItem;
                num -= plasticineItem.Width / 2f;
            }
        }

        public void SetTargetPointAngle(Vector2 touchPosition, float angle)
        {
            BodyClip.SetTargetPositionAngle(touchPosition, angle);
        }

        public Vector2 GetTargetPoint(Vector2 touchPosition)
        {
            Vector2 vector = touchPosition - initialPosition;
            float num;
            if (FarseerUtil.GetProjectionTarget(vector, BodyClip.Normal) < 0f)
            {
                num = 0.33333334f;
            }
            else
            {
                num = 1.3333334f;
            }
            float num2 = -FarseerUtil.GetProjectionTarget(vector, ortogonalVec);
            if (Maths.Abs(num2) > 0.4f)
            {
                float projectionTarget = FarseerUtil.GetProjectionTarget(vector, normalVec);
                num2 = Maths.Clamp(num2, -0.4f, 0.4f);
                vector = new Vector2(num2, projectionTarget);
                vector = FarseerUtil.rotate(vector, -ortoAngle);
            }
            if (vector.Length() <= num)
            {
                return initialPosition + vector;
            }
            vector.Normalize();
            vector *= num;
            return vector + initialPosition;
        }

        public PlasticineItem NextItem => (PlasticineItem)Next;

        public PlasticineItem PreviousItem => (PlasticineItem)Previous;

        public Vector2 GetSurfaceCenterVec()
        {
            return BodyClip.GetSurfaceCenter();
        }

        public Vector2 GetLeftOffset(float offset)
        {
            return BodyClip.Builder.ToPoint(BodyClip.GetLeftOffset(offset));
        }

        public Vector2 GetRightOffset(float offset)
        {
            return BodyClip.Builder.ToPoint(BodyClip.GetRightOffset(offset));
        }

        public Vector2 GetCenterOffset(float offset)
        {
            return BodyClip.Builder.ToPoint(BodyClip.GetCenterOffset(offset));
        }

        public Vector2 GetSurfaceCenter()
        {
            Vector2 surfaceCenter = BodyClip.GetSurfaceCenter();
            return BodyClip.Builder.ToPoint(surfaceCenter);
        }

        public Vector2 GetLeft()
        {
            return Body.GetWorldPoint(new Vector2(-width / 2f, 0f));
        }

        public Vector2 GetRight()
        {
            return Body.GetWorldPoint(new Vector2(width / 2f, 0f));
        }

        public Vector2 GetBorderVec(float offset)
        {
            return Body.GetWorldPoint(new Vector2(width / 2f, 0.41666666f + offset));
        }

        public Vector2 GetBorder(float offset)
        {
            Box2DConfig engineConfig = BodyClip.Builder.EngineConfig;
            return engineConfig.ToPoint(GetBorderVec(offset));
        }

        public Vector2 GetSurfacePosition(float horizontalOffset)
        {
            return Body.GetWorldPoint(new Vector2(horizontalOffset, 0.5833333f));
        }

        public Vector2 GetLeftSurfacePosition()
        {
            return Body.GetWorldPoint(GetLeftSurfacePositionLocal());
        }

        public Vector2 GetNextAnchorPosition()
        {
            return Body.GetWorldPoint(new Vector2(width * 2f, 0f));
        }

        public Vector2 GetPreviuosAnchorPosition()
        {
            return Body.GetWorldPoint(new Vector2(-width * 2f, 0f));
        }

        public Vector2 GetRightSurfacePosition()
        {
            return Body.GetWorldPoint(GetRightSurfacePositionLocal());
        }

        public Vector2 GetRightSurfacePositionLocal()
        {
            return new Vector2(width / 2f, 0.5833333f);
        }

        public Vector2 GetLeftSurfacePositionLocal()
        {
            return new Vector2(-width / 2f, 0.5833333f);
        }

        public Body Body => BodyClip.Body;

        public PlasticinePartBodyClip BodyClip => (PlasticinePartBodyClip)item;

        protected float width;

        protected World world;

        protected Vector2 initialPosition = Vector2.Zero;

        protected float initialAngle;

        protected Vector2 innerPosition;

        protected Vector2 outerPosition;

        protected Vector2 normalVec;

        protected float ortoAngle;

        protected Vector2 ortogonalVec;
    }
}
