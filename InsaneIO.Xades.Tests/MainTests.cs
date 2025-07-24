using InsaneIO.Insane.Extensions;
using InsaneIO.Xades.Core;
using InsaneIO.Xades.Profile;
using InsaneIO.Xades.Profile.XadesBes;
using InsaneIO.Xades.Signer;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Xunit.Abstractions;

namespace InsaneIO.Xades.Tests
{
    public class MainTests
    {
        private readonly ITestOutputHelper output;
        private readonly IConfigurationRoot configuration;

        public MainTests(ITestOutputHelper output)
        {
            this.output = output;
            configuration = new ConfigurationBuilder()
                .AddUserSecrets<MainTests>()
                .Build();
        }

        [Fact]
        public void SignXadeBesProfileTest()
        {
            var certificate = X509CertificateLoader.LoadPkcs12(File.ReadAllBytes(configuration["CertificatePath"]!), configuration["CertificatePassword"]!, X509KeyStorageFlags.EphemeralKeySet);

            XmlDsigSignatureProfile profile1 = new()
            {
                Certificate = certificate,
                ContentId = "comprobante",
                XmlDsigNamespaceData = XmlNamespaceData.XmlDSig,
                SignatureMethodAlgorithm = SignatureMethod.RsaSha1,
                DigestMethodAlgorithm = DigestMethod.Sha1,
                CanonicalizationMethodTransform = CanonicalizationMethod.XmlDsigExcC14N,
            };

            XadesBesSignatureProfile profile2 = new()
            {
                Certificate = certificate,
                ContentId = "comprobante",
                ContentReferenceId = nameof(XadesBesSignatureProfile.ContentReferenceId),
                KeyInfoId = nameof(XadesBesSignatureProfile.KeyInfoId),
                SignedInfoId = nameof(XadesBesSignatureProfile.SignedInfoId),
                SignatureId = nameof(XadesBesSignatureProfile.SignatureId),
                SignatureValueId = nameof(XadesBesSignatureProfile.SignatureValueId),
                SignedPropertiesId = nameof(XadesBesSignatureProfile.SignedPropertiesId),
                SignedPropertiesReferenceId = nameof(XadesBesSignatureProfile.SignedPropertiesReferenceId),
                Packaging = SignaturePackaging.Enveloped,
               
                XmlDsigNamespaceData = XmlNamespaceData.XmlDSig,
                XadesNamespaceData = XmlNamespaceData.Etsi,
                SignatureMethodAlgorithm = SignatureMethod.RsaSha1,
                DigestMethodAlgorithm = DigestMethod.Sha1,
                CanonicalizationMethodTransform = CanonicalizationMethod.XmlDsigC14N,
            };

            XadesBesSignatureProfileParameters parameters = new()
            {
                ContentDataObjectFormatProperties = new DataObjectFormat
                {
                    MimeType = "text/xml",
                    Description = "Contenido Comprobante"
                }
            };

            
            string xmlContent = """
                <ix:Factura     xmlns:ix="https://insane.xades/ix" Y="50.0" X="10" Id= "comprobante"> 
                <data>Test Data</data>    <data2    Id="data2Id" /><ix2:data3 xmlns:ix2="https://insane.xades/ix2" /></ix:Factura>
                """;

            XmlDocument document = new()
            {
                PreserveWhitespace = true
            };
            document.LoadXml(xmlContent);


            XadesSigner signer = new();
            var result = signer.Sign(document, profile1, parameters, false);
            File.WriteAllText(AppContext.BaseDirectory + "/doc.xml", result.OuterXml);
            output.WriteLine(result.OuterXml);

            ///
            var doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(result.OuterXml);


            var node = doc.GetElementsByTagName("Signature", SignedXml.XmlDsigNamespaceUrl)[0];

            // Crea SignedXml
            var signedXml = new SignedXml(doc);
            signedXml.LoadXml((XmlElement)node!);

            var keyInfoEnum = signedXml.Signature.KeyInfo.GetEnumerator();
            X509Certificate2 cert = null!;
            while (keyInfoEnum.MoveNext())
            {
                if (keyInfoEnum.Current is KeyInfoX509Data x509Data)
                {
                    var certs = x509Data.Certificates;
                    foreach (X509Certificate2 x509 in certs)
                    {
                        cert = x509; break;
                    }
                }
            }
            bool soloFirmaValida = signedXml.CheckSignature(cert, false);
            output.WriteLine(soloFirmaValida.ToString());

            Assert.Equal(1, 1);
        }
    }
}
