using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdataFileManager.ViewModel.Base
{
    public class ObjectWrapperViewModel<T> : ViewModelBase where T : ViewModelBase
    {
        public T Object { get; protected set; }

        public ObjectWrapperViewModel(T obj)
        {
            if (obj == null)
                throw new ArgumentException("obj");

            Object = obj;
        }
    }
}
