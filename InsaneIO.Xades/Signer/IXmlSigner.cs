using InsaneIO.Xades.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InsaneIO.Xades.Signer
{
    public interface IXmlSigner
    {
        public XmlDocument Sign(XmlDocument document, ISignatureProfile profile, ISignatureProfileParameters parameters, bool reuseDocument = false);
        public bool Verify(XmlDocument document, ISignatureProfile profile);
    }
}
