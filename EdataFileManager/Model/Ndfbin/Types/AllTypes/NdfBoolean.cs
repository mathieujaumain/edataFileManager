using System;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfBoolean : NdfFlatValueWrapper
    {
        public NdfBoolean(bool value, long offset)
            : base(NdfType.Boolean, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            try
            {
                return BitConverter.GetBytes((bool)Value);
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }
    }
}
