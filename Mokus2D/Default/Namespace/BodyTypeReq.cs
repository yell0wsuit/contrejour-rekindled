using FarseerPhysics.Dynamics;

namespace Default.Namespace
{
    public class BodyTypeReq(BodyType type) : IReq
    {
        public bool Meet(object objectP)
        {
            return ((Body)objectP).BodyType == type;
        }

        protected BodyType type = type;
    }
}
