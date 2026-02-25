using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class DustData : IUpdatable
    {
        public DustData(ContreJourGame _game, Vector2 bodySpeed, Vector2 position, float _speed, float alphaMult)
        {
            game = _game;
            alphaDiff = Maths.RandRangeMinMax(0.6f, 1.2f) / 255f;
            particle = game.Dust.AddOrGetInvisible();
            particle.Visible = true;
            position.Y -= CocosUtil.iPadValue(10f);
            particle.Position = position;
            particle.Scale = Maths.RandRangeMinMax(0.5f, 1.5f);
            particle.OpacityFloat = Maths.RandRangeMinMax(0.1f * alphaMult, 0.2f * alphaMult);
            speed = CocosUtil.toIPad(Box2DConfig.DefaultConfig.ToPoint(bodySpeed));
            speed.X = speed.X * Maths.RandRangeMinMax(0.2f, 0.4f);
            speed.Y = CocosUtil.iPadValue(Maths.min(Maths.RandRangeMinMax(5f, 20f) * _speed, 40f));
        }

        public bool Dragging
        {
            get => dragging; set => dragging = value;
        }

        public bool HasRemove => hasRemove;

        public void Update(float time)
        {
            float num = time * 3f * Maths.min(particle.Opacity / 255f, 0.3f);
            Vector2 vector = speed * num;
            particle.Position = particle.Position + vector;
            particle.OpacityFloat -= dragging ? (alphaDiff * 10f) : alphaDiff;
            if (particle.Opacity <= 0)
            {
                particle.Visible = false;
                hasRemove = true;
            }
        }

        protected float alphaDiff;

        protected bool dragging;

        protected ContreJourGame game;

        protected bool hasRemove;

        protected Particle particle;

        protected Vector2 speed;
    }
}
