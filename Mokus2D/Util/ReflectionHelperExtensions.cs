using System;

namespace Mokus2D.Util
{
    public static class ReflectionHelperExtensions
    {
        public static ReflectionHelper Reflect(this object target)
        {
            return ReflectionHelper.For(target);
        }

        public static bool IsGenericDefinition(this Type targetType, Type genericType)
        {
            return (targetType.IsGenericType && IsCurrentGenericDefinition(targetType, genericType)) || (targetType.BaseType != null && targetType.BaseType.IsGenericDefinition(genericType));
        }

        private static bool IsCurrentGenericDefinition(Type targetType, Type genericType)
        {
            return !genericType.IsInterface ? targetType.GetGenericTypeDefinition() == genericType : HasGenericInterface(targetType, genericType);
        }

        private static bool HasGenericInterface(Type targetType, Type genericInterface)
        {
            foreach (Type type in targetType.GetInterfaces())
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == genericInterface)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
