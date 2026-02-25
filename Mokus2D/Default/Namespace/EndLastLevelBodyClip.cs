using System;

using Mokus2D;
using Mokus2D.Util.MathUtils;
using Mokus2D.Visual;

namespace Mokus2D.Default.Namespace
{
    public class EndLastLevelBodyClip(LevelBuilderBase _builder, object _body, Sprite _clip, Hashtable _config) : EndLevelBodyClip(_builder, _body, _clip, _config)
    {
        public override void Restart()
        {
            base.Restart();
            portal.TargetScale = 1f;
            portal.ScaleStep = 0.05f;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (bounce)
            {
                scaleChanger.Update(time);
                portal.TargetScale = 1.7f + scaleChanger.Value;
            }
        }

        protected override void CompleteLevel(HeroBodyClip bodyClip)
        {
            bodyClip.CompleteLevelSpeed(Body.Position, 0.3f);
            Mokus2DGame.SoundManager.PlaySound("end", 0.5f, 0f, 0f);
            portal.TargetScale = 1.7f;
            scaleChanger.SetMiddleProgress();
            scaleChanger.Update(0f);
            portal.ScaleStep = 0.1f;
            _ = Schedule(new Action(Hide), 8f);
        }

        private void Hide()
        {
            bounce = false;
            portal.TargetScale = 0f;
        }

        private const float END_SCALE = 1.7f;

        protected CosChanger scaleChanger = new(-0.2f, 0.2f, 0.3f);

        protected bool bounce;
    }
}
