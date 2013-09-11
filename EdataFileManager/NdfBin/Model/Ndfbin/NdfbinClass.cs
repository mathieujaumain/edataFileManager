using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using EdataFileManager.ViewModel.Base;
using EdataFileManager.ViewModel.Filter;

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
        private readonly ObservableCollection<PropertyFilterExpression> _propertyFilterExpressions = new ObservableCollection<PropertyFilterExpression>();

        public NdfbinClass()
        {
            ApplyPropertyFilter = new ActionCommand(ApplyPropertyFilterExecute);
        }

        private void ApplyPropertyFilterExecute(object obj)
        {
            InstancesCollectionView.Refresh();
        }

        public ObservableCollection<PropertyFilterExpression> PropertyFilterExpressions
        {
            get { return _propertyFilterExpressions; }
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

        public ObservableCollection<NdfbinProperty> Properties
        {
            get { return _properties; }
        }

        public ObservableCollection<NdfbinObject> Instances
        {
            get { return _instances; }
        }

        public ICommand ApplyPropertyFilter { get; set; }

        public ICollectionView InstancesCollectionView
        {
            get
            {
                if (_instancesCollectionView == null)
                {
                    _instancesCollectionView = CollectionViewSource.GetDefaultView(Instances);
                    OnPropertyChanged(() => InstancesCollectionView);
                    _instancesCollectionView.CurrentChanged += InstancesCollectionViewCurrentChanged;
                    _instancesCollectionView.Filter = FilterInstances;
                }

                return _instancesCollectionView;
            }

        }

        public bool FilterInstances(object o)
        {
            var obj = o as NdfbinObject;

            if (obj == null)
                return false;

            foreach (var expr in PropertyFilterExpressions)
            {
                if (expr.PropertyName == null)
                    continue;

                var propVal = obj.PropertyValues.SingleOrDefault(x => x.Property.Name == expr.PropertyName);

                if (propVal == null)
                    return false;

                if (!propVal.Value.ToString().Contains(expr.Value) || expr.Value.Length == 0)
                    return false;
            }

            return true;
        }

        protected void InstancesCollectionViewCurrentChanged(object sender, EventArgs e)
        {
            foreach (var property in Properties)
                property.OnPropertyChanged(() => property.Value, () => property.ValueData, () => property.ValueType);
        }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.InvariantCulture);
        }
    }
}
