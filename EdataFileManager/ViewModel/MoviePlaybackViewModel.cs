using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using EdataFileManager.NdfBin;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel
{
    public class MoviePlaybackViewModel : ViewModelBase
    {
        private string _derp;

        public MoviePlaybackViewModel(string file)
        {
            _derp = file;
        }

        public string File
        {
            get { return _derp; }
            set { _derp = value; OnPropertyChanged(() => File); }
        }
    }
}