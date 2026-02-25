using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;

namespace Mokus2D.Default.Namespace
{
    public class PolygonProcessor(LevelBuilderBase _builder) : ShapeProcessor("polygon", _builder)
    {
        public override Shape CreateShape(Hashtable item)
        {
            ArrayList arrayList = item.GetArrayList("vertices");
            Vertices vertices = new(arrayList.ToListVector2());
            return new PolygonShape(vertices, 0.3f);
        }
    }
}
