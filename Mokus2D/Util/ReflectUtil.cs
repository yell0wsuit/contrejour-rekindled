using System;

namespace Mokus2D.Util
{
    public static class ReflectUtil
    {
        public static object CreateInstance(string typeName)
        {
            Type type = ResolveType(typeName);
            return Activator.CreateInstance(type);
        }

        public static object CreateInstance(string typeName, params object[] parameters)
        {
            Type type = ResolveType(typeName);
            return Activator.CreateInstance(type, parameters);
        }

        public static object CreateInstance(Type type, params object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }

        private static Type ResolveType(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                throw new ArgumentException("Type name cannot be null or empty.", nameof(typeName));
            }

            Type type = Type.GetType(typeName);
            return type ?? throw new TypeLoadException($"Failed to resolve type '{typeName}'. Verify namespace prefix and assembly.");
        }
    }
}
