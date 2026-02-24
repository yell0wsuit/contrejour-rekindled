using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Mokus2D;
using Mokus2D.Util;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class EndLevelBodyClip : RotatableBodyClip, IRestartable
    {
        public EndLevelBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            portal = new Portal((ContreJourGame)builder.Game, clip.Position);
            builder.Add(portal, 11);
            ((ContreJourGame)builder.Game).EndLevel = this;
            clip.Visible = false;
            portal.ItemsScale = 0f;
        }

        public void ShowPortal()
        {
            portal.ItemsScale = 0f;
            portal.TargetScale = 1f;
        }

        public void SetVisible(bool value)
        {
            portal.Visible = value;
        }

        public override void OnCollisionStartPoint(Body body2, Contact point)
        {
            HeroBodyClip heroBodyClip = body2.UserData as HeroBodyClip;
            if (!finishing && heroBodyClip != null && heroBodyClip.CanDie())
            {
                if (builder.Game.TotalTime <= 5f)
                {
                    XBoxUtil.AwardAchievement("rush_hour");
                }
                if (builder.Game.TotalTime <= 10f && ((ContreJourGame)builder.Game).StarsCollected == 3)
                {
                    XBoxUtil.AwardAchievement("fast_perfect");
                }
                finishing = true;
                CompleteLevel(heroBodyClip);
            }
        }

        protected virtual void CompleteLevel(HeroBodyClip bodyClip)
        {
            bodyClip.CompleteLevelSpeed(Body.Position, 0.3f);
            bodyClip.SetScaleTime(0f, 0.5f);
            Mokus2DGame.SoundManager.PlaySound("end", 0.5f, 0f, 0f);
            portal.TargetScale = 0f;
        }

        public virtual void Restart()
        {
            finishing = false;
        }

        protected Portal portal;

        protected bool finishing;

        protected float scale;
    }
}
