using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class ChapterZip(int _index, MainMenu _menu) : ChapterItem(_index, _menu)
    {
        protected override void CreateSprites()
        {
            openAnimation = (MovieClip)ClipFactory.CreateWithAnchor("McPlanetZip");
            background = openAnimation;
            arrow = new Tablo("McZipArrow");
            container.AddChild(arrow);
            arrow.Position = CocosUtil.ccpIPad(-82f, 30f);
            alphaItems.Add(arrow);
            blurBackground = ClipFactory.CreateWithAnchor("McPlanetZipBlur");
            openAnimation.Repeat = false;
            openAnimation.Stoped = true;
            openAnimation.Speed = 1.5f;
            container.AddChild(background);
            highlite = ClipFactory.CreateWithAnchor("McZipHighlite");
            container.AddChild(highlite);
            highlite.Visible = false;
        }

        public override void Update(float time)
        {
            base.Update(time);
            bool flag = Maths.FuzzyEquals(depth, 1f, 0.01f);
            arrow.Open = flag;
            if (flag && openAnimation.CurrentFrame < openAnimation.MaxFrame)
            {
                openAnimation.Stoped = false;
                openAnimation.Rewind = false;
            }
            else if (!flag)
            {
                openAnimation.Rewind = true;
                if (openAnimation.CurrentFrame >= openAnimation.MaxFrame)
                {
                    openAnimation.CurrentFrame = openAnimation.MaxFrame;
                    openAnimation.Stoped = false;
                }
            }
            if (flag && !highlite.Visible && openAnimation.CurrentFrame >= openAnimation.MaxFrame)
            {
                highlite.StopAllActions();
                highlite.Opacity = 0;
                Sequence sequence = new(
                [
                    new FadeIn(1.5f),
                    new FadeOut(1.5f)
                ]);
                highlite.Run(new RepeatForever(sequence));
                highlite.Visible = true;
                return;
            }
            if (!flag)
            {
                highlite.Visible = false;
            }
        }

        private const float HIGHLITE_TIME = 1.5f;

        protected Tablo arrow;

        protected Sprite highlite;

        protected MovieClip openAnimation;

        protected Sprite shadow;
    }
}
