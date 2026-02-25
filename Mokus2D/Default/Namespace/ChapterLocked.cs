using System;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class ChapterLocked : ChapterItem
    {
        public ChapterLocked(int _index, MainMenu _menu)
            : base(_index, _menu)
        {
            explosion = new Explosion("McSmokeBlack.png");
            explosion.Rotation = 90f;
            explosion.ScaleStep = -0.5f;
            explosion.HorizontalPosition = new Range(0f, 20f);
            explosion.VerticalPosition = new Range(0f, 20f);
            explosion.Speed = new Range(220f, 60f);
            explosion.ParticlesScale = new Range(1f, 0.5f);
            explosion.CreateOnStartPosition(75);
        }

        public ChapterItem TargetChapter { get; set; }

        public EventSender ExplodeEvent => explodeEvent;

        protected override void CreateClickListener()
        {
        }

        public override void RemoveListeners()
        {
        }

        protected override void CreateSprites()
        {
            background = ClipFactory.CreateWithAnchor("McPlanetLocked");
            container.AddChild(background);
            tablo = new Tablo();
            tablo.Position = CocosUtil.ccpIPad(-74f, 28f);
            tablo.Color = (index == 1) ? ContreJourConstants.BLUE_LIGHT_COLOR : ContreJourConstants.GREY_COLOR;
            container.AddChild(tablo);
            background.Color = tablo.Color;
            int num = UserData.StarsToUnlock(index);
            Label label = ContreJourLabel.CreateLabel(CocosUtil.iPad(26, 16), num.ToString(), true);
            if (index == 1)
            {
                label.Color = CocosUtil.ccc4Mix(tablo.Color, ContreJourConstants.GREY_COLOR, 0.7f);
            }
            else
            {
                label.Color = tablo.Color;
            }
            tablo.AddChild(label);
            label.Position = CocosUtil.ccpIPad(60f, 42f);
            Node node = ClipFactory.CreateWithAnchor("McEnergyIcon");
            tablo.AddChild(node);
            node.Position = CocosUtil.ccpIPad(100f, 46f);
            Sprite sprite = ClipFactory.CreateWithAnchor("McLockIcon");
            tablo.AddChild(sprite);
            sprite.Position = CocosUtil.ccpIPad(24f, 46f);
            sprite.Color = label.Color;
            blurBackground = ClipFactory.CreateWithAnchor("McChapterLockedBlur");
            hidingItems.Add(label);
            hidingItems.Add(tablo);
            hidingItems.Add(node);
        }

        protected override void OnClick()
        {
        }

        public override float Depth
        {
            set
            {
                base.Depth = value;
                tablo.Open = value > 0.9f;
                if (!exploding && TargetChapter != null && value == 1f)
                {
                    exploding = true;
                    Mokus2DGame.SoundManager.PlaySound("boom0", 0.7f, 0f, 0f);
                    NodeAction nodeAction = Actions.ShakeWithDurationOffsetCount(0.2f, 5f, 20);
                    Run(nodeAction);
                    _ = Schedule(new Action(DoExplode), 0.4f);
                }
            }
        }

        private void DoExplode()
        {
            Run(new FadeOut(0.35f));
            TargetChapter.Run(new FadeIn(0.3f));
            explosion.IgnoreParentOpacity = true;
            Parent.AddChild(explosion);
            explosion.Position = Position;
            _ = Schedule(new Action(explodeEvent.SendEvent), 1f);
            Run(new DelayedAction(new Action(RemoveFromParent), 1.5f));
        }

        public override void Update(float time)
        {
            if (TargetChapter != null)
            {
                Depth = TargetChapter.Depth;
            }
            base.Update(time);
        }

        private static readonly Color TABLO_COLOR = new(40, 127, 157, 255);

        protected EventSender explodeEvent = new();

        protected bool exploding;

        protected Explosion explosion;

        protected Tablo tablo;
    }
}
