using System;
using EdataFileManager.NdfBin.Model.Ndfbin.Types;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    class NdfBoolean : NdfFlatTypeValueWrapper
    {
        public NdfBoolean(bool value, long offset)
            : base(NdfType.Boolean, value, offset)
        {

        }
    }
}
