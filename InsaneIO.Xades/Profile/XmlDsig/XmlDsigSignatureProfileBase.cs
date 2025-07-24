using InsaneIO.Insane.Cryptography;
using InsaneIO.Xades.Core;
using InsaneIO.Xades.Profile.XadesBes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InsaneIO.Xades.Profile.XmlDsig
{
    public abstract class XmlDsigSignatureProfileBase: ISignatureProfile
    {

        public SignaturePackaging Packaging { get; init; } = SignaturePackaging.Enveloped;

        public required X509Certificate2 Certificate { get; init; } = null!;
        public DigestMethod DigestMethodAlgorithm { get; init; } = DigestMethod.Sha256;
        public SignatureMethod SignatureMethodAlgorithm { get; init; } = SignatureMethod.RsaSha256;
        public CanonicalizationMethod CanonicalizationMethodTransform { get; init; } = CanonicalizationMethod.XmlDsigC14N;
        public XmlNamespaceData XmlDsigNamespaceData { get; init; } = XmlNamespaceData.XmlDSig;

        public string SignatureId { get; set; } = GenerateHexId(nameof(SignatureId));
        public string ContentId { get; set; } = GenerateHexId(nameof(ContentId));
        public string SignedInfoId { get; set; } = GenerateHexId(nameof(SignedInfoId));
        public string ContentReferenceId { get; set; } = GenerateHexId(nameof(ContentReferenceId));
        public string SignatureValueId { get; set; } = GenerateHexId(nameof(SignatureValueId));
        public string KeyInfoId { get; set; } = GenerateHexId(nameof(KeyInfoId));

        public static string GenerateHexId(string prefix ,uint randomBytesSize = 16) => $"{prefix}_{HexEncoder.DefaultInstance.Encode(RandomExtensions.NextBytes(randomBytesSize))}";
   
        public static string GenerateNumericId(string prefix, int minValue = 999999, int maxValue = int.MaxValue) => $"{prefix}_{minValue.NextValue(maxValue)}";
        
        public abstract bool Verify(XmlDocument document);

        public abstract void AddSignatureValue(XmlDocument document);

        public abstract XmlDocument Sign(XmlDocument document, ISignatureProfileParameters signatureProfileParameters, bool reuseDocument = false);
        public abstract void PrepareSignature(XmlDocument document, ISignatureProfileParameters signatureProfileParameters);
    }
}
