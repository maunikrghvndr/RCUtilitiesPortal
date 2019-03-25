using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Risarc.Enterprise.Repository.Helpers
{
    public static class XmlDocumentExtentions
    {
        public static string SelectSingleNodeString(this XmlDocument xmlDoc, string xpath)
        {
            if (xmlDoc.SelectSingleNode(xpath) == null)
                throw new Exception(string.Format("The node '{0}' does not exist in '{1}'", (object)xpath, (object)xmlDoc.InnerXml));
            return xmlDoc.SelectSingleNode(xpath).InnerText;
        }
    }
}
