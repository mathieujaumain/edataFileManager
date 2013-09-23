using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfWideString : NdfFlatValueWrapper
    {
        public NdfWideString(string value, long offset)
            : base(NdfType.WideString, value, offset)
        {

        }

        public override byte[] GetBytes( out bool valid)
        {
            valid = false;

            return new byte[0];
        }
    }
}
