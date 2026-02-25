using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mokus2D.Util.Data
{
    public class ReverseEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
        public ReverseEnumerator(IList<T> source)
        {
            this.source = source;
            Reset();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            currentIndex--;
            return currentIndex >= 0;
        }

        public void Reset()
        {
            currentIndex = Enumerable.Count(source);
        }

        public T Current => source[currentIndex];

        object IEnumerator.Current => Current;

        private readonly IList<T> source;

        private int currentIndex;
    }
}
