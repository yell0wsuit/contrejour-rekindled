using System;
using System.Collections.Generic;

public class Pool<T> where T : class, new()
{
    public Action<T> Initialize { get; set; }

    public Action<T> Deinitialize { get; set; }

    public int ValidCount
    {
        get
        {
            return items.Count - InvalidCount;
        }
    }

    public int InvalidCount { get; private set; }

    public T this[int index]
    {
        get
        {
            index += InvalidCount;
            return index < InvalidCount || index >= items.Count
                ? throw new IndexOutOfRangeException("The index must be less than or equal to ValidCount")
                : items[index];
        }
    }

    public Pool(bool allocateImmediately = false, bool resizable = false, Predicate<T> validateFunc = null, Func<T> allocateFunc = null)
        : this(64, allocateImmediately, resizable, validateFunc, allocateFunc)
    {
    }

    public Pool(int initialSize, bool allocateImmediately = false, bool resizable = false, Predicate<T> validateFunc = null, Func<T> allocateFunc = null)
    {
        if (initialSize < 1)
        {
            throw new ArgumentOutOfRangeException("initialSize", "initialSize must be at least 1.");
        }
        this.resizable = resizable;
        items = new List<T>(initialSize);
        items.EnsureCapacity(initialSize);
        validate = validateFunc;
        Func<T> func = allocateFunc;
        if (allocateFunc == null)
        {
            func = () => new T();
        }
        allocate = func;
        if (allocateImmediately)
        {
            if (allocateFunc != null)
            {
                for (int i = 0; i < initialSize; i++)
                {
                    items.Add(allocateFunc.Invoke());
                }
            }
            else
            {
                for (int j = 0; j < initialSize; j++)
                {
                    items.Add(default(T));
                }
            }
            InvalidCount = items.Count;
        }
    }

    public void CleanUp()
    {
        for (int i = InvalidCount; i < items.Count; i++)
        {
            T t = items[i];
            if (!validate.Invoke(t))
            {
                DeleteObject(t, i);
            }
        }
    }

    private void DeleteObject(T obj, int i)
    {
        if (i != InvalidCount)
        {
            items[i] = items[InvalidCount];
            items[InvalidCount] = obj;
        }
        if (Deinitialize != null)
        {
            Deinitialize.Invoke(obj);
        }
        InvalidCount++;
    }

    public void Delete(T obj)
    {
        DeleteObject(obj, items.IndexOf(obj));
    }

    public T New()
    {
        if (InvalidCount == 0)
        {
            if (!resizable)
            {
                throw new InvalidOperationException("No free space in pool");
            }
            InvalidCount++;
            if (InvalidCount >= items.Capacity)
            {
                items.EnsureCapacity(items.Capacity * 2);
            }
            items[items.Count] = items[0];
            items[0] = default(T);
        }
        InvalidCount--;
        T t = items[InvalidCount];
        if (t == null)
        {
            t = CreateObject();
            if (t == null)
            {
                throw new InvalidOperationException("The pool's allocate method returned a null object reference.");
            }
            items[InvalidCount] = t;
        }
        if (Initialize != null)
        {
            Initialize.Invoke(t);
        }
        return t;
    }

    private T CreateObject()
    {
        return allocate != null ? allocate.Invoke() : default(T);
    }

    private List<T> items;

    private readonly Predicate<T> validate;

    private readonly Func<T> allocate;

    private readonly bool resizable;
}
