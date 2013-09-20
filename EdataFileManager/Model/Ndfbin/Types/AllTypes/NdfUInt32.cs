using System;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfUInt32 : NdfFlatValueWrapper
    {
        public NdfUInt32(uint value, long offset)
            : base(NdfType.UInt32, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            try
            {
                return BitConverter.GetBytes((UInt32)Value);
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }
    }
}
