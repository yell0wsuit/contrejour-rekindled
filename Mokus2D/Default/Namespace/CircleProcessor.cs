using FarseerPhysics.Collision.Shapes;

using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class CircleProcessor(LevelBuilderBase _builder) : ShapeProcessor("circle", _builder)
    {
        public override Shape CreateShape(Hashtable item)
        {
            float @float = item.GetFloat("radius");
            Vector2 vector = item.GetVector("position");
            return new CircleShape(@float, builder.EngineConfig.Density)
            {
                Position = vector
            };
        }
    }
}
