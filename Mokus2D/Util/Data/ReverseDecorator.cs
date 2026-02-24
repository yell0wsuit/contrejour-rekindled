using System.Collections;
using System.Collections.Generic;

namespace Mokus2D.Util.Data
{
    public class ReverseDecorator<T>(IList<T> source) : IEnumerable<T>, IEnumerable
    {
        public IEnumerator<T> GetEnumerator()
        {
            return new ReverseEnumerator<T>(source);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
