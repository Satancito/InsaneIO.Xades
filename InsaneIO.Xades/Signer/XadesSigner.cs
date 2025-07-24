using InsaneIO.Xades.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InsaneIO.Xades.Signer
{
    public  class XadesSigner : IXmlSigner
    {
        public XadesSigner()
        {
        }

        public XmlDocument Sign(XmlDocument document, ISignatureProfile profile, ISignatureProfileParameters parameters, bool reuseDocument = false)
        {
            if (!reuseDocument)
            {
                string xmlString = document.OuterXml;
                document = new XmlDocument();
                document.LoadXml(xmlString);
            }
            return profile.Sign(document,parameters, true);
        }

        public bool Verify(XmlDocument document, ISignatureProfile profile)
        {
            throw new NotImplementedException();
        }

    }
}
