using System.Collections.ObjectModel;
using EdataFileManager.NdfBin.Model.Ndfbin;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Model.Ndfbin
{
    public class NdfbinObject : ViewModelBase
    {
        private readonly ObservableCollection<NdfbinPropertyValue> _propertyValues = new ObservableCollection<NdfbinPropertyValue>();
        private NdfbinClass _class;
        private byte[] _data;
        private int _id;

        public NdfbinClass Class
        {
            get { return _class; }
            set { _class = value; OnPropertyChanged(() => Class); }
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged(() => Data); }
        }

        public ObservableCollection<NdfbinPropertyValue> PropertyValues
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
