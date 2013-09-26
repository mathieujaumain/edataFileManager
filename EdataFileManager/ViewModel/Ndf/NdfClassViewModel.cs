using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using EdataFileManager.Model.Ndfbin;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.View.Ndfbin;
using EdataFileManager.ViewModel.Base;
using EdataFileManager.ViewModel.Filter;

namespace EdataFileManager.ViewModel.Ndf
{
    public class NdfClassViewModel : ObjectWrapperViewModel<NdfClass>
    {
        private ICollectionView _instancesCollectionView;
        private readonly ObservableCollection<PropertyFilterExpression> _propertyFilterExpressions = new ObservableCollection<PropertyFilterExpression>();
        private readonly ObservableCollection<NdfObjectViewModel> _instances = new ObservableCollection<NdfObjectViewModel>();

        public NdfClassViewModel(NdfClass obj)
            : base(obj)
        {
            foreach (var instance in obj.Instances)
                Instances.Add(new NdfObjectViewModel(instance));


            ApplyPropertyFilter = new ActionCommand(ApplyPropertyFilterExecute);
        }

        public string Name
        {
            get { return Object.Name; }
            set { Object.Name = value; OnPropertyChanged("Name"); }
        }

        public int Id
        {
            get { return Object.Id; }
            set { Object.Id = value; OnPropertyChanged("Id"); }
        }

        public ObservableCollection<NdfProperty> Properties
        {
            get { return Object.Properties; }
        }

        public ObservableCollection<NdfObjectViewModel> Instances
        {
            get { return _instances; }
        }

        public ICommand ApplyPropertyFilter { get; set; }


        public ObservableCollection<PropertyFilterExpression> PropertyFilterExpressions
        {
            get { return _propertyFilterExpressions; }
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
                    _instancesCollectionView.Filter = FilterInstances;
                }

                return _instancesCollectionView;
            }
        }

        public bool FilterInstances(object o)
        {
            var obj = o as NdfObjectViewModel;

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

                if (propVal.Value.ToString().Contains(expr.Value) || propVal.BinValue.ToLower().Contains(expr.Value))
                    continue;

                return false;
            }

            return ret;
        }

        protected void InstancesCollectionViewCurrentChanged(object sender, EventArgs e)
        {
            foreach (var property in Object.Properties)
            {
                property.OnPropertyChanged("Value");
                //property.OnPropertyChanged("ValueData");
                //property.OnPropertyChanged("ValueType");
            }
        }

        private void ApplyPropertyFilterExecute(object obj)
        {
            InstancesCollectionView.Refresh();
        }
    }
}
