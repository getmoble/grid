using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Grid.Infrastructure
{
    public static class XmlSerializerHelper
    {
        private static readonly Dictionary<Type, XmlSerializer> XmlFormatter;

        static XmlSerializerHelper()
        {
            XmlFormatter = new Dictionary<Type, XmlSerializer>();
        }

        private static XmlSerializer GetFormatter(Type objType)
        {
            if (!XmlFormatter.ContainsKey(objType))
                XmlFormatter.Add(objType, new XmlSerializer(objType));
            return XmlFormatter[objType];
        }

        public static string ToXml<T>(T obj)
            where T : new()
        {
            using (var sw = new StringWriter())
            {
                GetFormatter(obj.GetType()).Serialize(sw, obj);
                return sw.ToString();
            }
        }

        public static T FromXml<T>(string objectAsXml)
             where T : new()
        {
            if (!string.IsNullOrEmpty(objectAsXml))
            {
                using (var sr = new StringReader(objectAsXml))
                {
                    return (T)GetFormatter(typeof(T)).Deserialize(sr);
                }
            }

            return default(T);
        }
    }
}
