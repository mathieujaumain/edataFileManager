using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using EdataFileManager.BL;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.View.Ndfbin;
using EdataFileManager.ViewModel.Base;
using EdataFileManager.ViewModel.Filter;

namespace EdataFileManager.Model.Ndfbin
{
    public class NdfClass : ViewModelBase
    {
        private int _id;
        private long _offset;
        private string _name;
        private readonly ObservableCollection<NdfProperty> _properties = new ObservableCollection<NdfProperty>();
        private readonly ObservableCollection<NdfObject> _instances = new ObservableCollection<NdfObject>();

        public NdfClass(NdfbinManager mgr)
        {
            Manager = mgr;
        }

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

        public ObservableCollection<NdfProperty> Properties
        {
            get { return _properties; }
        }

        public ObservableCollection<NdfObject> Instances
        {
            get { return _instances; }
        }

        public NdfbinManager Manager { get; protected set; }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.InvariantCulture);
        }
    }
}
