using System.IO;

using Mokus2D.Default.Namespace;

using Microsoft.Xna.Framework;

namespace Mokus2D.ContreJourMono.ContreJour.Menu.LevelComplete
{
    public class FakeHeroEye : RandomAnimationEye
    {
        protected override float ViewRadius => 12f;

        protected override EyeAnimation[] Animations => [];

        public FakeHeroEye()
            : base(null, true, new Vector2(80f, 80f))
        {
            AnimationsAllowed = false;
            EyeStep = 2f;
            UpdateEnabled = true;
        }

        public void Smile()
        {
            bool flag = true;
            PlayAnimation(new EyeAnimation("McFakeHeroEyeSmile", null, flag, false), true);
        }

        public void Open()
        {
            PlayAnimation(new EyeAnimation("McFakeHeroEyeOpen", null, false, false), true);
        }

        public void Blink()
        {
            PlayAnimation(new EyeAnimation("McFakeHeroEyeBlink", null, false, false), true);
        }

        protected override string ProcessName(string name)
        {
            return Path.Combine("Menu/FakeHero", base.ProcessName(name));
        }

        protected override void CreateDefaultView()
        {
            background = ClipFactory.CreateWithAnchor(ProcessName("McFakeHeroEye"));
            eyeBall = ClipFactory.CreateWithAnchor(ProcessName("McFakeHeroEyeBall"));
        }

        public override void Update(float time)
        {
            base.Update(time);
            currentBackground.Position = currentEyeBall.Position * 0.5f;
        }
    }
}
