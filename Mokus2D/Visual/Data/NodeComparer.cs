using System.Collections.Generic;

namespace Mokus2D.Visual.Data
{
    internal class NodeComparer : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            return x.Layer > y.Layer + 0.5f ? 1 : x.Layer < y.Layer + 0.5f ? -1 : 0;
        }
    }
}
