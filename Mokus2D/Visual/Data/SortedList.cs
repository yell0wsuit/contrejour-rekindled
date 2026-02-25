using System;
using System.Collections;
using System.Collections.Generic;

namespace Mokus2D.Visual.Data
{
    public class SortedList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private Comparison<T> Comparison
        {
            set
            {
                comparison = value;
                comparer = new ComparisonComparer<T>(comparison);
            }
        }

        public SortedList(Comparison<T> comparison)
        {
            Comparison = comparison;
            items = new List<T>();
        }

        public SortedList(int capacity, Comparison<T> comparison)
        {
            Comparison = comparison;
            items = new List<T>(capacity);
        }

        public SortedList(IEnumerable<T> collection, Comparison<T> comparison)
        {
            Comparison = comparison;
            items = new List<T>(collection);
            items.Sort(comparison);
        }

        public T this[int index]
        {
            get => items[index]; set => throw new NotSupportedException();
        }

        public void Add(T item)
        {
            int insertIndex = GetInsertIndex(item);
            items.Insert(insertIndex, item);
        }

        protected virtual int GetInsertIndex(T item)
        {
            int num = items.BinarySearch(item, comparer);
            if (num < 0)
            {
                num = ~num;
            }
            return num;
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T t in collection)
            {
                Add(t);
            }
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(T item)
        {
            return items.Contains(item);
        }

        public void CopyTo(T[] array)
        {
            items.CopyTo(array);
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            items.CopyTo(index, array, arrayIndex, count);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public void ForEach(Action<T> action)
        {
            items.ForEach(action);
        }

        public List<T> GetRange(int index, int count)
        {
            return items.GetRange(index, count);
        }

        public int IndexOf(T item)
        {
            return items.IndexOf(item);
        }

        public int IndexOf(T item, int index)
        {
            return items.IndexOf(item, index);
        }

        public int IndexOf(T item, int index, int count)
        {
            return items.IndexOf(item, index, count);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            throw new NotSupportedException();
        }

        public int LastIndexOf(T item)
        {
            return items.LastIndexOf(item);
        }

        public int LastIndexOf(T item, int index)
        {
            return items.LastIndexOf(item, index);
        }

        public int LastIndexOf(T item, int index, int count)
        {
            return items.LastIndexOf(item, index, count);
        }

        public bool Remove(T item)
        {
            return items.Remove(item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public void RemoveRange(int index, int count)
        {
            items.RemoveRange(index, count);
        }

        public void Reverse()
        {
            throw new NotSupportedException();
        }

        public void Reverse(int index, int count)
        {
            throw new NotSupportedException();
        }

        public T[] ToArray()
        {
            return items.ToArray();
        }

        public int Capacity
        {
            get => items.Capacity; set => items.Capacity = value;
        }

        public int Count => items.Count;

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Comparison<T> comparison;

        private IComparer<T> comparer;

        protected readonly List<T> items;
    }
}
