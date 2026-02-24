using System;

namespace Mokus2D.Util
{
    public static class ReflectUtil
    {
        public static object CreateInstance(string typeName)
        {
            Type type = Type.GetType(typeName);
            return Activator.CreateInstance(type);
        }

        public static object CreateInstance(string typeName, params object[] parameters)
        {
            Type type = Type.GetType(typeName);
            return Activator.CreateInstance(type, parameters);
        }

        public static object CreateInstance(Type type, params object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }
    }
}
