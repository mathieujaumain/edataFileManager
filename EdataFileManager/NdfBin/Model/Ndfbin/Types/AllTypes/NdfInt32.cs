using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.NdfBin.Model.Ndfbin.Types.AllTypes
{
    public class NdfInt32 : NdfFlatTypeValueWrapper<Int32>
    {
        public NdfInt32(int value)
            : base(NdfType.Int32, value)
        {

        }
    }
}
