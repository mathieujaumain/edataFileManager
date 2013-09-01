using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace EdataFileManager.ViewModel.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged<T>(Expression<Func<T>> prop)
        {
            var body = prop.Body as MemberExpression;
            if (PropertyChanged != null && body != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(body.Member.Name));
            }
        }
    }
}
