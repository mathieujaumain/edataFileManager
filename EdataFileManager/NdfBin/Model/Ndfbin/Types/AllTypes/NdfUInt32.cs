using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.NdfBin.Model.Ndfbin.Types.AllTypes
{
    public class NdfUInt32 : NdfFlatTypeValueWrapper<UInt32>
    {
        public NdfUInt32(uint value)
            : base(NdfType.UInt32, value)
        {

        }
    }
}
