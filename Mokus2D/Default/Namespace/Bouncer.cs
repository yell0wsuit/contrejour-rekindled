using Mokus2D.Util.MathUtils;

namespace Default.Namespace
{
    public class Bouncer(float _amplitude, float _amplitudeStep, float step) : IUpdatable
    {
        public float Amplitude
        {
            get
            {
                return amplitude;
            }
            set
            {
                amplitude = value;
            }
        }

        public float CurrentAmplitude
        {
            get
            {
                return currentAmplitude;
            }
        }

        public float AmplitudeStep
        {
            get
            {
                return amplitudeStep;
            }
            set
            {
                amplitudeStep = value;
            }
        }

        public float Step
        {
            get
            {
                return changer.Step;
            }
            set
            {
                changer.Step = value;
            }
        }

        public float Value
        {
            get
            {
                return changer.Value * currentAmplitude;
            }
        }

        public void Start()
        {
            currentAmplitude = amplitude;
            changer.Progress = 1.5707964f;
        }

        public void Update(float time)
        {
            if (Maths.FuzzyNotEquals(currentAmplitude, 0f, 0.0001f))
            {
                currentAmplitude = Maths.stepTo(currentAmplitude, 0f, amplitudeStep * time);
                changer.Update(time);
            }
        }

        protected float amplitude = _amplitude;

        protected float currentAmplitude;

        protected float amplitudeStep = _amplitudeStep;

        protected CosChanger changer = new(-1f, 1f, step);
    }
}
