using System;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfUInt32 : NdfFlatValueWrapper
    {
        public NdfUInt32(uint value, long offset)
            : base(NdfType.UInt32, value, offset)
        {

        }

        public override byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            valid = true;

            try
            {
                return BitConverter.GetBytes(Convert.ToUInt32(value));
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }
    }
}
