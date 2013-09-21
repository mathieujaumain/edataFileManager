using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using EdataFileManager.Model.Edata;

namespace EdataFileManager.View.Extension
{
    public class EdataFileTypeToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch ((EdataFileType)value)
            {
                case EdataFileType.Ndfbin:
                    return App.Current.Resources["ScriptIcon"] as BitmapImage;
                case EdataFileType.Dictionary:
                    return App.Current.Resources["OpenDictionayIcon"] as BitmapImage;
                case EdataFileType.Package:
                    return App.Current.Resources["PackageFileIcon"] as BitmapImage;
                default:
                    return App.Current.Resources["UnknownFileIcon"] as BitmapImage;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
