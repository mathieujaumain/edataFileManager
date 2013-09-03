using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
{
    public class NdfbinClass : ViewModelBase
    {
        private int _id;
        private long _offset;
        private string _name;
        private readonly ObservableCollection<NdfbinProperty> _properties = new ObservableCollection<NdfbinProperty>();

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

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(() => Name); }
        }

        public ObservableCollection<NdfbinProperty> Properties
        {
            get { return _properties; }
        }
    }
}
