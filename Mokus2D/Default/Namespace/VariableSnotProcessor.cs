using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class VariableSnotProcessor(LevelBuilderBase _builder) : SnotProcessor(_builder, "variableSnot")
    {
        public override RopeMetricsWithCoords GetRopeMetricsEndItem(Vector2 start, Vector2 end, Hashtable item)
        {
            Hashtable hashtable = item.GetHashtable("config");
            float num = hashtable.GetFloat("variableSize") * builder.SizeMult;
            float num2 = FarseerUtil.b2Vec2Distance(start, end) + num;
            return RopeUtil.GetRopeMetricsEndMaxPartSizeMinPartsLength(start, end, partSize, 3, num2);
        }
    }
}
