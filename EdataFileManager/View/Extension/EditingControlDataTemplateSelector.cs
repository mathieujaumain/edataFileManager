using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EdataFileManager.Model.Ndfbin;
using EdataFileManager.Model.Ndfbin.Types;
using EdataFileManager.Model.Ndfbin.Types.AllTypes;

namespace EdataFileManager.View.Extension
{
    public class EditingControlDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var element = container as FrameworkElement;

            if (!(item is IValueHolder) || element == null)
                return null;

            var propVal = item as IValueHolder;

            var wrap = propVal.Value;

            switch (wrap.Type)
            {
                case NdfType.Float32:
                case NdfType.Float64:
                    return element.FindResource("FloatEditingTemplate") as DataTemplate;
                case NdfType.UInt32:
                    return element.FindResource("UInt32EditingTemplate") as DataTemplate;
                case NdfType.Int32:
                    return element.FindResource("Int32EditingTemplate") as DataTemplate;
                case NdfType.Guid:
                    return element.FindResource("GuidEditingTemplate") as DataTemplate;
                case NdfType.Boolean:
                case NdfType.Boolean2:
                    return element.FindResource("BooleanEditingTemplate") as DataTemplate;

                case NdfType.Color32:
                    return null;

                case NdfType.Vector:
                    return null;

                case NdfType.ObjectReference:
                    return element.FindResource("ObjectReferenceEditingTemplate") as DataTemplate;

                case NdfType.LocalisationHash:
                    return null;

                case NdfType.List:
                    return null;

            }


            return null;
        }

    }
}
