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

        public override byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            throw new NotImplementedException();
        }
    }
}
