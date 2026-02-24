namespace Default.Namespace
{
    public class PositionProviderValue(IVectorPositionProvider _provider, float _value)
    {
        public float Value
        {
            get
            {
                return value;
            }
        }

        public IVectorPositionProvider Provider
        {
            get
            {
                return provider;
            }
        }

        protected IVectorPositionProvider provider = _provider;

        protected float value = _value;
    }
}
