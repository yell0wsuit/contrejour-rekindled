using System;
using System.Collections.Generic;

namespace Mokus2D.Visual.Data
{
    public class ComparisonComparer<T>(Comparison<T> comparison) : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            return comparison.Invoke(x, y);
        }
    }
}
