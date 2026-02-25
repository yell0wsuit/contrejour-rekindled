using System;
using System.Collections.Generic;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class RoseBodyClip : StickyBodyClip, IBonusAcceptable, IBodyClip
    {
        public RoseBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            Node node = new();
            _builder.ReplaceChildWith(_clip, node);
            node.Position = _clip.Position;
            clip = node;
            data = UserData.Instance;
            game = (ContreJourGame)_builder.Game;
            finalRose = (MovieClip)ClipFactory.CreateWithAnchor("McFinalRose");
            finalRose.Stoped = true;
            finalRose.Repeat = false;
            finalRose.Speed = 0f;
            game.BonusTarget = this;
            clip.AddChild(finalRose);
            if (game.CanShowIntro)
            {
                PlayIntro();
                return;
            }
            finished = true;
        }

        public Vector2 BonusTarget()
        {
            return clip.Position + new Vector2(0f, -30f);
        }

        public void ApplyBonus()
        {
            finalRose.Stoped = false;
            if (finalRose.CurrentFrame < 1f)
            {
                finalRose.Speed = 0.3f;
                return;
            }
            if (finalRose.CurrentFrame < 30f)
            {
                finalRose.Speed += 0.3f;
            }
        }

        public new List<string> TexturesToUnload()
        {
            List<string> list = new();
            list.Add("McStebloAnimation");
            list.Add("McRoseHead");
            list.Add("McRoseHeadDown");
            list.Add("McLystokMain");
            list.Add("McLystok1");
            list.Add("McLystok2");
            list.Add("McFinalRose");
            list.Add("McPuddle");
            list.Add("McInspired");
            list.Add("McIntroLogo");
            return list;
        }

        private void OnSkipClick()
        {
            if (!game.Paused)
            {
                game.Restart();
            }
        }

        public void PlayIntro()
        {
            skipButton = new Button("McSkipIcon");
            skipButton.ClickEvent.AddListenerSelector(new Action(OnSkipClick));
            skipButton.RealScale = 1.3f;
            skipButton.Icon.Scale = 0.75f;
            skipButton.Color = game.ButtonsColor;
            game.ClickableLayer.AddChild(skipButton);
            skipButton.Position = ScreenConstants.W7FromIPhoneSize - new Vector2(60f);
            intro = new IntroPlayer(game);
            _ = builder.AddChild(intro);
            stalk = (MovieClip)ClipFactory.CreateWithAnchor("McStebloAnimation");
            headLight = ClipFactory.CreateWithAnchor("McRoseHeadLight");
            headBack = ClipFactory.CreateWithAnchor("McRoseHeadBack");
            headFront = ClipFactory.CreateWithAnchor("McRoseHeadFront");
            headDown = (MovieClip)ClipFactory.CreateWithAnchor("McRoseHeadDown");
            leafMain = (MovieClip)ClipFactory.CreateWithAnchor("McLystokMain");
            leaf1 = (MovieClip)ClipFactory.CreateWithAnchor("McLystok1");
            leaf2 = (MovieClip)ClipFactory.CreateWithAnchor("McLystok2");
            finalRose.Visible = false;
            roseParts = new List<MovieClip>([stalk, headDown, leaf1, leaf2]);
            puddle = (MovieClip)ClipFactory.CreateWithAnchor("McPuddle");
            puddle.Position = clip.Position + CocosUtil.toIPad(PUDDLE_OFFSET);
            puddle.Repeat = false;
            puddle.Visible = false;
            puddle.Stoped = true;
            puddle.Speed = 0.7f;
            _ = builder.AddChild(puddle);
            foreach (MovieClip movieClip in roseParts)
            {
                movieClip.Repeat = false;
                movieClip.Stoped = true;
                clip.AddChild(movieClip);
            }
            _ = builder.AddChild(leafMain);
            leafMain.Repeat = false;
            leafMain.Stoped = true;
            leafMain.Position = clip.Position;
            clip.AddChild(headBack);
            clip.AddChild(headLight);
            clip.AddChild(headFront);
            leaf1.Speed = 0.5f;
            leafMain.MaxFrame = 79f;
            headDown.Visible = false;
            leaf1.EndEvent += new Action(OnLeaf1Fall);
            game.TouchEnabled = false;
            headLight.Opacity = 150;
            Sequence sequence = new(
            [
                new FadeTo(2.5f, 0.19607843f),
                new FadeTo(2.5f, 0.39215687f),
                new FadeTo(2.5f, 0.11764706f),
                new FadeTo(2.5f, 0.3137255f),
                new FadeTo(2.5f, 0f),
                new InstantAction(new Action(GoDown))
            ]);
            headLight.Run(sequence);
        }

        private void OnLeaf1Fall()
        {
            leafMain.MaxFrame = leafMain.TotalFrames;
            leafMain.Stoped = false;
            leafMain.EndEvent += new Action(OnLeafMainEnd);
        }

        private void OnLeafMainEnd()
        {
            leafMain.EndEvent += new Action(OnLeafMainEnd);
            _ = Schedule(new Action(ShowPuddle), 1f);
        }

        private void ShowPuddle()
        {
            puddle.Visible = true;
            puddle.Stoped = false;
            puddle.EndEvent += new Action(OnPuddleEnd);
        }

        private void OnPuddleEnd()
        {
            Mokus2DGame.SoundManager.PlaySound("begin5", 0.5f, 0f, 0f);
            puddle.EndEvent -= new Action(OnPuddleEnd);
            game.Hero.Body.BodyType = BodyType.Dynamic;
            game.Hero.SetPosition(puddle.Position);
            FarseerUtil.SetSensor(game.Hero.Body, true);
            game.Hero.Clip.Visible = true;
            game.Hero.EyeAnimationsAllowed = false;
            game.Hero.Body.ApplyLinearImpulse(HERO_JUMP_IMPULSE, game.Hero.Body.WorldCenter);
            game.Hero.Body.FixedRotation = true;
            _ = Schedule(new Action(OnHeroJump), 0.5f);
            _ = Schedule(new Action(LookAtRose), 2.5f);
        }

        private void OnHeroJump()
        {
            FarseerUtil.SetSensor(game.Hero.Body, false);
            puddle.Rewind = true;
            puddle.Stoped = false;
        }

        private void LookAtRose()
        {
            game.Hero.EyeMoveAllowed = false;
            game.Hero.SetEyeTargetAngle(MathHelper.ToRadians(30f));
            _ = Schedule(new Action(LookAtBonuses), 1.5f);
            _ = Schedule(new Action(game.ShowBonuses), 1f);
        }

        private void LookAtBonuses()
        {
            game.Hero.SetEyeTargetAngle(MathHelper.ToRadians(160f));
            _ = Schedule(new Action(FinishMovie), 1.5f);
            skipButton.ClickEvent.RemoveListenerSelector(new Action(OnSkipClick));
            skipButton.Run(new Sequence(
            [
                new FadeOut(0.3f),
                new Hide()
            ]));
            finished = true;
            finalRose.Visible = true;
            foreach (MovieClip movieClip in roseParts)
            {
                clip.RemoveChild(movieClip);
            }
            builder.RemoveChild(leafMain);
            clip.RemoveChild(headFront);
            clip.RemoveChild(headBack);
            clip.RemoveChild(headLight);
            builder.RemoveChild(puddle);
        }

        private void FinishMovie()
        {
            game.Hero.EyeAnimationsAllowed = true;
            game.Hero.EyeMoveAllowed = true;
            game.Hero.Body.FixedRotation = false;
            game.TouchEnabled = true;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (game.Debug)
            {
                return;
            }
            if (!finished)
            {
                foreach (MovieClip movieClip in roseParts)
                {
                    movieClip.Update(time);
                }
                leafMain.Update(time);
                puddle.Update(time);
            }
            else
            {
                if (finalRose.Speed > -1f)
                {
                    finalRose.Speed -= time / 20f * finalRose.CurrentFrame;
                }
                finalRose.Stoped = false;
                finalRose.Update(time);
            }
            if (game.CanShowIntro && !bonusHidden && time > 0f)
            {
                game.Hero.EyeAnimationsAllowed = false;
                game.Hero.Clip.Visible = false;
                game.HideBonuses();
                bonusHidden = true;
            }
        }

        private void GoDown()
        {
            headFront.Visible = false;
            headBack.Visible = false;
            headLight.Visible = false;
            headDown.Visible = true;
            foreach (MovieClip movieClip in roseParts)
            {
                movieClip.Stoped = false;
            }
            leafMain.Stoped = false;
        }

        private static readonly Vector2 HERO_JUMP_IMPULSE = new(0f, 2f);

        private static readonly Vector2 PUDDLE_OFFSET = new(-80f, -1f);

        protected bool bonusHidden;

        protected UserData data;

        protected MovieClip finalRose;

        protected bool finished;

        protected ContreJourGame game;

        protected Sprite headBack;

        protected MovieClip headDown;

        protected Sprite headFront;

        protected Sprite headLight;

        protected IntroPlayer intro;

        protected MovieClip leaf1;

        protected MovieClip leaf2;

        protected MovieClip leafMain;

        protected MovieClip puddle;

        protected List<MovieClip> roseParts;

        protected Button skipButton;

        protected MovieClip stalk;
    }
}
