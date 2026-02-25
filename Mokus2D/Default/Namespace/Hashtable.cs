using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class Hashtable : Dictionary<object, object>
    {
        private object GetObject(string key, bool checkForNull)
        {
            string[] array = key.Split(['/']);
            Hashtable hashtable = this;
            int i = 0;
            while (i < array.Length)
            {
                string text = array[i];
                if (!Enumerable.Contains(hashtable.Keys, text))
                {
                    return checkForNull
                        ? throw new Exception(string.Concat(new string[] { "Hashtable key `", key, "` not found - at `", text, "`." }))
                        : null;
                }
                else
                {
                    object obj = hashtable[text];
                    if (obj == null)
                    {
                        return checkForNull
                            ? throw new Exception(string.Concat(new string[] { "Hashtable key `", key, "` is null - at `", text, "`." }))
                            : null;
                    }
                    else
                    {
                        if (i == array.Length - 1)
                        {
                            return obj;
                        }
                        hashtable = obj as Hashtable;
                        i++;
                    }
                }
            }
            return null;
        }

        public object GetObject(string key)
        {
            return GetObject(key, true);
        }

        public bool Exists(string key)
        {
            return GetObject(key, false) != null;
        }

        public bool NotExists(string key)
        {
            return !Exists(key);
        }

        public Hashtable GetHashtable(string key)
        {
            return GetObject(key) as Hashtable;
        }

        public string GetString(string key)
        {
            return GetObject(key) as string;
        }

        public float GetFloat(string key)
        {
            return (float)Convert.ToDouble(GetString(key), CultureInfo.InvariantCulture);
        }

        public bool GetBool(string key)
        {
            return Exists(key) && Convert.ToBoolean(GetObject(key));
        }

        public int GetInt(string key)
        {
            return Convert.ToInt32(GetString(key));
        }

        public uint GetUInt(string key)
        {
            return Convert.ToUInt32(GetString(key));
        }

        public int GetShort(string key)
        {
            return Convert.ToInt16(GetString(key));
        }

        public ArrayList GetArrayList(string key)
        {
            return GetObject(key) as ArrayList;
        }

        public Vector2 GetVector(string key)
        {
            return (Vector2)GetObject(key);
        }

        public string GetString(string key, string defaultValue)
        {
            return !Exists(key) ? defaultValue : GetString(key);
        }

        public float GetFloat(string key, float defaultValue)
        {
            return !Exists(key) ? defaultValue : GetFloat(key);
        }

        public bool GetBool(string key, bool defaultValue)
        {
            return !Exists(key) ? defaultValue : GetBool(key);
        }

        public int GetInt(string key, int defaultValue)
        {
            return !Exists(key) ? defaultValue : GetInt(key);
        }

        public ArrayList GetArrayList(string key, ArrayList defaultValue)
        {
            return !Exists(key) ? defaultValue : GetArrayList(key);
        }

        public Vector2 GetVector(string key, Vector2 defaultValue)
        {
            return !Exists(key) ? defaultValue : GetVector(key);
        }

        public override string ToString()
        {
            string text3;
            using (StringWriter stringWriter = new())
            {
                foreach (KeyValuePair<object, object> keyValuePair in this)
                {
                    string text = "<null>";
                    if (keyValuePair.Value != null)
                    {
                        text = keyValuePair.Value.ToString();
                        if (keyValuePair.Value is Hashtable)
                        {
                            StringWriter stringWriter2 = new();
                            using (StringReader stringReader = new(text))
                            {
                                string text2;
                                while ((text2 = stringReader.ReadLine()) != null)
                                {
                                    stringWriter2.WriteLine("|    " + text2);
                                }
                            }
                            text = "\n" + stringWriter2;
                        }
                    }
                    stringWriter.WriteLine(keyValuePair.Key + ": " + text);
                }
                text3 = stringWriter.ToString();
            }
            return text3;
        }

        public void Trace()
        {
        }
    }
}
