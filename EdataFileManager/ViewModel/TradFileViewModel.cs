﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using EdataFileManager.BL;
using EdataFileManager.Model.Edata;
using EdataFileManager.Model.Trad;
using EdataFileManager.Util;
using EdataFileManager.ViewModel.Base;
using EdataFileManager.ViewModel.Edata;

namespace EdataFileManager.ViewModel
{
    public class TradFileViewModel : ViewModelBase
    {
        private string _filterExpression = string.Empty;
        private string _titleText;
        private ObservableCollection<TradEntry> _entries;
        private ICollectionView _entriesCollectionView;

        public TradManager Manager { get; protected set; }

        public EdataFileViewModel OwnerVm { get; protected set; }
        public EdataContentFile OwnerFile { get; protected set; }

        public ICommand SaveCommand { get; protected set; }
        public ICommand CreateHashCommand { get; protected set; }


        public TradFileViewModel(EdataContentFile owner, EdataFileViewModel contentFile)
        {
            SaveCommand = new ActionCommand(SaveCommandExecute);
            CreateHashCommand = new ActionCommand(CreateHashExecute, CreateHashCanExecute);

            OwnerFile = owner;
            OwnerVm = contentFile;

            Manager = new TradManager(OwnerVm.EdataManager.GetRawData(OwnerFile));

            Entries = Manager.Entries;

            TitleText = string.Format("Dictionary Viewer [{0}]", OwnerFile.Path);
        }

        private bool CreateHashCanExecute()
        {
            var item = EntriesCollectionView.CurrentItem as TradEntry;

            if (item == null || !item.UserCreated)
                return false;

            return true;
        }

        private void CreateHashExecute(object obj)
        {
            var item = EntriesCollectionView.CurrentItem as TradEntry;

            if (item == null || !item.UserCreated)
                return;
            
            item.Hash = Utils.CreateLocalisationHash(Utils.GenerateCoupon(8,new Random()));
        }

        private void SaveCommandExecute(object obj)
        {
            var newFile = Manager.BuildTradFile();

            OwnerVm.EdataManager.ReplaceFile(OwnerFile, newFile);

            OwnerVm.LoadFile(OwnerVm.LoadedFile);

            var newOwen = OwnerVm.EdataManager.Files.Single(x => x.Path == OwnerFile.Path);

            OwnerFile = newOwen;
        }

        public ObservableCollection<TradEntry> Entries
        {
            get { return _entries; }
            set { _entries = value; OnPropertyChanged(() => Entries); }
        }

        public ICollectionView EntriesCollectionView
        {
            get
            {
                if (_entriesCollectionView == null)
                {
                    _entriesCollectionView = CollectionViewSource.GetDefaultView(Entries);
                    _entriesCollectionView.Filter = FilterEntries;
                }

                return _entriesCollectionView;
            }
        }

        public string FilterExpression
        {
            get { return _filterExpression; }
            set
            {
                _filterExpression = value;
                OnPropertyChanged(() => FilterExpression);
                EntriesCollectionView.Refresh();
            }
        }

        public string TitleText
        {
            get { return _titleText; }
            set { _titleText = value; OnPropertyChanged(() => TitleText); }
        }

        private bool FilterEntries(object obj)
        {
            var entr = obj as TradEntry;

            if (entr == null)
                return false;

            if (FilterExpression.Length < 2)
                return true;

            if (entr.Content.ToLower().Contains(FilterExpression.ToLower()) || entr.HashView.ToLower().Contains(FilterExpression.ToLower()))
                return true;

            return false;
        }
    }
}
