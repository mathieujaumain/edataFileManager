using System;
using EdataFileManager.NdfBin.Model.Ndfbin.Types;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfDouble : NdfFlatTypeValueWrapper
    {
        public NdfDouble(double value, long offset)
            : base(NdfType.Float64, value, offset)
        {

        }
    }
}
