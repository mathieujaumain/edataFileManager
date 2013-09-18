using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Data;
using EdataFileManager.BL;
using EdataFileManager.Model.Edata;
using EdataFileManager.NdfBin;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel
{
    public class EdataFileViewModel : ViewModelBase
    {
        private ObservableCollection<EdataContentFile> _files;
        private ICollectionView _filesCollectionView;
        private string _filterExpression = string.Empty;
        private string _loadedFile = string.Empty;

        public EdataManager EdataManager { get; protected set; }

        public string LoadedFile
        {
            get { return _loadedFile; }
            set
            {
                _loadedFile = value;
                OnPropertyChanged(() => LoadedFile);
                OnPropertyChanged(() => HeaderText);
            }
        }

        public string HeaderText
        {
            get
            {
                var f = new FileInfo(LoadedFile);

                return f.Name;
            }
        }

        public ObservableCollection<EdataContentFile> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                OnPropertyChanged(() => Files);
            }
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

        public void LoadFile(string path)
        {
            EdataManager = new EdataManager(path);

            LoadedFile = EdataManager.FilePath;

            EdataManager.ParseEdataFile();
            Files = EdataManager.Files;
            CreateFilesCollectionView();
        }

        public bool FilterPath(object item)
        {
            var file = item as EdataContentFile;

            if (file == null || FilterExpression == string.Empty || FilterExpression.Length < 3)
            {
                return true;
            }

            return file.Path.Contains(FilterExpression);
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
    }
}
