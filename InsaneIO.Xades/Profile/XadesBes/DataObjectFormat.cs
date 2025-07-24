using InsaneIO.Xades.Profile.XmlDsig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsaneIO.Xades.Profile.XadesBes
{
    public class DataObjectFormat
    {
        public string ObjectReference { get; set; } = XmlDsigSignatureProfileBase.GenerateHexId(nameof(ObjectReference), 16);
        public string Description { get; set; } = null!;
        public string MimeType { get; set; } = null!;
        public string? Encoding { get; set; } 
        public ObjectIdentifier? ObjectIdentifier { get; set; }
    }
}
