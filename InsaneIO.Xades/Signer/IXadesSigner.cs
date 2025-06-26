using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsaneIO.Xades.Signer
{
    public interface IXadesSigner
    {
        public string Sign();
        public bool Verify();
    }
}
