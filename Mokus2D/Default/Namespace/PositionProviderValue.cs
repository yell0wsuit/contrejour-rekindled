namespace Mokus2D.Default.Namespace
{
    public class PositionProviderValue(IVectorPositionProvider _provider, float _value)
    {
        public float Value => value;

        public IVectorPositionProvider Provider => provider;

        protected IVectorPositionProvider provider = _provider;

        protected float value = _value;
    }
}
