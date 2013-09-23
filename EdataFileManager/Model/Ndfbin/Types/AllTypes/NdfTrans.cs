using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    class NdfTrans : NdfFlatValueWrapper
    {
        public NdfTrans(NdfTranReference value, long offset)
            : base(NdfType.TransTableReference, value, offset)
        {
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = false;

            return new byte[0];
        }
    }

}
