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
        private ICollectionView _instancesCollectionView;
        private readonly ObservableCollection<PropertyFilterExpression> _propertyFilterExpressions = new ObservableCollection<PropertyFilterExpression>();

        public NdfbinManager Manager { get; protected set; }

        public NdfClass(NdfbinManager mgr)
        {
            Manager = mgr;
            ApplyPropertyFilter = new ActionCommand(ApplyPropertyFilterExecute);
            DetailsCommand = new ActionCommand(DetailsCommandExecute);
        }

        private void DetailsCommandExecute(object obj)
        {
            var item = obj as IEnumerable<DataGridCellInfo>;

            if (item == null)
                return;

            var prop = item.First().Item as NdfProperty;

            if (prop == null || prop.ValueType != NdfType.ObjectReference)
                return;

            var refe = prop.Value as NdfObjectReference;

            var inst = refe.NdfClass.Instances.SingleOrDefault(x => x.Id == refe.InstanceId);

            if (inst == null)
                return;

            refe.NdfClass.InstancesCollectionView.MoveCurrentTo(inst);

            var view = new InstanceWindowView { DataContext = refe.NdfClass };

            view.Show();
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

        public ObservableCollection<NdfProperty> Properties
        {
            get { return _properties; }
        }

        public ObservableCollection<NdfObject> Instances
        {
            get { return _instances; }
        }

        public ICommand ApplyPropertyFilter { get; set; }
        public ICommand DetailsCommand { get; set; }

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
            var obj = o as NdfObject;

            if (obj == null)
                return false;

            bool ret = true;

            foreach (var expr in PropertyFilterExpressions)
            {
                if (expr.PropertyName == null)
                    continue;

                var propVal = obj.PropertyValues.SingleOrDefault(x => x.Property.Name == expr.PropertyName);

                if (propVal == null)
                {
                    ret = false;
                    continue;
                }

                if (propVal.Value == null)
                {
                    if (expr.Value.Length > 0)
                        ret = false;

                    continue;
                }

                if (propVal.Value.ToString().Contains(expr.Value) || propVal.Property.ValueData.ToLower().Contains(expr.Value))
                    continue;

                return false;
            }

            return ret;
        }

        protected void InstancesCollectionViewCurrentChanged(object sender, EventArgs e)
        {
            foreach (var property in Properties)
            {
                property.OnPropertyChanged("Value");
                property.OnPropertyChanged("ValueData");
                property.OnPropertyChanged("ValueType");

            }
        }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.InvariantCulture);
        }
    }
}
