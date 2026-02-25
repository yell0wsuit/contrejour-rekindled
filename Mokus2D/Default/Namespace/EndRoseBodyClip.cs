using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Mokus2D.Effects.Actions;
using Mokus2D.Util;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class EndRoseBodyClip : StickyBodyClip, IBonusAcceptable, IBodyClip
    {
        public EndRoseBodyClip(LevelBuilderBase _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            Game.BonusTarget = this;
            movie = (MovieClip)clip;
            movie.Rewind = true;
            movie.Repeat = false;
            UserData instance = UserData.Instance;
            saved = instance.RoseSaved;
            if (!saved)
            {
                movie.MaxFrame = movie.MaxFrame * 0.55f;
            }
            maxTime = saved ? 4f : 2.9629629f;
            movie.Color = ContreJourConstants.BLACK_COLOR_3;
            colorChanger = new CosChanger(-0.1f, 0f, 0.05f);
            builder.RegisterObject(this, "rose");
        }

        public void ApplyBonus()
        {
            if (!started)
            {
                started = true;
                _ = Schedule(new Action(Start), saved ? 5 : 6);
            }
            float num = (Math.Abs(movie.CurrentFrame - movie.MaxFrame) < 8f) ? 0.03f : 0.01f;
            colorStep = Math.Max(num, colorStep + 0.003f);
        }

        public Vector2 BonusTarget()
        {
            Vector2 vector = clip.Position + CocosUtil.ccpIPad(28f, 78f);
            return vector + (CocosUtil.ccpIPad(-20f, 20f) * movie.CurrentFrame / movie.MaxFrame);
        }

        private void AddLight(float direction)
        {
            Sprite sprite = ClipFactory.CreateWithAnchor("McRoseLight");
            sprite.Position = CocosUtil.ccpIPad(22f, 114f) + clip.Position;
            sprite.Blend = BlendState.Additive;
            sprite.Opacity = 120;
            _ = builder.AddChild(sprite);
            sprite.Run(new RepeatForever(new RotateBy(Maths.randRange(12f, 15f), direction)));
            sprite.Run(new FadeIn(2f));
        }

        public void ShowLights()
        {
            AddLight(-1f);
            AddLight(1f);
        }

        public void DropTear()
        {
            MovieClip movieClip = (MovieClip)ClipFactory.CreateWithAnchor("McTear");
            movieClip.Repeat = false;
            _ = builder.AddChild(movieClip);
            movieClip.Position = clip.Position;
            movieClip.Speed = 0.7f;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (startTime != 0f)
            {
                float num = Maths.Clamp((Game.TotalTime - startTime) / maxTime, 0f, 1f);
                float num2 = Maths.easeInOutSine(num, movie.MaxFrame);
                movie.CurrentFrame = goingDown ? (movie.MaxFrame - num2) : num2;
                if (movie.CurrentFrame == movie.MaxFrame)
                {
                    if (!saved && !goingDown)
                    {
                        goingDown = true;
                        startTime = Game.TotalTime;
                    }
                    else if (saved && !rised)
                    {
                        rised = true;
                        XBoxUtil.AwardAchievement("little_prince");
                    }
                }
            }
            if (!goingDown)
            {
                if (rised)
                {
                    colorStep = 0.05f;
                }
                else
                {
                    if (Math.Abs(movie.CurrentFrame - movie.MaxFrame) > 8f)
                    {
                        colorStep -= 0.0005f;
                    }
                    colorStep = Maths.Clamp(colorStep, -0.05f, 0.035f);
                }
                colorProgress = Maths.Clamp(colorProgress + colorStep, 0f, 1f);
            }
            else
            {
                colorProgress -= 0.002f;
            }
            colorChanger.Update(time);
            movie.Color = ContreJourConstants.WHITE_COLOR_3 * Maths.Clamp(colorChanger.Value + colorProgress, 0f, 0.98f);
        }

        private void Start()
        {
            startTime = Game.TotalTime;
        }

        protected CosChanger colorChanger;

        protected float colorProgress;

        protected float colorStep;

        protected bool goingDown;

        protected float maxTime;

        protected MovieClip movie;

        protected bool rised;

        protected bool saved;

        protected float startTime;

        protected bool started;
    }
}
