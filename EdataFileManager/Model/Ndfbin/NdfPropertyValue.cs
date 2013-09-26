﻿using System;
using System.Windows.Input;
using EdataFileManager.BL;
using EdataFileManager.Model.Ndfbin.ChangeManager;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.Util;
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

        public NdfPropertyValue(NdfObject instance)
        {
            _instance = instance;

            DetailsCommand = new ActionCommand(NdfObjectViewModel.DetailsCommandExecute);
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
