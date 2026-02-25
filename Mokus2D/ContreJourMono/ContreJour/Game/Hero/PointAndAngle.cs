using System;

using Microsoft.Xna.Framework;

using Mokus2D.Default.Namespace;
using Mokus2D.Util.MathUtils;

namespace Mokus2D.ContreJourMono.ContreJour.Game.Hero
{
    public class PointAndAngle(float length, float angleStep, float angleOffset)
    {
        public float AngleStep => angleStep;

        public Vector2 Position => Maths.RotateAngle(position, Angle);

        public void Update(float speed, bool onGround, float timeCoeff)
        {
            float num = speed.Clamp(0.5f, 1.1f) * length;
            position.X = position.X.StepTo(num, timeCoeff);
            position.Y = amplitude * (float)Math.Cos((double)(fawnProgress + angleOffset));
            if (onGround)
            {
                fawnProgress += Math.Max(0.3f * speed, 0.2f * timeCoeff);
                return;
            }
            fawnProgress += 0.3f * timeCoeff;
        }

        private const float FAWN_MIN_STEP = 0.2f;

        private const float FAWN_STEP = 0.3f;

        public float Angle;
        private Vector2 position = new(length * 0.5f, 0f);
        private readonly float amplitude = 4f;

        private float fawnProgress;
    }
}
