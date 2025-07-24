using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsaneIO.Xades.Core
{
    public class XmlNamespaceData
    {
        public static readonly XmlNamespaceData Empty = new()
        {
            NamespacePrefix = string.Empty,
            NamespaceUri = string.Empty
        };

        public static readonly XmlNamespaceData Xades = new()
        {
            NamespacePrefix = Constants.XADES_PREFIX,
            NamespaceUri = Constants.XADES_NAMESPACE_URI
        };

        public static readonly XmlNamespaceData Etsi = new()
        {
            NamespacePrefix = Constants.ETSI_PREFIX,
            NamespaceUri = Constants.ETSI_NAMESPACE_URI
        };

        public static readonly XmlNamespaceData XmlDSig = new()
        {
            NamespacePrefix = Constants.XMLDSIG_PREFIX,
            NamespaceUri = Constants.XMLDSIG_NAMESPACE_URI
        };

        public required string NamespacePrefix { init; get; }
        public required string NamespaceUri { init; get; }

        public XmlNamespaceData()
        {
          
        }
    }
}
