using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlParser;

public class XmlToCSharpClassConverter
{
    public static string ConvertXmlToCSharpClass(string xml, string className)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        var rootNode = doc.DocumentElement;

        StringBuilder classBuilder = new StringBuilder();

        classBuilder.AppendLine("public class " + className);
        classBuilder.AppendLine("{");

        ParseXmlNode(rootNode, classBuilder, new HashSet<string>());

        classBuilder.AppendLine("}");

        return classBuilder.ToString();
    }

    private static void ParseXmlNode(XmlNode node, StringBuilder classBuilder, HashSet<string> generatedClasses)
    {
        foreach (XmlNode childNode in node.ChildNodes)
        {
            if (childNode is XmlElement element)
            {
                string propertyName = char.ToUpper(element.Name[0]) + element.Name.Substring(1);

                // Handle attributes of self-closing tags and complex types
                if (element.HasAttributes || element.HasChildNodes && element.FirstChild is XmlElement)
                {
                    if (!generatedClasses.Contains(propertyName))
                    {
                        generatedClasses.Add(propertyName);
                        classBuilder.AppendLine($"    public {propertyName} {propertyName} {{ get; set; }}");
                        classBuilder.AppendLine("}");
                        classBuilder.AppendLine($"public class {propertyName}");
                        classBuilder.AppendLine("{");

                        foreach (XmlAttribute attr in element.Attributes)
                        {
                            string attrName = char.ToUpper(attr.Name[0]) + attr.Name.Substring(1);
                            classBuilder.AppendLine($"    public string {attrName} {{ get; set; }}");
                        }

                        ParseXmlNode(element, classBuilder, generatedClasses);
                    }
                }
                else if (element.HasChildNodes && element.FirstChild is XmlText)
                {
                    // Handle simple text nodes
                    classBuilder.AppendLine($"    public string {propertyName} {{ get; set; }}");
                }
            }
        }
    }
}
