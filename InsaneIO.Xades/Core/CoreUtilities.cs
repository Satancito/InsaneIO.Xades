using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace InsaneIO.Xades.Core
{
    public  static class CoreUtilities
    {

        public static Dictionary<string, string> GetAllParentNamespaces(this XmlElement element)
        {
            var namespaces = new Dictionary<string, string>();
            XmlNode? current = element;

            while (current != null && current.NodeType == XmlNodeType.Element)
            {
                var attrs = current.Attributes;
                if (attrs != null)
                {
                    foreach (XmlAttribute attr in attrs)
                    {
                        if (attr.Prefix == "xmlns")
                        {
                            // Prefixed namespace: xmlns:prefix="uri"
                            if (!namespaces.ContainsKey(attr.LocalName))
                                namespaces[attr.LocalName] = attr.Value;
                        }
                        else if (attr.Name == "xmlns")
                        {
                            // Default namespace: xmlns="uri"
                            if (!namespaces.ContainsKey(string.Empty))
                                namespaces[string.Empty] = attr.Value;
                        }
                    }
                }
                current = current.ParentNode;
            }
            return namespaces;
        }

        public static string RemoveSignatureNode(this string xmlInput)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.LoadXml(xmlInput);
            XmlNamespaceManager ns = new XmlNamespaceManager(xmlDoc.NameTable);
            XmlNode signatureNode = xmlDoc.SelectSingleNode("//*[local-name()='Signature']")!;
            if (signatureNode != null && signatureNode.ParentNode != null)
            {
                signatureNode.ParentNode.RemoveChild(signatureNode);
            }
            return xmlDoc.OuterXml;
        }

        
    }
}
