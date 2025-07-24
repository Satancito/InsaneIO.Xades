using InsaneIO.Insane.Extensions;
using ITfoxtec.Identity.Saml2.Cryptography;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InsaneIO.Xades.Core
{
    public class CanonicalizationMethod
    {
        public static readonly CanonicalizationMethod XmlDsigC14N = new CanonicalizationMethod(SignedXml.XmlDsigC14NTransformUrl, () => new XmlDsigC14NTransform());
        public static readonly CanonicalizationMethod XmlDsigC14NWithComments = new CanonicalizationMethod(SignedXml.XmlDsigC14NWithCommentsTransformUrl, () => new XmlDsigC14NWithCommentsTransform());
        public static readonly CanonicalizationMethod XmlDsigExcC14N = new CanonicalizationMethod(SignedXml.XmlDsigExcC14NTransformUrl, () => new XmlDsigExcC14NTransform());
        public static readonly CanonicalizationMethod XmlDsigExcC14NWithComments = new CanonicalizationMethod(SignedXml.XmlDsigExcC14NWithCommentsTransformUrl, () => new XmlDsigExcC14NWithCommentsTransform());

        private CanonicalizationMethod(string uri, Func<Transform> creationDelegate)
        {
            Uri = uri;
            Create = creationDelegate;
        }
        public  string Uri { get; } = null!;
        public  Func<Transform> Create { get; } = null!;
    }

    public static class CanonicalizationMethodExtensions
    {
        public static string CanonicalizeXml(this CanonicalizationMethod canonicalizationMethod, string xmlInput)
        {
            XmlDocument document = new XmlDocument
            {
                PreserveWhitespace = true,

            };
            document.LoadXml(xmlInput);
            var transform = canonicalizationMethod.Create();
            transform.LoadInput(document);
            using var stream = (Stream)transform.GetOutput(typeof(Stream));
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }


        public static string CanonicalizeXml(this CanonicalizationMethod canonicalizationMethod, XmlElement xmlInput)
        {
            XmlDocument document = new XmlDocument
            {
                PreserveWhitespace = true,

            };
            document.LoadXml(xmlInput.OuterXml);
            var transform = canonicalizationMethod.Create();
            transform.LoadInput(document);
            using var stream = (Stream)transform.GetOutput(typeof(Stream));
            using var reader = new StreamReader(stream, Encoding.UTF8);
            return reader.ReadToEnd();
        }


    }

}

    
