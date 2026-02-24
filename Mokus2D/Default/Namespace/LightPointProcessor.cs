using Microsoft.Xna.Framework;

namespace Default.Namespace
{
    public class LightPointProcessor(LevelBuilderBase _builder) : TypeProcessorBase("lightPoint", _builder)
    {
        public override object ProcessItem(Hashtable item)
        {
            Vector2 vector = item.GetVector("position");
            ContreJourGame contreJourGame = (ContreJourGame)builder.Game;
            contreJourGame.LightPoint = vector;
            return null;
        }
    }
}
