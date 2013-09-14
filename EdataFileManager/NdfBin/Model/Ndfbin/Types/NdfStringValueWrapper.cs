using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin.Types
{
    public class NdfStringValueWrapper<T> : ViewModelBase
        where T : NdfbinString
    {
        private NdfType _type;
        private T _stringInstance;

        public string Value
        {
            get { return StringInstance.Value; }
        }

        public T StringInstance
        {
            get { return _stringInstance; }
            set
            {
                _stringInstance = value;
                OnPropertyChanged(() => StringInstance);
                OnPropertyChanged(() => Value);
            }
        }

        public NdfType Type
        {
            get { return _type; }
            protected set { _type = value; OnPropertyChanged(() => Type); }
        }
    }
}
