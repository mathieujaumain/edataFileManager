using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdataFileManager.Util;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfLocalisationHash : NdfFlatValueWrapper
    {
        public NdfLocalisationHash(byte[] value, long offset)
            : base(NdfType.LocalisationHash, value, offset)
        {

        }

        public new byte[] Value
        {
            get { return (byte[])base.Value; }
            set
            {
                base.Value = value;
                OnPropertyChanged(() => Value);
            }
        }

        public override byte[] GetBytes(object value, NdfType type, out bool valid)
        {
            valid = true;

            return Utils.StringToByteArrayFastest((string) value);
        }

        public override string ToString()
        {
            return Utils.ByteArrayToBigEndianHeyByteString(Value);
        }
    }
}
