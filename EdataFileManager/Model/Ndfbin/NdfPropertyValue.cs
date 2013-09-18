using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Model.Ndfbin
{
    public class NdfPropertyValue : ViewModelBase
    {
        private NdfProperty _property;
        private NdfValueWrapper _value;
        private NdfType _type;
        private byte[] _valueData;

        public NdfType Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged("Type"); }
        }

        public byte[] ValueData
        {
            get { return _valueData; }
            set { _valueData = value;  OnPropertyChanged("ValueData");}
        }

        public NdfValueWrapper Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        public NdfProperty Property
        {
            get { return _property; }
            set { _property = value; OnPropertyChanged("Property"); }
        }
    }
}
