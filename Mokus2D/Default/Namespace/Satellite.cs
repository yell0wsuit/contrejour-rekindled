using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class Satellite : IRemovable, IUpdatable
    {
        public Particle Clip => clip;

        public float SpeedValue
        {
            get => speedValue; set => speedValue = value;
        }

        public float AngleStep
        {
            get => angleStep; set => angleStep = value;
        }

        public Satellite(ContreJourGame _game, Particle _clip, BodyClip parent, float _direction, Vector2 position)
        {
            target = parent;
            initialPosition = position;
            clip = _clip;
            speedValue = Maths.RandRangeMinMax(CocosUtil.iPadValue(15f), CocosUtil.iPadValue(25f)) * 2f;
            angleStep = Maths.RandRangeMinMax(0.18f, 0.28f);
            direction = _direction;
            clip.Position = position;
            if (_game != null)
            {
                game = _game;
                game.AddUpdatable(this);
            }
            hasRemove = false;
        }

        protected virtual Vector2 TargetPosition => target == null ? initialPosition : game.Builder.ToIPadPoint(target.Body.Position);

        public virtual void Update(float time)
        {
            float num = Maths.min(time, 0.033333335f);
            Vector2 targetPosition = TargetPosition;
            Vector2 vector = targetPosition - clip.Position;
            float num2 = Maths.atan2(vector.Y, vector.X);
            num2 = Maths.SimplifyAngleRadiansStartValue(num2, direction - 3.1415927f);
            float num3 = angleStep * num * 30f;
            direction = Maths.StepToTargetMaxStep(direction, num2, num3);
            Vector2 vector2 = Maths.ToPointAngle(speedValue * num, direction);
            clip.Position = clip.Position + vector2;
        }

        public bool HasRemove()
        {
            return hasRemove;
        }

        public void Remove()
        {
            clip.Visible = false;
            hasRemove = true;
        }

        private const float MAX_STEP = 0.1f;

        private const float MIN_STEP = 0.05f;

        protected float direction;

        protected float speedValue;

        protected float angleStep;

        protected bool hasRemove;

        protected Particle clip;

        protected ContreJourGame game;

        protected Vector2 initialPosition;

        protected BodyClip target;
    }
}
