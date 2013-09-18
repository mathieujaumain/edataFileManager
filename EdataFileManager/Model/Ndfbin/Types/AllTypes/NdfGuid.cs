using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfGuid : NdfFlatValueWrapper
    {
        public NdfGuid(Guid value, long offset)
            : base(NdfType.Guid, value, offset)
        {

        }

        public override byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            Guid guid;
            valid = true;

           if (!Guid.TryParse((string) value, out guid))
           {
               valid = false;
               return new byte[0];
           }

            return guid.ToByteArray();
        }
    }
}
