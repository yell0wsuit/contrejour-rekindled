using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace Mokus2D.Default.Namespace
{
    public class BodyProcessor : ShapeProcessor
    {
        public BodyProcessor(LevelBuilderBase _builder)
            : base("body", _builder)
        {
            processors = new Hashtable();
            processors["circle"] = new CircleProcessor(_builder);
            processors["polygon"] = new PolygonProcessor(_builder);
        }

        public override void AddShapesItem(Body body, Hashtable item)
        {
            ArrayList arrayList = item.GetArrayList("shapes");
            foreach (object obj in arrayList)
            {
                Hashtable hashtable = (Hashtable)obj;
                string @string = hashtable.GetString("config/type");
                ShapeProcessor shapeProcessor = (ShapeProcessor)processors.GetObject(@string);
                Shape shape = shapeProcessor.CreateShape(hashtable);
                Fixture fixture = AddShapeItemShape(body, item, shape);
                if (hashtable.Exists("fixtureConfig"))
                {
                    Hashtable hashtable2 = hashtable.GetHashtable("fixtureConfig");
                    fixture.UserData = hashtable2;
                }
            }
        }

        protected Hashtable processors;
    }
}
