using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.Model.Ndfbin.Types.AllTypes
{
    public class NdfMap : NdfFlatValueWrapper
    {
        private NdfValueWrapper _key;

        public NdfMap(NdfValueWrapper key, NdfValueWrapper  value, long offset) : base(NdfType.Map, value, offset)
        {
            Key = key;
        }

        public NdfValueWrapper Key
        {
            get { return _key; }
            set { _key = value; OnPropertyChanged("Key"); }
        }

        public override byte[] GetBytes(out bool valid)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return string.Format("Map: {0} : {1}", Key, Value);
        }
    }
}
