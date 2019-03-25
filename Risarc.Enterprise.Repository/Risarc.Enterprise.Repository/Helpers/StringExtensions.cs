using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Risarc.Enterprise.Repository.Helpers
{
    public static class StringExtensions
    {
        public static string AppendNode(this string xmlString, string node, string value)
        {
            if (xmlString.Length == 0)
                xmlString = "<PARAMETERS></PARAMETERS>";
            else if (xmlString.IndexOf("<PARAMETERS>") == -1)
                xmlString = "<PARAMETERS>" + xmlString + "</PARAMETERS>";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            XmlNode oldChild = xmlDocument.SelectSingleNode("PARAMETERS/" + node);
            if (oldChild != null)
                xmlDocument.DocumentElement.RemoveChild(oldChild);
            XmlElement element = xmlDocument.CreateElement(node.Replace(" ", "").Trim().ToUpper());
            element.InnerText = value;
            xmlDocument.DocumentElement.AppendChild((XmlNode)element);
            xmlString = xmlDocument.SelectSingleNode("PARAMETERS").OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
            return xmlString;
        }
    }
}
