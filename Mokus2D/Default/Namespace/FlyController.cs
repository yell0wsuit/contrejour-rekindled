using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class FlyController(ContreJourGame _game, PlasticinePartBodyClip _ground, Particle _particle) : FlyBase(_particle, Maths.RandRangeMinMax(0.8f, 1.2f))
    {
        public void Scare(int direction)
        {
            if (scared == 0)
            {
                scared = direction;
            }
            scareTime = Maths.RandRangeMinMax(0.5f, 2.5f);
        }

        public new void Update(float time)
        {
            if (initialGroundY == -1f)
            {
                initialGroundY = ground.GrassController.Y;
            }
            if (!game.Debug)
            {
                targetPosition = ChooseTarget();
                if (scared != 0)
                {
                    targetPosition.X = targetPosition.X + (scared * scareOffset.X);
                    targetPosition.Y = targetPosition.Y + scareOffset.Y;
                    stepY = Maths.Abs(particle.Position.Y - targetPosition.Y) / scareOffset.Y;
                    scareTime -= time;
                    if (scareTime <= 0f)
                    {
                        Unscare();
                    }
                }
                base.Update(time);
            }
        }

        private Vector2 ChooseTarget()
        {
            return new Vector2(initialPosition.X + (horizontalOffset * game.WindManager.GetWind(windOffset)), initialPosition.Y + (verticalOffset * Maths.Sin(verticalStep)) + (ground.GrassController.Y - initialGroundY));
        }

        public void Unscare()
        {
            scared = 0;
        }

        protected const float MIN_VERTICAL_OFFSET = -5f;

        protected const float MAX_VERTICAL_OFFSET = 20f;

        protected const float MAX_HORIZONTAL_OFFSET = 20f;

        protected ContreJourGame game = _game;

        protected PlasticinePartBodyClip ground = _ground;

        protected Vector2 scareOffset = CocosUtil.ccpIPad(Maths.RandRangeMinMax(10f, 30f), Maths.RandRangeMinMax(30f, 50f));

        protected int scared = 0;

        protected float scareTime = 0f;

        protected float initialGroundY = -1f;

        protected float windOffset = Maths.RandRangeMinMax(-0.5f, 0.5f);

        protected float horizontalOffset = Maths.RandRangeMinMax(10f, 20f);

        protected float verticalOffset = Maths.RandRangeMinMax(-5f, 20f);

        protected float verticalOffsetMultiplier = Maths.RandRangeMinMax(1f, 2f);
    }
}
