﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfNull : NdfValueWrapper
    {
        public NdfNull(long offset)
            : base(NdfType.Unset, offset)
        {
        }

        public override string ToString()
        {
            return "null";
        }

        public override byte[] GetBytes(out bool valid)
        {
            throw new NotImplementedException();
        }
    }
}
