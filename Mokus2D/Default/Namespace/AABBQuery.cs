using System.Collections.Generic;

using FarseerPhysics.Dynamics;

namespace Default.Namespace
{
    public class AABBQuery
    {
        public List<Fixture> Fixtures
        {
            get
            {
                return fixtures;
            }
        }

        public bool CallbackReportFixture(Fixture fixture)
        {
            fixtures.Add(fixture);
            return true;
        }

        private List<Fixture> fixtures = new();
    }
}
