using System.IO;
using System.Xml.Linq;

namespace Mokus2D.Util.Xml
{
    public class XmlSerializer : XmlSerializerBase
    {
        public override object DeserializeFile(Stream stream)
        {
            XDocument xdocument = XDocument.Load(stream);
            return Deserialize(xdocument.Root);
        }

        public object Deserialize(XElement element)
        {
            string text = element.Attribute("__type__").Value;
            text = UnprocessValue(text);
            if (text == "null")
            {
                return null;
            }
            object obj = ReflectUtil.CreateInstance(text);
            foreach (XAttribute xattribute in element.Attributes())
            {
                if (xattribute.Name != "__type__")
                {
                    SetObjectValue(obj, xattribute.Value, xattribute.Name.ToString());
                }
            }
            foreach (XNode xnode in element.Nodes())
            {
                XElement xelement = (XElement)xnode;
                SetObjectValue(obj, Deserialize(xelement), xelement.Name.ToString());
            }
            return obj;
        }
    }
}
