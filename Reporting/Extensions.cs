using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;

namespace Reporting
{
    public static class Extensions
    {
        public static XElement ToXml<T>(this T obj)
        {
            Type type = typeof(T);
            IEnumerable<dynamic> content()
            {
                List<Type> lists = new List<Type>() { typeof(IList<T>), typeof(IList) };
                List<Type> dictionary = new List<Type> { typeof(IDictionary<String, String>), typeof(IDictionary) };
                foreach (var pi in type.GetProperties())
                {
                    dynamic value = (dynamic)pi.GetValue(obj, null);
                    if (!pi.GetIndexParameters().Any())
                    {
                        if (pi.PropertyType.GetInterfaces().Contains(typeof(ICollection)))
                        {
                            if (pi.PropertyType.GetInterfaces().Any(i => lists.Any(l => i == l)))
                            {
                                foreach (var listItem in value as IList)
                                {
                                    yield return ToXml((dynamic)listItem);
                                }
                            }
                            else if (pi.PropertyType.GetInterfaces().Any(i => dictionary.Any(d => i == d)))
                            {
                                if (pi.Name == "AttributesForXml")
                                {
                                    foreach (var dictionaryItem in value as IDictionary<String, String>)
                                    {
                                        yield return new XAttribute(dictionaryItem.Key, dictionaryItem.Value);
                                    }
                                }
                                else if (pi.Name == "ElementsForXml")
                                {
                                    foreach (var dictionaryItem in value as IDictionary<String, String>)
                                    {
                                        yield return new XElement(dictionaryItem.Key, dictionaryItem.Value);
                                    }
                                }
                                else continue;
                            }
                            else continue;
                        }
                        else if (pi.Name == "ElementTextForXml" && pi.PropertyType == typeof(String) && (String) value != String.Empty)
                        {
                            yield return value;
                        }
                        else if (pi.PropertyType == typeof(String) || pi.PropertyType.IsPrimitive || pi.PropertyType.IsEnum)
                        {
                            yield return new XAttribute(pi.Name, value);
                        }
                        else continue;
                    }
                }
            }
            return new XElement(new XElement(type.Name,content()));
        }
        public static XDocument Transform(this XElement xml, string xsl)
        {
            var originalXml = new XDocument(xml);

            var transformedXml = new XDocument();
            using (var xmlWriter = transformedXml.CreateWriter())
            {
                var xslt = new XslCompiledTransform();
                xslt.Load(XmlReader.Create(new StreamReader(xsl)));

                using (StreamWriter streamWriter = new StreamWriter("index.html"))
                {
                    xslt.Transform(originalXml.CreateReader(), null, streamWriter);
                }
            } 

            return transformedXml;
        }
    }
}
