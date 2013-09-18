﻿using System;
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

        public override byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            throw new System.NotImplementedException();
        }
    }

}
