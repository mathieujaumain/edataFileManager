using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.Model.Ndfbin.ChangeManager
{
    public class ChangeManager
    {
        private readonly ObservableCollection<ChangeEntry> _changes = new ObservableCollection<ChangeEntry>();

        public ObservableCollection<ChangeEntry> Changes
        {
            get { return _changes; }
        }

        public ICommand RevertChange
        {
            get;
            protected set;
        }

        public bool HasChanges
        {
            get { return Changes.Count > 0; }
        }

        public ChangeManager()
        {
            RevertChange = new ActionCommand(ReverChangeExeCute);
        }

        protected void ReverChangeExeCute(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
