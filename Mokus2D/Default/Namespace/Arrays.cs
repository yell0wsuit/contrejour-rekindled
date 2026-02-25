using System.Collections.Generic;

namespace Mokus2D.Default.Namespace
{
    public class Arrays
    {
        public static object RandomItem(ArrayList source)
        {
            return source[Maths.Random(source.Count)];
        }

        public static object MaxItem<T>(List<T> source, MaxItemDelegate getValueDelegate, object param)
        {
            float num = -100100100f;
            object obj = null;
            foreach (T t in source)
            {
                object obj2 = t;
                float num2 = getValueDelegate(obj2, param);
                if (num2 > num)
                {
                    num = num2;
                    obj = obj2;
                }
            }
            return obj;
        }
    }
}
