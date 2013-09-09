using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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
        private readonly ObservableCollection<EdataFileViewModel> _openFiles = new ObservableCollection<EdataFileViewModel>();

        public ICommand ExportNdfCommand { get; set; }
        public ICommand ExportRawCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand CloseFileCommand { get; set; }
        public ICommand ChangeExportPathCommand { get; set; }
        public ICommand ViewContentCommand { get; set; }
        public ICommand PlayMovieCommand { get; set; }
        public ICommand AboutUsCommand { get; set; }

        public ObservableCollection<EdataFileViewModel> OpenFiles
        {
            get { return _openFiles; }
        }

        public ManagerMainViewModel()
        {
            InitializeCommands();

            Settings.Settings settings = SettingsManager.Load();

            foreach (var file in settings.LastOpenedFiles)
            {
                var fileInfo = new FileInfo(file);

                if (fileInfo.Exists)
                    AddFile(fileInfo.FullName);
            }

            CollectionViewSource.GetDefaultView(OpenFiles).MoveCurrentToFirst();

            OpenFiles.CollectionChanged += OpenFilesCollectionChanged;
        }

        protected void OpenFilesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var set = SettingsManager.Load();
            set.LastOpenedFiles.Clear();
            set.LastOpenedFiles.AddRange(OpenFiles.Select(x => x.LoadedFile).ToList());
            SettingsManager.Save(set);
        }

        public void AddFile(string path)
        {
            var vm = new EdataFileViewModel();
            vm.LoadFile(path);

            OpenFiles.Add(vm);
            CollectionViewSource.GetDefaultView(OpenFiles).MoveCurrentTo(vm);
        }

        public void CloseFile(EdataFileViewModel vm)
        {
            if (!OpenFiles.Contains(vm))
                return;

            OpenFiles.Remove(vm);
        }

        protected void InitializeCommands()
        {
            ExportNdfCommand = new ActionCommand(ExportNdfExecute);
            ExportRawCommand = new ActionCommand(ExportRawExecute);
            OpenFileCommand = new ActionCommand(OpenFileExecute);
            CloseFileCommand = new ActionCommand(CloseFileExecute);
            PlayMovieCommand = new ActionCommand(PlayMovieExecute);

            AboutUsCommand = new ActionCommand(AboutUsExecute);

            ChangeExportPathCommand = new ActionCommand(ChangeExportPathExecute);
            ViewContentCommand = new ActionCommand(ViewContentExecute);
        }

        protected void ViewContentExecute(object obj)
        {
            var vm = CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel;

            if (vm == null)
                return;

            var ndf = vm.FilesCollectionView.CurrentItem as NdfFile;

            if (ndf == null)
                return;

            var detailsVm = new NdfDetailsViewModel(ndf, vm.EdataManager);

            var view = new NdfDetailView { DataContext = detailsVm };

            view.Show();
        }

        protected void ExportNdfExecute(object obj)
        {
            var vm = CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel;

            if (vm == null)
                return;

            var ndf = vm.FilesCollectionView.CurrentItem as NdfFile;

            if (ndf == null)
                return;

            Settings.Settings settings = SettingsManager.Load();

            NdfFileContent content = vm.EdataManager.GetNdfContent(ndf);

            var f = new FileInfo(ndf.Path);

            using (var fs = new FileStream(Path.Combine(settings.SavePath, f.Name), FileMode.OpenOrCreate))
            {
                fs.Write(content.Body, 0, content.Body.Length);
                fs.Flush();
            }
        }

        protected void ExportRawExecute(object obj)
        {
            var vm = CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel;

            if (vm == null)
                return;

            var ndf = vm.FilesCollectionView.CurrentItem as NdfFile;

            if (ndf == null)
                return;

            Settings.Settings settings = SettingsManager.Load();

            byte[] buffer = vm.EdataManager.GetRawData(ndf);

            var f = new FileInfo(ndf.Path);

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
                                   InitialDirectory = settings.LastOpenFolder,
                                   DefaultExt = ".dat",
                                   Multiselect = true,
                                   Filter = "Edat (.dat)|*.dat|All Files|*.*"
                               };

            if (openfDlg.ShowDialog().Value)
            {
                settings.LastOpenFolder = new FileInfo(openfDlg.FileName).DirectoryName;
                SettingsManager.Save(settings);
                foreach (var fileName in openfDlg.FileNames)
                {
                    AddFile(fileName);
                }
            }
        }

        protected void CloseFileExecute(object obj)
        {
            CloseFile(CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel);
        }

        protected void PlayMovieExecute(object obj)
        {
            string name = "temp.wmv";
            var vm = CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel;

            if (vm == null)
                return;

            var ndf = vm.FilesCollectionView.CurrentItem as NdfFile;

            if (ndf == null)
                return;

            Settings.Settings settings = SettingsManager.Load();

            byte[] buffer = vm.EdataManager.GetRawData(ndf);

            var f = new FileInfo(ndf.Path);

            using (var fs = new FileStream(Path.Combine(settings.SavePath, name), FileMode.OpenOrCreate))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }

            var detailsVm = new MoviePlaybackViewModel(Path.Combine(settings.SavePath, name));

            var view = new MoviePlaybackView { DataContext = detailsVm };

            view.Show();
        }

        protected void AboutUsExecute(object obj)
        {
            //TODO: MAKE IT MORE PROPER.
            MessageBox.Show("EdataFileManager V.0.0.0.0.1\nMade by enohka with contributions from Kamrat Roger\nThanks to Wargame:EE DAT Unpacker by Giovanni Condello \n Uses Icon8 link: http://icons8.com/license/");
        }
    }
}