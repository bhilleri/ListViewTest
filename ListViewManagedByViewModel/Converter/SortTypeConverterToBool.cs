using ListViewManagedByViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ListViewManagedByViewModel.Converter
{
    class SortTypeConverterToBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is SortType viewModelValue && parameter is string parameterString)
            {
                if(Enum.TryParse(parameterString, out SortType parameterSortType))
                {
                    return viewModelValue == parameterSortType;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
