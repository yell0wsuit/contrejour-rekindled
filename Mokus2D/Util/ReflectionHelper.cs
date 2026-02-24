using System;
using System.Globalization;
using System.Reflection;

namespace Mokus2D.Util
{
    public class ReflectionHelper
    {
        private ReflectionHelper(object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }
            this.target = target;
        }

        public static FieldInfo FindField(Type type, string name)
        {
            return type == null || type == typeof(object) ? null : type.GetField(name, (BindingFlags)54) ?? FindField(type.BaseType, name);
        }

        public static PropertyInfo FindProperty(Type type, string name)
        {
            return type == null || type == typeof(object) ? null : type.GetProperty(name, (BindingFlags)54) ?? FindProperty(type.BaseType, name);
        }

        public static ReflectionHelper For(object targetInstance)
        {
            return new ReflectionHelper(targetInstance);
        }

        public ReflectionHelper Field(string fieldName)
        {
            currentMemberInfo = FindField(target.GetType(), fieldName);
            return currentMemberInfo == null
                ? throw new NullReferenceException(string.Format("Sorry, There is no such field = {0} in class type {1}", fieldName, target.GetType().FullName))
                : this;
        }

        public ReflectionHelper FieldOrProperty(string fieldName)
        {
            currentMemberInfo = FindField(target.GetType(), fieldName);
            if (currentMemberInfo == null)
            {
                currentMemberInfo = FindProperty(target.GetType(), fieldName);
                if (currentMemberInfo == null)
                {
                    throw new NullReferenceException(string.Format("Sorry, There is no such field or property = {0} in class type {1}", fieldName, target.GetType().FullName));
                }
            }
            return this;
        }

        public object GetValue()
        {
            Type type = currentMemberInfo.GetType();
            return type.IsSubclassOf(typeof(PropertyInfo))
                ? ((PropertyInfo)currentMemberInfo).GetValue(target, null)
                : type.IsSubclassOf(typeof(FieldInfo))
                ? ((FieldInfo)currentMemberInfo).GetValue(target)
                : throw new NotSupportedException(string.Format("Unsupported type of modified memeber. {0}", type.Name));
        }

        public ReflectionHelper Property(string propertyName)
        {
            currentMemberInfo = FindProperty(target.GetType(), propertyName);
            return currentMemberInfo == null
                ? throw new NullReferenceException(string.Format("Sorry, There is no such property = {0} in class type {1}", propertyName, target.GetType().FullName))
                : this;
        }

        public T Return<T>()
        {
            return (T)target;
        }

        public ReflectionHelper SetValue(object value)
        {
            if (currentMemberInfo == null)
            {
                throw new NullReferenceException("Modified member is null.");
            }
            Type type = currentMemberInfo.GetType();
            if (type.IsSubclassOf(typeof(PropertyInfo)))
            {
                PropertyInfo propertyInfo = (PropertyInfo)currentMemberInfo;
                value = Convert.ChangeType(value, propertyInfo.PropertyType, CultureInfo.InvariantCulture.NumberFormat);
                propertyInfo.SetValue(target, value, null);
            }
            else
            {
                if (!type.IsSubclassOf(typeof(FieldInfo)))
                {
                    throw new NotSupportedException(string.Format("Unsupported type of modified memeber. {0}", type.Name));
                }
                FieldInfo fieldInfo = (FieldInfo)currentMemberInfo;
                value = Convert.ChangeType(value, fieldInfo.FieldType, CultureInfo.InvariantCulture.NumberFormat);
                fieldInfo.SetValue(target, value);
            }
            return this;
        }

        private readonly object target;

        private MemberInfo currentMemberInfo;
    }
}
