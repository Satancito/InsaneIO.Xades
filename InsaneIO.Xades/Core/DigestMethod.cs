using InsaneIO.Insane.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace InsaneIO.Xades.Core
{

    public class DigestMethod
    {
        public static readonly DigestMethod Sha1 = new(SignedXml.XmlDsigSHA1Url);
        public static readonly DigestMethod Sha256 = new(SignedXml.XmlDsigSHA256Url);
        public static readonly DigestMethod Sha384 = new(SignedXml.XmlDsigSHA384Url);
        public static readonly DigestMethod Sha512 = new(SignedXml.XmlDsigSHA512Url);

        private DigestMethod(string uri)
        {
            Uri = uri;
        }

        public string Uri { get; }
    }


    public static class DigestMethodExtensions
    {
        public static HashAlgorithm GetHashAlgorithm(this DigestMethod digestMethod)
        {
            return digestMethod switch 
            {
                var method when method.Equals(DigestMethod.Sha1) => HashAlgorithm.Sha1,
                var method when method.Equals(DigestMethod.Sha256) => HashAlgorithm.Sha256,
                var method when method.Equals(DigestMethod.Sha384) => HashAlgorithm.Sha384,
                var method when method.Equals(DigestMethod.Sha512) => HashAlgorithm.Sha512,
                _ => throw new NotSupportedException($"Digest method {digestMethod.Uri} is not supported.")
            };
        }

    }
}
