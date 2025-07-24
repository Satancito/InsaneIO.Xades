using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace InsaneIO.Xades.Misc
{
    public class XmlWriterSettingsProvider
    {
        public static readonly XmlWriterSettings DefaultXmlWriterSettings = new()
        {
            Indent = true,
            IndentChars = "  ",
            NewLineChars = "\n",
            NewLineHandling = NewLineHandling.Replace,
            OmitXmlDeclaration = false
        };

       
    }
}

