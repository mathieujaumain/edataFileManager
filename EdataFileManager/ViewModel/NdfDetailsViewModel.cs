using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Input;
using EdataFileManager.BL;
using EdataFileManager.Model.Edata;
using EdataFileManager.Model.Ndfbin;
using EdataFileManager.NdfBin;
using EdataFileManager.ViewModel.Base;
using EdataFileManager.ViewModel.Filter;

namespace EdataFileManager.ViewModel
{
    public class NdfDetailsViewModel : ViewModelBase
    {
        private ObservableCollection<NdfClass> _classes;
        private ICollectionView _classesCollectionView;
        private string _classesFilterExpression = string.Empty;

        private ObservableCollection<NdfStringReference> _strings;
        private ICollectionView _stringCollectionView;
        private string _stringFilterExpression = string.Empty;

        private ObservableCollection<NdfTranReference> _trans;
        private ICollectionView _transCollectionView;
        private string _transFilterExpression = string.Empty;

        public NdfDetailsViewModel(EdataContentFile contentFile, EdataFileViewModel ownerVm)
        {
            OwnerFile = contentFile;
            EdataFileViewModel = ownerVm;

            var ndfbinManager = new NdfbinManager(ownerVm.EdataManager.GetRawData(contentFile));
            NdfbinManager = ndfbinManager;

            ndfbinManager.Initialize();

            Classes = ndfbinManager.Classes;
            Strings = ndfbinManager.Strings;
            Trans = ndfbinManager.Trans;

            SaveNdfbinCommand = new ActionCommand(SaveNdfbinExecute);
        }

        public NdfDetailsViewModel(EdataContentFile contentFile)
        {
            
        }

        private void SaveNdfbinExecute(object obj)
        {
            var newFile = NdfbinManager.BuildNdfFile(true);
            
            EdataFileViewModel.EdataManager.ReplaceFile(OwnerFile, newFile);

            EdataFileViewModel.LoadFile(EdataFileViewModel.LoadedFile);
        }

        public NdfbinManager NdfbinManager { get; set; }

        public EdataFileViewModel EdataFileViewModel { get; set; }

        protected EdataContentFile OwnerFile { get; set; }

        public ICommand SaveNdfbinCommand { get; set; }

        public ObservableCollection<NdfClass> Classes
        {
            get { return _classes; }
            set
            {
                _classes = value;
                OnPropertyChanged(() => Classes);
            }
        }

        public ObservableCollection<NdfStringReference> Strings
        {
            get { return _strings; }
            set
            {
                _strings = value;
                OnPropertyChanged(() => Strings);
            }
        }

        public ObservableCollection<NdfTranReference> Trans
        {
            get { return _trans; }
            set
            {
                _trans = value;
                OnPropertyChanged(() => Trans);
            }
        }

        public string Title
        {
            get { return string.Format("Ndf Content Viewer [{0}]", OwnerFile.Path); }
        }

        public ICollectionView ClassesCollectionView
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

        public ICollectionView StringCollectionView
        {
            get
            {
                if (_stringCollectionView == null)
                {
                    BuildStringCollectionView();
                }

                return _stringCollectionView;
            }
        }

        public ICollectionView TransCollectionView
        {
            get
            {
                if (_transCollectionView == null)
                {
                    BuildTransCollectionView();
                }

                return _transCollectionView;
            }
        }

        public string ClassesFilterExpression
        {
            get { return _classesFilterExpression; }
            set
            {
                _classesFilterExpression = value;
                OnPropertyChanged(() => ClassesFilterExpression);

                ClassesCollectionView.Refresh();
            }
        }

        public string StringFilterExpression
        {
            get { return _stringFilterExpression; }
            set
            {
                _stringFilterExpression = value;
                OnPropertyChanged(() => StringFilterExpression);
                StringCollectionView.Refresh();
            }
        }

        public string TransFilterExpression
        {
            get { return _transFilterExpression; }
            set
            {
                _transFilterExpression = value;
                OnPropertyChanged(() => TransFilterExpression);
                TransCollectionView.Refresh();
            }
        }

        private void BuildClassesCollectionView()
        {
            _classesCollectionView = CollectionViewSource.GetDefaultView(Classes);
            _classesCollectionView.Filter = FilterClasses;

            OnPropertyChanged(() => ClassesCollectionView);
        }

        private void BuildStringCollectionView()
        {
            _stringCollectionView = CollectionViewSource.GetDefaultView(Strings);
            _stringCollectionView.Filter = FilterStrings;

            OnPropertyChanged(() => StringCollectionView);
        }

        private void BuildTransCollectionView()
        {
            _transCollectionView = CollectionViewSource.GetDefaultView(Trans);
            _transCollectionView.Filter = FilterTrans;

            OnPropertyChanged(() => TransCollectionView);
        }

        public bool FilterClasses(object o)
        {
            var clas = o as NdfClass;

            if (clas == null || ClassesFilterExpression == string.Empty)
                return true;

            return clas.Name.ToLower().Contains(ClassesFilterExpression) ||
                   clas.Id.ToString(CultureInfo.CurrentCulture).Contains(ClassesFilterExpression);
        }

        public bool FilterStrings(object o)
        {
            var str = o as NdfStringReference;

            if (str == null || StringFilterExpression == string.Empty)
                return true;

            return str.Value.ToLower().Contains(StringFilterExpression) ||
                   str.Id.ToString(CultureInfo.CurrentCulture).Contains(StringFilterExpression);
        }

        public bool FilterTrans(object o)
        {
            var tran = o as NdfTranReference;

            if (tran == null || TransFilterExpression == string.Empty)
                return true;

            return tran.Value.ToLower().Contains(TransFilterExpression) ||
                   tran.Id.ToString(CultureInfo.CurrentCulture).Contains(TransFilterExpression);
        }

    }
}