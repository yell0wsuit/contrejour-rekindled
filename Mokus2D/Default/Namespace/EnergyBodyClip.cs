using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Mokus2D;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class EnergyBodyClip : BodyClip, IRestartable
    {
        public EventSender CollectEvent
        {
            get
            {
                return collectEvent;
            }
        }

        public EnergyBodyClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config)
            : base(_builder, _body, _clip, _config)
        {
            _clip.Visible = false;
            contreJourBuilder = (ContreJourLevelBuilder)builder;
            energyParts = new ArrayList();
            collectEvent = new EventSender();
            CreateParts();
        }

        public void CreateParts()
        {
            for (int i = 0; i < 5; i++)
            {
                EnergyPart energyPart = new((ContreJourGame)builder.Game, this, 1.2566371f * i, clip.Position);
                energyParts.Add(energyPart);
                energyPart.Clip.Opacity = 0;
            }
        }

        public override void OnCollisionStartPoint(Body body2, Contact point)
        {
            HeroBodyClip heroBodyClip = body2.UserData as HeroBodyClip;
            if (heroBodyClip != null && !collected && heroBodyClip.CanDie())
            {
                collected = true;
                collectEvent.SendEvent();
                Mokus2DGame.SoundManager.PlayRandomSound(Sounds.BONUS, 0.5f);
                contreJourBuilder.ContreJour.CollectStar();
                foreach (object obj in energyParts)
                {
                    EnergyPart energyPart = (EnergyPart)obj;
                    energyPart.Collect();
                }
            }
        }

        public void Restart()
        {
            if (collected)
            {
                collected = false;
                energyParts.Clear();
                CreateParts();
            }
        }

        private const int ENERGY_PARTS_COUNT = 5;

        protected ContreJourLevelBuilder contreJourBuilder;

        protected ArrayList energyParts;

        protected bool collected;

        protected EventSender collectEvent;
    }
}
