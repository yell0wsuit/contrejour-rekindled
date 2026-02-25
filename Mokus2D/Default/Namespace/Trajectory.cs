using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;

namespace Default.Namespace
{
    public class Trajectory : ParticleSystem
    {
        public Trajectory(ContreJourGame _game)
            : base(_game.BlackSide ? "McTrampolinePathBlack.png" : "McTrampolinePath.png", 7)
        {
            enabledOpacity = (_game.Chapter == 4) ? 0.7f : 0.5f;
            Visible = false;
            int num = 0;
            foreach (Particle particle in Particles)
            {
                particle.OpacityFloat = 1f - Maths.Abs(num - 3) / 4f;
                num++;
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            float num = Maths.Cos(Angle);
            float num2 = Maths.Sin(Angle);
            int num3 = 0;
            foreach (Particle particle in Particles)
            {
                particle.Position = new Vector2(num * (Impulse * num3 * 30f), num2 * (Impulse * num3 * 30f) - 2.2f * num3 * num3);
                num3++;
            }
        }

        public bool Enabled
        {
            get;
            set
            {
                if (field != value)
                {
                    field = value;
                    RefreshOpacity();
                }
            }
        }

        private void RefreshOpacity()
        {
            StopAllActions();
            if (Enabled)
            {
                Visible = true;
                Run(new FadeTo(0.5f, enabledOpacity));
                return;
            }
            Run(new Sequence(
            [
                new FadeOut(0.5f),
                new Hide()
            ]));
        }

        private const float PATH_PART_DISTANCE = 30f;

        private const int PATH_PARTS = 7;

        public float Impulse = 1f;

        public float Angle;
        private readonly float enabledOpacity;
    }
}
