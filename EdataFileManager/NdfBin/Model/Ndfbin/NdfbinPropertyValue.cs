using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdataFileManager.NdfBin.Model.Ndfbin.Types;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
{
    public class NdfbinPropertyValue : ViewModelBase
    {
        private NdfbinProperty _property;
        private object _value;
        private NdfType _type;
        private byte[] _typeData;
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

        public object Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        public NdfbinProperty Property
        {
            get { return _property; }
            set { _property = value; OnPropertyChanged("Property"); }
        }
    }
}
