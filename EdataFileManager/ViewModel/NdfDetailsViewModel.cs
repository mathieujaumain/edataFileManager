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
        private ObservableCollection<NdfbinString> _strings;
        private ObservableCollection<NdfbinTran> _trans;


        public NdfDetailsViewModel(NdfFile file, EdataManager manager)
        {
            var content = manager.GetNdfContent(file);
            
            var ndfbinManager = new NdfbinManager(content.Body);

            ndfbinManager.ParseData();

            Classes = ndfbinManager.Classes;
            Strings = ndfbinManager.Strings;
            Trans = ndfbinManager.Trans;
        }

        public ObservableCollection<NdfbinClass> Classes
        {
            get { return _classes; }
            set { _classes = value; OnPropertyChanged(() => Classes);}
        }

        public ObservableCollection<NdfbinString> Strings
        {
            get { return _strings; }
            set { _strings = value; OnPropertyChanged(() => Strings);}
        }

        public ObservableCollection<NdfbinTran> Trans
        {
            get { return _trans; }
            set { _trans = value; OnPropertyChanged(() => Trans); }
        }
    }
}
