namespace Mokus2D.Default.Namespace
{
    public interface ISnotLinked
    {
        EventSender DestroyEvent { get; }

        int SnotJoinedCount { get; set; }

        bool SnotEnabled { get; }
    }
}
