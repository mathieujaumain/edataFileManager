using System;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfDouble : NdfFlatValueWrapper
    {
        public NdfDouble(double value, long offset)
            : base(NdfType.Float64, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            try
            {
                return BitConverter.GetBytes((Double)Value);
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }
    }
}
