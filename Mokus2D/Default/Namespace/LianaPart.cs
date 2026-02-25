using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

using Mokus2D.Util.MathUtils;

namespace Mokus2D.Default.Namespace
{
    public class LianaPart : IUpdatable
    {
        public LianaPart(Body _body)
        {
            body = _body;
            float num = Maths.randRange(0.05f, 0.1f) * body.Mass;
            forceChanger = new CosChanger(-num, num, Maths.randRange(0.01f, 0.02f));
            forceAngle = Maths.randRange(0f, 6.2831855f);
            body.GravityScale = 0f;
        }

        public void Update(float time)
        {
            forceChanger.Update(time);
            Vector2 vector = FarseerUtil.toVec(forceChanger.Value, forceAngle);
            body.ApplyForce(vector, body.WorldCenter);
            FarseerUtil.LimitSpeedSpeed(body, 0.3f);
        }

        protected Body body;

        protected CosChanger forceChanger;

        protected float forceAngle;
    }
}
