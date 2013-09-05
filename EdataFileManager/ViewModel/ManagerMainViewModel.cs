using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model.Edata;
using EdataFileManager.Settings;
using EdataFileManager.View;
using EdataFileManager.ViewModel.Base;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace EdataFileManager.ViewModel
{
    public class ManagerMainViewModel : ViewModelBase
    {
        private ObservableCollection<NdfFile> _files;
        private ICollectionView _filesCollectionView;
        private string _filterExpression = string.Empty;

        public ManagerMainViewModel()
        {
            InitializeCommands();

            Settings.Settings settings = SettingsManager.Load();

            var fileInfo = new FileInfo(settings.LastOpenedFile);

            if (fileInfo.Exists)
                LoadFile(fileInfo.FullName);
        }

        public ICommand ExportNdfCommand { get; set; }
        public ICommand ExportTextureCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand ChangeExportPathCommand { get; set; }
        public ICommand ViewNdfContentCommand { get; set; }

        protected EdataManager EdataManager { get; set; }

        public string LoadedFile { get; set; }

        public ObservableCollection<NdfFile> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                OnPropertyChanged(() => Files);
            }
        }

        public string TitleText
        {
            get { return string.Format("Edata File Manager [{0}]", EdataManager.FilePath); }
        }

        public ICollectionView FilesCollectionView
        {
            get
            {
                if (_filesCollectionView == null)
                {
                    CreateFilesCollectionView();
                }

                return _filesCollectionView;
            }
        }

        public string FilterExpression
        {
            get { return _filterExpression; }
            set
            {
                _filterExpression = value;
                OnPropertyChanged(() => FilterExpression);
                FilesCollectionView.Refresh();
            }
        }

        private void CreateFilesCollectionView()
        {
            _filesCollectionView = CollectionViewSource.GetDefaultView(Files);
            _filesCollectionView.Filter = FilterPath;

            OnPropertyChanged(() => FilesCollectionView);
        }

        protected void InitializeCommands()
        {
            ExportNdfCommand = new ActionCommand(ExportNdfExecute);
            ExportTextureCommand = new ActionCommand(ExportTextureExecute);
            OpenFileCommand = new ActionCommand(OpenFileExecute);
            ChangeExportPathCommand = new ActionCommand(ChangeExportPathExecute);
            ViewNdfContentCommand = new ActionCommand(ViewNdfContentExecute);
        }

        protected void ViewNdfContentExecute(object obj)
        {
            var file = FilesCollectionView.CurrentItem as NdfFile;

            if (file == null)
                return;

            var vm = new NdfDetailsViewModel(file, EdataManager);

            var view = new NdfDetailView {DataContext = vm};

            view.Show();
        }

        protected void ExportNdfExecute(object obj)
        {
            var file = FilesCollectionView.CurrentItem as NdfFile;

            if (file == null)
                return;

            Settings.Settings settings = SettingsManager.Load();

            NdfFileContent content = EdataManager.GetNdfContent(file);

            var f = new FileInfo(file.Path);

            using (var fs = new FileStream(Path.Combine(settings.SavePath, f.Name), FileMode.OpenOrCreate))
            {
                fs.Write(content.Body, 0, content.Body.Length);
                fs.Flush();
            }
        }

        protected void ExportTextureExecute(object obj)
        {
            var file = FilesCollectionView.CurrentItem as NdfFile;

            if (file == null)
                return;

            Settings.Settings settings = SettingsManager.Load();

            byte[] buffer = EdataManager.GetRawData(file);

            var f = new FileInfo(file.Path);

            using (var fs = new FileStream(Path.Combine(settings.SavePath, f.Name), FileMode.OpenOrCreate))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
        }

        protected void ExportAll()
        {
            //foreach (var file in Files)
            //{
            //    var f = new FileInfo(file.Path);

            //    var dirToCreate = Path.Combine("c:\\temp\\", f.DirectoryName);

            //    if (!Directory.Exists(dirToCreate))
            //        Directory.CreateDirectory(dirToCreate);

            //    var buffer = NdfManager.GetRawData(file);
            //    using (var fs = new FileStream(Path.Combine(dirToCreate, f.Name), FileMode.OpenOrCreate))
            //    {
            //        fs.Write(buffer, 0, buffer.Length);
            //        fs.Flush();
            //    }

            //}
        }

        protected void ChangeExportPathExecute(object obj)
        {
            Settings.Settings settings = SettingsManager.Load();

            var folderDlg = new FolderBrowserDialog
                                {
                                    SelectedPath = settings.SavePath,
                                    //RootFolder = Environment.SpecialFolder.MyComputer,
                                    ShowNewFolderButton = true,
                                };

            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                settings.SavePath = folderDlg.SelectedPath;
                SettingsManager.Save(settings);
            }
        }

        protected void OpenFileExecute(object obj)
        {
            Settings.Settings settings = SettingsManager.Load();

            var openfDlg = new OpenFileDialog
                               {
                                   FileName = settings.LastOpenedFile,
                                   DefaultExt = ".dat",
                                   Multiselect = false,
                                   Filter = "Edat (.dat)|*.dat|All Files|*.*"
                               };

            if (openfDlg.ShowDialog().Value)
            {
                settings.LastOpenedFile = openfDlg.FileName;
                SettingsManager.Save(settings);
            }

            LoadFile(settings.LastOpenedFile);
        }

        protected void LoadFile(string path)
        {
            EdataManager = new EdataManager(path);

            LoadedFile = EdataManager.FilePath;

            EdataManager.ParseEdataFile();
            Files = EdataManager.Files;
            CreateFilesCollectionView();

            OnPropertyChanged(() => TitleText);
        }

        public bool FilterPath(object item)
        {
            var file = item as NdfFile;

            if (file == null || FilterExpression == string.Empty || FilterExpression.Length < 3)
            {
                return true;
            }

            return file.Path.Contains(FilterExpression);
        }
    }
}