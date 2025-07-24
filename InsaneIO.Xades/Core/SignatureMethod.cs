using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using HashAlgorithm = InsaneIO.Insane.Cryptography.HashAlgorithm;

namespace InsaneIO.Xades.Core
{
    public class SignatureMethod
    {
        public static readonly SignatureMethod RsaSha1 = new(SignedXml.XmlDsigRSASHA1Url);
        public static readonly SignatureMethod RsaSha256 = new(SignedXml.XmlDsigRSASHA256Url);
        public static readonly SignatureMethod RsaSha384 = new(SignedXml.XmlDsigRSASHA384Url);
        public static readonly SignatureMethod RsaSha512 = new(SignedXml.XmlDsigRSASHA512Url);
        private SignatureMethod(string uri)
        {
            Uri = uri;
        }
        public string Uri { get; }
    }

    public static class SignatureMethodExtensions
    {
        public static HashAlgorithm GetHashAlgorithm(this SignatureMethod signatureMethod)
        {
            return signatureMethod switch
            {
                var method when method.Equals(SignatureMethod.RsaSha1) => HashAlgorithm.Sha1,
                var method when method.Equals(SignatureMethod.RsaSha256) => HashAlgorithm.Sha256,
                var method when method.Equals(SignatureMethod.RsaSha384) => HashAlgorithm.Sha384,
                var method when method.Equals(SignatureMethod.RsaSha512) => HashAlgorithm.Sha512,
                _ => throw new NotSupportedException($"Signature method {signatureMethod.Uri} is not supported.")
            };
        }

        public static HashAlgorithmName GetHashAlgorithmName(this SignatureMethod signatureMethod)
        {
            return signatureMethod switch
            {
                var method when method.Equals(SignatureMethod.RsaSha1) => HashAlgorithmName.SHA1,
                var method when method.Equals(SignatureMethod.RsaSha256) => HashAlgorithmName.SHA256,
                var method when method.Equals(SignatureMethod.RsaSha384) => HashAlgorithmName.SHA384,
                var method when method.Equals(SignatureMethod.RsaSha512) => HashAlgorithmName.SHA512,
                _ => throw new NotSupportedException($"Signature method {signatureMethod.Uri} is not supported.")
            };
        }
    }
}
