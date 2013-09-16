using System;
using EdataFileManager.NdfBin.Model.Ndfbin.Types;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfUInt32 : NdfFlatTypeValueWrapper
    {
        public NdfUInt32(uint value, long offset)
            : base(NdfType.UInt32, value, offset)
        {

        }
    }
}
