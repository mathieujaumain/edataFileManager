using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model;
using EdataFileManager.Settings;
using EdataFileManager.ViewModel.Base;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace EdataFileManager.ViewModel
{
    public class ManagerMainViewModel : ViewModelBase
    {
        private ObservableCollection<NdfFile> _files;

        protected NdfBinManager NdfManager { get; set; }

        public ICommand ExportNdfCommand { get; set; }
        public ICommand ExportTextureCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand ChangeExportPathCommand { get; set; }

        public string LoadedFile { get; set; }

        public ObservableCollection<NdfFile> Files
        {
            get { return _files; }
            set { _files = value; OnPropertyChanged(() => Files); }
        }

        protected void ExportNdfExecute(object obj)
        {
            var file = obj as NdfFile;

            if (file == null)
                return;

            var settings = SettingsManager.Load();

            var content = NdfManager.GetNdfContent(file);

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

            var buffer = NdfManager.GetRawData(file);

            var f = new FileInfo(file.Path);

            using (var fs = new FileStream(Path.Combine(settings.SavePath , f.Name), FileMode.OpenOrCreate))
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

        public ManagerMainViewModel()
        {
            ExportNdfCommand = new ActionCommand(ExportNdfExecute);
            ExportTextureCommand = new ActionCommand(ExportTextureExecute);
            OpenFileCommand = new ActionCommand(OpenFileExecute);
            ChangeExportPathCommand = new ActionCommand(ChangeExportPathExecute);

            var settings = SettingsManager.Load();

            LoadedFile = settings.LastOpenedFile;

            NdfManager = new NdfBinManager(LoadedFile);

            NdfManager.ParseEdataFile();
            Files = NdfManager.Files;
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

            NdfManager.FilePath = settings.LastOpenedFile;

            NdfManager.ParseEdataFile();
            Files = NdfManager.Files;
        }
    }
}
