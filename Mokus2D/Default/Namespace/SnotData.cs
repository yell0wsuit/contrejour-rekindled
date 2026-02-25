using System.Collections.Generic;

using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class SnotData
    {
        public Body EyeBody
        {
            get => eyeBody; set => eyeBody = value;
        }

        public RevoluteJoint EyeJoint
        {
            get => eyeJoint; set => eyeJoint = value;
        }

        public Body JoinedBody => joinedBody;

        public float InitialLength => initialLength;

        public Vector2 LocalStartAnchor => localStartAnchor;

        public SnotBodyClipBase Snot
        {
            get => snot; set => snot = value;
        }

        public RopeMetrics Metrics => metrics;

        public SnotData(Body _eyeBody, RevoluteJoint _eyeJoint, Body _joinedBody, Vector2 _localStartAnchor, List<Body> _bodies, List<Joint> _joints, RopeMetrics _metrics)
        {
            eyeBody = _eyeBody;
            eyeJoint = _eyeJoint;
            bodies = _bodies;
            joints = _joints;
            joinedBody = _joinedBody;
            localStartAnchor = _localStartAnchor;
            initialLength = (EndBody.Position - EyeBody.Position).Length();
            metrics = _metrics;
        }

        public Body BodyAt(int i)
        {
            return bodies[i];
        }

        public Joint JointAt(int i)
        {
            return joints[i];
        }

        public int BodiesSize()
        {
            return bodies.Count;
        }

        public int JoitsSize => joints.Count;

        public Body FirstBody => bodies[0];

        public Body EndBody => bodies[bodies.Count - 1];

        public Vector2 GetWorldStartPoint()
        {
            return joinedBody.GetWorldPoint(localStartAnchor);
        }

        protected Body joinedBody;

        protected Body eyeBody;

        protected RevoluteJoint eyeJoint;

        protected Vector2 localStartAnchor;

        protected List<Body> bodies;

        protected List<Joint> joints;

        protected float initialLength;

        protected SnotBodyClipBase snot;

        protected RopeMetrics metrics;
    }
}
