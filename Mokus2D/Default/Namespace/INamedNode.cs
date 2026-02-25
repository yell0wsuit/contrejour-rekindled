namespace Mokus2D.Default.Namespace
{
    public interface INamedNode
    {
        string Name();

        Hashtable Config();

        CGSize Size();
    }
}
