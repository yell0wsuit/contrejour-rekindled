namespace Mokus2D.Default.Namespace
{
    public class TypeProcessorBase(string _type, LevelBuilderBase _builder)
    {
        public virtual bool Match(Hashtable item)
        {
            string @string = item.GetString("config/type");
            return @string == type;
        }

        public virtual object ProcessItem(Hashtable item)
        {
            return null;
        }

        protected string type = _type;

        protected LevelBuilderBase builder = _builder;
    }
}
