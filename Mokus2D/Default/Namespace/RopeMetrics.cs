namespace Default.Namespace
{
    public class RopeMetrics(int _parts, float _partSize)
    {
        public int Parts
        {
            get => parts; set => parts = value;
        }

        public float PartSize
        {
            get => partSize; set => partSize = value;
        }

        protected int parts = _parts;

        protected float partSize = _partSize;
    }
}
