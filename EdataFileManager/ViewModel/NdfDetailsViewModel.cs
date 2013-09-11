using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model.Edata;
using EdataFileManager.NdfBin.Model.Ndfbin;
using EdataFileManager.ViewModel.Base;
using EdataFileManager.ViewModel.Filter;

namespace EdataFileManager.ViewModel
{
    public class NdfDetailsViewModel : ViewModelBase
    {
        private ObservableCollection<NdfbinClass> _classes;
        private ICollectionView _classesCollectionView;
        private string _classesFilterExpression = string.Empty;

        private ObservableCollection<NdfbinString> _strings;
        private ICollectionView _stringCollectionView;
        private string _stringFilterExpression = string.Empty;

        private ObservableCollection<NdfbinTran> _trans;
        private ICollectionView _transCollectionView;
        private string _transFilterExpression = string.Empty;

        public NdfDetailsViewModel(NdfFile file, EdataManager manager)
        {
            OwnerFile = file;
            NdfFileContent content = manager.GetNdfContent(file);

            var ndfbinManager = new NdfbinManager(content.Body);

            ndfbinManager.ParseData();

            Classes = ndfbinManager.Classes;
            Strings = ndfbinManager.Strings;
            Trans = ndfbinManager.Trans;

        }

        protected NdfFile OwnerFile { get; set; }

        public ObservableCollection<NdfbinClass> Classes
        {
            get { return _classes; }
            set
            {
                _classes = value;
                OnPropertyChanged(() => Classes);
            }
        }

        public ObservableCollection<NdfbinString> Strings
        {
            get { return _strings; }
            set
            {
                _strings = value;
                OnPropertyChanged(() => Strings);
            }
        }

        public ObservableCollection<NdfbinTran> Trans
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
            var clas = o as NdfbinClass;

            if (clas == null || ClassesFilterExpression == string.Empty)
                return true;

            return clas.Name.ToLower().Contains(ClassesFilterExpression) ||
                   clas.Id.ToString(CultureInfo.CurrentCulture).Contains(ClassesFilterExpression);
        }

        public bool FilterStrings(object o)
        {
            var str = o as NdfbinString;

            if (str == null || StringFilterExpression == string.Empty)
                return true;

            return str.Value.ToLower().Contains(StringFilterExpression) ||
                   str.Id.ToString(CultureInfo.CurrentCulture).Contains(StringFilterExpression);
        }

        public bool FilterTrans(object o)
        {
            var tran = o as NdfbinTran;

            if (tran == null || TransFilterExpression == string.Empty)
                return true;

            return tran.Value.ToLower().Contains(TransFilterExpression) ||
                   tran.Id.ToString(CultureInfo.CurrentCulture).Contains(TransFilterExpression);
        }

    }
}