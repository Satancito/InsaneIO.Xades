using InsaneIO.Insane.Cryptography;
using InsaneIO.Insane.Extensions;
using InsaneIO.Xades.Core;
using InsaneIO.Xades.Profile.XadesBes;
using InsaneIO.Xades.Profile.XmlDsig;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Xml;

namespace InsaneIO.Xades.Profile
{
    public partial class XadesBesSignatureProfile : XmlDsigSignatureProfile
    {
        public XmlNamespaceData XadesNamespaceData { get; init; } = XmlNamespaceData.Xades;

        public string SignedPropertiesId { get; set; } = GenerateHexId(nameof(SignedPropertiesId));
        public string SignedPropertiesReferenceId { get; set; } = GenerateHexId(nameof(SignedPropertiesReferenceId));

        [GeneratedRegex(@"<(\w+:)?SignedProperties\b[^>]*>.*?</(\w+:)?SignedProperties>", RegexOptions.Singleline)]
        private static partial Regex SignedPropertiesRegex();

        

        public XadesBesSignatureProfile()
        {

        }
        
        private void AddSignedInfoReferences(XmlDocument document, ISignatureProfileParameters parameters)
        {
            XmlElement signature = (XmlElement)document.SelectSingleNode($"//*[@Id='{SignatureId}']")!;
            XmlElement signedInfo = (XmlElement)document.SelectSingleNode($"//*[@Id='{SignedInfoId}']")!;
            XmlElement signedProperties = (XmlElement)document.SelectSingleNode($"//*[@Id='{SignedPropertiesId}']")!;
            XadesBesSignatureProfileParameters xadesParameters = (XadesBesSignatureProfileParameters)parameters;
            
            XmlElement reference = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Reference, XmlDsigNamespaceData.NamespaceUri);
            reference.SetAttribute(XmlDsigAttribNames.Id, $"{SignedPropertiesReferenceId}");
            reference.SetAttribute(XadesBesAttribNames.Type, XadesBesAttribValues.Type);
            reference.SetAttribute(XmlDsigAttribNames.URI, $"#{SignedPropertiesId}");
            signedInfo.AppendChild(reference);

