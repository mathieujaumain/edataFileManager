using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Settings
{
    public class Settings : ViewModelBase
    {
        private string _lastOpenedFile = @"C:\";
        private string _savePath = @"C:\";

        public string SavePath
        {
            get { return _savePath; }
            set
            {
                _savePath = value;
                OnPropertyChanged(() => SavePath);
            }
        }

        public string LastOpenedFile
        {
            get { return _lastOpenedFile; }
            set
            {
                _lastOpenedFile = value;
                OnPropertyChanged(() => LastOpenedFile);
            }
        }
    }
}