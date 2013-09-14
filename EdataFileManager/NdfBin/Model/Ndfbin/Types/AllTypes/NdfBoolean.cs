using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.NdfBin.Model.Ndfbin.Types.AllTypes
{
    class NdfBoolean : NdfFlatTypeValueWrapper<Boolean>
    {
        public NdfBoolean(bool value)
            : base(NdfType.Boolean, value)
        {

        }
    }
}
