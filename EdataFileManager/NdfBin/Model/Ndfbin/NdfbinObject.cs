using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
{
    public class NdfbinObject : ViewModelBase
    {
        private readonly ObservableCollection<NdfbinPropertyValue> _propertyValues = new ObservableCollection<NdfbinPropertyValue>();
        private NdfbinClass _class;
        private byte[] _data;
        private int _id;

        private string _name;


        public NdfbinClass Class
        {
            get { return _class; }
            set { _class = value; OnPropertyChanged(() => Class); }
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged(() => Data); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(() => Name); }
        }

        public ObservableCollection<NdfbinPropertyValue> Propertyvalues
        {
            get { return _propertyValues; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(() => Id); }
        }
    }
}
