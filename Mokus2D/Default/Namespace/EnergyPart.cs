using System;

using Microsoft.Xna.Framework;

using Mokus2D.Extensions;

namespace Default.Namespace
{
    public class EnergyPart(ContreJourGame _game, BodyClip parent, Particle particle, float _direction, Vector2 position) : Satellite(_game, particle, parent, _direction, position)
    {
        public EnergyPart(ContreJourGame _game, BodyClip parent, float _direction, Vector2 position)
            : this(_game, parent, _game.Energy.AddOrGetInvisible(), _direction, position)
        {
        }

        public void Collect()
        {
            target = (BodyClip)game.BonusTarget;
            game.Hero.FinishEvent.AddListenerSelector(new Action(OnHeroFinish));
            collected = true;
            speedValue = CocosUtil.iPadValue(Maths.RandRangeMinMax(150f, 250f));
            angleStep = Maths.RandRangeMinMax(0.05f, 0.1f);
            baseScale = 1.2f;
        }

        public void OnHeroFinish()
        {
            game.Hero.FinishEvent.RemoveListenerSelector(new Action(OnHeroFinish));
            collected = false;
            finished = true;
        }

        protected override Vector2 TargetPosition
        {
            get
            {
                return !collected ? base.TargetPosition : game.BonusTarget.BonusTarget();
            }
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (collected)
            {
                timeToEnd -= time;
            }
            if (game != null)
            {
                float num = clip.Position.DistanceTo(TargetPosition);
                if (collected && num < 50f)
                {
                    clip.OpacityFloat = num / 50f;
                }
                else
                {
                    clip.Opacity = 255;
                }
                if ((timeToEnd <= 0f && num <= 10f) || opacity <= 0f)
                {
                    Remove();
                    if (opacity > 0f && game.BonusTarget != null)
                    {
                        game.BonusTarget.ApplyBonus();
                    }
                }
                else if (timeToEnd <= 0f)
                {
                    angleStep += 0.005f;
                    speedValue += 1f;
                }
                if (finished && opacity > 0f)
                {
                    opacity = Maths.stepTo(opacity, 0f, 5f);
                    clip.Opacity = (int)opacity;
                }
            }
        }

        private const float MAX_OPACITY = 200f;

        private const float MIN_OPACITY = 100f;

        private const float SCALE_CHANGE = 0.3f;

        private const float MIN_LENGTH = 10f;

        protected float timeToEnd = Maths.RandRangeMinMax(1f, 2f);

        protected bool collected = false;

        protected bool finished;

        protected float baseScale = 0.7f;

        protected float opacity = 255f;

        protected bool dealloced;
    }
}
