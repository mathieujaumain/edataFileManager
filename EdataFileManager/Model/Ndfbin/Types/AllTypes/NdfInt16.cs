using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfInt16 : NdfFlatValueWrapper
    {
        public NdfInt16(short value, long offset)
            : base(NdfType.Int16, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            try
            {
                return BitConverter.GetBytes(Convert.ToInt16(Value));
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }
    }
}
