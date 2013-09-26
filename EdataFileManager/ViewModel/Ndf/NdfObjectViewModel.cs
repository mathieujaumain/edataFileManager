﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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

            foreach (var property in obj.Class.Properties)
                if (propVals.Count(x => x.Property == property) == 0)
                    propVals.Add(new NdfPropertyValue(Object) { Property = property, Value = new NdfNull(0) });

            foreach (var source in propVals.OrderBy(x => x.Property.Id))
                _propertyValues.Add(source);

            DetailsCommand = new ActionCommand(DetailsCommandExecute);
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

        public ICommand DetailsCommand { get; set; }

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
