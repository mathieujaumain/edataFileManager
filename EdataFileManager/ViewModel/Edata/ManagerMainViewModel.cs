﻿using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using EdataFileManager.BL;
using EdataFileManager.Model.Edata;
using EdataFileManager.Settings;
using EdataFileManager.View;
using EdataFileManager.View.Ndfbin;
using EdataFileManager.ViewModel.Base;
using EdataFileManager.ViewModel.Ndf;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace EdataFileManager.ViewModel.Edata
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
        public ICommand ViewTradFileCommand { get; set; }
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
            OpenFileCommand = new ActionCommand(OpenFileExecute);
            CloseFileCommand = new ActionCommand(CloseFileExecute);


            ChangeExportPathCommand = new ActionCommand(ChangeExportPathExecute);


            ExportNdfCommand = new ActionCommand(ExportNdfExecute, () => IsOfType(EdataFileType.Ndfbin));
            ExportRawCommand = new ActionCommand(ExportRawExecute);
            PlayMovieCommand = new ActionCommand(PlayMovieExecute);

            AboutUsCommand = new ActionCommand(AboutUsExecute);


            ViewTradFileCommand = new ActionCommand(ViewTradFileExecute, () => IsOfType(EdataFileType.Dictionary));

            ViewContentCommand = new ActionCommand(ViewContentExecute, () => IsOfType(EdataFileType.Ndfbin));
        }

        private bool IsOfType(EdataFileType type)
        {
            var vm = CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel;

            if (vm == null)
                return false;

            var ndf = vm.FilesCollectionView.CurrentItem as EdataContentFile;

            if (ndf == null)
                return false;

            return ndf.FileType == type;
        }

        private void ViewTradFileExecute(object obj)
        {
            var vm = CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel;

            if (vm == null)
                return;

            var ndf = vm.FilesCollectionView.CurrentItem as EdataContentFile;

            if (ndf == null)
                return;

            var tradVm = new TradFileViewModel(vm.EdataManager.GetRawData(ndf), ndf);

            var view = new TradFileView() { DataContext = tradVm };

            view.Show();
        }

        protected void ViewContentExecute(object obj)
        {
            var vm = CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel;

            if (vm == null)
                return;

            var ndf = vm.FilesCollectionView.CurrentItem as EdataContentFile;

            if (ndf == null)
                return;

            var detailsVm = new NdfDetailsViewModel(ndf, vm);

            var view = new NdfbinView { DataContext = detailsVm };

            view.Show();
        }

        protected void ExportNdfExecute(object obj)
        {
            var vm = CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel;

            if (vm == null)
                return;

            var ndf = vm.FilesCollectionView.CurrentItem as EdataContentFile;

            if (ndf == null)
                return;

            Settings.Settings settings = SettingsManager.Load();

            var content = new NdfbinManager(ndf.Manager.GetRawData(ndf)).GetContent();

            var f = new FileInfo(ndf.Path);

            using (var fs = new FileStream(Path.Combine(settings.SavePath, f.Name), FileMode.OpenOrCreate))
            {
                fs.Write(content, 0, content.Length);
                fs.Flush();
            }
        }

        protected void ExportRawExecute(object obj)
        {
            var vm = CollectionViewSource.GetDefaultView(OpenFiles).CurrentItem as EdataFileViewModel;

            if (vm == null)
                return;

            var ndf = vm.FilesCollectionView.CurrentItem as EdataContentFile;

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
                                   DefaultExt = ".dat",
                                   Multiselect = true,
                                   Filter = "Edat (.dat)|*.dat|All Files|*.*"
                               };

            if (File.Exists(settings.LastOpenFolder))
                openfDlg.InitialDirectory = settings.LastOpenFolder;


            if (openfDlg.ShowDialog().Value)
            {
                settings.LastOpenFolder = new FileInfo(openfDlg.FileName).DirectoryName;
                SettingsManager.Save(settings);
                foreach (var fileName in openfDlg.FileNames)
                {
                    HandleNewFile(fileName);
                }
            }
        }

        private void HandleNewFile(string fileName)
        {
            byte[] headerBuffer;

            var type = EdataFileType.Unknown;

            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                headerBuffer = new byte[12];
                fs.Read(headerBuffer, 0, headerBuffer.Length);

                type = EdataManager.GetFileTypeFromHeaderData(headerBuffer);
                if (type == EdataFileType.Ndfbin)
                {
                    var buffer = new byte[fs.Length];

                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(buffer, 0, buffer.Length);

                    var detailsVm = new NdfDetailsViewModel(buffer);

                    var view = new NdfbinView { DataContext = detailsVm };

                    view.Show();
                }
            }

            if (type == EdataFileType.Package)
                AddFile(fileName);
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

            var ndf = vm.FilesCollectionView.CurrentItem as EdataContentFile;

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