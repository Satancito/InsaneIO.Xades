using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace InsaneIO.Xades.Profile.XadesBes
{
    public class ObjectIdentifier
    {
        public required string Identifier { get; set; }

        public string? Description { get; set; } 

        public List<string> DocumentationReference { get; set; } = new List<string>();
    }
}
