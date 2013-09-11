using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using EdataFileManager.Util;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
{
    public class NdfbinProperty : ViewModelBase
    {
        private NdfbinClass _class;
        private int _id;
        private string _name;
        private long _offset;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(() => Id);
            }
        }

        public string BinId
        {
            get { return Utils.Int32ToBigEndianHexByteString(Id); }
        }

        public NdfbinClass Class
        {
            get { return _class; }
            set
            {
                _class = value;
                OnPropertyChanged(() => Class);
            }
        }

        public long Offset
        {
            get { return _offset; }
            set
            {
                _offset = value;
                OnPropertyChanged(() => Offset);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public object Value
        {
            get
            {
                var currentInstance = Class.InstancesCollectionView.CurrentItem as NdfbinObject;

                if (currentInstance == null)
                    return null;

                var value = currentInstance.PropertyValues.SingleOrDefault(x => x.Property == this);

                if (value != null)
                    return value.Value;

                return null;
            }
        }

        public string ValueType
        {
            get
            {
                var currentInstance = Class.InstancesCollectionView.CurrentItem as NdfbinObject;

                if (currentInstance == null)
                    return null;

                var value = currentInstance.PropertyValues.SingleOrDefault(x => x.Property == this);

                if (value == null)
                    return null;

                return value.Type.ToString();

            }
        }

        public string ValueData
        {
            get
            {
                var currentInstance = Class.InstancesCollectionView.CurrentItem as NdfbinObject;

                if (currentInstance == null)
                    return null;

                var value = currentInstance.PropertyValues.SingleOrDefault(x => x.Property == this);

                if (value == null)
                    return null;

                return Utils.ByteArrayToBigEndianHeyByteString(value.ValueData);
            }
        }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.InvariantCulture);
        }
    }
}