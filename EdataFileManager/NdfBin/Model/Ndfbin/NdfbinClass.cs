using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
{
    public class NdfbinClass : ViewModelBase
    {
        private int _id;
        private long _offset;
        private string _name;
        private readonly ObservableCollection<NdfbinProperty> _properties = new ObservableCollection<NdfbinProperty>();
        private readonly ObservableCollection<NdfbinObject> _instances = new ObservableCollection<NdfbinObject>();
        private ICollectionView _instancesCollectionView;

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

        public ObservableCollection<NdfbinObject> Instances
        {
            get { return _instances; }
        }

        public ICollectionView InstancesCollectionView
        {
            get
            {
                if (_instancesCollectionView == null)
                {
                    _instancesCollectionView = CollectionViewSource.GetDefaultView(Instances);
                    OnPropertyChanged(() => InstancesCollectionView);
                    _instancesCollectionView.CurrentChanged += InstancesCollectionViewCurrentChanged;
                }

                return _instancesCollectionView;
            }

        }

        protected void InstancesCollectionViewCurrentChanged(object sender, EventArgs e)
        {
            foreach (var property in Properties)
                property.OnPropertyChanged(() => property.Value);
        }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.InvariantCulture);
        }
    }
}
