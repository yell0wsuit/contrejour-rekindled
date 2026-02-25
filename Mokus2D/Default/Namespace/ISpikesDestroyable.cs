namespace Mokus2D.Default.Namespace
{
    public interface ISpikesDestroyable : IBodyClip
    {
        void Explode();

        void DoExplode();

        bool CanDie();
    }
}
