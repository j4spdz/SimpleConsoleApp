using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConsoleUI.Helper
{
    /// <summary>
    /// Reference: https://stackoverflow.com/questions/1123718/format-xml-string-to-print-friendly-xml-string
    /// </summary>
    public static class XmlExtensions
    {
        public static string FormatXml(this string xml, bool indent = true, bool newLineOnAttributes = false, string indentChars = "  ", ConformanceLevel conformanceLevel = ConformanceLevel.Document) =>
            xml.FormatXml(new XmlWriterSettings { Indent = indent, NewLineOnAttributes = newLineOnAttributes, IndentChars = indentChars, ConformanceLevel = conformanceLevel });

        public static string FormatXml(this string xml, XmlWriterSettings settings)
        {
            using (var textReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(textReader, new XmlReaderSettings { ConformanceLevel = settings.ConformanceLevel }))
            using (var textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                    xmlWriter.WriteNode(xmlReader, true);
                return textWriter.ToString();
            }
        }
    }
}
