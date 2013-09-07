using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
{
    public class NdfbinPropertyValue : ViewModelBase
    {
        private NdfbinProperty _property;
        private object _value;
        private Byte[] _data;
        private byte[] _typeData;
        private byte[] _valueData;

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged(() => Data); }
        }

        public byte[] TypeData
        {
            get { return _typeData; }
            set { _typeData = value; OnPropertyChanged(() => Data); }
        }

        public byte[] ValueData
        {
            get { return _valueData; }
            set { _valueData = value;  OnPropertyChanged(() => ValueData);}
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(() => Value); }
        }
    }
}
