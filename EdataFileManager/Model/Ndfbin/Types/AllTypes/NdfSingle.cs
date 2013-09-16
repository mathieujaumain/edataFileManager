using System;
using EdataFileManager.NdfBin.Model.Ndfbin.Types;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfSingle : NdfFlatTypeValueWrapper
    {
        public NdfSingle(float value, long offset)
            : base(NdfType.Float32, value, offset)
        {

        }
    }
}
