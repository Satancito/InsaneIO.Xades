using InsaneIO.Xades.Profile.XmlDsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsaneIO.Xades.Profile.XadesBes
{
    public class XadesBesNodeNames
    {
        public const string Object = nameof(Object);
        public const string QualifyingProperties = nameof(QualifyingProperties);
        public const string SignedProperties = nameof(SignedProperties);
        public const string SignedSignatureProperties = nameof(SignedSignatureProperties);
        public const string SigningTime = nameof(SigningTime);
        public const string SigningCertificate = nameof(SigningCertificate);
        public const string Cert = nameof(Cert);
        public const string CertDigest = nameof(CertDigest);
        public const string DigestMethod = XmlDSigNodeNames.DigestMethod;
        public const string DigestValue = XmlDSigNodeNames.DigestValue; 
        public const string IssuerSerial = nameof(IssuerSerial);
        public const string X509IssuerName = XmlDSigNodeNames.X509IssuerName;
        public const string X509SerialNumber = XmlDSigNodeNames.X509SerialNumber;
        public const string X509SubjectName = XmlDSigNodeNames.X509SubjectName;
        public const string SignedDataObjectProperties = nameof(SignedDataObjectProperties);
        public const string DataObjectFormat = nameof(DataObjectFormat);
        public const string Reference = XmlDSigNodeNames.Reference;

        public const string Description = nameof(Description);
        public const string MimeType = nameof(MimeType);
        public const string Encoding = nameof(Encoding);
    }
}
