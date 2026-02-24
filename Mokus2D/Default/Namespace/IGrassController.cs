namespace Default.Namespace
{
    public interface IGrassController : IUpdatable
    {
        void ScareFlyes(float offset);

        void OnTouchWith(float offset, BodyClip objectP);

        float Y { get; }
    }
}
