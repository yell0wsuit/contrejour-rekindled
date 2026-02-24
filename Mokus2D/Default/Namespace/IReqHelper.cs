using System.Collections;

namespace Default.Namespace
{
    public static class IReqHelper
    {
        public static ArrayList Filter(IList objects, IReq req)
        {
            ArrayList arrayList = new();
            foreach (object obj in objects)
            {
                if (req.Meet(obj))
                {
                    arrayList.Add(obj);
                }
            }
            return arrayList;
        }
    }
}
