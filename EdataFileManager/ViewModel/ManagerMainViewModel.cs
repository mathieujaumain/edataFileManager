using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model;
using EdataFileManager.Settings;
using EdataFileManager.View;
using EdataFileManager.ViewModel.Base;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace EdataFileManager.ViewModel
{
    public class ManagerMainViewModel : ViewModelBase
    {
        private ObservableCollection<NdfFile> _files;

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
            set { _files = value; OnPropertyChanged(() => Files); }
        }

        public ManagerMainViewModel()
        {
            InitializeCommands();

            var settings = SettingsManager.Load();

            var fileInfo = new FileInfo(settings.LastOpenedFile);

            if (fileInfo.Exists)
                LoadFile(fileInfo.FullName);
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
            var file = obj as NdfFile;

            if (file == null)
                return;

            var vm = new NdfDetailsViewModel(file, EdataManager);

            var view = new NdfDetailView {DataContext = vm};

            view.Show();
        }

        protected void ExportNdfExecute(object obj)
        {
            var file = obj as NdfFile;

            if (file == null)
                return;

            var settings = SettingsManager.Load();

            var content = EdataManager.GetNdfContent(file);

            var f = new FileInfo(file.Path);

            using (var fs = new FileStream(Path.Combine(settings.SavePath, f.Name), FileMode.OpenOrCreate))
            {
                fs.Write(content.Body, 0, content.Body.Length);
                fs.Flush();
            }
        }

        protected void ExportTextureExecute(object obj)
        {
            var file = obj as NdfFile;

            if (file == null)
                return;

            var settings = SettingsManager.Load();

            var buffer = EdataManager.GetRawData(file);

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
            var settings = SettingsManager.Load();

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
            var settings = SettingsManager.Load();

            var openfDlg = new OpenFileDialog
            {
                FileName = settings.LastOpenedFile,
                DefaultExt = ".dat",
                Multiselect = false,
                Filter = "Edat (.dat)|*.dat"
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
        }
    }
}
