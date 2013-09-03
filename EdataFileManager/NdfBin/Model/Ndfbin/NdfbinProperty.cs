using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
{
    public class NdfbinProperty : ViewModelBase
    {
        private int _id;
        private int _classId;
        private long _offset;
        private string _name;

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(() => Id); }
        }

        public int ClassId
        {
            get { return _classId; }
            set { _classId = value; OnPropertyChanged(() => ClassId); }
        }

        public long Offset
        {
            get { return _offset; }
            set { _offset = value; OnPropertyChanged(() => Offset); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(() => Name); }
        }
    }
}
