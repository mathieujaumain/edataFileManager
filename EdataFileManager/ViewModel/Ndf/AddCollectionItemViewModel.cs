using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel.Ndf
{
    public class AddCollectionItemViewModel : ViewModelBase
    {
        private NdfType _type;

        public NdfType Type
        {
            get { return _type; }
            set { _type = value; OnPropertyChanged(() => Type); }
        }


    }
}
