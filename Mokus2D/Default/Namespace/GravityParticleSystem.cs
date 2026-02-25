using Microsoft.Xna.Framework;

using Mokus2D.Visual.Data;

namespace Default.Namespace
{
    public class GravityParticleSystem : ParticleSystem
    {
        public Range Speed
        {
            get => speed; set => speed = value;
        }

        public Range Angle
        {
            get => angle; set => angle = value;
        }

        public Range AngularSpeed
        {
            get => angularSpeed; set => angularSpeed = value;
        }

        public Range ParticlesScale
        {
            get => particlesScale; set => particlesScale = value;
        }

        public Range HorizontalPosition
        {
            get => horizontalPosition; set => horizontalPosition = value;
        }

        public Range VerticalPosition
        {
            get => verticalPosition; set => verticalPosition = value;
        }

        public Vector2 TopRightBound
        {
            get => topRightBound; set => topRightBound = value;
        }

        public Vector2 BottomLeftBound
        {
            get => bottomLeftBound; set => bottomLeftBound = value;
        }

        public Range StartOpacity
        {
            get => startOpacity; set => startOpacity = value;
        }

        public Vector2 Gravity
        {
            get => gravity; set => gravity = value;
        }

        public GravityParticleSystem(string textureName)
            : base(textureName)
        {
        }

        public GravityParticleSystem(TextureData config, int count)
            : base(config, count)
        {
        }

        public GravityParticleSystem(TextureData config)
            : base(config)
        {
        }

        public GravityParticleSystem(string textureName, int count)
            : base(textureName, count)
        {
        }

        protected override void OnShowParticle(Particle particle)
        {
            initParticle((GravityParticle)particle);
        }

        public virtual void initParticle(GravityParticle gravityParticle)
        {
            float valueInRange = speed.GetValueInRange();
            float num = MathHelper.ToRadians(angle.GetValueInRange());
            Vector2 vector = new(Maths.Cos(num) * valueInRange, Maths.Sin(num) * valueInRange);
            gravityParticle.Speed = vector;
            gravityParticle.Opacity = (int)startOpacity.GetValueInRange();
            gravityParticle.AngularSpeed = angularSpeed.GetValueInRange();
            gravityParticle.Scale = particlesScale.GetValueInRange();
            gravityParticle.Position = new Vector2(horizontalPosition.GetValueInRange(), verticalPosition.GetValueInRange());
        }

        public void CreateOnStartPosition(int count)
        {
            while (particles.Count < count)
            {
                Vector2 vector = new(horizontalPosition.GetValueInRange(), verticalPosition.GetValueInRange());
                AddParticle(vector);
            }
        }

        public void CreateBetweenBounds(int count)
        {
            while (particles.Count < count)
            {
                Vector2 vector = new(Maths.RandRangeMinMax(bottomLeftBound.X, topRightBound.X), Maths.RandRangeMinMax(bottomLeftBound.Y, topRightBound.Y));
                AddParticle(vector);
            }
        }

        public override Particle CreateParticle()
        {
            GravityParticle gravityParticle = new(this);
            initParticle(gravityParticle);
            return gravityParticle;
        }

        public override void UpdateParticleTime(Particle particle, float time)
        {
            GravityParticle gravityParticle = (GravityParticle)particle;
            gravityParticle.Speed += gravity * time;
            Vector2 vector = gravityParticle.Speed * time;
            gravityParticle.Position += vector;
            gravityParticle.Rotation += gravityParticle.AngularSpeed;
            base.UpdateParticleTime(particle, time);
            if (gravityParticle.Position.X > topRightBound.X || gravityParticle.Position.Y > topRightBound.Y || gravityParticle.Position.X < bottomLeftBound.X || gravityParticle.Position.Y < bottomLeftBound.Y)
            {
                initParticle(gravityParticle);
            }
        }

        protected Vector2 gravity;

        protected Range speed = new();

        protected Range angle = new();

        protected Range horizontalPosition = new();

        protected Range verticalPosition = new();

        protected Range angularSpeed = new();

        private Range particlesScale = new(1f, 0f);

        protected Range startOpacity = new(255f, 0f);

        protected Vector2 bottomLeftBound = new(-100100100f, -100100100f);

        protected Vector2 topRightBound = new(100100100f, 100100100f);
    }
}
