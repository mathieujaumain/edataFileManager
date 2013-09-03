using System.Collections.ObjectModel;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.NdfBin.Model.Ndfbin
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
    public class NdfbinFooter : ViewModelBase
    {
        private readonly ObservableCollection<NdfbinFooterEntry> _entries =new ObservableCollection<NdfbinFooterEntry>();

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

        public ObservableCollection<NdfbinFooterEntry> Entries
        {
            get { return _entries; }
        }
    }
}