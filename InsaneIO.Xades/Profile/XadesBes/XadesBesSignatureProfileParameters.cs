using InsaneIO.Xades.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsaneIO.Xades.Profile.XadesBes
{
    public class XadesBesSignatureProfileParameters: ISignatureProfileParameters
    {
        public DataObjectFormat ContentDataObjectFormatProperties;
        public List<DataObjectFormat> ExtraDataObjectFormatProperties = new List<DataObjectFormat>();
    }
}
