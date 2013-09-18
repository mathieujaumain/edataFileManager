namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfString : NdfFlatValueWrapper
    {
        public NdfString(NdfStringReference value, long offset)
            : base(NdfType.TableString, value, offset)
        {
        }

        public override byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            throw new System.NotImplementedException();
        }
    }
}
