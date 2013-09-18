using System;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfSingle : NdfFlatValueWrapper
    {
        public NdfSingle(float value, long offset)
            : base(NdfType.Float32, value, offset)
        {

        }

        public override byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            valid = true;

            try
            {
                return BitConverter.GetBytes(Convert.ToSingle(value));
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }
    }
}
