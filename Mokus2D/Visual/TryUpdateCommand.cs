namespace Mokus2D.Visual
{
    internal class TryUpdateCommand : INodeCommand
    {
        public void Execute(Node node)
        {
            node.TryUpdateNode(Time);
        }

        public float Time;
    }
}
