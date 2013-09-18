using System.Collections.ObjectModel;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Model.Ndfbin
{
    public class NdfObject : ViewModelBase
    {
        private readonly ObservableCollection<NdfPropertyValue> _propertyValues = new ObservableCollection<NdfPropertyValue>();
        private NdfClass _class;
        private byte[] _data;
        private int _id;

        public NdfClass Class
        {
            get { return _class; }
            set { _class = value; OnPropertyChanged(() => Class); }
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged(() => Data); }
        }

        public ObservableCollection<NdfPropertyValue> PropertyValues
        {
            get { return _propertyValues; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(() => Id); }
        }

        public long Offset { get; set; }
    }
}
