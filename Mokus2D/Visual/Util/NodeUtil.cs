namespace Mokus2D.Visual.Util
{
    public static class NodeUtil
    {
        public static bool IsBranchVisible(Node node)
        {
            while (node != null && !node.IsRoot && node.Visible)
            {
                node = node.Parent;
            }
            return node != null && node.IsRoot;
        }
    }
}
