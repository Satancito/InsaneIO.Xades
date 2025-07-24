using InsaneIO.Insane.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InsaneIO.Xades.Core
{
    public interface ISignatureProfile
    {

        XmlDocument Sign(XmlDocument document, ISignatureProfileParameters signatureProfileParameters, bool reuseDocument = false);
        bool Verify(XmlDocument document);

        
    }
}
