using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
{
    public class NdfbinTran : ViewModelBase
    {
        private int _id;
        private long _offset;
        private string _value;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(() => Id); }
        }

        public long Offset
        {
            get { return _offset; }
            set { _offset = value; OnPropertyChanged(() => Offset); }
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged(() => Value); }
        }
    }
}
