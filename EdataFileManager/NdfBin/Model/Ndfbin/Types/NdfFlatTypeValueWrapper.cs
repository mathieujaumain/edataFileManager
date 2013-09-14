using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin.Types
{
    public class NdfFlatTypeValueWrapper<T> : ViewModelBase
    {
        private NdfType _type;
        private T _value;

        public NdfFlatTypeValueWrapper(NdfType type, T value)
        {
            Type = type;
            Value = value;
        }

        public NdfType Type
        {
            get { return _type; }
            protected set { _type = value; OnPropertyChanged(() => Type); }
        }

        public T Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(() => Value); }
        }
    }
}
