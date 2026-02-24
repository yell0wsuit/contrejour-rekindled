using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;

using Microsoft.Xna.Framework;

using Mokus2D;
using Mokus2D.Visual;

namespace Default.Namespace
{
    public class DestroyOnHitClip(LevelBuilderBase _builder, object _body, Node _clip, Hashtable _config) : BodyClip(_builder, _body, _clip, _config)
    {
        public int SnotJoinedCount
        {
            get
            {
                return snotJoinedCount;
            }
            set
            {
                snotJoinedCount = value;
            }
        }

        public override void OnCollisionStartPoint(Body body2, Contact point)
        {
            HeroBodyClip heroBodyClip = body2.UserData as HeroBodyClip;
            if (heroBodyClip != null)
            {
                Explode();
            }
        }

        public override void PostSolvePointImpulse(Body body2, Contact point, ContactVelocityConstraint impulse)
        {
            if (impulse.points[0].normalImpulse > 7f)
            {
                Explode();
            }
        }

        public void Explode()
        {
            UserData instance = UserData.Instance;
            instance.BlocksDestroyed++;
            Mokus2DGame.SoundManager.PlayRandomSound(Sounds.EXPLOSIONS, Maths.randRange(0.2f, 0.6f));
            DestroyLater();
            explosion = new Explosion("McGreySmoke");
            builder.Add(explosion, 10);
            explosion.Position = clip.Position;
            explosion.Speed = new Range(CocosUtil.iPadValue(100f), CocosUtil.iPadValue(20f));
            explosion.CreateOnStartPosition(25);
            Vector2 size = ((Sprite)clip).Size;
            int num = 0;
            foreach (Particle particle in explosion.Particles)
            {
                GravityParticle gravityParticle = (GravityParticle)particle;
                Vector2 vector = new(0f, num * size.Y / 25f - size.Y / 2f);
                vector = builder.ToRootChild(vector, clip);
                vector -= clip.Position;
                gravityParticle.Position = vector;
                num++;
            }
        }

        private const int EXPLOSION_PARTICLES = 25;

        private const float MIN_IMPULSE = 7f;

        protected Explosion explosion;

        protected int snotJoinedCount;
    }
}
