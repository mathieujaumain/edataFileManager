using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using EdataFileManager.Model.Ndfbin;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.View.Ndfbin;
using EdataFileManager.View.Ndfbin.Lists;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel.Ndf
{
    public class NdfObjectViewModel : ObjectWrapperViewModel<NdfObject>
    {
        private readonly ObservableCollection<NdfPropertyValue> _propertyValues = new ObservableCollection<NdfPropertyValue>();

        public NdfObjectViewModel(NdfObject obj)
            : base(obj)
        {
            var propVals = new List<NdfPropertyValue>();

            propVals.AddRange(obj.PropertyValues);

            foreach (var source in propVals.OrderBy(x => x.Property.Id))
                _propertyValues.Add(source);

            DetailsCommand = new ActionCommand(DetailsCommandExecute);
            AddPropertyCommand = new ActionCommand(AddPropertyExecute, AddPropertyCanExecute);
            RemovePropertyCommand = new ActionCommand(RemovePropertyExecute, RemovePropertyCanExecute);
        }

        private void AddPropertyExecute(object obj)
        {
            var cv = CollectionViewSource.GetDefaultView(PropertyValues);

            var item = cv.CurrentItem as NdfPropertyValue;

            if (item == null)
                return;

            var type = NdfType.Unset;

            foreach (var instance in Object.Class.Instances)
            {
                foreach (var propertyValue in instance.PropertyValues)
                {
                    if (propertyValue.Property.Id == item.Property.Id)
                        if (propertyValue.Type != NdfType.Unset)
                            type = propertyValue.Type;
                }
            }

            if (type == NdfType.Unset || type == NdfType.Unknown)
                return;

            item.Value = NdfTypeManager.GetValue(new byte[NdfTypeManager.SizeofType(type)], type, item.Manager, 0);
        }

        private bool AddPropertyCanExecute()
        {
            var cv = CollectionViewSource.GetDefaultView(PropertyValues);

            var item = cv.CurrentItem as NdfPropertyValue;

            if (item == null)
                return false;

            return item.Type == NdfType.Unset;
        }

        private void RemovePropertyExecute(object obj)
        {
            var cv = CollectionViewSource.GetDefaultView(PropertyValues);

            var item = cv.CurrentItem as NdfPropertyValue;

            if (item == null || item.Type == NdfType.Unset || item.Type == NdfType.Unknown)
                return;

            var result = MessageBox.Show("Do you want set this property to null?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                item.Value = NdfTypeManager.GetValue(new byte[0], NdfType.Unset, item.Manager, 0);
        }

        private bool RemovePropertyCanExecute()
        {
            var cv = CollectionViewSource.GetDefaultView(PropertyValues);

            var item = cv.CurrentItem as NdfPropertyValue;

            if (item == null)
                return false;

            return item.Type != NdfType.Unset;
        }

        public uint Id
        {
            get { return Object.Id; }
            set { Object.Id = value; OnPropertyChanged("Id"); }
        }

        public ObservableCollection<NdfPropertyValue> PropertyValues
        {
            get { return _propertyValues; }
        }

        public ICommand DetailsCommand { get; protected set; }
        public ICommand AddPropertyCommand { get; protected set; }
        public ICommand RemovePropertyCommand { get; protected set; }

        public static void DetailsCommandExecute(object obj)
        {
            var item = obj as IEnumerable<DataGridCellInfo>;

            if (item == null)
                return;

            var prop = item.First().Item as IValueHolder;

            if (prop == null)
                return;

            switch (prop.Value.Type)
            {
                case NdfType.MapList:
                case NdfType.List:
                    FollowList(prop);
                    break;
                case NdfType.ObjectReference:
                    FollowObjectReference(prop);
                    break;
                default:
                    return;
            }
        }

        private static void FollowObjectReference(IValueHolder prop)
        {
            var refe = prop.Value as NdfObjectReference;

            if (refe == null)
                return;

            var vm = new NdfClassViewModel(refe.Class);

            var inst = vm.Instances.SingleOrDefault(x => x.Id == refe.InstanceId);

            if (inst == null)
                return;

            vm.InstancesCollectionView.MoveCurrentTo(inst);

            var view = new InstanceWindowView { DataContext = vm };

            view.Show();
        }

        private static void FollowList(IValueHolder prop)
        {
            var refe = prop.Value as NdfCollection;

            if (refe == null)
                return;

            var view = new ListEditorWindow { DataContext = prop };

            view.Show();
        }
    }
}
