namespace Mokus2D.Visual
{
    internal class SetRootCommand : INodeCommand
    {
        public void Execute(Node node)
        {
            node.SetRoot(Root);
        }

        public RootNode Root;
    }
}
