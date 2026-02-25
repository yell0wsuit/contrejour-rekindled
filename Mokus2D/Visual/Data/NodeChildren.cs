using System;

namespace Mokus2D.Visual.Data
{
    public class NodeChildren : SortedList<Node>
    {
        public NodeChildren()
            : base(64, null)
        {
        }

        protected override int GetInsertIndex(Node item)
        {
            int num = items.BinarySearch(item, comparer);
            return num < 0 ? ~num : throw new InvalidOperationException("items can not be equal");
        }

        private static readonly NodeComparer comparer = new();
    }
}
