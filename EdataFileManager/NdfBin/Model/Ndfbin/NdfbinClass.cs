using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using EdataFileManager.NdfBin.Model.Ndfbin.Types;
using EdataFileManager.View.Ndfbin;
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

        public NdfbinManager Manager { get; protected set; }

        public NdfbinClass(NdfbinManager mgr)
        {
            Manager = mgr;
            ApplyPropertyFilter = new ActionCommand(ApplyPropertyFilterExecute);
            DetailsCommand = new ActionCommand(DetailsCommandExecute);
        }

        private void DetailsCommandExecute(object obj)
        {
            var item = obj as IEnumerable<DataGridCellInfo>;

            var prop = item.First().Item as NdfbinProperty;

            if (prop == null || prop.ValueType != NdfType.ObjectReference)
                return;

            var tVal = prop.Value.ToString().Split(new string[] { " : " }, StringSplitOptions.None);

            var cls = Manager.Classes.SingleOrDefault(x => x.Id == Int32.Parse(tVal[0]));

            if (cls == null)
                return;

            var inst = cls.Instances.SingleOrDefault(x => x.Id == Int32.Parse(tVal[1]));

            if (inst == null)
                return;

            cls.InstancesCollectionView.MoveCurrentTo(inst);

            var view = new InstanceWindowView();
            view.DataContext = cls;

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

        public ObservableCollection<NdfbinProperty> Properties
        {
            get { return _properties; }
        }

        public ObservableCollection<NdfbinObject> Instances
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
            var obj = o as NdfbinObject;

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
                property.OnPropertyChanged(() => property.Value, () => property.ValueData, () => property.ValueType);
                property.OnPropertyChanged(() => property.ValueType);
            }
        }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.InvariantCulture);
        }
    }
}
