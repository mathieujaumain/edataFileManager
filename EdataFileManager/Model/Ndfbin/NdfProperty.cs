using System.Globalization;
using System.IO;
using System.Linq;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;
using EdataFileManager.Util;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Model.Ndfbin
{
    // TODO: biggest crap of a class ever.
    public class NdfProperty : ViewModelBase
    {
        private NdfClass _class;
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

        public NdfClass Class
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

        public NdfValueWrapper Value
        {
            get
            {
                var currentInstance = Class.InstancesCollectionView.CurrentItem as NdfObject;

                if (currentInstance == null)
                    return null;

                var value = currentInstance.PropertyValues.SingleOrDefault(x => x.Property == this);

                if (value != null)
                    return value.Value;

                return null;
            }
            set
            {
                var val = Value as NdfFlatValueWrapper;
                if (val == null)
                    return;

                var instance = Class.InstancesCollectionView.CurrentItem as NdfObject;

                bool valid;

                var buffer = NdfTypeManager.GetBytes(value, val.Type, out valid);

                if (!valid)
                    return;

                using (var ms = new MemoryStream(Class.Manager.ContentData))
                {
                    ms.Seek(val.OffSet + instance.Offset, SeekOrigin.Begin);
                    ms.Write(buffer,0,buffer.Length);
                }

                Class.Manager.HasChanges = true;
                val.Value = value;
            }
        }

        public NdfType ValueType
        {
            get
            {
                var currentInstance = Class.InstancesCollectionView.CurrentItem as NdfObject;

                if (currentInstance == null)
                    return NdfType.Unset;

                var value = currentInstance.PropertyValues.SingleOrDefault(x => x.Property == this);

                if (value == null)
                    return NdfType.Unset;

                return value.Value.Type;
            }
        }

        public string ValueData
        {
            get
            {
                var currentInstance = Class.InstancesCollectionView.CurrentItem as NdfObject;

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