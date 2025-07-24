using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsaneIO.Xades.Profile.XmlDsig
{
    internal class XmlDSigNodeNames
    {
        public const string Signature  = nameof(Signature);
        public const string DigestMethod = nameof(DigestMethod);
        public const string DigestValue = nameof(DigestValue);
        public const string SignedInfo  = nameof(SignedInfo);
        public const string CanonicalizationMethod  = nameof(CanonicalizationMethod);
        public const string SignatureMethod = nameof(SignatureMethod);
        public const string Reference = nameof(Reference);
        public const string Transforms = nameof(Transforms);
        public const string Transform = nameof(Transform);
        public const string SignatureValue = nameof(SignatureValue);   
        public const string KeyInfo = nameof(KeyInfo);
        public const string X509Data = nameof(X509Data);
        public const string X509Certificate = nameof(X509Certificate);
        public const string X509IssuerSerial = nameof(X509IssuerSerial);
        public const string X509IssuerName = nameof(X509IssuerName);
        public const string X509SerialNumber = nameof(X509SerialNumber);
        public const string X509SubjectName = nameof(X509SubjectName);
        public const string KeyValue = nameof(KeyValue);
        public const string RSAKeyValue = nameof(RSAKeyValue);
        public const string Modulus = nameof(Modulus);
        public const string Exponent = nameof(Exponent);
        
    }
}
