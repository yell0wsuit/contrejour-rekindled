using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class Tablo : Sprite
    {
        public Tablo(string clipName)
            : base(clipName)
        {
            Scale = 0f;
            Rotation = -90f;
        }

        public Tablo()
            : this("McPlanetTablo")
        {
        }

        public bool Open
        {
            get => open;
            set
            {
                if (value != open)
                {
                    open = value;
                    StopAllActions();
                    float num = open ? 1 : 0;
                    float num2 = (open ? 0f : 1.5707964f);
                    Spawn spawn = new(
                    [
                        new ScaleTo(0.2f, num),
                        new RotateTo(0.2f, num2)
                    ]);
                    if (open)
                    {
                        Visible = true;
                        Run(spawn);
                        return;
                    }
                    Run(new Sequence(
                    [
                        spawn,
                        new Hide()
                    ]));
                }
            }
        }

        private const float EFFECT_TIME = 0.2f;

        protected bool open;
    }
}
