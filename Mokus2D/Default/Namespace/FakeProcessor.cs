namespace Default.Namespace
{
    public class FakeProcessor(string _type, LevelBuilderBase _builder) : TypeProcessorBase(_type, _builder)
    {
        public override object ProcessItem(Hashtable item)
        {
            return STATIC_RESULT;
        }

        private static int STATIC_RESULT = 1;
    }
}
