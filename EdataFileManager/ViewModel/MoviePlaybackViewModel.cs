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
            set
            {
                _derp = value;
                OnPropertyChanged(() => File);
            }
        }
    }
}