using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Mokus2D.Default.Namespace
{
    public class ArrayList : List<object>
    {
        public ArrayList()
        {
        }

        public ArrayList(params object[] objects)
        {
            AddRange(objects);
        }

        public override string ToString()
        {
            string text = "";
            foreach (object obj in this)
            {
                if (text != "")
                {
                    text += ", ";
                }
                text += obj.ToString();
            }
            return "[" + text + "]";
        }

        public List<Vector2> ToListVector2()
        {
            List<Vector2> list = new();
            foreach (object obj in this)
            {
                Vector2 vector = (Vector2)obj;
                list.Add(vector);
            }
            return list;
        }
    }
}
