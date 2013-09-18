using System.Collections.ObjectModel;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Model.Ndfbin
{
    /// <summary>
    /// typedef struct ndfbinFooterPart {
    ///    char partName[4];
    ///    DWORD junk1;
    ///    DWORD offset;
    ///    DWORD junk2;
    ///    DWORD length;
    ///    DWORD junk3;
    /// } footerPart;
    ///
    /// struct ndfbinFooter {
    ///    char header[8];
    ///    footerPart entries[9];
    /// };
    /// </summary>
    public class NdfFooter : ViewModelBase
    {
        private readonly ObservableCollection<NdfFooterEntry> _entries =new ObservableCollection<NdfFooterEntry>();

        private string _header;

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged(() => Header);
            }
        }

        public ObservableCollection<NdfFooterEntry> Entries
        {
            get { return _entries; }
        }
    }
}