namespace Mokus2D.Default.Namespace
{
    public class FakeProcessor(string _type, LevelBuilderBase _builder) : TypeProcessorBase(_type, _builder)
    {
        public override object ProcessItem(Hashtable item)
        {
            return STATIC_RESULT;
        }

        private static readonly int STATIC_RESULT = 1;
    }
}
