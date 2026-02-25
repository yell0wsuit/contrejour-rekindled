using System.Collections.Generic;

using FarseerPhysics.Dynamics;

namespace Default.Namespace
{
    public class AABBQuery
    {
        public List<Fixture> Fixtures { get; } = new();

        public bool CallbackReportFixture(Fixture fixture)
        {
            Fixtures.Add(fixture);
            return true;
        }
    }
}
