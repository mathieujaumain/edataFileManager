using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
{
    public class NdfbinFooterEntry : ViewModelBase
    {
        private string _name;
        private long _offset;
        private long _size;

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(() => Name);}
        }

        public long Offset
        {
            get { return _offset; }
            set { _offset = value; OnPropertyChanged(() => Offset);}
        }

        public long Size
        {
            get { return _size; }
            set { _size = value; OnPropertyChanged(() => Size); }
        }
    }
}
