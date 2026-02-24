using Microsoft.Xna.Framework;

using Mokus2D.Util.MathUtils;

namespace Default.Namespace
{
    public class RotatorGrass
    {
        public float InitialAngle
        {
            get
            {
                return initialAngle;
            }
            set
            {
                initialAngle = value;
                initialDegrees = MathHelper.ToDegrees(value) - 90f;
                particle.Rotation = initialDegrees;
            }
        }

        public float ContactAngle
        {
            get
            {
                return contactAngle;
            }
            set
            {
                contactAngle = value;
            }
        }

        public Particle Particle
        {
            get
            {
                return particle;
            }
            set
            {
                particle = value;
            }
        }

        public RotatorGrass(Particle _particle)
        {
            rotationChanger = new CosChanger(-15f, 15f, Maths.randRange(0.005f, 0.01f));
            rotationChanger.Progress = Maths.randRange(0f, 6.2831855f);
            particle = _particle;
            contactAngle = 0f;
            currentContactAngle = 0f;
        }

        public void UpdateAngle(float time, float angle)
        {
            rotationChanger.Update(time);
            if (Maths.Abs(currentContactAngle) > Maths.Abs(contactAngle))
            {
                currentContactAngle = Maths.stepTo(currentContactAngle, contactAngle, 0.05f);
            }
            else
            {
                currentContactAngle = contactAngle;
            }
            float num = initialDegrees + angle + rotationChanger.Value + MathHelper.ToDegrees(currentContactAngle);
            float num2 = Maths.min(Maths.Abs(particle.Rotation - num) / 7f, 3f);
            particle.Rotation = Maths.stepTo(particle.Rotation, num, num2);
        }

        protected CosChanger rotationChanger;

        protected float initialAngle;

        protected float initialDegrees;

        protected float contactAngle;

        protected float currentContactAngle;

        protected Particle particle;
    }
}
