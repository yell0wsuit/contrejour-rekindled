using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Mokus2D.Util.Xml
{
    public abstract class XmlSerializerBase
    {
        public void AddAlias(string source, string alias)
        {
            aliases[alias] = source;
        }

        public abstract object DeserializeFile(Stream stream);

        protected string UnprocessValue(string source)
        {
            foreach (KeyValuePair<string, string> keyValuePair in aliases)
            {
                if (source.ToLower().Equals(keyValuePair.Value.ToLower()))
                {
                    return keyValuePair.Key;
                }
            }
            return source;
        }

        protected void SetObjectValue(object target, object targetValue, string key)
        {
            key = UnprocessAttributeName(key);
            if (target is IList)
            {
                int num = Convert.ToInt32(key);
                IList list = target as IList;
                while (list.Count < num + 1)
                {
                    _ = list.Add(null);
                }
                list[num] = targetValue;
                return;
            }
            if (target is IDictionary)
            {
                ((IDictionary)target)[key] = targetValue;
                return;
            }
            _ = target.Reflect().FieldOrProperty(key).SetValue(targetValue);
        }

        private string UnprocessAttributeName(string attributeName)
        {
            return attributeName.StartsWith("__") ? attributeName[2..] : UnprocessValue(attributeName);
        }

        private readonly Dictionary<string, string> aliases = new();
    }
}
