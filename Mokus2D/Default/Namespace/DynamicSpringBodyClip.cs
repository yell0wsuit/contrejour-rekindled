using Microsoft.Xna.Framework;

using Mokus2D.Visual;

namespace Default.Namespace
{
    public class DynamicSpringBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config) : SpringBodyClip(_builder, _body, _clip, _config)
    {
        private void RefreshPositions()
        {
            relativePosition = Body.GetLocalPoint(sticked.Body.Position);
            worldPosition = sticked.Body.Position;
            oldAngleForSticked = Body.Rotation;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (TransformChanged)
            {
                RefreshPoints();
                oldPosition = Body.Position;
                oldAngle = Body.Rotation;
            }
        }

        protected override void UpdateSticked()
        {
            if (TransformChanged)
            {
                Vector2 worldPoint = Body.GetWorldPoint(relativePosition);
                Vector2 vector = worldPoint - worldPosition;
                float num = Body.Rotation - oldAngleForSticked;
                sticked.Body.SetTransform(sticked.Body.Position + vector, sticked.Body.Rotation + num);
                RefreshPoints();
                sticked.UpdatePosition();
            }
            base.UpdateSticked();
            RefreshPositions();
        }

        private bool TransformChanged
        {
            get
            {
                return !FarseerUtil.b2Vec2Equal(oldPosition, Body.Position) || oldAngle != Body.Rotation;
            }
        }

        protected override void SetSticked(ILaunchable value)
        {
            base.SetSticked(value);
            if (value != null)
            {
                RefreshPositions();
            }
        }

        protected Vector2 relativePosition;

        protected Vector2 worldPosition;

        protected Vector2 oldPosition;

        private float oldAngle;

        protected float oldAngleForSticked;
    }
}
