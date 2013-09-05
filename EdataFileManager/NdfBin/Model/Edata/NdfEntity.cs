using System.Globalization;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Edata
{
    public class NdfEntity: ViewModelBase
    {
        private int _groupId;
        private string _name;
        private int _fileEntrySize;

        public int GroupId
        {
            get { return _groupId; }
            set { _groupId = value; OnPropertyChanged(() => GroupId); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(() => Name); }
        }

        public int FileEntrySize
        {
            get { return _fileEntrySize; }
            set { _fileEntrySize = value; OnPropertyChanged(() => FileEntrySize); }
        }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.CurrentCulture);
        }
    }
}
