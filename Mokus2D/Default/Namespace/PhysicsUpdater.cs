using FarseerPhysics.Dynamics;

namespace Mokus2D.Default.Namespace
{
    public class PhysicsUpdater : Updatable
    {
        public World World => world;

        public PhysicsUpdater(World _world)
        {
            world = _world;
            listener = new ContactListener(world);
        }

        public override void Update(float time)
        {
            foreach (Body body in world.BodyList)
            {
                BodyClip bodyClip = body.UserData as BodyClip;
                if (bodyClip != null)
                {
                    bodyClip.Update(time);
                }
            }
        }

        protected World world;

        protected ContactListener listener;
    }
}
