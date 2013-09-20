using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Model.Ndfbin.ChangeManager
{
    public class ChangeEntry : ViewModelBase
    {
        private bool _triggersOffsetreordering;
        private byte[] _oldValue;
        private byte[] _newValue;
        private NdfPropertyValue _changedValue;

        public bool TriggersOffsetreordering
        {
            get { return _triggersOffsetreordering; }
            set { _triggersOffsetreordering = value; OnPropertyChanged(() => TriggersOffsetreordering); }
        }

        public object OldValueDisplay
        {
            get { return NdfTypeManager.GetValue(OldValue, ChangedValue.Value.Type, null, 0); }
        }


        public byte[] OldValue
        {
            get { return _oldValue; }
            set { _oldValue = value; OnPropertyChanged(() => OldValue); }
        }

        public byte[] NewValue
        {
            get { return _newValue; }
            set { _newValue = value; OnPropertyChanged(() => NewValue); }
        }

        public NdfPropertyValue ChangedValue
        {
            get { return _changedValue; }
            set { _changedValue = value; OnPropertyChanged(() => ChangedValue); }
        }

    }
}
