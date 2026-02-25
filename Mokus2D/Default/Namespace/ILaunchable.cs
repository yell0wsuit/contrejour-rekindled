namespace Mokus2D.Default.Namespace
{
    public interface ILaunchable : IRadius, IBodyClip
    {
        EventSender DestroyEvent { get; }

        void SetSpeedLocked(bool value);

        bool HitEnabled { set; }

        bool CanLaunch();
    }
}
