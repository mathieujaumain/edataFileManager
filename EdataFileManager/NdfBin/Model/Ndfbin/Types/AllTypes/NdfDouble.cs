using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.NdfBin.Model.Ndfbin.Types.AllTypes
{
    public class NdfDouble : NdfFlatTypeValueWrapper<Double>
    {
        public NdfDouble(double value)
            : base(NdfType.Float64, value)
        {

        }
    }
}
