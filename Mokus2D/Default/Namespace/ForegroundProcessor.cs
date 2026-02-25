namespace Mokus2D.Default.Namespace
{
    public class ForegroundProcessor(LevelBuilderBase _builder) : TypeProcessorBase("foreground", _builder)
    {
        public override object ProcessItem(Hashtable item)
        {
            Hashtable hashtable = item.GetHashtable("config");
            if (hashtable.NotExists("z"))
            {
                hashtable["z"] = "12";
            }
            if (hashtable.NotExists("clipType"))
            {
                hashtable["skipClip"] = "true";
            }
            return STATIC_RESULT;
        }

        private static readonly int STATIC_RESULT = 1;
    }
}