            XmlElement transforms = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Transforms, XmlDsigNamespaceData.NamespaceUri);
            reference.AppendChild(transforms);
            XmlElement transform = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Transform, XmlDsigNamespaceData.NamespaceUri);
            transform.AppendChild(document.CreateTextNode(string.Empty));
            transform.SetAttribute(XmlDsigAttribNames.Algorithm, CanonicalizationMethodTransform.Uri);
            transforms.AppendChild(transform);

            XmlElement referenceDigestMethod = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.DigestMethod, XmlDsigNamespaceData.NamespaceUri);
            referenceDigestMethod.SetAttribute(XmlDsigAttribNames.Algorithm, DigestMethodAlgorithm.Uri);
            referenceDigestMethod.AppendChild(document.CreateTextNode(string.Empty));
            reference.AppendChild(referenceDigestMethod);

            XmlElement referenceDigestValue = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.DigestValue, XmlDsigNamespaceData.NamespaceUri);
            
            string content = SignedPropertiesRegex().Match(document.OuterXml).Value;
            var namespaces = string.Join(" ", signedProperties.GetAllParentNamespaces().Select(ns => $@"xmlns:{ns.Key}=""{ns.Value}"""));
            content = content.Replace("SignedProperties Id", $"SignedProperties {namespaces} Id");
            content = CanonicalizationMethodTransform.CanonicalizeXml(content);
            content = content.ComputeEncodedHash(Base64Encoder.DefaultInstance, DigestMethodAlgorithm.GetHashAlgorithm());
            referenceDigestValue.AppendChild(document.CreateTextNode(content));
            reference.AppendChild(referenceDigestValue);


            
        }


        public override XmlDocument Sign(XmlDocument document,ISignatureProfileParameters parameters, bool reuseDocument = false)
        {
            if (!reuseDocument)
            {
                string xmlString = document.OuterXml;
                document = new XmlDocument();
                document.LoadXml(xmlString);
            }
            base.PrepareSignature(document, parameters);
            XadesBesSignatureProfileParameters xadesParameters = (XadesBesSignatureProfileParameters)parameters;

            XmlElement signature = (XmlElement)document.SelectSingleNode($"//*[@Id='{SignatureId}']")!;
            if (!string.IsNullOrWhiteSpace(XadesNamespaceData.NamespacePrefix))
            {
                signature.SetAttribute($"xmlns:{XadesNamespaceData.NamespacePrefix}", XadesNamespaceData.NamespaceUri);
            }
            XmlElement objectElement = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XadesBesNodeNames.Object, XmlDsigNamespaceData.NamespaceUri);
            signature.AppendChild(objectElement);

            XmlElement qualifyingProperties = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.QualifyingProperties, XadesNamespaceData.NamespaceUri);
            qualifyingProperties.SetAttribute("Target", $"#{SignatureId}");
            objectElement.AppendChild(qualifyingProperties);

            XmlElement signedProperties = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.SignedProperties, XadesNamespaceData.NamespaceUri);
            //signedProperties.SetAttribute($"xmlns:{XmlDsigNamespaceData.NamespacePrefix}", XmlDsigNamespaceData.NamespaceUri);
            //signedProperties.SetAttribute($"xmlns:ix", "https://insane.xades/ix");
            //signedProperties.SetAttribute($"xmlns:{XadesNamespaceData.NamespacePrefix}", XadesNamespaceData.NamespaceUri);
            signedProperties.SetAttribute("Id", SignedPropertiesId);
            qualifyingProperties.AppendChild(signedProperties);

            XmlElement signedSignatureProperties = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.SignedSignatureProperties, XadesNamespaceData.NamespaceUri);
            signedProperties.AppendChild(signedSignatureProperties);
            
            XmlElement signingTime = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.SigningTime, XadesNamespaceData.NamespaceUri);
            signingTime.InnerText = "2025-07-17T04:22:33.5811247Z";// DateTime.UtcNow.ToString("o");//OJO
            signedSignatureProperties.AppendChild(signingTime);

            XmlElement signingCertificate = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.SigningCertificate, XadesNamespaceData.NamespaceUri);
            signedSignatureProperties.AppendChild(signingCertificate);

            XmlElement cert = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.Cert, XadesNamespaceData.NamespaceUri);
            signingCertificate.AppendChild(cert);

            XmlElement certDigest = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.CertDigest, XadesNamespaceData.NamespaceUri);
            cert.AppendChild(certDigest);

            XmlElement digestMethod = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XadesBesNodeNames.DigestMethod, XmlDsigNamespaceData.NamespaceUri);
            digestMethod.SetAttribute(XmlDsigAttribNames.Algorithm, DigestMethodAlgorithm.Uri);
            digestMethod.AppendChild(document.CreateTextNode(string.Empty));
            certDigest.AppendChild(digestMethod);

            XmlElement digestValue = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XadesBesNodeNames.DigestValue, XmlDsigNamespaceData.NamespaceUri);
            digestValue.InnerText = Certificate.RawData.ComputeEncodedHash(Base64Encoder.DefaultInstance, DigestMethodAlgorithm.GetHashAlgorithm());
            certDigest.AppendChild(digestValue);

            XmlElement issuerSerial = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.IssuerSerial, XadesNamespaceData.NamespaceUri);
            cert.AppendChild(issuerSerial);

            XmlElement issuerName = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XadesBesNodeNames.X509IssuerName, XmlDsigNamespaceData.NamespaceUri);
            issuerName.InnerText = Certificate.Issuer;
            issuerSerial.AppendChild(issuerName);

            XmlElement serialNumber = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XadesBesNodeNames.X509SerialNumber, XmlDsigNamespaceData.NamespaceUri);
            serialNumber.InnerText = Certificate.SerialNumber;
            issuerSerial.AppendChild(serialNumber);

            XmlElement signedDataObjectProperties = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.SignedDataObjectProperties, XadesNamespaceData.NamespaceUri);
            signedProperties.AppendChild(signedDataObjectProperties);

            XmlElement dataObjectFormat = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.DataObjectFormat, XadesNamespaceData.NamespaceUri);
            dataObjectFormat.SetAttribute("ObjectReference", $"#{ContentReferenceId}");
            signedDataObjectProperties.AppendChild(dataObjectFormat);

            XmlElement description = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.Description, XadesNamespaceData.NamespaceUri);
            description.AppendChild(document.CreateTextNode(xadesParameters.ContentDataObjectFormatProperties.Description));
            dataObjectFormat.AppendChild(description);

            XmlElement mimeType = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.MimeType, XadesNamespaceData.NamespaceUri);
            mimeType.AppendChild(document.CreateTextNode(xadesParameters.ContentDataObjectFormatProperties.MimeType));
            dataObjectFormat.AppendChild(mimeType);
            
            AddSignedInfoReferences(document, parameters);



            base.AddSignatureValue(document);

            return document;
        }

        
    }

}




















//XmlElement referenceDigestValue = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.DigestValue, XmlDsigNamespaceData.NamespaceUri);
//string canonicalizedContent = document.OuterXml.RemoveSignatureNode().CanonicalizeXml(CanonicalizationMethodTransform);
//referenceDigestValue.InnerText = canonicalizedContent.ComputeEncodedHash(Base64Encoder.DefaultInstance, DigestMethodAlgorithm.GetHashAlgorithm());
//reference.AppendChild(referenceDigestValue);
//if (xadesParameters.ExtraDataObjectFormatProperties is not null and { Count: > 0} )
//{
//    foreach (DataObjectFormat dataObjectFormat in xadesParameters.ExtraDataObjectFormatProperties)
//    {
//        XmlElement dataObjectFormatElement = document.CreateElement(XadesNamespaceData.NamespacePrefix, XadesBesNodeNames.DataObjectFormat, XadesNamespaceData.NamespaceUri);
//        dataObjectFormatElement.SetAttribute("ObjectReference", $"#{dataObjectFormat.ObjectReference}");
//        if (!string.IsNullOrWhiteSpace(dataObjectFormat.Description))
//        {
//            dataObjectFormatElement.SetAttribute("Description", dataObjectFormat.Description);
//        }
//        if (!string.IsNullOrWhiteSpace(dataObjectFormat.MimeType))
//        {
//            dataObjectFormatElement.SetAttribute("MimeType", dataObjectFormat.MimeType);
//        }
//        signedInfo.AppendChild(dataObjectFormatElement);
//    }
//}






//XmlElement transforms = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Transforms, XmlDsigNamespaceData.NamespaceUri);
//reference.AppendChild(transforms);
//XmlElement transform = document.CreateElement(XmlDsigNamespaceData.NamespacePrefix, XmlDSigNodeNames.Transform, XmlDsigNamespaceData.NamespaceUri);
//transform.AppendChild(document.CreateTextNode(string.Empty));
//transform.SetAttribute(XmlDsigAttribNames.Algorithm, CanonicalizationMethodTransform.Uri);
//transforms.AppendChild(transform);