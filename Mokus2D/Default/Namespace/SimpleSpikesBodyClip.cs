using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D.Effects.Actions;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class SimpleSpikesBodyClip : ContreJourBodyClip, IRestartable
    {
        public SimpleSpikesBodyClip(LevelBuilderBase _builder, object _body, Sprite _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            ContreJourGame contreJourGame = (ContreJourGame)_builder.Game;
            floating = _clip.Texture.Name.Contains("Circle");
            if (!contreJourGame.BlackSide)
            {
                string text = _clip.Texture.Name;
                string text2 = contreJourGame.ChooseSide(null, "White", "_5", "Black", "_6");
                text += text2;
                clip = _builder.ReplaceClipWith(_clip, text);
            }
            clip.UpdateEnabled = false;
            prickTime = -2f;
            initialPosition = Clip.Position;
            initialScale = Clip.ScaleX;
            RunActions();
        }

        public void RunActions()
        {
            if (actionsRunning)
            {
                return;
            }
            actionsRunning = true;
            if (floating)
            {
                speed = CocosUtil.iPadValue(Maths.randRange(2f, 3f));
                direction = Maths.randRange(0f, 6.2831855f);
                angleStep = Maths.randRange(0.8f, 1.2f);
                float num = Maths.randRange(0.95f, 0.98f) * initialScale;
                float num2 = Maths.randRange(1.02f, 1.05f) * initialScale;
                float num3 = Maths.randRange(2f, 3f);
                EaseInOut easeInOut = new(new ScaleTo(num3, num), 2f);
                EaseInOut easeInOut2 = new(new ScaleTo(num3, num2), 2f);
                Sequence sequence = new([easeInOut, easeInOut2]);
                Clip.Run(new RepeatForever(sequence));
            }
        }

        public void Restart()
        {
            prickTime = -2f;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!actionsRunning && Game.TotalTime - prickTime >= 2f)
            {
                RunActions();
                MovieClip movieClip = (MovieClip)Clip;
                movieClip.Rewind = true;
                movieClip.Stoped = false;
            }
            if (floating && actionsRunning)
            {
                float num = Maths.min(time, 0.033333335f);
                Vector2 vector = initialPosition - Clip.Position;
                float num2 = Maths.atan2(vector.Y, vector.X);
                num2 = Maths.SimplifyAngleRadiansStartValue(num2, direction - 3.1415927f);
                float num3 = angleStep * num;
                direction = Maths.StepToTargetMaxStep(direction, num2, num3);
                Vector2 vector2 = Maths.ToPointAngle(speed * num, direction);
                Clip.Position = Clip.Position + vector2;
            }
        }

        public override void UpdatePosition()
        {
            if (!floating)
            {
                base.UpdatePosition();
            }
        }

        public override void OnCollisionStartPoint(Body body2, Contact point)
        {
            ISpikesDestroyable spikesDestroyable = body2.UserData as ISpikesDestroyable;
            if (spikesDestroyable != null && !FarseerUtil.IsSensor(point) && spikesDestroyable.CanDie())
            {
                OnHeroHitPoint(spikesDestroyable, point);
                MovieClip movieClip = (MovieClip)Clip;
                movieClip.UpdateEnabled = true;
                movieClip.Repeat = false;
                movieClip.Rewind = false;
                movieClip.Stoped = false;
                Clip.StopAllActions();
                prickTime = Game.TotalTime;
                actionsRunning = false;
            }
        }

        public void OnHeroHitPoint(ISpikesDestroyable hero, Contact point)
        {
            hero.Explode();
        }

        private const float PRICK_TIME = 2f;

        protected bool floating;

        protected float speed;

        protected float direction;

        protected float angleStep;

        protected Vector2 initialPosition;

        protected float prickTime;

        protected bool actionsRunning;

        protected float initialScale;
    }
}
