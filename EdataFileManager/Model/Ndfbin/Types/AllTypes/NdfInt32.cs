using System;
using EdataFileManager.NdfBin.Model.Ndfbin.Types;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfInt32 : NdfFlatTypeValueWrapper
    {
        public NdfInt32(int value, long offset)
            : base(NdfType.Int32, value, offset)
        {

        }
    }
}
