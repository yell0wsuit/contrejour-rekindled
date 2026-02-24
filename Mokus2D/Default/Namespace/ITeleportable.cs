namespace Default.Namespace
{
    public interface ITeleportable
    {
        void Teleport(BodyClip teleport);

        bool SnotEnabled { set; }

        void SetScaleTime(float scale, float time);

        void AfterTeleport();

        void ForceClipPosition();

        bool CanTeleport();
    }
}
