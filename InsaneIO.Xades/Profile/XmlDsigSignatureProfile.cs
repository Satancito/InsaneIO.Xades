using InsaneIO.Insane.Cryptography;
using InsaneIO.Insane.Extensions;
using InsaneIO.Xades.Core;
using InsaneIO.Xades.Profile.XmlDsig;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InsaneIO.Xades.Profile
{

    public partial class XmlDsigSignatureProfile : XmlDsigSignatureProfileBase
    {
        [GeneratedRegex(@"<(\w+:)?SignedInfo\b[^>]*>.*?</(\w+:)?SignedInfo>", RegexOptions.Singleline)]
        private static partial Regex SignedInfoRegex();

        public XmlDsigSignatureProfile()
        {
        }

        public override void PrepareSignature(XmlDocument document, ISignatureProfileParameters parameters)
        {
            XmlElement root = document.DocumentElement!;
            XmlElement signature = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Signature, XmlDsigNamespaceData.NamespaceUri);
            signature.SetAttribute($"xmlns:{XmlDsigNamespaceData.NamespacePrefix}", XmlDsigNamespaceData.NamespaceUri);
            signature.SetAttribute(XmlDsigAttribNames.Id, SignatureId);
            root.AppendChild(signature);

            XmlElement signedInfo = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.SignedInfo, XmlDsigNamespaceData.NamespaceUri);
            //signedInfo.SetAttribute($"xmlns:{XmlDsigNamespaceData.NamespacePrefix}", XmlDsigNamespaceData.NamespaceUri);
            signedInfo.SetAttribute(XmlDsigAttribNames.Id, SignedInfoId);
            signature.AppendChild(signedInfo);

            XmlElement canonicalizationMethod = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.CanonicalizationMethod, XmlDsigNamespaceData.NamespaceUri);
            canonicalizationMethod.SetAttribute(XmlDsigAttribNames.Algorithm, CanonicalizationMethodTransform.Uri);
            canonicalizationMethod.AppendChild(document.CreateTextNode(string.Empty));
            signedInfo.AppendChild(canonicalizationMethod);

            XmlElement signatureMethod = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.SignatureMethod, XmlDsigNamespaceData.NamespaceUri);
            signatureMethod.SetAttribute(XmlDsigAttribNames.Algorithm, SignatureMethodAlgorithm.Uri);
            signatureMethod.AppendChild(document.CreateTextNode(string.Empty));
            signedInfo.AppendChild(signatureMethod);

            AddSignedInfoReferences(document, signedInfo, parameters);
            AddKeyInfo(document, signature);
        }

        public override XmlDocument Sign(XmlDocument document, ISignatureProfileParameters parameters, bool reuseDocument = false)
        {
            if (!reuseDocument)
            {
                string xmlString = document.OuterXml;
                document = new XmlDocument();
                document.LoadXml(xmlString);
            }
            PrepareSignature(document, parameters);
            AddSignatureValue(document);
            return document;
        }

        private void AddKeyInfo(XmlDocument document, XmlElement signature)
        {
            XmlElement keyInfo = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.KeyInfo, XmlDsigNamespaceData.NamespaceUri);
            keyInfo.SetAttribute(XmlDsigAttribNames.Id, KeyInfoId);
            signature.AppendChild(keyInfo);
            AddX509Data(document, keyInfo);
            AddKeyValue(document, keyInfo);
        }

        private void AddX509Data(XmlDocument document, XmlElement keyInfo)
        {
            XmlElement x509Data = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.X509Data, XmlDsigNamespaceData.NamespaceUri);
            keyInfo.AppendChild(x509Data);

            XmlElement x509Certificate = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.X509Certificate, XmlDsigNamespaceData.NamespaceUri);
            x509Certificate.InnerText = Certificate.RawData.EncodeToBase64();
            x509Data.AppendChild(x509Certificate);

            XmlElement x509IssuerSerial = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.X509IssuerSerial, XmlDsigNamespaceData.NamespaceUri);
            x509Data.AppendChild(x509IssuerSerial);

            XmlElement x509IssuerName = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.X509IssuerName, XmlDsigNamespaceData.NamespaceUri);
            x509IssuerName.InnerText = Certificate.Issuer;
            x509IssuerSerial.AppendChild(x509IssuerName);

            XmlElement x509SerialNumber = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.X509SerialNumber, XmlDsigNamespaceData.NamespaceUri);
            x509SerialNumber.InnerText = Certificate.SerialNumber.ToUpper();
            x509IssuerSerial.AppendChild(x509SerialNumber);

            XmlElement x509SubjectName = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.X509SubjectName, XmlDsigNamespaceData.NamespaceUri);
            x509SubjectName.InnerText = Certificate.Subject;
            x509Data.AppendChild(x509SubjectName);
        }

        private void AddKeyValue(XmlDocument document, XmlElement keyInfo)
        {
            XmlElement keyValue = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.KeyValue, XmlDsigNamespaceData.NamespaceUri);
            keyInfo.AppendChild(keyValue);
            XmlElement rsaKeyValue = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.RSAKeyValue, XmlDsigNamespaceData.NamespaceUri);
            keyValue.AppendChild(rsaKeyValue);
            XmlElement modulus = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Modulus, XmlDsigNamespaceData.NamespaceUri);
            XmlElement exponent = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Exponent, XmlDsigNamespaceData.NamespaceUri);
            RSAParameters rsaParams = Certificate.GetRSAPublicKey()!.ExportParameters(false);
            modulus.InnerText = rsaParams.Modulus!.EncodeToBase64();
            exponent.InnerText = rsaParams.Exponent!.EncodeToBase64();
            rsaKeyValue.AppendChild(modulus);
            rsaKeyValue.AppendChild(exponent);
        }

        public override void AddSignatureValue(XmlDocument document)
        {
            XmlElement signature = (XmlElement)document.SelectSingleNode($"//*[@Id='{SignatureId}']")!;
            XmlElement signedInfo = (XmlElement)document.SelectSingleNode($"//*[@Id='{SignedInfoId}']")!;

            string content = SignedInfoRegex().Match(document.OuterXml).Value;
            var namespaces = string.Join(" ", signedInfo.GetAllParentNamespaces().Select(ns => $@"xmlns:{ns.Key}=""{ns.Value}"""));
            content = content.Replace("SignedInfo Id", $"SignedInfo {namespaces} Id");
            content = CanonicalizationMethodTransform.CanonicalizeXml(content);
     
            using var rsa = Certificate.GetRSAPrivateKey();
            var signatureBase64 = rsa!.SignData(content.ToByteArrayUtf8(), SignatureMethodAlgorithm.GetHashAlgorithmName(), RSASignaturePadding.Pkcs1).EncodeToBase64();
            XmlElement signatureValue = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.SignatureValue, XmlDsigNamespaceData.NamespaceUri);
            signatureValue.SetAttribute(XmlDsigAttribNames.Id, SignatureValueId);
            signatureValue.AppendChild(document.CreateTextNode(signatureBase64));
            signature.InsertAfter(signatureValue, signedInfo);
        }

        private void AddSignedInfoReferences(XmlDocument document, XmlElement signedInfo, ISignatureProfileParameters parameters)
        {
            XmlElement reference = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Reference, XmlDsigNamespaceData.NamespaceUri);
            reference.SetAttribute(XmlDsigAttribNames.Id, ContentReferenceId);
            reference.SetAttribute(XmlDsigAttribNames.URI, $"#{ContentId}");
            signedInfo.AppendChild(reference);

            XmlElement transforms = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Transforms, XmlDsigNamespaceData.NamespaceUri);
            XmlElement transform = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Transform, XmlDsigNamespaceData.NamespaceUri);
            transform.AppendChild(document.CreateTextNode(string.Empty));
            transform.SetAttribute(XmlDsigAttribNames.Algorithm, SignedXml.XmlDsigEnvelopedSignatureTransformUrl);
            transforms.AppendChild(transform);
            reference.AppendChild(transforms);

            XmlElement referenceDigestMethod = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.DigestMethod, XmlDsigNamespaceData.NamespaceUri);
            referenceDigestMethod.SetAttribute(XmlDsigAttribNames.Algorithm, DigestMethodAlgorithm.Uri);
            referenceDigestMethod.AppendChild(document.CreateTextNode(string.Empty));
            reference.AppendChild(referenceDigestMethod);
            XmlElement referenceDigestValue = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.DigestValue, XmlDsigNamespaceData.NamespaceUri);
            string canonicalizedContent = CanonicalizationMethodTransform.CanonicalizeXml( document.OuterXml.RemoveSignatureNode());
            referenceDigestValue.InnerText = canonicalizedContent.ComputeEncodedHash(Base64Encoder.DefaultInstance, DigestMethodAlgorithm.GetHashAlgorithm());
            reference.AppendChild(referenceDigestValue);

        }

        public override bool Verify(XmlDocument document)
        {
            throw new NotImplementedException();
        }


    }
}
