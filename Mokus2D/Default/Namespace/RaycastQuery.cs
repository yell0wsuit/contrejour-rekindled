using System.Collections.Generic;

using FarseerPhysics.Dynamics;

using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class RaycastQuery
    {
        public List<Fixture> Fixtures { get; } = new();

        public float ReportFixture(Fixture fixture, Vector2 point, Vector2 normal, float fraction)
        {
            Fixtures.Add(fixture);
            return -1f;
        }
    }
}
