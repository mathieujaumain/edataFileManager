using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model;
using EdataFileManager.NdfBin.Model.Ndfbin;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel
{
    public class NdfDetailsViewModel : ViewModelBase
    {
        private ObservableCollection<NdfbinClass> _classes;

        public NdfDetailsViewModel(NdfFile file, EdataManager manager)
        {
            var content = manager.GetNdfContent(file);
            
            var ndfbinManager = new NdfbinManager(content.Body);

            ndfbinManager.ParseData();

            Classes = ndfbinManager.Classes;
        }

        public ObservableCollection<NdfbinClass> Classes
        {
            get { return _classes; }
            set { _classes = value; OnPropertyChanged(() => Classes);}
        }
    }
}
