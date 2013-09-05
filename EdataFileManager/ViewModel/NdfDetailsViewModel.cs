using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model;
using EdataFileManager.NdfBin.Model.Ndfbin;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel
{
    public class NdfDetailsViewModel : ViewModelBase
    {
        private ObservableCollection<NdfbinClass> _classes;
        private ICollectionView _classesCollectionView;

        private ObservableCollection<NdfbinString> _strings;
        private ObservableCollection<NdfbinTran> _trans;

        private string _classesFilterExpression = string.Empty;

        protected NdfFile OwnerFile { get; set; }

        public NdfDetailsViewModel(NdfFile file, EdataManager manager)
        {
            OwnerFile = file;
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
            set { _classes = value; OnPropertyChanged(() => Classes); }
        }

        public ObservableCollection<NdfbinString> Strings
        {
            get { return _strings; }
            set { _strings = value; OnPropertyChanged(() => Strings); }
        }

        public ObservableCollection<NdfbinTran> Trans
        {
            get { return _trans; }
            set { _trans = value; OnPropertyChanged(() => Trans); }
        }

        public string Title
        {
            get { return string.Format("Ndf Content Viewer [{0}]", OwnerFile.Path); }
        }

        public ICollectionView ClassesCollectoinView
        {
            get
            {
                if (_classesCollectionView == null)
                {
                    BuildClassesCollectionView();
                }

                return _classesCollectionView;
            }
        }

        public string ClassesFilterExpression
        {
            get { return _classesFilterExpression; }
            set
            {
                _classesFilterExpression = value; OnPropertyChanged(() => ClassesFilterExpression);

                ClassesCollectoinView.Refresh();
            }
        }

        private void BuildClassesCollectionView()
        {
            _classesCollectionView = CollectionViewSource.GetDefaultView(Classes);
            _classesCollectionView.Filter = FilterClasses;

            OnPropertyChanged(() => ClassesCollectoinView);
        }

        public bool FilterClasses(object o)
        {
            var clas = o as NdfbinClass;

            if (clas == null || ClassesFilterExpression == string.Empty)
                return true;

            return clas.Name.ToLower().Contains(ClassesFilterExpression) || clas.Id.ToString().Contains(ClassesFilterExpression);
        }
    }
}
