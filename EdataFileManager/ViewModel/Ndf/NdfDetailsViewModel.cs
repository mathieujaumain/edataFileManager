using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using EdataFileManager.BL;
using EdataFileManager.Model.Edata;
using EdataFileManager.Model.Ndfbin;
using EdataFileManager.ViewModel.Base;
using EdataFileManager.ViewModel.Edata;

namespace EdataFileManager.ViewModel.Ndf
{
    public class NdfDetailsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<NdfClassViewModel> _classes = new ObservableCollection<NdfClassViewModel>();
        private ICollectionView _classesCollectionView;
        private string _classesFilterExpression = string.Empty;

        private ObservableCollection<NdfStringReference> _strings;
        private ICollectionView _stringCollectionView;
        private string _stringFilterExpression = string.Empty;

        private ObservableCollection<NdfTranReference> _trans;
        private ICollectionView _transCollectionView;
        private string _transFilterExpression = string.Empty;

        private string _statusText = string.Empty;

        public NdfDetailsViewModel(EdataContentFile contentFile, EdataFileViewModel ownerVm)
        {
            OwnerFile = contentFile;
            EdataFileViewModel = ownerVm;

            var ndfbinManager = new NdfbinManager(ownerVm.EdataManager.GetRawData(contentFile));
            NdfbinManager = ndfbinManager;

            ndfbinManager.Initialize();

            foreach (var cls in ndfbinManager.Classes)
                Classes.Add(new NdfClassViewModel(cls));

            Strings = ndfbinManager.Strings;
            Trans = ndfbinManager.Trans;

            SaveNdfbinCommand = new ActionCommand(SaveNdfbinExecute, () => NdfbinManager.ChangeManager.HasChanges);
        }

        /// <summary>
        /// Virtual call
        /// </summary>
        /// <param name="content"></param>
        public NdfDetailsViewModel(byte[] content)
        {
            OwnerFile = null;
            EdataFileViewModel = null;

            var ndfbinManager = new NdfbinManager(content);
            NdfbinManager = ndfbinManager;

            ndfbinManager.Initialize();

            foreach (var cls in ndfbinManager.Classes)
                Classes.Add(new NdfClassViewModel(cls));

            Strings = ndfbinManager.Strings;
            Trans = ndfbinManager.Trans;

            SaveNdfbinCommand = new ActionCommand(SaveNdfbinExecute, () => NdfbinManager.ChangeManager.HasChanges);
        }

        public NdfbinManager NdfbinManager { get; protected set; }
        protected EdataFileViewModel EdataFileViewModel { get; set; }
        protected EdataContentFile OwnerFile { get; set; }

        public ICommand SaveNdfbinCommand { get; set; }

        public string Title
        {
            get
            {
                string path = "Virtual";

                if (OwnerFile != null)
                    path = OwnerFile.Path;

                return string.Format("Ndf Editor [{0}]", path);
            }
        }

        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; OnPropertyChanged(() => StatusText); }
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

        public ObservableCollection<NdfClassViewModel> Classes
        {
            get { return _classes; }
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
            var clas = o as NdfClassViewModel;

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

        private void SaveNdfbinExecute(object obj)
        {
            StatusText = string.Format("Saving back {0} changes into {1}", NdfbinManager.ChangeManager.Changes.Count, EdataFileViewModel.EdataManager.FilePath);

            try
            {
                var changesCount = NdfbinManager.ChangeManager.Changes.Count;

                NdfbinManager.CommitChanges();

                var newFile = NdfbinManager.BuildNdfFile(NdfbinManager.Header.IsCompressedBody);

                EdataFileViewModel.EdataManager.ReplaceFile(OwnerFile, newFile);

                EdataFileViewModel.LoadFile(EdataFileViewModel.LoadedFile);

                var newOwen = EdataFileViewModel.EdataManager.Files.Single(x => x.Path == OwnerFile.Path);
                OwnerFile = newOwen;

                StatusText = string.Format("Saving of {0} changes finished! {1}", changesCount, EdataFileViewModel.EdataManager.FilePath);
            }
            catch (Exception e)
            {
                StatusText = string.Format("Saving interrupted - Did you start Wargame before I was ready?");
            }
        }
    }
}