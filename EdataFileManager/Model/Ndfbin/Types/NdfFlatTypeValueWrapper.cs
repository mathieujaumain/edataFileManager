using EdataFileManager.NdfBin.Model.Ndfbin.Types;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Model.Ndfbin.Types
{
    public class NdfFlatTypeValueWrapper : ViewModelBase
    {
        private NdfType _type;
        private object _value;

        public NdfFlatTypeValueWrapper(NdfType type, object value, long offset)
        {
            Type = type;
            Value = value;
            OffSet = offset;
        }

        public NdfType Type
        {
            get { return _type; }
            protected set { _type = value; OnPropertyChanged("Type"); }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        public long OffSet
        {
            get;
            set;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
