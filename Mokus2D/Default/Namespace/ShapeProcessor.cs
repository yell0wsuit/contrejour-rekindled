using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace Mokus2D.Default.Namespace
{
    public class ShapeProcessor(string _type, LevelBuilderBase _builder) : TypeProcessorBase(_type, _builder)
    {
        public override object ProcessItem(Hashtable item)
        {
            Hashtable hashtable = item.GetHashtable("config");
            Body body = BodyFactory.CreateBody(builder.World);
            if (hashtable.Exists("dynamic"))
            {
                body.BodyType = BodyType.Dynamic;
            }
            body.Position = item.GetVector("position");
            body.IsBullet = hashtable.Exists("bullet");
            if (hashtable.Exists("angularDamping"))
            {
                body.AngularDamping = hashtable.GetFloat("angularDamping");
            }
            body.FixedRotation = hashtable.GetBool("fixedRotation");
            AddShapesItem(body, item);
            if (hashtable.Exists("id"))
            {
                builder.CreatedObjects[hashtable.GetString("id")] = body;
            }
            return body;
        }

        public Fixture AddShapeItemShape(Body body, Hashtable item, Shape shape)
        {
            Hashtable hashtable = item.GetHashtable("config");
            float num;
            if (hashtable.Exists("density"))
            {
                num = hashtable.GetFloat("density");
            }
            else
            {
                num = builder.EngineConfig.Density;
            }
            Fixture fixture = body.CreateFixture(shape, num);
            float num2 = hashtable.Exists("friction") ? hashtable.GetFloat("friction") : builder.EngineConfig.Friction;
            fixture.Friction = num2;
            float num3 = hashtable.Exists("restitution") ? hashtable.GetFloat("restitution") : builder.EngineConfig.Restitution;
            fixture.Restitution = num3;
            fixture.IsSensor = hashtable.GetBool("sensor");
            if (hashtable.Exists("categoryBits") || hashtable.Exists("maskBits"))
            {
                if (hashtable.Exists("categoryBits"))
                {
                    fixture.CollisionCategories = (Category)hashtable.GetInt("categoryBits");
                }
                if (hashtable.Exists("maskBits"))
                {
                    fixture.CollidesWith = (Category)hashtable.GetInt("maskBits");
                }
            }
            if (hashtable.Exists("filterGroup"))
            {
                fixture.CollisionGroup = (short)hashtable.GetShort("filterGroup");
            }
            return fixture;
        }

        public virtual void AddShapesItem(Body body, Hashtable item)
        {
            Shape shape = CreateShape(item);
            _ = AddShapeItemShape(body, item, shape);
        }

        public virtual Shape CreateShape(Hashtable item)
        {
            return null;
        }
    }
}
