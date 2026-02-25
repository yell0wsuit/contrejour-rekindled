using System.Collections.Generic;

using FarseerPhysics.Dynamics;

namespace Mokus2D.Default.Namespace
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
