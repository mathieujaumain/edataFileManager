using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model.Edata;
using EdataFileManager.NdfBin.Model.Ndfbin;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel
{
    public class MoviePlaybackViewModel : ViewModelBase
    {
        protected string derp { get; set; }

        public MoviePlaybackViewModel(string file, EdataManager manager)
        {
            //System.Windows.MessageBox.Show(file);
            derp = file;
        }

        

        public string File
        {
            get { return derp; }
        }

        

    }
}