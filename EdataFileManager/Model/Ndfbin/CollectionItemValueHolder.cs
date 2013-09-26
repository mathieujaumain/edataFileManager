using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdataFileManager.BL;
using EdataFileManager.Model.Ndfbin.ChangeManager;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.Util;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Model.Ndfbin
{
    public class CollectionItemValueHolder : ViewModelBase, IValueHolder
    {
        private NdfValueWrapper _value;

        public CollectionItemValueHolder(NdfValueWrapper value, NdfbinManager manager, long instanceOffset)
        {
            Value = value;
            Manager = manager;
            InstanceOffset = instanceOffset;
        }

        public virtual NdfbinManager Manager { get; private set; }
        public virtual long InstanceOffset { get; private set; }

        public virtual NdfValueWrapper Value
        {
            get { return _value; }
            set
            {
                _value = value; OnPropertyChanged("Value");
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
                Manager.ChangeManager.Changes.Add(new ChangeEntry()
                {
                    ChangedValue = this,
                    NewValue = newValue,
                    OldValue = _oldVal
                });

                OnPropertyChanged(() => Value);

                _oldVal = newValue;
            }

            base.EndEdit();
        }

    }
}
