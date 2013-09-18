using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using EdataFileManager.BL;
using EdataFileManager.Model.Edata;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model.Trad;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel
{
    public class TradFileViewModel : ViewModelBase
    {
        private string _filterExpression = string.Empty;
        private string _titleText;
        private ObservableCollection<TradEntry> _entries;
        private ICollectionView _entriesCollectionView;

        public TradFileViewModel(byte[] data, EdataContentFile contentFile)
        {
            var mgr = new TradManager(data);
            Entries = mgr.Entries;

            TitleText = string.Format("Dictionary Viewer [{0}]", contentFile.Path);
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
