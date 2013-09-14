using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.NdfBin.Model.Ndfbin.Types.AllTypes
{
    public class NdfSingle : NdfFlatTypeValueWrapper<Single>
    {
        public NdfSingle(float value)
            : base(NdfType.Float32, value)
        {

        }
    }
}
