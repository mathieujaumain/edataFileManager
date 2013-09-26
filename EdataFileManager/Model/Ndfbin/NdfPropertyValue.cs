using System;
using System.Windows.Data;
using System.Windows.Input;
using EdataFileManager.BL;
using EdataFileManager.Model.Ndfbin.ChangeManager;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.Util;
using EdataFileManager.View.Ndfbin.Viewer;
using EdataFileManager.ViewModel.Base;
using EdataFileManager.ViewModel.Ndf;

namespace EdataFileManager.Model.Ndfbin
{
    public class NdfPropertyValue : ViewModelBase, IValueHolder
    {
        private NdfObject _instance;
        private NdfProperty _property;
        private NdfValueWrapper _value;
        private byte[] _valueData;

        public ICommand DetailsCommand { get; set; }
        public ICommand AddRowCommand { get; protected set; }
        public ICommand DeleteRowCommand { get; protected set; }

        public NdfPropertyValue(NdfObject instance)
        {
            _instance = instance;

            DetailsCommand = new ActionCommand(NdfObjectViewModel.DetailsCommandExecute);
            AddRowCommand = new ActionCommand(AddRowExecute);
            DeleteRowCommand = new ActionCommand(DeleteRowExecute, DeleteRowCanExecute);
        }

        private bool DeleteRowCanExecute()
        {
            var cv = CollectionViewSource.GetDefaultView(Value);

            return cv != null && cv.CurrentItem != null;
        }

        private void DeleteRowExecute(object obj)
        {
            var cv = CollectionViewSource.GetDefaultView(Value);

            if (cv == null || cv.CurrentItem == null)
                return;

            var val = cv.CurrentItem as CollectionItemValueHolder;

            if (val == null)
                return;

            ((NdfCollection)Value).Remove(cv.CurrentItem);
        }

        private void AddRowExecute(object obj)
        {
            var cv = CollectionViewSource.GetDefaultView(Value);

            if (cv == null)
                return;

            var view = new AddCollectionItemView();
            var vm = new AddCollectionItemViewModel(Manager, view);

            view.DataContext = vm;

            var ret = view.ShowDialog();

            if (ret.HasValue && ret.Value)
                ((NdfCollection)Value).Add(vm.Wrapper);
        }


        public NdfType Type
        {
            get
            {
                if (Value == null)
                    return NdfType.Unset;
                else
                    return Value.Type;
            }
        }

        public byte[] ValueData
        {
            get { return _valueData; }
            set
            {
                _valueData = value; OnPropertyChanged("ValueData");
                OnPropertyChanged("BinValue");
            }
        }

        public string BinValue
        {
            get { return Utils.ByteArrayToBigEndianHeyByteString(ValueData); }
        }

        public NdfValueWrapper Value
        {
            get { return _value; }
            set { _value = value; OnPropertyChanged("Value"); }
        }

        public NdfbinManager Manager
        {
            get { return Property.Class.Manager; }
        }

        public long InstanceOffset
        {
            get
            {
                return Instance.Offset;
            }
        }

        public NdfProperty Property
        {
            get { return _property; }
            set { _property = value; OnPropertyChanged("Property"); }
        }

        public NdfObject Instance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                OnPropertyChanged("Instance");
            }
        }

        private byte[] _oldVal = new byte[0];

        public override void BeginEdit()
        {
            bool valid;
            _oldVal = Value.GetBytes(out valid);

            base.BeginEdit();
        }

        public override void EndEdit()
        {
            bool valid;

            var newValue = Value.GetBytes(out valid);

            if (valid && !Utils.ByteArrayCompare(newValue, _oldVal))
            {
                Property.Class.Manager.ChangeManager.Changes.Add(new ChangeEntry()
                                                                     {
                                                                         ChangedValue = this,
                                                                         NewValue = newValue,
                                                                         OldValue = _oldVal
                                                                     });

                _oldVal = newValue;

            }

            base.EndEdit();
        }

    }
}
