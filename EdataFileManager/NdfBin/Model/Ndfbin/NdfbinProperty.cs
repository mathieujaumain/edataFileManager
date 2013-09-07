using System.Linq;
using System.Windows.Data;
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
                var value = ((NdfbinObject)CollectionViewSource.GetDefaultView(Class.Instances).CurrentItem).PropertyValues.SingleOrDefault(x => x.Property == this);
                if (value != null)
                    return value.Value;

                return null;
            }
        }
    }
}